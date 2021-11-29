using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class EuclideanDistance : MeasureSimilarity
    {
        public override double Calculate(double[] vector1, double[] vector2)
        {
            CheckVectorsValidity(vector1, vector2);

            double d = 0;
            for (int i = 0; i < vector1.Length; i++)
            {
                d += Math.Pow(vector1[i] - vector2[i], 2);
            }
            return Math.Sqrt(d);
        }
    }
}
