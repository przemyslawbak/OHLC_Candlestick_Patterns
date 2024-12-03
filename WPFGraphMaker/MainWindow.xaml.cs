using Candlestick_Patterns;
using GraphMaker;
using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.WPF;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WPFGraphMaker
{
    public partial class MainWindow : Window
    {
        private readonly IFiboTester _fiboTester = new FiboTester();
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MyLoadedRoutedEventHandler;
            WpfPlot1.Refresh();
            
        }

        private async void MyLoadedRoutedEventHandler(object sender, RoutedEventArgs e)
        {
            var url = "https://gist.githubusercontent.com/przemyslawbak/c90528453d512a8d85ad2deea5cf6ad2/raw/aapl_us_d.csv";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();

            var patternName = "BullishButterfly";
            List<ZigZagObject> points = GetGraphData(patternName, json);
            ViewGraph(points);

        }

        private void ViewGraph(List<ZigZagObject> points)
        {

            var numbers = new List<int>();
            var newList = new List<double>();

            for (int i = 0; i < points.Count; i++)
            {
                numbers.Add(i);
            }

            var pointsPlot = points.Select(x => x.Close).ToArray();
            var signals = points.Select(x => x.Signal).ToArray();

            foreach (var a in numbers)
            {
                newList.Add(a);
            }

            var numbersArray = numbers.ToArray();
            var myScatter = WpfPlot1.Plot.Add.Scatter(numbersArray, pointsPlot);

            ScottPlot.Palettes.Category20 palette = new();
            for (int i = 0; i < points.Count; i++)
            {
                var item = points[i];
                if (item.Signal == true)
                {
                    var mp = WpfPlot1.Plot.Add.Marker(i, (double)item.Close);
                    mp.MarkerShape = MarkerShape.OpenDiamond;

                    mp.MarkerStyle.FillColor = palette.GetColor(8);
                    mp.MarkerStyle.Size = 1.5F;
                    mp.MarkerStyle.OutlineColor = palette.GetColor(8);
                    mp.MarkerStyle.OutlineWidth = 2;
                    mp.MarkerStyle.LineWidth = 2f;
                    mp.MarkerStyle.LineColor = palette.GetColor(10);
                }
            }

            myScatter.Color = ScottPlot.Colors.Green;
            myScatter.LineWidth = 1;
            myScatter.MarkerSize = 1.2F;
            myScatter.MarkerShape = MarkerShape.OpenSquare;
            myScatter.LinePattern = LinePattern.Solid;
            WpfPlot1.Plot.Axes.AutoScale();

            WpfPlot1.Refresh();

            //double[] dataX = { 1, 2, 3, 4, 5 };
            //double[] dataY = { 1, 4, 9, 16, 25 };
            //WpfPlot1.Plot.Add.Scatter(dataX, dataY);
            ////WpfPlot1.Plot.Axes.SetLimits(0, 5000, 0, 250);
            //WpfPlot1.Plot.Axes.AutoScale();
            //WpfPlot1.Refresh();

        }

        private List<ZigZagObject> GetGraphData(string patternName, string json)
        {
            return _fiboTester.GetDataFromJson(patternName, json);
        }
    }
}