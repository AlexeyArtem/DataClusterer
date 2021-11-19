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

    enum TypeMethod 
    {
        KMeans,
        KMeansPlusPlus,
        CMeans
    }

    enum TypeMeasure 
    {
        EuclideanDistance
    }

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
            cbMethodSelection.ItemsSource = Enum.GetValues(typeof(TypeMethod)).Cast<TypeMethod>();
            cbMeasureSelection.ItemsSource = Enum.GetValues(typeof(TypeMeasure)).Cast<TypeMeasure>();
        }

        private List<double[]> LoadCSVFile(out string nameFile) 
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выберите csv-файл данных для кластеризации";
            ofd.Filter = "Excel Files|*.csv";
            
            if (ofd.ShowDialog() == false)
            {
                nameFile = "";
                return null;
            }

            nameFile = ofd.SafeFileName;

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

        private void SetClusteringMethod() 
        {
            MeasureSimilarity measureSimilarity;

            Enum.TryParse(cbMeasureSelection.SelectedItem.ToString(), out TypeMeasure typeMeasure);
            switch (typeMeasure) 
            {
                case TypeMeasure.EuclideanDistance:
                    measureSimilarity = new EuclideanDistance();
                    break;
                default:
                    measureSimilarity = new EuclideanDistance();
                    break;
            }

            Enum.TryParse(cbMethodSelection.SelectedItem.ToString(), out TypeMethod typeMethod);
            int amountClusters = (int)udAmountClusters.Value;
            switch (typeMethod)
            {
                case TypeMethod.KMeans:
                    _clusteringMethod = new KMeans(amountClusters, measureSimilarity);
                    break;
                case TypeMethod.CMeans:
                    _clusteringMethod = new CMeans(amountClusters, measureSimilarity);
                    break;
                case TypeMethod.KMeansPlusPlus:
                    _clusteringMethod = new KMeansPlusPlus(amountClusters, measureSimilarity);
                    break;
            }
        }

        private void btSelectionFile_Click(object sender, RoutedEventArgs e)
        {
            _data = LoadCSVFile(out string nameFile);
            tbNameFile.Text = nameFile;
        }

        private void btExecuteClusterization_Click(object sender, RoutedEventArgs e)
        {
            if (_data == null) 
            {
                MessageBox.Show("Чтобы выполнить кластеризацию, выберите файл с данными", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            SetClusteringMethod();
            var result = _clusteringMethod.ExecuteClusterization(DataConverter.ReduceDemension(_data.ToArray(), 2));
            chart.Series = FillSeriesCollection(result.Clusters);
        }

        private void btRefreshColor_Click(object sender, RoutedEventArgs e)
        {
            SeriesCollection series = chart.Series;
            if (series == null) return;

            foreach (var s in series.Where(s => s.Title != "Centroid")) 
            {
                if (s is ScatterSeries scatter)
                {
                    scatter.Fill = GetRandomBrush();
                }
            }
        }
    }
}
