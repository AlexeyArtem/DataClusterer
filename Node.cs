﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class Node
    {
        public Node(double[] data)
        {
            Data = data;
        }

        public Node(int number)
        {
            Number = number;
        }

        public int Number { get; }
        public double[] Data { get; }
    }
}
