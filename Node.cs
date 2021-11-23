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

        public int Number { get; }
        public double[] Data { get; }
    }
}
