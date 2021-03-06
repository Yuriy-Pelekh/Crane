﻿namespace Crane.Core
{
    public abstract class SolverBase : ISolver
    {
        public delegate DoubleVectorExtended Function(DoubleVectorExtended w, double um);

        protected readonly Function F;
        protected readonly DoubleVectorExtended W = new DoubleVectorExtended(4);

        protected SolverBase(Function function)
        {
            F = function;
        }

        public abstract DoubleVectorExtended Execute(double um);
    }
}
