using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class KMeans : ClusteringMethod
    {
        protected static Random _random = new Random();
        protected Dictionary<double[], IList<double[]>> _clusters;
        protected const double ACCURACY = 0.01;


        public KMeans(MeasureSimilarity measureSimilarity) : base(measureSimilarity)
        {
            _measureSimilarity = measureSimilarity ?? throw new ArgumentException("Measure similarity is null");
        }


        //Начальная инициализация центров масс (центроидов) случайным образом
        protected virtual void InitializeCentroids(IList<double[]> data, int amountClusters) 
        {
            if (data.Count < 2) throw new ArgumentException("The number of vectors for clustering must be greater than 1");

            _clusters = new Dictionary<double[], IList<double[]>>();
            while (_clusters.Count != amountClusters)
            {
                int randIndex = _random.Next(0, data.Count);

                double[] vector = data[randIndex];
                if (!_clusters.ContainsKey(vector))
                {
                    _clusters.Add(vector, new List<double[]>());
                }
            }
        }


        public virtual IClusterableResult ExecuteClusterization(IList<double[]> data) 
        {
            if (data.Count <= 1) throw new ArgumentException();

            double accuracy = 1;
            int amountClusters = 1;

            VectorClusters clusterizationResult = ExecuteClusterization(data, amountClusters) as VectorClusters; ;
            
            double previousSum = int.MaxValue;
            double temp = previousSum;
            double currentSum = 0;
            foreach (var centroid in clusterizationResult.Data.Keys)
            {
                foreach (var vector in clusterizationResult.Data[centroid])
                {
                    currentSum += Math.Pow(_measureSimilarity.Calculate(vector, centroid), 2);
                }
            }

            while (previousSum - currentSum > accuracy)
            {
                amountClusters++;
                previousSum = currentSum; 
                currentSum = 0;
                clusterizationResult = ExecuteClusterization(data, amountClusters) as VectorClusters;
                foreach(var centroid in clusterizationResult.Data.Keys)
                {
                    foreach (var vector in clusterizationResult.Data[centroid])
                    {
                        currentSum += Math.Pow(_measureSimilarity.Calculate(vector, centroid), 2);
                    }
                }
            }

            return ExecuteClusterization(data, amountClusters - 1);
        }


        public override IClusterableResult ExecuteClusterization(IList<double[]> data, int amountClusters)
        {
            CheckData(data, amountClusters);
            InitializeCentroids(data, amountClusters);

            bool isCentroidsChange = true;
            while (isCentroidsChange) 
            {
                List<double[]> centroids = _clusters.Keys.ToList();
                //1. Распределение последовательностей (векторов) данных по кластерам
                for (int i = 0; i < data.Count; i++)
                {
                    _clusters[_measureSimilarity.FindCentroid(centroids, data[i])].Add(data[i]);
                }

                //2. Перерасчет центров масс кластеров
                Dictionary<double[], IList<double[]>> newClusters = new Dictionary<double[], IList<double[]>>();
                foreach (double[] centroid in _clusters.Keys)
                {
                    //Вычесление средней последовательности
                    int length = data[0].Length; //Длина одной последовательности данных
                    int count = _clusters[centroid].Count; //Количество последовательностей в кластере
                    double[] averageCentroid = new double[length];
                    for (int i = 0; i < length; i++)
                    {
                        dynamic sum = 0;
                        for (int j = 0; j < count; j++)
                        {
                            sum += _clusters[centroid][j][i];
                        }
                        averageCentroid[i] = sum != 0 ? sum / count : 0;
                    }
                    newClusters.Add(_measureSimilarity.FindCentroid(_clusters[centroid], averageCentroid), _clusters[centroid]);
                }

                //3. Сравнение текущих и новых центроидов
                List<double[]> currentCentroids = _clusters.Keys.ToList();
                List<double[]> newCentroids = newClusters.Keys.ToList();
                isCentroidsChange = false;
                for (int i = 0; i < currentCentroids.Count; i++)
                {
                    if (!currentCentroids[i].SequenceEqual(newCentroids[i]))
                    {
                        isCentroidsChange = true;

                        //4. Обновление центров масс в кластерах
                        _clusters.Clear();
                        _clusters = newClusters;
                        foreach (double[] c in _clusters.Keys)
                            _clusters[c].Clear();

                        break;
                    }
                }
            }

            return new VectorClusters(_clusters);
        }
    }
}
