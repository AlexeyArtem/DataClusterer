using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class Component
    {

        public Component(List<Node> nodes)
        {
            Nodes = nodes;
        }

        public Component()
        {
            Nodes = new List<Node>();
        }

        public List<Node> Nodes { get; }

        public static Component operator+ (Component component1, Component component2) 
        {
            List<Node> resultList = component1.Nodes.Concat(component2.Nodes).ToList();
            Component result = new Component(resultList);
            return result;
        }
    }
}
