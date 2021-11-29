using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class GraphCluster : IClusterableData
    {
        List<Node> Nodes { get; }
        public IList<double[]> GetClusterData()
        {
            List<double[]> data = new List<double[]>();
            foreach (var n in Nodes)
            {
                data.Add(n.Data);
            }

            return data;
        }
    }
}
