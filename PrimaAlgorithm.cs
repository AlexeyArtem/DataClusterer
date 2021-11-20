using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class PrimaAlgorithm : ClusteringMethod
    {
        private int _amountClusters;
        private MeasureSimilarity _measureSimilarity;
        private Random _rand;

        public PrimaAlgorithm(int amountClusters, MeasureSimilarity measureSimilarity) 
        {
            if (amountClusters <= 1) throw new ArgumentException("Amount clusters must be bigger than 1");
            _measureSimilarity = measureSimilarity ?? throw new ArgumentException("Measure similarity is null");
            _amountClusters = amountClusters;
            _rand = new Random();
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

        public override ClusterizationResult ExecuteClusterization(IList<double[]> data)
        {
            Graph graph = new Graph(data, _measureSimilarity);
            double?[,] distanceMatrix = graph.DistanceMatrix;
            int randIndex = _rand.Next(0, distanceMatrix.GetLength(0));

            double?[,] resultMatrix = new double?[distanceMatrix.GetLength(0), distanceMatrix.GetLength(1)];
            for (int i = 0; i < resultMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < resultMatrix.GetLength(1); j++)
                {
                    resultMatrix[i, j] = null;
                }
            }

            List<int?> usesIndexes = new List<int?>();
            usesIndexes.Add(randIndex);
            
            while (usesIndexes.Count != distanceMatrix.GetLength(0)) 
            {
                foreach (int i in usesIndexes) 
                {
                }
            }

            for (int i = 0; i < distanceMatrix.GetLength(1); i++)
            {
                double? minValue = int.MaxValue;
                int? index = null;
                if (distanceMatrix[randIndex, i] < minValue && distanceMatrix[randIndex, i] != null && !usesIndexes.Contains(i)) 
                {
                    minValue = distanceMatrix[randIndex, i];
                    index = i;
                }

                if (index != null) 
                {
                    usesIndexes.Add(index);
                }
            }

            return null;
        }
    }
}
