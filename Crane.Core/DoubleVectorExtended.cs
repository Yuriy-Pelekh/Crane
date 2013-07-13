using Sapienware.Types;

namespace Crane.Core
{
    public class DoubleVectorExtended : DoubleVector
    {
        public DoubleVectorExtended()
        {

        }

        public DoubleVectorExtended(int length)
            : base(length)
        {

        }

        public DoubleVectorExtended(params double[] data)
            : base(data)
        {
        }

        public static DoubleVectorExtended operator +(DoubleVectorExtended vector1, DoubleVectorExtended vector2)
        {
            vector1.Add(vector2);
            return vector1;
        }

        public static DoubleVectorExtended operator *(DoubleVectorExtended vector, double scalar)
        {
            for (var i = 0; i < vector.Length; i++)
            {
                vector[i] *= scalar;
            }

            return vector;
        }

        public static DoubleVectorExtended operator *(double scalar, DoubleVectorExtended vector)
        {
            return vector*scalar;
        }
    }
}
