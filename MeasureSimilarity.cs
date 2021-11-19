using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    abstract class MeasureSimilarity
    {
        public abstract double Calculate(double[] vector1, double[] vector2);

        //Поиск центроида, к которому ближе всего находится вектор
        public virtual double[] FindCentroid(IList<double[]> centroids, double[] vector)
        {
            double minMeasure = int.MaxValue;
            double[] centroid = null;
            foreach (var c in centroids)
            {
                double measure = Calculate(vector, c);
                if (minMeasure > measure) 
                {
                    centroid = c;
                    minMeasure = measure;
                }
            }
            return centroid;
        }

        protected void CheckVectorsValidity(double[] vector1, double[] vector2)
        {
            if (vector1.Length != vector2.Length)
                throw new ArgumentException("The lengths of the vectors are not equal");
            if (vector1.Length == 0 || vector2.Length == 0)
                throw new ArgumentException("Vector dimension can't be 0");
        }
    }
}
