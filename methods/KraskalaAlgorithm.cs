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

        public override IClusterableResult ExecuteClusterization(IList<double[]> data, int amountClusters)
        {
            CheckData(data, amountClusters);

            GraphClusters linkedGraph = new GraphClusters();
            for (int i = 0; i < data.Count; i++)
            {
                Node node1 = new Node(i, data[i]);
                for (int j = 0; j < data.Count; j++)
                {
                    double weight = _measureSimilarity.Calculate(data[i], data[j]);
                    Node node2 = new Node(j, data[j]);
                    linkedGraph.AddEdge(new Edge(node1, node2, weight));
                }
            }
            
            linkedGraph.Edges.Sort();
            GraphClusters resultGraph = new GraphClusters();

            List<Component> components = new List<Component>();
            bool isContinue = true;
            while (isContinue) 
            {
                Edge edge = linkedGraph.Edges.First();

                var queryFirst = from component in components
                                 from node in component.Nodes
                                 where node.Number == edge.FirstNode.Number
                                 select component;
                Component componentFirst = queryFirst.FirstOrDefault();

                var querySecond = from component in components
                                 from node in component.Nodes
                                 where node.Number == edge.SecondNode.Number
                                 select component;
                Component componentSecond = querySecond.FirstOrDefault();

                if (componentFirst == componentSecond) 
                {
                    if (componentFirst == null && componentSecond == null)
                    {
                        components.Add(new Component(new List<Node>() { edge.FirstNode, edge.SecondNode }));
                        resultGraph.AddEdge(edge);
                    }
                }
                else if (componentFirst == null && componentSecond != null)
                {
                    componentSecond.Nodes.Add(edge.FirstNode);
                    resultGraph.AddEdge(edge);
                }
                else if (componentFirst != null && componentSecond == null)
                {
                    componentFirst.Nodes.Add(edge.SecondNode);
                    resultGraph.AddEdge(edge);
                }
                else if (componentFirst != null && componentSecond != null) 
                {
                    components.Remove(componentFirst);
                    components.Remove(componentSecond);
                    components.Add(componentFirst + componentSecond);

                    resultGraph.AddEdge(edge);
                }

                linkedGraph.RemoveEdge(edge);
                if (components.Count == 1 && components[0].Nodes.Count == data.Count) 
                {
                    isContinue = false;
                }
            }

            int k = 0;
            while (k != amountClusters - 1)
            {
                Edge maxEdge = null;
                double maxWeight = double.MinValue;
                foreach (Edge e in resultGraph.Edges)
                {
                    if (maxWeight < e.Weight)
                    {
                        maxEdge = e;
                        maxWeight = e.Weight;
                    }
                }
                resultGraph.RemoveEdge(maxEdge);
                k++;
            }

            return resultGraph;

        }
    }
}
