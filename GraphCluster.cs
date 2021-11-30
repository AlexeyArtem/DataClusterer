using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class GraphCluster : IClusterableData
    {
        public IList<double[]> GetClusterData()
        {
            List<double[]> data = new List<double[]>();
            foreach (var n in Nodes)
            {
                data.Add(n.Data);
            }

            return data;
        }

        public List<Node> Nodes { get; }
    }
}
