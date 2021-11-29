using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class VectorCluster : IClusterableData
    {
        public VectorCluster(IList<double[]> data, double[] centroid)
        {
            if (data == null || centroid == null) throw new ArgumentNullException();
            Centroid = centroid;
            Data = data;
        }

        public double[] Centroid { get; }
        public IList<double[]> Data { get; }

        public IList<double[]> GetClusterData()
        {
            return Data;
        }
    }
}
