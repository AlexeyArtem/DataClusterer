using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CsvHelper;
using System.Globalization;
using System.IO;
using CsvHelper.Configuration;
using Accord.Statistics.Analysis;

namespace DataClusterer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random _rand;
        private ClusteringMethod _clusteringMethod;
        private List<double[]> _data;

        public MainWindow()
        {
            InitializeComponent();
            _rand = new Random();
            _clusteringMethod = new KMeansPlusPlus(3, new EuclideanDistance());
        }

        private List<double[]> LoadCSVFile() 
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выберите csv-файл данных для кластеризации";
            ofd.Filter = "Excel Files|*.csv";
            
            if (ofd.ShowDialog() == false)
            {
                return null;
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };
            List<double[]> data = new List<double[]>();
            using (var csv = new CsvReader(new StreamReader(ofd.OpenFile()), config))
            {
                csv.Read();
                while (csv.Read())
                {
                    List<double> values = new List<double>();
                    for (int i = 0; csv.TryGetField(i, out double value); i++)
                    {
                        values.Add(value);
                    }
                    data.Add(values.ToArray());
                }
            }

            return data;
        }

        private SeriesCollection FillSeriesCollection(IReadOnlyDictionary<double[], IList<double[]>> clustersData) 
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
            foreach (double[] centroid in clustersData.Keys) 
            {
                numCluster++;

                string title = "Cluster " + numCluster;
                Brush brush = GetRandomBrush();

                ChartValues<ObservablePoint> values = new ChartValues<ObservablePoint>();
                foreach (double[] vector in clustersData[centroid])
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

        private Brush GetRandomBrush() 
        {
            Type brushesType = typeof(Brushes);
            PropertyInfo[] properties = brushesType.GetProperties();

            int random = _rand.Next(properties.Length);
            Brush result = (Brush)properties[random].GetValue(null, null);

            return result;
        }

        private void btSelectionFile_Click(object sender, RoutedEventArgs e)
        {
            _data = LoadCSVFile();
            var result = _clusteringMethod.ExecuteClusterization(DataConverter.ReduceDemension(_data.ToArray(), 2));
            chart.Series = FillSeriesCollection(result.Clusters);
        }
    }
}
