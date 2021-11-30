using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class GraphClusters : IClusterableResult
    {
        private List<Edge> edges;
        private List<Node> nodes;

        public GraphClusters()
        {
            edges = new List<Edge>();
            nodes = new List<Node>();
        }

        public List<Edge> Edges { get => edges; }
        public List<Node> Nodes { get => nodes; }

        public void AddNode(Node node)
        {
            if (node == null) return;

            foreach (Node n in nodes)
            {
                if (node.Number == n.Number) return;
            }

            nodes.Add(node);
        }

        public void AddEdge(Edge edge) 
        {
            if (edge == null) return;
            if (edge.FirstNode.Number == edge.SecondNode.Number) return;

            foreach (Edge e in edges)
            {
                if ((e.FirstNode.Number == edge.FirstNode.Number && e.SecondNode.Number == edge.SecondNode.Number) ||
                    (e.SecondNode.Number == edge.FirstNode.Number && e.FirstNode.Number == edge.SecondNode.Number))
                    return;
            }

            edges.Add(edge);
            AddNode(edge.FirstNode);
            AddNode(edge.SecondNode);
        }

        public void RemoveEdge(Edge edge) 
        {
            if (edge == null) return;
            edges.Remove(edge);
        }

        public IList<double[]> GetClusterData()
        {
            List<double[]> data = new List<double[]>();
            foreach (var n in Nodes)
            {
                data.Add(n.Data);
            }
            return data;
        }
    }
}
