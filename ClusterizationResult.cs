using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class ClusterizationResult<T> where T: struct
    {
        public ClusterizationResult(Dictionary<T[], IList<T[]>> clusters)
        {
            Clusters = clusters;
        }

        public IReadOnlyDictionary<T[], IList<T[]>> Clusters { get; }
    }
}
