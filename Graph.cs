using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class Graph
    {
        private List<Edge> edges;
        private List<Node> nodes;

        public Graph()
        {
            edges = new List<Edge>();
            nodes = new List<Node>();
        }

        public IReadOnlyCollection<Edge> Edges { get => edges; }
        public IReadOnlyCollection<Node> Nodes { get => nodes; }

        private void AddNode(Node node)
        {
            foreach (Node n in nodes)
            {
                if (node.Number == n.Number) return;
            }

            nodes.Add(node);
        }

        public void AddEdge(Edge edge) 
        {
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
            edges.Remove(edge);
        }
    }
}
