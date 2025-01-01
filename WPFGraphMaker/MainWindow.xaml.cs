using Candlestick_Patterns;
using ScottPlot;
using ScottPlot.Palettes;
using ScottPlot.WPF;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace WPFGraphMaker
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly IDataFromJson _data = new DataFromJson();
        private readonly IMethodsDictionary _dict = new MethodsDictionary();
        private readonly ICandlesticAmountDictionary _candle = new CandlesticAmountDictionary();
        private List<ZigZagObject> _points = new List<ZigZagObject>();
        private List<OhlcvObject> _pointsOhlcv = new List<OhlcvObject>();
        private readonly int _startPoints = 100;
        private readonly int _scrollStep = 10;
        private int _lastPosition = 100;
        private List<int> _foundPatternIndexList = new();
        int counter = 0; 

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

        private void OnMouseWheelEvent(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                DeltaPlus();
            }
            else if (e.Delta < 0)
            {
                DeltaMinus();
            }
        }

        private void DeltaPlus()
        {
            if (_lastPosition - _startPoints < 0)
            {
                _lastPosition = _startPoints;
            }

            _lastPosition = _lastPosition + _scrollStep;
            if (_points.Count != 0)
            {
                DeltaPlusForZigZagPoints();
            }
            else
            {
                DeltaPlusForCandlestickOhlcvPoints();
            }
        }

        private void DeltaMinus()
        {
            _lastPosition = _lastPosition - _scrollStep;
            if (_points.Count != 0)
            {
                DeltaMinusForZigZagPoints();
            }
            else
            {
                DeltaMinusForCandlestickOhlcvPoints();
            }
        }

        private void DeltaMinusForZigZagPoints()
        {
            if (_lastPosition - _startPoints <= 0)
            {
                var yMinStart = GetYMinStartForZigZagPoints();
                var yMaxStart = GetYMaxStartForZigZagPoints();
                _lastPosition = 0;
                OnDataLoadedScale(yMinStart, yMaxStart);
            }
            else
            {
                DoMouseWheelAction();
            }
        }

        private void DeltaMinusForCandlestickOhlcvPoints()
        {
            if (_lastPosition - _startPoints <= 0)
            {
                var yMinStart = GetYMinStartForCandlesticGraph();
                var yMaxStart = GetYMaxStartForCandlesticGraph();
                _lastPosition = 0;
                OnDataLoadedScale(yMinStart, yMaxStart);
            }
            else
            {
                DoMouseWheelActionForCandlestick();
            }
        }

        private void DeltaPlusForCandlestickOhlcvPoints()
        {
            if (_lastPosition + _startPoints > _pointsOhlcv.Count)
            {
                var yMinStart = _pointsOhlcv.Select(x => x.Low).TakeLast(_startPoints).Min();
                var yMaxStart = _pointsOhlcv.Select(x => x.High).TakeLast(_startPoints).Max();
                WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
                WpfPlot1.Plot.Axes.SetLimitsX(_pointsOhlcv.Count - _startPoints, _pointsOhlcv.Count);
                _lastPosition = _pointsOhlcv.Count;
            }
            else
            {
                DoMouseWheelActionForCandlestick();
            }
        }

        private void DeltaPlusForZigZagPoints()
        {
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

        private void DoMouseWheelAction()
        {
            var yMinStart = _points.Select(x => x.Close).Skip(_lastPosition - _startPoints).Take(_startPoints).Min();
            var yMaxStart = _points.Select(x => x.Close).Skip(_lastPosition - _startPoints).Take(_startPoints).Max();

            OnMouseWheelScale(yMinStart, yMaxStart);
        }

        private void DoMouseWheelActionForCandlestick()
        {
            var yMinStart = _pointsOhlcv.Select(x => x.Low).Skip(_lastPosition - _startPoints).Take(_startPoints).Min();
            var yMaxStart = _pointsOhlcv.Select(x => x.High).Skip(_lastPosition - _startPoints).Take(_startPoints).Max();
            OnMouseWheelScale(yMinStart, yMaxStart);
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
                    OnPropertyChanged();
                }
            }
        }

        private int _foundXTimes;

        public int FoundXTimes
        {
            get { return _foundXTimes; }
            set
            {
                if (value != _foundXTimes)
                {
                    _foundXTimes = value;
                    OnPropertyChanged();
                }
            }
        }

        private string GetSuitableGroupByPatternName(string methodName)
        {
            var groupName = _dict.GetCategory().Where(x => x.Key.ToLower() == methodName).ToDictionary().Values.First();
            return groupName;
        }

        private async void OnFindNext(object sender, RoutedEventArgs e)
        {
            if (_points.Count > 0)
            {
                FindNext(_points);
            }
            if (_pointsOhlcv.Count > 0)
            {
                FindNext(_pointsOhlcv);
            }
        }

        private void FindNext(List<OhlcvObject> pointsOhlcv)
        {
            var number = 49;
            if (counter < _foundPatternIndexList.Count && _foundXTimes > 0 && _pointsOhlcv.Count > 0)
            {
                var currentIndex = _foundPatternIndexList[counter];
                if (_foundPatternIndexList[counter] - number <= 0)
                {
                    var yMinStart = GetYMinStartForCandlesticGraph();
                    var yMaxStart = GetYMaxStartForCandlesticGraph();
                    _lastPosition = 0;
                    OnDataLoadedScale(yMinStart, yMaxStart);
                }
                else if (_foundPatternIndexList[counter] + number >= _pointsOhlcv.Count)
                {
                    var yMinStart = _pointsOhlcv.Select(x => x.Close).TakeLast(_startPoints).Min();
                    var yMaxStart = _pointsOhlcv.Select(x => x.Close).TakeLast(_startPoints).Max();
                    WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
                    WpfPlot1.Plot.Axes.SetLimitsX(_pointsOhlcv.Count - _startPoints, _pointsOhlcv.Count);
                    _lastPosition = _pointsOhlcv.Count;
                }
                else
                {
                    var yMinStart = _pointsOhlcv.Select(x => x.Close).Skip(currentIndex - number).Take(_startPoints).Min();
                    var yMaxStart = _pointsOhlcv.Select(x => x.Close).Skip(currentIndex - number).Take(_startPoints).Max();
                    WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
                    WpfPlot1.Plot.Axes.SetLimitsX(currentIndex - number, currentIndex + number);
                    _lastPosition = currentIndex;
                }
                WpfPlot1.Refresh();
            }
            counter += 1;
        }

        private void FindNext(List<ZigZagObject> _points)
        {
            var number = 49;
            if (counter < _foundPatternIndexList.Count && _foundXTimes > 0 && _points.Count > 0)
            {
                var currentIndex = _foundPatternIndexList[counter];
                if (_foundPatternIndexList[counter] - number <= 0)
                {
                    var yMinStart = GetYMinStartForZigZagPoints();
                    var yMaxStart = GetYMaxStartForZigZagPoints();
                    _lastPosition = 0;
                    OnDataLoadedScale(yMinStart, yMaxStart);
                }
                else if (_foundPatternIndexList[counter] + number >= _points.Count)
                {
                    var yMinStart = _points.Select(x => x.Close).TakeLast(_startPoints).Min();
                    var yMaxStart = _points.Select(x => x.Close).TakeLast(_startPoints).Max();
                    WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
                    WpfPlot1.Plot.Axes.SetLimitsX(_points.Count - _startPoints, _points.Count);
                    _lastPosition = _points.Count;
                }
                else
                {
                    var yMinStart = _points.Select(x => x.Close).Skip(currentIndex - number).Take(_startPoints).Min();
                    var yMaxStart = _points.Select(x => x.Close).Skip(currentIndex - number).Take(_startPoints).Max();
                    WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
                    WpfPlot1.Plot.Axes.SetLimitsX(currentIndex - number, currentIndex + number);
                    _lastPosition = currentIndex;
                }
                WpfPlot1.Refresh();
            }
            counter += 1;
        }
             
        private async void OnStartClick(object sender, RoutedEventArgs e)
        {
            _foundPatternIndexList = new();
            FoundXTimes = 0;
            counter = 0;
            WpfPlot1.Plot.Clear();
            var url = "https://gist.github.com/przemyslawbak/92c3d4bba27cfd2b88d0dd916bbdad14/raw/AAL_1min.json";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();

            var patternName = patternNameTextBox.Text == string.Empty ? "BearishBlackClosingMarubozu" : patternNameTextBox.Text; // Bullish3InsideUp
            patternName = patternName.Trim().Replace(" ", "").ToLower();
            var getSuitableMethodByGivenName = GetSuitableGroupByPatternName(patternName);
            
            if (getSuitableMethodByGivenName == "patterns")
            {
                _points = new ();
                _pointsOhlcv = GetCandlestickData(patternName, json);
                ViewCandlestickGraph(_pointsOhlcv);
                MarkCandlestickOnGraph(_pointsOhlcv, patternName);
                FoundXTimes = _pointsOhlcv.Where(x => x.Signal == true).Count();

                var newModel = _pointsOhlcv.Select((x, index) => new ZigZagObject()
                {
                    Signal = x.Signal,
                    IndexOHLCV = index,
                }).ToList();

                var indexesForSignalTrue = newModel.Where(x => x.Signal == true).ToList();
                for (int i = 0; i < indexesForSignalTrue.Count; i++)
                {
                    _foundPatternIndexList.Add(indexesForSignalTrue[i].IndexOHLCV);
                }
            }
            else
            {
                _pointsOhlcv = new();
                _points = GetGraphData(patternName, json, getSuitableMethodByGivenName);
                ViewGraph(_points);
                FoundXTimes = _points.Where(x => x.Signal == true).Count();
                
                var newModel = _points.Select((x, index) => new ZigZagObject()
                {
                    Signal = x.Signal,
                    IndexOHLCV = index,
                }).ToList();

                var indexesForSignalTrue = newModel.Where(x => x.Signal == true).ToList();
                for (int i = 0; i < indexesForSignalTrue.Count; i++)
                {
                    _foundPatternIndexList.Add(indexesForSignalTrue[i].IndexOHLCV);
                }
            }

            if (getSuitableMethodByGivenName == "patterns" && _pointsOhlcv.Count > _startPoints)
            {
                var yMinStart = GetYMinStartForCandlesticGraph();
                var yMaxStart = GetYMaxStartForCandlesticGraph();

                WpfPlot1.Plot.Axes.SetLimitsY((double)yMinStart - 0.1, (double)yMaxStart + 0.1);
                WpfPlot1.Plot.Axes.SetLimitsX(-1, _startPoints);

                WpfPlot1.Refresh();
            }

            else if (_points.Count > _startPoints)
            {
                var yMinStart = GetYMinStartForZigZagPoints();
                var yMaxStart = GetYMaxStartForZigZagPoints(); 

                OnDataLoadedScale(yMinStart, yMaxStart);

                WpfPlot1.Refresh();
            }
            else
            {
                MessageBox.Show("Not enough data loaded");
            }
        }

        private decimal GetYMaxStartForZigZagPoints()
        {
           return _points.Select(x => x.Close).Take(_startPoints).Max();
        }

        private decimal GetYMinStartForZigZagPoints()
        {
            return _points.Select(x => x.Close).Take(_startPoints).Min();
        }

        private decimal GetYMaxStartForCandlesticGraph()
        {
            return _pointsOhlcv.Select(x => x.High).Take(_startPoints).Max();
        }

        private decimal GetYMinStartForCandlesticGraph()
        {
            return _pointsOhlcv.Select(x => x.Low).Take(_startPoints).Min();
        }

        private List<ZigZagObject> GetGraphData(string patternName, string json, string group)
        {
            return _data.GetDataFromJson(patternName, json, group);
        }

        private List<OhlcvObject> GetCandlestickData(string patternName, string json)
        {
            return _data.GetCandlestickDataFromJson(patternName, json);
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
            mp.MarkerStyle.Size = 15F;
            mp.MarkerStyle.OutlineWidth = 2;
            mp.MarkerStyle.LineWidth = 3f;
            mp.MarkerStyle.LineColor = palette.GetColor(color);
        }

        private void ViewCandlestickGraph(List<OhlcvObject> points)
        {
            var ohlcvList = new List<ScottPlot.OHLC>();
            ohlcvList = points.Select(x => new OHLC() { Open = (double)x.Open, High = (double)x.High, Low = (double)x.Low, Close = (double)x.Close,}).ToList();
            var myScatter = WpfPlot1.Plot.Add.Candlestick(ohlcvList);
            myScatter.RisingColor = Colors.Green; 
            myScatter.FallingColor = Colors.Red;
            myScatter.Sequential = true;

            WpfPlot1.Refresh();
        }

        private void MarkCandlestickOnGraph(List<OhlcvObject> _pointsOhlcv, string patternName)
        {
            var candlesNumnbers = _candle.GetCandlestickAmount();
            var singleCandleAmount = candlesNumnbers.Where(x => x.Key.ToLower() == patternName).ToDictionary().Values.First();
            var palette = new ScottPlot.Palettes.Normal();
            var ohlcvList = new List<OhlcvObject>();
            ohlcvList = _pointsOhlcv.Select(x => new OhlcvObject() { Open = x.Open, High = x.High, Low = x.Low, Close = x.Close, Signal = x.Signal, Volume = x.Volume}).ToList();
            var myScatter = WpfPlot1.Plot.Add.Signal(ohlcvList.Select(x => x.Close).ToList());
            for (int i = 0; i < ohlcvList.Count; i++)
            {
                var item = ohlcvList[i];
                var x = i;
                if (item.Signal == true)
                {
                    for (int j = 0; j < singleCandleAmount; j++)
                    {
                        MarkCandleOnChart(i-j, item, palette, 0);
                    }
                }
            }
            myScatter.Color = Colors.Orange;

            WpfPlot1.Refresh();
        }

        private void MarkCandleOnChart(int i, OhlcvObject item, Normal palette, int color)
        {
            var mp = WpfPlot1.Plot.Add.Marker(i, (double)item.Close);
            mp.MarkerShape = MarkerShape.OpenTriangleDown;
            mp.MarkerStyle.Size = 20F;
            mp.MarkerStyle.OutlineWidth = 2;
            mp.MarkerStyle.LineWidth = 3f;
            mp.MarkerStyle.MarkerColor = palette.GetColor(color);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}