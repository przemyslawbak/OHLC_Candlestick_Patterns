using Candlestick_Patterns;
using GraphMaker;
using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.WPF;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Point = System.Windows.Point;

namespace WPFGraphMaker
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly IFiboTester _fiboTester = new FiboTester();
        private Crosshair Crosshair;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            WpfPlot1 = new WpfPlot
            {
                Width = SystemParameters.WorkArea.Width,
                Height = SystemParameters.WorkArea.Height,
                //or
                /*Width = SystemParameters.PrimaryScreenWidth,
                Height = SystemParameters.PrimaryScreenHeight,*/

            };
            
            var cross = WpfPlot1.Plot.Add.Crosshair(0, 0);

            WpfPlot1.MouseWheel += (s, e) =>
            {
                var scrollViewer = new ScrollViewer
                {
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right,
                    VerticalContentAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Visible,
                    CanContentScroll = true
                };
                var myStackPanel = new StackPanel
                {
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                    VerticalAlignment = System.Windows.VerticalAlignment.Top
                };
                scrollViewer.Content = myStackPanel;
                ((IScrollInfo)myStackPanel).CanVerticallyScroll = true;
                ((IScrollInfo)myStackPanel).CanHorizontallyScroll = true;
                ((IScrollInfo)myStackPanel).ScrollOwner = scrollViewer;
                ((IScrollInfo)myStackPanel).MouseWheelRight();
                //WpfPlot1.Plot.Axes.ZoomIn(1, 1);
                var st = new ScaleTransform();
                if (e.Delta > 0)
                {
                    st.ScaleX *= 2;
                    st.ScaleY *= 2;
                }
                else
                {
                    st.ScaleX /= 2;
                    st.ScaleY /= 2;
                }
                e.Handled = true;
            };

            WpfPlot1.MouseMove += (s, e) =>
            {
                var scrollViewer = new ScrollViewer
                {
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch,
                    VerticalContentAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                    CanContentScroll = true
                };

                var viewer = new ScrollViewer();
                scrollViewer.Content = viewer;
                ScrollViewer.SetCanContentScroll(viewer, true);
                ScrollViewer.SetHorizontalScrollBarVisibility(viewer, ScrollBarVisibility.Auto);

                var myStackPanel = new StackPanel
                {
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    VerticalAlignment = System.Windows.VerticalAlignment.Top
                };
                scrollViewer.Content = myStackPanel;

                Grid.SetRow(scrollViewer, 1);
                Point p = e.GetPosition(WpfPlot1);
                ScottPlot.Pixel mousePixel = new(p.X * WpfPlot1.DisplayScale, p.Y * WpfPlot1.DisplayScale);
                ScottPlot.Coordinates coordinates = WpfPlot1.Plot.GetCoordinates(mousePixel);
                cross.Position = coordinates;
                this.SizeToContent = SizeToContent.WidthAndHeight;

                WpfPlot1.Refresh();
            };

            WpfPlot1.MouseEnter += (s, e) =>
            {
                var scrollViewer = new ScrollViewer
                {
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left,
                    VerticalContentAlignment = System.Windows.VerticalAlignment.Top,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                    CanContentScroll = true
                };
                var st = new ScaleTransform();
                Pixel mousePixel = new();
                Coordinates mouseCoordinates = WpfPlot1.Plot.GetCoordinates(mousePixel);
                cross.Position = mouseCoordinates;
                WpfPlot1.Plot.ScaleFactor = 3;
                scrollViewer.ScrollToTop();
                WpfPlot1.Plot.Axes.SetLimitsY(200, 240);
                WpfPlot1.Plot.Axes.SetLimitsX(-1, 40);
                WpfPlot1.Refresh();
            };
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
            WpfPlot1.Plot.Axes.Margins(.15, .15);
            WpfPlot1.Plot.Axes.AutoScale();
            //WpfPlot1.Plot.Axes.SetLimitsY(0,(double)pointsPlot.Max());
            //WpfPlot1.Plot.Axes.SetLimitsX(0,(double)pointsPlot.Length);
            WpfPlot1.Refresh();
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

            var patternName = patternNameTextBox.Text == string.Empty ? "BullishButterfly" : patternNameTextBox.Text;
            List<ZigZagObject> points = GetGraphData(patternName, json);
            ViewGraph(points);
            WpfPlot1.Refresh();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}