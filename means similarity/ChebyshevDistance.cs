using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class ChebyshevDistance : MeasureSimilarity
    {
        public override double Calculate(double[] vector1, double[] vector2)
        {
            CheckVectorsValidity(vector1, vector2);

            double d = 0;
            for (int i = 0; i < vector1.Length; i++)
            {
                double temp = Math.Abs(vector1[i] - vector2[i]);
                if (temp > d) d = temp;
            }
            return Math.Sqrt(d);
        }
    }
}
