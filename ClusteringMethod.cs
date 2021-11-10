using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    abstract class ClusteringMethod<T>where T : struct
    {
        public abstract ClusterizationResult<T> ExecuteClusterization(IList<T[]> data);
        
        public virtual IList<T[]> NormalizeData(IList<T[]> data) 
        {
            return null;
        }
    }
}
