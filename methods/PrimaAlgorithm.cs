using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class PrimaAlgorithm : ClusteringMethod
    {
        public PrimaAlgorithm(MeasureSimilarity measureSimilarity) : base(measureSimilarity) { }

        public override ClusterizationResult ExecuteClusterization(IList<double[]> data, int amountClusters)
        {
            CheckData(data, amountClusters);

            double?[][] distanceMatrix = new double?[data.Count][];
            
            for (int i = 0; i < data.Count; i++)
            {
                distanceMatrix[i] = new double?[data.Count];
                for (int j = 0; j < data.Count; j++)
                {
                    double? value = null;
                    if (i != j)
                        value = Math.Pow(_measureSimilarity.Calculate(data[i], data[j]), 2);

                    distanceMatrix[i][j] = value;
                }
            }
            //int randIndex = _rand.Next(0, distanceMatrix.GetLength(0));
            int randIndex = 0;

            Graph graph = new Graph();
            graph.AddEdge(new Edge(new Node(randIndex, data[randIndex]), new Node(randIndex, data[randIndex]), 0));

            int temp = 0;
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
                graph.AddEdge(new Edge(node, new Node(minDistanceIndex, data[minDistanceIndex]), (double)minDistance));
                temp++;
            }

            //Удаление первого нулевого ребра
            graph.RemoveEdge(graph.Edges.First());

            //Удаление ребер с наибольшими расстояними
            int k = 0;
            while (k != amountClusters - 1) 
            {
                Edge maxEdge = null;
                double maxWeight = double.MinValue;
                foreach (Edge e in graph.Edges)
                {
                    if (maxWeight < e.Weight) 
                    {
                        maxEdge = e;
                        maxWeight = e.Weight;
                    }
                }
                graph.RemoveEdge(maxEdge);

                k++;
            }

            return new ClusterizationResult(graph);
        }
    }
}
