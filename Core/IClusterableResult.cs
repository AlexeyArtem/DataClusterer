using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    interface IClusterableResult
    {
        IList<double[]> GetClusterData();
    }
}
