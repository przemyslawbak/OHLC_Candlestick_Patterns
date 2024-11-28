using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Candlestick_Patterns;
using OHLC_Candlestick_Patterns;
using ScottPlot;

namespace GraphMaker
{
    internal interface IFiboTester
    {
        void ShowOnGraph(List<OhlcvObject> dataOhlcv, string patternName);
    }

    internal class FiboTester : IFiboTester
    {
        IFibonacci _fibonacci;

        void IFiboTester.ShowOnGraph(List<OhlcvObject> dataOhlcv, string patternName)
        {
            _fibonacci = new Fibonacci(dataOhlcv);
            var signalList = _fibonacci.GetFibonacciSignalsList(patternName);
            GetGraph(signalList);
        }

        static void GetGraph(List<ZigZagObject> points)
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
                    //mp.MarkerStyle.OutlineWidth = 2;
                    //mp.MarkerStyle.LineWidth = 2f;
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
            var fn = $"Plot_points.png";
           // plt.SavePng(fn, 10000, 1000);
            fn = $"Plot_points.svg";
            plt.SaveSvg(fn, 50000, 2000);

        }
    }
}


