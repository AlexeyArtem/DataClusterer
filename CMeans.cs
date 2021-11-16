using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class CMeans<T> : KMeans<T> where T: struct
    {
        public CMeans(int amountClusters, MeasureSimilarity<T> measureSimilarity) : base (amountClusters, measureSimilarity) { }

        protected override void InitializeCentroids(IList<T[]> data)
        {

        }

        public override ClusterizationResult<T> ExecuteClusterization(IList<T[]> data)
        {
            return null;
        }
    }
}
