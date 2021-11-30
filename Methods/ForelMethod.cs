using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class ForelMethod : KMeans
    {
        protected new const double ACCURACY = 0.1;

        public ForelMethod(MeasureSimilarity measureSimilarity) : base(measureSimilarity) { }

        private double[] GetRandomCentroid(IList<double[]> data) 
        {
            bool isUniqueCentroid = false;
            double[] newCentroid = null;
            while (!isUniqueCentroid) 
            {
                int randIndex = _rand.Next(0, data.Count);
                newCentroid = data[randIndex];
                var listCentroids = _clusters.Keys.ToList();

                if (!listCentroids.Contains(newCentroid)) 
                {
                    isUniqueCentroid = true;
                }
            }

            return newCentroid;
        }

        public IClusterableResult ExecuteClusterization(IList<double[]> data, double radius) 
        {
            if (radius <= 0 || data.Count <= 0) throw new ArgumentException("Радиус должен быть больше нуля и количество данных не должно быть нулевым");

            _clusters = new Dictionary<double[], IList<double[]>>();
            double[][] arrData = new double[data.Count][];
            data.CopyTo(arrData, 0);
            List<double[]> currentData = arrData.ToList();
            int countVectors = 0; //Количество использованных векторов

            while (countVectors != data.Count)
            {
                //1. Инициализация нового центроида
                double[] currentCentroid = GetRandomCentroid(currentData);
                double[] previusCentroid = currentCentroid;
                double[] averageCentroid = null;
                List<double[]> dataClusters = null;
                double distanceCentroids = int.MaxValue; //Расстояние между предыдущим центроидом и текущим

                while (distanceCentroids > ACCURACY)
                {
                    currentCentroid = previusCentroid;
                    dataClusters = new List<double[]>();

                    //2. Заполнение кластера векторами
                    foreach (double[] vector in currentData)
                    {
                        double distance = _measureSimilarity.Calculate(vector, currentCentroid);
                        if (distance <= radius)
                        {
                            dataClusters.Add(vector);
                        }
                    }

                    //3. Вычисление средней последовательности
                    int length = currentData[0].Length; //Длина одной последовательности данных
                    int count = dataClusters.Count; //Количество последовательностей в кластере
                    averageCentroid = new double[length];
                    for (int i = 0; i < length; i++)
                    {
                        dynamic sum = 0;
                        for (int j = 0; j < count; j++)
                        {
                            sum += dataClusters[j][i];
                        }
                        averageCentroid[i] = sum != 0 ? sum / count : 0;
                    }
                    previusCentroid = averageCentroid;
                    //4. Вычисление расстояния между центроидами
                    distanceCentroids = _measureSimilarity.Calculate(previusCentroid, currentCentroid);

                }
                countVectors += dataClusters.Count;
                _clusters.Add(currentCentroid, dataClusters);
                currentData.RemoveAll(item => dataClusters.Any(item2 => item == item2));
            }

            return new VectorClusters(_clusters);
        }
    }
}
