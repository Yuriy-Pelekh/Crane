using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using Sapienware.Algorithms;
using Sapienware.Algorithms.TController;
using Sapienware.Types;

namespace Crane.Core
{
    public class ControllerHelper
    {
        #region Properties

        public string Name { get; private set; }

        public List<Variable> Inputs { get; private set; }

        public List<Rule> Rules { get; private set; }

        public List<CranePosition> CraneData { get; private set; }

        #endregion

        #region Constructors

        public ControllerHelper()
        {
            Inputs = new List<Variable>();
            Rules = new List<Rule>();
            CraneData = new List<CranePosition>();
        }

        #endregion

        #region Methods

        public void Initialize(FileStream fileStream)
        {
            Inputs.Clear();
            Rules.Clear();

            var controller = new Controller();
            var serializer = new DataContractJsonSerializer(controller.GetType());

            var serializedData = serializer.ReadObject(fileStream);
            controller = serializedData as Controller;

            Name = controller.Name;

            foreach (var variable in controller.VXList)
            {
                Inputs.Add(variable);
            }

            foreach (var rule in controller.RuleCollection)
            {
                Rules.Add(rule);
            }
        }

        public Controller CreateController()
        {
            var controller = new Controller {Name = Name};

            foreach (var variable in Inputs)
            {
                if (variable.Terms.Count < 2)
                {
                    throw new AlgorithmException("Input variable \"" + variable.Name +
                                                 "\" should contain at least 2 terms.");
                }
                if (HaveDuplicates(variable.Terms))
                {
                    throw new AlgorithmException("Input variable \"" + variable.Name + "\" contains duplicate terms.");
                }
            }

            if (Rules.Count < 2)
            {
                throw new AlgorithmException("Output variable \"" + Name + "\" should contain at least 2 terms.");
            }

            if (HaveDuplicates(Rules))
            {
                throw new AlgorithmException("Output variable \"" + Name + "\" contains duplicate terms.");
            }

            foreach (
                var item in
                    Inputs.Select(
                        variable =>
                        new Variable(variable.Name) {Terms = variable.Terms, NumericInput = variable.NumericInput}))
            {
                controller.VXList.Add(item);
            }

            foreach (var rule in Rules)
            {
                controller.RuleCollection.Add(rule);
            }

            return controller;
        }

        public void Execute(Controller controller, Solvers solverType)
        {
            CraneData.Clear();

            var solver = CreateSolver(solverType);

            CraneData.Add(new CranePosition {Angle = 0, Distance = 0});

            for (var i = 0; i < 10000 && (CraneData.Last().Distance < 1 || Check(solverType)); i++)
            {
                var matrix = new DoubleMatrix(1, 2);

                matrix[0, 0] = CraneData[i].Angle;
                matrix[0, 1] = CraneData[i].Distance;

                var res = controller.Apply(matrix)[0];
                CraneData.Last().Power = res;

                var result = solver.Execute(res);

                CraneData.Add(new CranePosition
                    {
                        Iteration = i,
                        Angle = result[1],
                        Distance = result[0]
                    });
            }
        }

        private bool Check(Solvers solverType)
        {
            var res = false;
            var delta = solverType == Solvers.Euler ? 0.001 : 0.3;

            for (var i = CraneData.Count - 1; i >= 0 && i > CraneData.Count - 10; i--)
            {
                if (Math.Abs(CraneData[i].Angle) > delta)
                {
                    res = true;
                }
            }

            return res;
        }

        private static ISolver CreateSolver(Solvers solverType)
        {
            switch (solverType)
            {
                case Solvers.Euler:
                    return new Euler(Tap.Calculate);
                case Solvers.RungeKutta:
                    return new RungeKutta(Tap.Calculate);
                default:
                    throw new NotSupportedException(string.Format("Not supported solver type: {0}", solverType));
            }
        }

        private static bool HaveDuplicates(IEnumerable<FuzzyTerm> items)
        {
            var distinctItemsCount = items.Select(i => i.Term).Distinct().Count();
            return distinctItemsCount != items.Count();
        }

        #endregion
    }
}
