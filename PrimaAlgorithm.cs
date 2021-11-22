using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class PrimaAlgorithm : ClusteringMethod
    {
        private Random _rand;

        public PrimaAlgorithm(MeasureSimilarity measureSimilarity) : base(measureSimilarity)
        {
            _measureSimilarity = measureSimilarity ?? throw new ArgumentException("Measure similarity is null");
        }

        public override ClusterizationResult ExecuteClusterization(IList<double[]> data, int amountClusters)
        {
            CheckData(data, amountClusters);

            double?[][] distanceMatrix = new double?[data.Count][];
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data.Count; j++)
                {
                    double? value = null;
                    if (i != j)
                        value = _measureSimilarity.Calculate(data[i], data[j]);

                    distanceMatrix[i][j] = value;
                }
            }
            int randIndex = _rand.Next(0, distanceMatrix.GetLength(0));

            Graph graph = new Graph();
            graph.AddEdge(new Edge(null, new Node(randIndex), 0));

            while (graph.Nodes.Count != distanceMatrix.GetLength(0)) 
            {
                //Поиск следующего наименьшего расстояния
                double? minDistance = int.MaxValue;
                Node node = null;
                foreach (Node n in graph.Nodes) 
                {
                    double? value = distanceMatrix[n.Number].Min();
                    if (value < minDistance) 
                    {
                        minDistance = value;
                        node = n;
                    }
                }

                //Исключение найденного значения из матрицы расстояний
                int minDistanceIndex = Array.IndexOf(distanceMatrix[node.Number], minDistance);
                distanceMatrix[node.Number][minDistanceIndex] = null;
                distanceMatrix[minDistanceIndex][node.Number] = null;

                //Добавление нового ребра
                graph.AddEdge(new Edge(node, new Node(minDistanceIndex), (double)minDistance));
            }

            //Удаление ребер с наибольшими расстояними
            int k = 0;
            while (k != amountClusters - 1) 
            {
                Edge edge = graph.Edges.First();
                foreach (Edge e in graph.Edges)
                {
                    if (edge.Weight < e.Weight)
                        edge = e;
                }
                graph.RemoveEdge(edge);

                k++;
            }

            //Распределение данных по кластерам


            return null;
        }
    }
}
