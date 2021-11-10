using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataClusterer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<double[]> data = new List<double[]>()
            {
                new double[] { 2, 2 },
                new double[] { 1, 2 },
                new double[] { 1, 3 },
                new double[] { 3, 2 },
                new double[] { 4, 5 },
                new double[] { 4, 4 },
                new double[] { 5, 5 },
                new double[] { 6, 5 }

            };
            KMeans<double> kMeans = new KMeans<double>(2, new EuclideanDistance());
            ClusterizationResult<double> result = kMeans.ExecuteClusterization(data);
            chart.Series = FillSeriesCollection(result.Clusters);
            chart.LegendLocation = LegendLocation.Left;
        }

        public SeriesCollection FillSeriesCollection(IReadOnlyDictionary<double[], IList<double[]>> data) 
        {
            SeriesCollection series = new SeriesCollection();
            int numCluster = 0;
            ScatterSeries centroidsSeries = new ScatterSeries()
            {
                Title = "Centroids",
                Fill = Brushes.Red,
                Values = new ChartValues<ObservablePoint>(),
                PointGeometry = DefaultGeometries.Diamond
            };
            foreach (double[] centroid in data.Keys) 
            {
                numCluster++;

                string title = "Cluster " + numCluster;
                Brush brush = GetRandomBrush();

                ChartValues<ObservablePoint> values = new ChartValues<ObservablePoint>();
                foreach (double[] vector in data[centroid])
                {
                    if (vector.SequenceEqual(centroid)) continue;
                    values.Add(new ObservablePoint(vector[0], vector[1]));
                }

                //Добавление векторов кластера
                series.Add(new ScatterSeries() { Title = title, Fill = brush, Values = values });

                //Добавление координат центроидов
                centroidsSeries.Values.Add(new ObservablePoint(centroid[0], centroid[1]));
            }
            series.Add(centroidsSeries);
            return series;
        }

        public Brush GetRandomBrush() 
        {
            Brush result = Brushes.Transparent;
            
            Random rnd = new Random();
            Type brushesType = typeof(Brushes);
            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rnd.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);

            return result;
        }
    }
}
