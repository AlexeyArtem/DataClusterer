using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class Edge : IComparable
    {
        public Edge(Node firstNode, Node secondNode, double weight)
        {
            if (firstNode == null || secondNode == null) throw new ArgumentNullException("Node is null");
            FirstNode = firstNode;
            SecondNode = secondNode;
            Weight = weight;
        }

        public Node FirstNode { get; }
        public Node SecondNode { get; }
        public double Weight { get; }

        public int CompareTo(object obj)
        {
            int position = 0;
            if (obj is Edge edge)
            {
                if (edge.Weight > Weight)
                    position = -1;

                else if (edge.Weight < Weight)
                    position = 1;
            }

            return position;
        }
    }
}
