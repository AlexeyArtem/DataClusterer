using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    abstract class ClusteringMethod
    {
        protected MeasureSimilarity _measureSimilarity;
        protected Random _rand;

        public ClusteringMethod(MeasureSimilarity measureSimilarity) 
        {
            if (measureSimilarity == null) throw new ArgumentNullException("Measure similarity is null");
            _measureSimilarity = measureSimilarity;
            _rand = new Random();
        }

        protected virtual void CheckData(IList<double[]> data, int amountClusters) 
        {
            if (data.Count < 2) throw new ArgumentException("The number of vectors for clustering must be greater than 1");
            if (amountClusters < 1) throw new ArgumentException("Amount clusters must be bigger than 0");
            if (amountClusters > data.Count) throw new ArgumentException("Amount clusters is greater than the amount data vectors");

        }

        public abstract IClusterableResult ExecuteClusterization(IList<double[]> data, int amountClusters);

    }
}
