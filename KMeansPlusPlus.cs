using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class KMeansPlusPlus<T> : KMeans<T> where T: struct
    {
        public KMeansPlusPlus(int amountClusters, MeasureSimilarity<T> measureSimilarity) : base(amountClusters, measureSimilarity) { }

        protected override void InitializeCentroids(IList<T[]> data)
        {
            if (data.Count < 2) throw new ArgumentException("The number of vectors for clustering must be greater than 1");

            //1. Выбор первого центроида случайным образом
            _clusters = new Dictionary<T[], IList<T[]>>();
            var randIndex = _random.Next(0, data.Count);
            _clusters.Add(data[randIndex], new List<T[]>());

            while (_clusters.Count != _amountClusters) 
            {
                //2. Нахождение для каждого вектора расстояния до ближайшего центроида (из тех, которые уже выбраны)
                Dictionary<T[], double> distances = new Dictionary<T[], double>();
                foreach (T[] vector in data)
                {
                    double minDistance = int.MaxValue;
                    double sum = 0;
                    foreach (T[] c in _clusters.Keys)
                    {
                        double distance = Math.Pow(_measureSimilarity.Calculate(vector, c), 2);
                        if (minDistance > distance)
                        {
                            minDistance = distance;
                            sum += distance;
                        }
                    }
                    distances.Add(vector, minDistance);
                }

                //3. Выбрать из векторов следующий центроид так, чтобы вероятность выбора вектора была пропорциональна вычисленному для неё квадрту расстояния
                double sumDistances = 0;
                foreach (T[] vector in distances.Keys)
                {
                    sumDistances += distances[vector];
                }
                double rnd = _random.NextDouble() * sumDistances;

                T[] centroid = null;
                sumDistances = 0;
                foreach (T[] vector in distances.Keys)
                {
                    sumDistances += distances[vector];
                    if (sumDistances > rnd)
                        centroid = vector;
                }
                if(centroid != null) _clusters.Add(centroid, new List<T[]>());
            }
        }
    }
}
