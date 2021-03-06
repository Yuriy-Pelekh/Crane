﻿using System;
using System.Collections.Generic;
using System.IO;
using Sapienware.Algorithms.TController;

namespace Crane.Core
{
    public static class TapTask
    {
        private const string RulesFileName = "CranePosition.tcw";

        public static List<CranePosition> Execute(Solvers solverType)
        {
            var resultsFileName = string.Format("Results_{0}.txt", DateTime.Now.Ticks);

            var controllerHelper = new ControllerHelper<Controller>();

            using (var fileStream = new FileStream(RulesFileName, FileMode.Open))
            {
                try
                {
                    controllerHelper.Initialize(fileStream);
                }
                finally
                {
                    fileStream.Close();
                }
            }

            var contrller = controllerHelper.CreateController();
            controllerHelper.Execute(contrller, solverType);

            using (var file = new StreamWriter(resultsFileName))
            {
                var date = DateTime.Now;
                file.WriteLine("Method: {0}   -   {1}, {2}{3}", solverType, date.ToLongDateString(),
                               date.ToLongTimeString(), Environment.NewLine);
                file.WriteLine("Iteration\t\tAngle\t\tDistance\t\tPower");

                foreach (var cranePosition in controllerHelper.CraneData)
                {
                    file.WriteLine("{0}\t\t{1}\t\t{2}\t\t{3}", cranePosition.Iteration, cranePosition.Angle,
                                   cranePosition.Distance, cranePosition.Power);
                }
                file.Close();
            }

            return controllerHelper.CraneData;
        }

        public static string GetRules()
        {
            using (var streamReader = new StreamReader(RulesFileName))
            {
                try
                {
                    return streamReader.ReadToEnd();
                }
                finally
                {
                    streamReader.Close();
                }
            }
        }

        public static void SetRules(string rules)
        {
            using (var streamWriter = new StreamWriter(RulesFileName,false))
            {
                try
                {
                    streamWriter.Write(rules);
                }
                finally
                {
                    streamWriter.Close();
                }
            }
        }
    }
}
