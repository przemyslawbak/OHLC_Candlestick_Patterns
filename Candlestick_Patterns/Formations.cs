using System.Reflection;

namespace Candlestick_Patterns
{
    public class Formations : IFormations
    {
        private readonly List<ZigZagObject> _data;
        private readonly decimal _priceMovement;
        private readonly decimal _percentageMargin;
        private readonly List<int> _formationsLenght;
        private readonly decimal _minShift;
        private readonly List<double> _advance;
        private List<(decimal point, DateTime utc)> _peaksFromZigZag;

        public Formations(List<ZigZagObject> data)
        {
            _data = data;
            _priceMovement = 0.002M; 
            _peaksFromZigZag = PeaksFromZigZag();
            _percentageMargin = (decimal) 0.035; 
            _formationsLenght = new List<int>() { 4, 7 };
            _minShift = 2;
            _advance = new List<double>() { 0.10, 0.20 };
        }

        private List<ZigZagObject> BearishDoubleTops()
        {
            var count = 0;
            var dateList = new List<DateTime>();
            var points = _peaksFromZigZag.Select(x => new ZigZagObject() { Date = x.utc, Close = x.point, Signal = false }).ToList();
            for (int i = 2; i < points.Count-2; i++)
            {
                if (!dateList.Contains(points[i].Date))
                {
                    if (points[i - 2].Close < points[i].Close && points[i].Close > points[i + 2].Close)
                    {
                        var neck = points[i].Close;
                        var change = (Math.Abs((points[i - 1].Close - points[i + 1].Close)) / points[i - 1].Close);
                        if (points[i - 1].Close > neck && points[i + 1].Close > neck && change < _percentageMargin)
                        {
                            for (int x = -2; x < 3; x++)
                            {
                                dateList.Add(points[i + x].Date);
                            }

                            if (dateList.Count >= _formationsLenght.Min())
                            {
                                points[i].Signal = true;
                                count++;
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<ZigZagObject> BearishTripleTops() 
        {
            var count = 0;
            var dateList = new List<DateTime>();
            var points = _peaksFromZigZag.Select(x => new ZigZagObject() { Date = x.utc, Close = x.point, Signal = false }).ToList();
            for (int i = 3; i < points.Count - 3; i++)
            {
                if (!dateList.Contains(points[i].Date))
                {
                    if (points[i - 2].Close < points[i].Close && points[i - 1].Close > points[i].Close && points[i+1].Close> points[i].Close && points[i + 3].Close > points[i+2].Close)
                    {
                        var neck = new List<decimal>() { points[i].Close, points[i - 3].Close, points[i + 2].Close };
                        var change = (Math.Abs((points[i - 1].Close - points[i + 1].Close)) / points[i - 1].Close);
                        var change1 = (Math.Abs((points[i + 1].Close - points[i + 3].Close)) / points[i + 1].Close);
                        var diff1 = Math.Abs((points[i - 1].Close - points[i].Close) / points[i - 1].Close);
                        var diff2 = Math.Abs((points[i + 1].Close - points[i+2].Close) / points[i + 1].Close);
                        if (Math.Abs((neck.Min() - neck.Average())/neck.Min()) < _percentageMargin)
                        {
                            if (points[i - 1].Close > neck.Average() && points[i + 1].Close > neck.Average() && change < (decimal) _advance.Max())
                            {
                                if (points[i + 3].Close > neck.Average() && change1 < _percentageMargin && Math.Abs(diff1 - diff2) < _percentageMargin)
                                {
                                    if (Math.Abs((points[i - 3].Close - points[i + 2].Close) / points[i - 3].Close) < _percentageMargin)
                                    {
                                        for (int x = -3; x < 4; x++)
                                        {
                                            dateList.Add(points[i + x].Date);
                                        }

                                        if (dateList.Count >= _formationsLenght.Max())
                                        {
                                            points[i].Signal = true;
                                            count++;
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
            var dateList = new List<DateTime>();
            var points = _peaksFromZigZag.Select(x => new ZigZagObject() { Date = x.utc, Close = x.point, Signal = false }).ToList();
            for (int i = 2; i < points.Count - 2; i++)
            {
                if (!dateList.Contains(points[i].Date))
                {
                    if (points[i - 2].Close > points[i].Close && points[i].Close < points[i + 2].Close)
                    {
                        var neck = points[i].Close;
                        var change = (Math.Abs((points[i - 1].Close - points[i + 1].Close)) / points[i - 1].Close);

                        if (Math.Abs((points[i - 1].Close - neck)/ points[i - 1].Close ) < (decimal)_advance.Max() && Math.Abs( (points[i + 1].Close - neck)/ neck) < (decimal)_advance.Max() && points[i + 1].Close < neck && change < _percentageMargin )
                        {
                            for (int x = -2; x < 3; x++)
                            {
                                dateList.Add(points[i + x].Date);
                            }

                            if (dateList.Count >= _formationsLenght.Min())
                            {
                                points[i].Signal = true;
                            }
                        }
                    }
                }
            }

            return points;
        }

        private List<(decimal point, DateTime utc)> PeaksFromZigZag()
        {
            var change = 0M;
            var zigZagList = new List<(decimal point, DateTime utc)>();
            var data = _data.Select(x => new ZigZagObject() { Date = x.Date,  Close = x.Close, Signal = false }).ToList();

            for (int i = 1; i < _data.Count; i++)
            {
                if (zigZagList.Count == 0)
                { 
                    change = _data[0].Close * _priceMovement;
                    zigZagList.Add((_data[0].Close, _data[0].Date));
                }
                else
                {
                    change = zigZagList.Last().point * _priceMovement;
                }

                var lastPoint = zigZagList.Last().point;
                if (_data[i].Close < (lastPoint - change) || _data[i].Close > (lastPoint + change))
                {
                    var point = _data[i].Close;
                    var utc = _data[i].Date;
                    zigZagList.Add((point,utc));
                    change = point * _priceMovement; 
                }
            }

            return GetValleysAndPeaksFromZigZAg(zigZagList);
        }

        private List<(decimal point, DateTime utc)>  GetValleysAndPeaksFromZigZAg(List<(decimal point, DateTime utc)> zigZagList)
        {
            var Allpoints = new List<(decimal point, DateTime utc)>();

            bool directionUp = zigZagList[0].point <= zigZagList[1].point;
            for (int i = 1; i < zigZagList.Count - 1; i++)
            {
                if (directionUp && zigZagList[i + 1].point < zigZagList[i].point)
                {
                    Allpoints.Add((zigZagList[i].point, zigZagList[i].utc));
                    directionUp = false;
                }
                else if (!directionUp && zigZagList[i + 1].point > zigZagList[i].point)
                {
                    Allpoints.Add((zigZagList[i].point, zigZagList[i].utc));
                    directionUp = true;
                }
            }

            return Allpoints;
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
    }
}
