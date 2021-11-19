using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    abstract class ClusteringMethod
    {
        public abstract ClusterizationResult ExecuteClusterization(IList<double[]> data);
    }
}
