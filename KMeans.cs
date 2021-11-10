using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class KMeans<T> : ClusteringMethod<T> where T : struct
    {
        private static Random _random = new Random();
        private int _amountClusters;
        private MeasureSimilarity<T> _measureSimilarity;
        private Dictionary<T[], IList<T[]>> clusters;


        public KMeans(int amountClusters, MeasureSimilarity<T> measureSimilarity)
        {
            if (amountClusters <= 0) throw new ArgumentException("Amount clusters must be bigger than zero");
            _measureSimilarity = measureSimilarity ?? throw new ArgumentException("Measure similarity is null");
            _amountClusters = amountClusters;
        }

        public int AmountClusters
        {
            get 
            {
                return _amountClusters;
            }
            set 
            {
                _amountClusters = value;
            }
        }

        public override ClusterizationResult<T> ExecuteClusterization(IList<T[]> data)
        {
            //0. Начальная инициализация центров масс (центроидов) случайным образом
            clusters = new Dictionary<T[], IList<T[]>>();
            while (clusters.Count != _amountClusters)
            {
                int randIndex = _random.Next(0, data.Count);
                
                T[] vector = data[randIndex];
                if (!clusters.ContainsKey(vector))
                {
                    clusters.Add(vector, new List<T[]>());
                }
            }

            bool isCentroidsChange = true;
            while (isCentroidsChange) 
            {
                List<T[]> centroids = clusters.Keys.ToList();
                //1. Распределение последовательностей (векторов) данных по кластерам
                for (int i = 0; i < data.Count; i++)
                {
                    clusters[_measureSimilarity.FindCentroid(centroids, data[i])].Add(data[i]);
                }

                //2. Перерасчет центров масс кластеров
                Dictionary<T[], IList<T[]>> newClusters = new Dictionary<T[], IList<T[]>>();
                foreach (T[] centroid in clusters.Keys)
                {
                    //Вычесление средней последовательности
                    int length = data[0].Length; //Длина одной последовательности данных
                    int count = clusters[centroid].Count; //Количество последовательностей в кластере
                    T[] averageCentroid = new T[length];
                    for (int i = 0; i < length; i++)
                    {
                        dynamic sum = 0;
                        for (int j = 0; j < count; j++)
                        {
                            sum += clusters[centroid][j][i];
                        }
                        averageCentroid[i] = sum != 0 ? sum / count : 0;
                    }
                    newClusters.Add(_measureSimilarity.FindCentroid(clusters[centroid], averageCentroid), clusters[centroid]);
                }

                //3. Сравнение текущих и новых центров масс
                List<T[]> currentCentroids = clusters.Keys.ToList();
                List<T[]> newCentroids = newClusters.Keys.ToList();
                isCentroidsChange = false;
                for (int i = 0; i < currentCentroids.Count; i++)
                {
                    if (!currentCentroids[i].SequenceEqual(newCentroids[i]))
                    {
                        isCentroidsChange = true;

                        //4. Обновление центров масс в кластерах
                        clusters.Clear();
                        clusters = newClusters;
                        foreach (T[] c in clusters.Keys)
                            clusters[c].Clear();

                        break;
                    }
                }
            }

            return new ClusterizationResult<T>(clusters);
        }
    }
}
