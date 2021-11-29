using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class GraphKraskala
    {
        public GraphKraskala(List<Node> nodex, List<Edge> edges, List<Cluster> clusters, List<Edge> mstEdges)
        {
            Nodes = nodex;
            Edges = edges;
            Clusters = clusters;
            MstEdges = mstEdges;
        }

        public List<Node> Nodes;
        public List<Edge> Edges;
        public List<Cluster> Clusters;
        public List<Edge> MstEdges;

        public void AddNode(Node node) 
        {

        }
    }
}
