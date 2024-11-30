using Candlestick_Patterns;
using Newtonsoft.Json;
using ScottPlot;

namespace GraphMaker
{
    public interface IFiboTester
    {
        void ShowOnGraph(List<OhlcvObject> dataOhlcv, string patternName);
        List<ZigZagObject> GetGraphData(string patternName);
        Task<List<ZigZagObject>> GetPoints(string patternName);
    }
    
    public class FiboTester : IFiboTester
    {
        IFibonacci _fibonacci;
        public async Task<List<ZigZagObject>> GetPoints(string patternName)
        {
            string json = string.Empty;
            ISignals _signals = new Signals();
            IFiboTester _fiboTester = new FiboTester();
            IFibonacci _fibonacci;

            var client = new HttpClient();
            var url = "https://gist.githubusercontent.com/przemyslawbak/c90528453d512a8d85ad2deea5cf6ad2/raw/aapl_us_d.csv";

            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                using (HttpContent content = response.Content)
                {
                    json = content.ReadAsStringAsync().Result;
                }
            }

            var dataOhlcv = JsonConvert.DeserializeObject<List<OhlcvObject>>(json).Select(x => new OhlcvObject()
            {
                Open = x.Open,
                High = x.High,
                Low = x.Low,
                Close = x.Close,
                Volume = x.Volume,
            }).Reverse().ToList();


            _fibonacci = new Fibonacci(dataOhlcv);
            var signalList = _fibonacci.GetFibonacciSignalsList(patternName);
            return signalList;
        }

        void IFiboTester.ShowOnGraph(List<OhlcvObject> dataOhlcv, string patternName)
        {
            _fibonacci = new Fibonacci(dataOhlcv);
            var signalList = _fibonacci.GetFibonacciSignalsList(patternName);
            GetGraph(signalList, patternName);
        }

        static void GetGraph(List<ZigZagObject> points, string patternName)
        {
            var plt = new Plot();
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
            var myScatter = plt.Add.Scatter(numbersArray, pointsPlot);

            ScottPlot.Palettes.Category20 palette = new();
            for (int i = 0; i < points.Count; i++)
            {
                var item = points[i];
                if (item.Signal == true)
                {
                    var mp = plt.Add.Marker(i, (double)item.Close);
                    mp.MarkerShape = MarkerShape.OpenDiamond;
                    
                    mp.MarkerStyle.FillColor = palette.GetColor(8);
                    mp.MarkerStyle.Size = 1.5F;
                    mp.MarkerStyle.OutlineColor = palette.GetColor(8);
                    mp.MarkerStyle.OutlineWidth = 2;
                    mp.MarkerStyle.LineWidth = 2f;
                    mp.MarkerStyle.LineColor = palette.GetColor(10);
                }
            }
           
            myScatter.Color = Colors.Green;
            myScatter.LineWidth = 1;
            myScatter.MarkerSize = 1.2F;
            myScatter.MarkerShape = MarkerShape.OpenSquare;
            myScatter.LinePattern = LinePattern.Solid;

            //plt.Axes.AutoScale();
            //myScatter.MinRenderIndex = 0;
            //myScatter.MaxRenderIndex = points.Count;

            // save
            var fn = $"Data\\graph_{patternName}.svg";
            plt.SaveSvg(fn, 50000, 2000);

        }

        public List<ZigZagObject> GetGraphData(string patternName)
        {
            var signalList = GetPoints(patternName);
            List<ZigZagObject> points = signalList.Result;
            return points;
        }
    }
}


