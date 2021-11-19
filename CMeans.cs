using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class CMeans : KMeans
    {
        private const double ACCURACY = 0.01;
        public CMeans(int amountClusters, MeasureSimilarity measureSimilarity) : base (amountClusters, measureSimilarity) { }

        //Вычисление критерия нечеткой ошибки
        private double GetE(IList<double[]> data, Dictionary<double[], IList<double[]>> clusters) 
        {
            double[,] matrixU = GetMatrixU(data, clusters);
            List<double[]> centroids = clusters.Keys.ToList();
            double E = 0;
            for (int i = 0; i < data.Count; i++)
            {
                for (int k = 0; k < _amountClusters; k++)
                {
                    E += matrixU[i, k] * Math.Pow(_measureSimilarity.Calculate(data[i], centroids[k]), 2);
                }
            }
            return E;
        }

        //Получение матрицы U
        private double[,] GetMatrixU(IList<double[]> data, Dictionary<double[], IList<double[]>> clusters) 
        {
            double[,] matrixU = new double[data.Count, _amountClusters];
            for (int i = 0; i < data.Count; i++)
            {
                List<double> distance = new List<double>();
                foreach (double[] c in clusters.Keys)
                    distance.Add(_measureSimilarity.Calculate(data[i], c));

                for (int j = 0; j < _amountClusters; j++)
                {
                    matrixU[i, j] = 1 - distance[j] / distance.Sum();
                }
            }
            return matrixU;
        }

        public override ClusterizationResult ExecuteClusterization(IList<double[]> data)
        {
            InitializeCentroids(data);

            double currentAccuracy = int.MaxValue;
            while (currentAccuracy > ACCURACY) 
            {
                //0. Распеределение векторов по кластерам
                List<double[]> centroids = _clusters.Keys.ToList();
                for (int i = 0; i < data.Count; i++)
                {
                    _clusters[_measureSimilarity.FindCentroid(centroids, data[i])].Add(data[i]);
                }

                //1. Заполнение матрицы U
                double[,] matrixU = new double[data.Count, _amountClusters];
                for (int i = 0; i < data.Count; i++)
                {
                    List<double> distance = new List<double>();
                    foreach (double[] c in _clusters.Keys)
                        distance.Add(_measureSimilarity.Calculate(data[i], c));

                    for (int j = 0; j < _amountClusters; j++)
                    {
                        matrixU[i, j] = 1 - distance[j] / distance.Sum();
                    }
                }

                //2. Вычисление критерия нечеткой ошибки
                double E = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    for (int k = 0; k < _amountClusters; k++)
                    {
                        E += matrixU[i, k] * Math.Pow(_measureSimilarity.Calculate(data[i], centroids[k]), 2);
                    }
                }

                //3. Перерасчет центров масс кластеров
                Dictionary<double[], IList<double[]>> newClusters = new Dictionary<double[], IList<double[]>>();
                int length = data[0].Length;
                for (int k = 0; k < centroids.Count; k++)
                {
                    double[] averageCentroid = new double[length];
                    for (int j = 0; j < length; j++)
                    {
                        dynamic sumVectors = 0;
                        double sumP = 0;
                        for (int i = 0; i < data.Count; i++)
                        {
                            dynamic value = data[i][j];
                            sumVectors += value * matrixU[i, k];
                            sumP += matrixU[i, k];
                        }
                        averageCentroid[j] = sumVectors / sumP;
                    }
                    newClusters.Add(_measureSimilarity.FindCentroid(_clusters[centroids[k]], averageCentroid), _clusters[centroids[k]]);
                }

                //4. Обновление центров масс в кластерах
                double newE = GetE(data, newClusters);
                currentAccuracy = E - newE;

                if (currentAccuracy > ACCURACY) 
                {
                    _clusters.Clear();
                    _clusters = newClusters;
                    foreach (double[] c in _clusters.Keys)
                        _clusters[c].Clear();
                }
            }

            return new ClusterizationResult(_clusters);
        }
    }
}
