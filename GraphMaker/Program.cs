using ScottPlot;

var newList = new List<double>();
//var path = "points.txt";
var path = "inverse_head.txt";
List<string> text = File.ReadAllLines(path).ToList();
foreach (var a in text)
{
    newList.Add(Convert.ToDouble(a));
}

var numbers  = new List<int>();
for (int i = 0; i < newList.Count; i++)
{
    numbers.Add(i);
}

var numbersArray = numbers.ToArray();
var plotList = newList.ToArray();
var plt = new Plot();
var barPlot = plt.Add.Scatter(numbersArray, plotList);

var fn = $"Plot_points.png";
plt.SavePng(fn, 1200, 800);