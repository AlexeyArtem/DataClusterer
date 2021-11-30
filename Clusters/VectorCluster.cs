using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class VectorClusters : IClusterableResult
    {
        public VectorClusters(Dictionary<double[], IList<double[]>> data)
        {
            Data = data ?? throw new ArgumentNullException("Данные не заполнены");
        }

        public Dictionary<double[], IList<double[]>> Data { get; }
        public IList<double[]> GetClusterData()
        {
            return (IList<double[]>)Data.Values.ToList();
        }
    }
}
