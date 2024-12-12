using Candlestick_Patterns;
using GraphMaker;
using ScottPlot;
using ScottPlot.Palettes;
using ScottPlot.Plottables;
using ScottPlot.WPF;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace WPFGraphMaker
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly IFiboTester _fiboTester = new FiboTester();
        private List<ZigZagObject> _points = new List<ZigZagObject>();
        private readonly int _startPoints = 100;
        private readonly int _scrollStep = 10;
        private int _lastPosition = 100;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            WpfPlot1 = new WpfPlot
            {
                Width = mainWin.Width - 30,
                Height = mainWin.Height - 80,
            };

            mainWin.SizeChanged += OnSizeChangedEvent;

            WpfPlot1.MouseWheel += OnMouseWheelEvent;
        }

        private void DoMouseWheelAction()
        {
            var yMinStart = _points.Select(x => x.Close).Skip(_lastPosition - _startPoints).Take(_startPoints).Min();
            var yMaxStart = _points.Select(x => x.Close).Skip(_lastPosition - _startPoints).Take(_startPoints).Max();

            OnMouseWheelScale(yMinStart, yMaxStart);

        }

        private void OnMouseWheelEvent(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (_lastPosition - _startPoints < 0)
                {
                    _lastPosition = _startPoints;
                }
                _lastPosition = _lastPosition + _scrollStep;

                if (_lastPosition + _startPoints > _points.Count)
                {
                    var yMinStart = _points.Select(x => x.Close).TakeLast(_startPoints).Min();
                    var yMaxStart = _points.Select(x => x.Close).TakeLast(_startPoints).Max();
                    WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
                    WpfPlot1.Plot.Axes.SetLimitsX(_points.Count - _startPoints, _points.Count);
                    _lastPosition = _points.Count;
                }
                else 
                {

                    DoMouseWheelAction();
                }
            }
            else if (e.Delta < 0)
            {
                _lastPosition = _lastPosition - _scrollStep;
                if (_lastPosition - _startPoints <= 0)
                {
                    var yMinStart = _points.Select(x => x.Close).Take(_startPoints).Min();
                    var yMaxStart = _points.Select(x => x.Close).Take(_startPoints).Max();
                    _lastPosition = 0;
                    OnDataLoadedScale(yMinStart, yMaxStart);
                }
                else
                {
                    DoMouseWheelAction();
                }
            }
        }

        private void OnMouseWheelScale(decimal yMinStart, decimal yMaxStart)
        {
            WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
            WpfPlot1.Plot.Axes.SetLimitsX(_lastPosition - _startPoints, _lastPosition);

            WpfPlot1.Refresh();
        }

        private void OnDataLoadedScale(decimal yMinStart, decimal yMaxStart)
        {
            WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
            WpfPlot1.Plot.Axes.SetLimitsX(-1, _startPoints);

            WpfPlot1.Refresh();
        }

        private void OnSizeChangedEvent(object sender, SizeChangedEventArgs e)
        {
            WpfPlot1.Width = e.NewSize.Width - 30;
            WpfPlot1.Height = e.NewSize.Height - 80;
            WpfPlot1.Refresh();
        }

        public WpfPlot WpfPlot1 { get; set; }

        private string _patternName;
        public string PatternName
        {
            get { return _patternName; }
            set
            {
                if (value != _patternName)
                {
                    _patternName = value;
                    OnPropertyChanged(PatternName);
                }
            }
        }

        private void ViewGraph(List<ZigZagObject> points)
        {
            var xAxesNumbers = new List<double>();
            for (int i = 0; i < points.Count; i++)
            {
                xAxesNumbers.Add((double)i);
            }

            ScottPlot.Palettes.Category20 palette = new();
            var numbersArray = xAxesNumbers.ToArray();
            var pointsPlot = points.Select(x => x.Close).ToArray();
            var myScatter = WpfPlot1.Plot.Add.Scatter(numbersArray, pointsPlot);

            for (int i = 0; i < points.Count; i++)
            {
                var item = points[i];
                if (item.Signal == true)
                {
                    MarkSingalOnChart(i, item, palette, 2);
                }
                if (item.Initiation == true)
                {
                    MarkSingalOnChart(i, item, palette, 8);
                }
            }

            myScatter.Color = Colors.Green;
            myScatter.LineWidth = 1;
            myScatter.MarkerSize = 1.2F;
            myScatter.MarkerShape = MarkerShape.OpenSquare;
            myScatter.LinePattern = LinePattern.Solid;
            WpfPlot1.Refresh();
        }

        private void MarkSingalOnChart(int i, ZigZagObject item, Category20 palette, int color)
        {
            var mp = WpfPlot1.Plot.Add.Marker(i, (double)item.Close);
            mp.MarkerShape = MarkerShape.OpenDiamond;
            //mp.MarkerStyle.FillColor = palette.GetColor(color);
            mp.MarkerStyle.Size = 15F;
            //mp.MarkerStyle.OutlineColor = palette.GetColor(8);
            mp.MarkerStyle.OutlineWidth = 2;
            mp.MarkerStyle.LineWidth = 3f;
            mp.MarkerStyle.LineColor = palette.GetColor(color);
        }

        private List<ZigZagObject> GetGraphData(string patternName, string json)
        {
            return _fiboTester.GetDataFromJson(patternName, json);
        }

        private async void OnStartClick(object sender, RoutedEventArgs e)
        {
            var url = "https://gist.github.com/przemyslawbak/92c3d4bba27cfd2b88d0dd916bbdad14/raw/AAL_1min.json";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();

            var patternName = patternNameTextBox.Text == string.Empty ? "Bearish3Extension" : patternNameTextBox.Text;
            _points = GetGraphData(patternName, json);
            ViewGraph(_points);


            if (_points.Count > _startPoints)
            {
                var yMinStart = _points.Select(x => x.Close).Take(_startPoints).Min();
                var yMaxStart = _points.Select(x => x.Close).Take(_startPoints).Max();

                OnDataLoadedScale(yMinStart, yMaxStart);

                WpfPlot1.Refresh();
            }
            else
            {
                MessageBox.Show("Not enough data loaded");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task<List<ZigZagObject>> GetPoints()
        {
            var url = "https://gist.github.com/przemyslawbak/92c3d4bba27cfd2b88d0dd916bbdad14/raw/AAL_1min.json";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();

            var patternName = patternNameTextBox.Text == string.Empty ? "BullishButterfly" : patternNameTextBox.Text;
            List<ZigZagObject> points = GetGraphData(patternName, json);
            return points;
        }
    }
}