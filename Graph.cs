using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class Graph
    {
        public Graph(IList<double[]> data, MeasureSimilarity measureSimilarity) 
        {
            Edges = new List<Edge>();
            DistanceMatrix = new double?[data.Count, data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                Node firstNode = new Node(data[i]);
                if (i == data.Count - 1) break;
                for (int j = i + 1; j < data.Count; j++)
                {
                    Node secondNode = new Node(data[j]);
                    Edges.Add(new Edge(firstNode, secondNode, measureSimilarity.Calculate(data[i], data[j])));
                }

                for (int j = 0; j < data.Count; j++)
                {
                    double? value = null;
                    if (i != j) 
                        value = measureSimilarity.Calculate(data[i], data[j]);

                    DistanceMatrix[i, j] = value;
                }
            }
        }

        public Graph()
        {
            Edges = new List<Edge>();
            DistanceMatrix = new double?[0, 0];
        }

        public List<Edge> Edges { get; }
        public double?[,] DistanceMatrix { get; }
    }
}
