using Accord.Statistics.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    class ClusterizationResult
    {
        public ClusterizationResult(Dictionary<double[], IList<double[]>> clusters)
        {
            Clusters = clusters;
            ClustersGraph = null;
        }

        public ClusterizationResult(Graph clustersGraph)
        {
            ClustersGraph = clustersGraph;
            Clusters = null;
        }

        public IReadOnlyDictionary<double[], IList<double[]>> Clusters { get; }
        public Graph ClustersGraph { get; }
    }

}
