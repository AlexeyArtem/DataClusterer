using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class KraskalaAlgorithm : ClusteringMethod
    {
        public KraskalaAlgorithm(MeasureSimilarity measureSimilarity) : base(measureSimilarity) { }

        public override ClusterizationResult ExecuteClusterization(IList<double[]> data, int amountClusters)
        {
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

            Graph graph = new Graph();
            List<List<Node>> components = null;

            //while (/*Все узлы входят в один компонент*/)
            //{
            //    //Поиск наименьшего расстояния
            //    double? minDistance = double.MaxValue;
            //    int[] minDistanceIndexes = new int[2];

            //    for (int i = 0; i < distanceMatrix.GetLength(0); i++)
            //    {
            //        for (int j = 0; j < distanceMatrix.GetLength(1); j++)
            //        {
            //            if (distanceMatrix[i][j] < minDistance)
            //            { 
            //                minDistance = distanceMatrix[i][j];
            //                minDistanceIndexes[0] = i;
            //                minDistanceIndexes[1] = j;
            //            }
            //        }
            //    }

            //    //Проверка на содержание ребра в том же компоненте
            //    Node node1 = new Node(minDistanceIndexes[0]);
            //    Node node2 = new Node(minDistanceIndexes[1]);
            //    Edge edge = new Edge(new Node(minDistanceIndexes[0]), new Node(minDistanceIndexes[1]), (double)minDistance);
            //    bool isContinue = true;
            //    bool isNeedNewComponent = true;
            //    foreach(List<Node> component in components)
            //    {
            //        if (component.Contains(node1) && component.Contains(node2)) isContinue = false;  //Ребро не добавляется
            //        else if (component.Contains(node1) && !component.Contains(node2))
            //        {
            //            component.Add(node2);
            //            isNeedNewComponent = false;
            //        }
            //        else if (component.Contains(node2) && !component.Contains(node1))
            //        {
            //            component.Add(node1);
            //            isNeedNewComponent = false;
            //        }
            //    }
            //    if (isContinue == false) continue;
            //    if (isNeedNewComponent == true)
            //    {
            //        List<Node> list = new List<Node>();
            //        list.Add(node1);
            //        list.Add(node2);
            //        components.Add(list);
            //    }


            //    //Добавление нового ребра
            //    graph.AddEdge(edge);

            //    //Исключение найденного значения из матрицы расстояний
            //    distanceMatrix[minDistanceIndexes[0]][minDistanceIndexes[1]] = null;
            //    distanceMatrix[minDistanceIndexes[1]][minDistanceIndexes[0]] = null;

            //}

            //Удаление ребер с наибольшими расстояними

            //Распределение данных по кластерам


            return null;
        }
    }
}
