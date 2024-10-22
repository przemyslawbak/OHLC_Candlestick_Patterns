using System.Reflection;

namespace Candlestick_Patterns
{
    public class Formations : IFormations
    {
        private readonly List<OhlcvObject> _dataOhlcv;
        private readonly List<ZigZagObject> _data;

        private readonly decimal _priceMovement;
        private readonly decimal _percentageMargin;
        private readonly List<int> _formationsLenght;
        private readonly decimal _minShift;
        private readonly List<double> _advance;
        private List<decimal> _peaksFromZigZag;

        public Formations(List<OhlcvObject> dataOhlcv)
        {
            _dataOhlcv = dataOhlcv;
            _data = GetCloseAndSignalsData(dataOhlcv);

            _priceMovement = 0.002M; 
            _peaksFromZigZag = PeaksFromZigZag();
            _percentageMargin = (decimal) 0.035; 
            _formationsLenght = new List<int>() { 4, 7 };
            _minShift = 2;
            _advance = new List<double>() { 0.10, 0.20 };
        }

        private List<ZigZagObject> BearishDoubleTops()
        {
            var dateList = new List<decimal>();
            var points = _peaksFromZigZag.Select(x => new ZigZagObject() { Close = x, Signal = false }).ToList();
            for (int i = 4; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 4].Close < points[i - 2].Close && points[i - 2].Close > points[i].Close)
                    {
                        var neck = points[i - 2].Close;
                        var change = (Math.Abs((points[i - 1].Close - points[i - 3].Close)) / points[i - 3].Close);
                        if (points[i - 3].Close > neck && points[i - 1].Close > neck && change < _percentageMargin)
                        {
                            for (int x = -4; x < 1; x++)
                            {
                                dateList.Add(points[i + x].Close);
                            }

                            if (dateList.Count > _formationsLenght.Min())
                            {
                                points[i].Signal = true;
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BearishTripleTops() 
        {
            var dateList = new List<decimal>();
            var points = _peaksFromZigZag.Select(x => new ZigZagObject() {Close = x, Signal = false }).ToList();
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 6].Close < points[i - 5].Close && points[i - 4].Close < points[i - 3].Close && points[i - 2].Close < points[i - 3].Close && points[i].Close < points[i - 1].Close)
                    {
                        var neck = new List<decimal>() { points[i - 2].Close, points[i - 4].Close };
                        var change = (Math.Abs((points[i - 1].Close - points[i - 2].Close)) / points[i - 2].Close);
                        var change1 = (Math.Abs((points[i - 3].Close - points[i - 4].Close)) / points[i - 4].Close);
                        var diff1 = Math.Abs((points[i - 1].Close - points[i - 3].Close) / points[i - 3].Close);
                        var diff2 = Math.Abs((points[i - 5].Close - points[i - 3].Close) / points[i - 5].Close);
                        if (Math.Abs((neck.Min() - neck.Average()) / neck.Min()) < _percentageMargin)
                        {
                            if (points[i - 5].Close > neck.Average() && points[i - 3].Close > neck.Average() && change < (decimal)_advance.Max())
                            {
                                if (points[i - 1].Close > neck.Average() && change1 < _percentageMargin && Math.Abs(diff1 - diff2) < _percentageMargin)
                                {
                                    if (points[i - 6].Close < neck.Min() && points[i].Close < neck.Min())
                                    {
                                        for (int x = -6; x < 1; x++)
                                        {
                                            dateList.Add(points[i + x].Close);
                                        }

                                        if (dateList.Count >= _formationsLenght.Max())
                                        {
                                            points[i].Signal = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BullishDoubleBottoms()
        {
            var dateList = new List<decimal>();
            var points = _peaksFromZigZag.Select(x => new ZigZagObject() {Close = x, Signal = false }).ToList();
            for (int i = 4; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 4].Close > points[i - 2].Close && points[i - 2].Close < points[i].Close)
                    {
                        var neck = points[i - 2].Close;
                        var change = (Math.Abs((points[i - 3].Close - points[i - 1].Close)) / points[i - 3].Close);

                        if (Math.Abs((points[i - 3].Close - neck) / points[i - 3].Close) < (decimal)_advance.Max() && Math.Abs((points[i - 1].Close - neck) / neck) < (decimal)_advance.Max() && points[i - 1].Close < neck && change < _percentageMargin)
                        {
                            for (int x = -4; x < 1; x++)
                            {
                                dateList.Add(points[i + x].Close);
                            }

                            if (dateList.Count > _formationsLenght.Min())
                            {
                                points[i].Signal = true;
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BullishTripleBottoms()
        {
            var dateList = new List<decimal>();
            var points = _peaksFromZigZag.Select(x => new ZigZagObject() { Close = x, Signal = false }).ToList();
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i].Close > points[i - 5].Close && points[i - 4].Close > points[i - 3].Close && points[i - 2].Close > points[i - 1].Close)
                    {
                        if (points[i - 6].Close > points[i - 4].Close && points[i - 4].Close < points[i - 2].Close)
                        {
                            var neck = new List<decimal>() { points[i - 4].Close, points[i - 2].Close };
                            var change = (Math.Abs((points[i - 5].Close - points[i - 3].Close)) / points[i - 5].Close);
                            var change2 = (Math.Abs((points[i - 1].Close - points[i - 3].Close)) / points[i - 3].Close);
                            var diff1 = Math.Abs((points[i - 5].Close - neck.Average()) / points[i - 5].Close);
                            var diff2 = Math.Abs((points[i - 1].Close - neck.Average()) / points[i - 1].Close);
                            if (Math.Abs(diff1 - diff2) < _percentageMargin && change < _percentageMargin && change2 < _percentageMargin)
                            {
                                for (int x = -6; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count >= _formationsLenght.Max())
                                {
                                    points[i].Signal = true;
                                }
                            }
                        }
                    }
                }
            }

            return points;
        }
        private List<ZigZagObject> BearishHeadAndShoulders()
        {
            var dateList = new List<decimal>();
            var points = _peaksFromZigZag.Select(x => new ZigZagObject() { Close = x, Signal = false }).ToList();
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 3].Close > points[i - 4].Close && points[i - 4].Close < points[i - 5].Close && points[i - 6].Close < points[i - 5].Close && points[i - 3].Close > points[i - 2].Close && points[i - 2].Close < points[i - 1].Close && points[i - 1].Close > points[i].Close)
                    {
                        var neckline = new List<decimal>() { points[i - 4].Close, points[i - 2].Close };
                        var change = Math.Abs((neckline.Max() - neckline.Min()) / neckline.Min());
                        if (change < _percentageMargin && points[i - 6].Close < neckline.Min() && points[i].Close < neckline.Min())
                        {
                            for (int x = -6; x < 1; x++)
                            {
                                dateList.Add(points[i + x].Close);
                            }

                            if (dateList.Count >= _formationsLenght.Max())
                            {
                                points[i].Signal = true;
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BullishCupAndHandle()
        {
            var dateList = new List<decimal>();
            var points = _peaksFromZigZag.Select(x => new ZigZagObject() { Close = x, Signal = false }).ToList();
            for (int i = 13; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if(points[i].Close> points[i - 1].Close && points[i - 1].Close< points[i - 3].Close && points[i - 5].Close < points[i - 3].Close && points[i - 4].Close > points[i -2].Close && points[i - 4].Close < points[i].Close && points[i - 5].Close< points[i - 3].Close)
                    {
                        if(points[i - 7].Close < points[i - 5].Close && points[i - 7].Close > points[i - 9].Close && points[i - 6].Close > points[i - 8].Close && points[i - 11].Close > points[i - 9].Close && points[i - 13].Close> points[i - 11].Close)
                        {
                            var diff1 = Math.Abs(points[i - 2].Close - points[i - 3].Close);
                            var diff2 = Math.Abs(points[i - 5].Close - points[i - 4 ].Close);
                            var diff3 = Math.Abs(points[i - 5].Close - points[i - 6].Close);
                            var diff4 = Math.Abs(points[i - 7].Close - points[i - 6].Close);
                            var change1 = Math.Abs((points[i - 8].Close - points[i - 10].Close) / points[i - 10].Close);
                            var change2 = Math.Abs((points[i - 7].Close - points[i - 11].Close) / points[i - 11].Close);

                            if (diff2 >= _minShift * diff1 && diff2 > diff3 && diff3 < diff4 && change1 < _percentageMargin && change2 < _percentageMargin)
                            {
                                for (int x = -13; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count >= _minShift * _formationsLenght.Max())
                                {
                                    points[i].Signal = true;
                                }
                            }
                        }
                    
                    }
                }
            }

            return points;
        }
        private List<ZigZagObject> BullishInverseHeadAndShoulders()
        {
            var dateList = new List<decimal>();
            var points = _peaksFromZigZag.Select(x => new ZigZagObject() { Close = x, Signal = false }).ToList();
            for (int i = 6; i < points.Count; i++)
            {
                if (!dateList.Contains(points[i].Close))
                {
                    if (points[i - 3].Close < points[i - 4].Close && points[i - 3].Close < points[i - 2].Close && points[i - 1].Close > points[i - 3].Close && points[i - 5].Close > points[i - 3].Close && points[i].Close > points[i - 2].Close && points[i - 6].Close > points[i - 4].Close)
                    {
                        if (points[i - 4].Close > points[i - 5].Close && points[i - 2].Close > points[i - 1].Close)
                        {
                            var neck = new List<decimal>() { points[i - 4].Close, points[i - 2].Close };
                            var diff1 = Math.Abs((points[i - 2].Close - points[i - 4].Close) / points[i - 4].Close);
                            var diff2 = Math.Abs((points[i - 5].Close - points[i - 1].Close) / points[i - 5].Close);
                            if (diff2 < _percentageMargin && diff1 < _percentageMargin && Math.Abs((points[i - 5].Close - points[i - 3].Close) / points[i - 3].Close) > _percentageMargin)
                            {
                                for (int x = -6; x < 1; x++)
                                {
                                    dateList.Add(points[i + x].Close);
                                }

                                if (dateList.Count >= _formationsLenght.Max())
                                {
                                    points[i].Signal = true;
                                }
                            }

                        }
                    }
                }
            }

            return points;
        }

        private List<decimal> PeaksFromZigZag()
        {
            var change = 0M;
            var zigZagList = new List<decimal>();
            var dataZigZag = _data.Select(x => new ZigZagObject() {Close = x.Close, Signal = false }).ToList();

            for (int i = 1; i < _data.Count; i++)
            {
                if (zigZagList.Count == 0)
                { 
                    change = _data[0].Close * _priceMovement;
                    zigZagList.Add(_data[0].Close);
                }
                else
                {
                    change = zigZagList.Last() * _priceMovement;
                }

                var lastPoint = zigZagList.Last();
                if (_data[i].Close < (lastPoint - change) || _data[i].Close > (lastPoint + change))
                {
                    var point = _data[i].Close;
                    zigZagList.Add((point));
                    change = point * _priceMovement; 
                }
            }

            return GetValleysAndPeaksFromZigZAg(zigZagList);
        }

        private List<decimal>  GetValleysAndPeaksFromZigZAg(List<decimal> zigZagList)
        {
            var allPoints = new List<decimal>();

            bool directionUp = zigZagList[0] <= zigZagList[1];
            for (int i = 1; i < zigZagList.Count - 1; i++)
            {
                if (directionUp && zigZagList[i + 1] < zigZagList[i])
                {
                    allPoints.Add(zigZagList[i]);
                    directionUp = false;
                }
                else if (!directionUp && zigZagList[i + 1] > zigZagList[i])
                {
                    allPoints.Add(zigZagList[i]);
                    directionUp = true;
                }
            }

            return allPoints;
        }

        public List<string> GetFormationsAllMethodNames()
        {
            List<string> methods = new List<string>();
            foreach (MethodInfo item in typeof(Formations).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                methods.Add(item.Name);
            }
            return methods;
        }

        public List<ZigZagObject> GetFormationsSignalsQuantities(string patternName)
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

        public int GetFormationsSignalsCount(string patternName)
        {
            var methodName = patternName.Trim().Replace(" ", "");
            return GetFormationsSignalsQuantities(methodName).Where(x => x.Signal == true).Count();
        }

        private List<ZigZagObject> GetCloseAndSignalsData(List<OhlcvObject> data)
        {
            var dataToShapeZigZag = data.Select(x => new ZigZagObject()
            {
                Signal = x.Signal,
                Close = x.Close,
            }).Reverse().ToList();

            return dataToShapeZigZag;
        }
    }
}
