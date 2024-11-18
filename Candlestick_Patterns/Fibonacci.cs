﻿using Candlestick_Patterns;
using OHLC_Candlestick_Patterns;
using System.Drawing;
using System.Reflection;

namespace Candlestick_Patterns
{
    public class Fibonacci : IFibonacci
    {
        private readonly List<ZigZagObject> _data;

        private readonly decimal _priceMovement1;
        private readonly Fibonacci3DrivePattern _pattern;

        private List<decimal> _peaksFromZigZag;

        public List<OhlcvObject> _dataOhlcv { get; }

        internal Fibonacci(List<OhlcvObject> dataOhlcv)
        {
            _dataOhlcv = dataOhlcv;
            _data = SetPeaksVallyes.GetCloseAndSignalsData(dataOhlcv);
            _peaksFromZigZag = SetPeaksVallyes.PeaksFromZigZag(_data);
            _priceMovement1 = 0.002M;
            _pattern = new Fibonacci3DrivePattern();
        }

        private List<ZigZagObject> Bearish3Drive()
        {
            return _pattern.ThreeDrivePattern("bearish", _peaksFromZigZag, _priceMovement1);
        }

        private List<ZigZagObject> Bullish3Drive()
        {
            return _pattern.ThreeDrivePattern("bullish", _peaksFromZigZag, _priceMovement1);
        }

        public List<string> GetFibonacciAllMethodNames()
        {
            List<string> methods = new List<string>();
            foreach (MethodInfo item in typeof(Fibonacci).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                methods.Add(item.Name);
            }
            return methods;
        }

        public List<ZigZagObject> GetFibonacciSignalsQuantities(string patternName)
        {
            var methodName = patternName.Trim().Replace(" ", "");
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (theMethod != null)
            {
                List<ZigZagObject> result = (List<ZigZagObject>)theMethod.Invoke(this, null);
                return result;
            }
            else
            {
                return _data;
            }
        }

        public int GetFibonacciSignalsCount(string patternName)
        {
            var methodName = patternName.Trim().Replace(" ", "");
            return GetFibonacciSignalsQuantities(methodName).Where(x => x.Signal == true).Count();
        }
    }
}