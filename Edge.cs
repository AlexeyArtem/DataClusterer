using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class Edge
    {
        public Edge(Node firstNode, Node secondNode, double weight)
        {
            FirstNode = firstNode;
            SecondNode = secondNode;
            Weight = weight;
        }

        public Node FirstNode { get; set; }
        public Node SecondNode { get; set; }
        public double Weight { get; }
    }
}
