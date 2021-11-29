using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    interface IClusterableData
    {
        IList<double[]> GetClusterData();
    }
}
