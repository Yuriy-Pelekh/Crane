namespace Crane.Core
{
    public static class Tap
    {
        public static DoubleVectorExtended Calculate(DoubleVectorExtended w, double um)
        {
            var matrix = new[,]
                {
                    {0, 0, 1, 0},
                    {0, 0, 0, 1},
                    {0, 1.5216, -11.6513, 0.0049},
                    {0, -26.1093, 26.8458, -0.0841}
                };

            var vector = new[] {0, 0, 1.5304, -3.5261};

            var v0 = matrix[0, 0]*w[0] + matrix[0, 1]*w[1] + matrix[0, 2]*w[2] + matrix[0, 3]*w[3];
            var v1 = matrix[1, 0]*w[0] + matrix[1, 1]*w[1] + matrix[1, 2]*w[2] + matrix[1, 3]*w[3];
            var v2 = matrix[2, 0]*w[0] + matrix[2, 1]*w[1] + matrix[2, 2]*w[2] + matrix[2, 3]*w[3];
            var v3 = matrix[3, 0]*w[0] + matrix[3, 1]*w[1] + matrix[3, 2]*w[2] + matrix[3, 3]*w[3];

            var data = new[] {v0 + vector[0]*um, v1 + vector[1]*um, v2 + vector[2]*um, v3 + vector[3]*um};
            return new DoubleVectorExtended(data);
        }
    }
}
