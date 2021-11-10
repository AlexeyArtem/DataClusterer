using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    abstract class MeasureSimilarity<T>
    {
        public abstract double Calculate(T[] vector1, T[] vector2);

        //Поиск центроида, к которому ближе всего находится вектор
        public virtual T[] FindCentroid(IList<T[]> centroids, T[] vector)
        {
            double minMeasure = int.MaxValue;
            T[] centroid = null;
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

        protected void CheckVectorsValidity(T[] vector1, T[] vector2)
        {
            if (vector1.Length != vector2.Length)
                throw new ArgumentException("The lengths of the vectors are not equal");
            if (vector1.Length == 0 || vector2.Length == 0)
                throw new ArgumentException("Vector dimension can't be 0");
        }
    }
}
