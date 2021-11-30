using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class GraphKraskala
    {
        public GraphKraskala(List<Node> nodes, List<Edge> edges, List<GraphCluster> clusters, List<Edge> mstEdges)
        {
            Nodes = nodes;
            Edges = edges;
            Clusters = clusters;
            MstEdges = mstEdges;
        }

        public GraphKraskala()
        {
            Nodes = new List<Node>();
            Edges = new List<Edge>();
            Clusters = new List<GraphCluster>();
            MstEdges = new List<Edge>();
        }

        public List<Node> Nodes;
        public List<Edge> Edges;
        public List<GraphCluster> Clusters;
        public List<Edge> MstEdges;

        public void AddNode(Node node) 
        {
            Nodes.Add(node);
            GraphCluster graphCluster = new GraphCluster();
            graphCluster.Nodes.Add(node);
            Clusters.Add(graphCluster);
        }
    }
}
