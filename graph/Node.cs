using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class Node
    {
        public Node(int number, double[] data)
        {
            Number = number;
            Data = data;
        }

        public Node(int number, double[] data, GraphCluster cluster)
        {
            Number = number;
            Data = data;
            Cluster = cluster;
        }

        public GraphCluster Cluster { get; }
        public int Number { get; }
        public double[] Data { get; }
    }
}
