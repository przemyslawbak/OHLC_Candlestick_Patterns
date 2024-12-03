using Candlestick_Patterns;
using System.Windows.Forms.DataVisualization.Charting;
using GraphMaker;

namespace GraphCreator
{
    public class Graph : Form
    {
        private readonly IFiboTester _fiboTester = new FiboTester();
        private readonly System.ComponentModel.IContainer components = null;
        Chart chart1;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Graph());
        }

        public Graph()
        {
            InitializeComponent();
        }

        private void LoadData(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            var series1 = new Series
            {
                Name = "graph",
                Color = Color.Green,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };

            chart1.Series.Add(series1);

            var patternName = "BullishButterfly";
            //var points = GetGraphData(patternName);
            var points = new List<ZigZagObject>()
            {
                new() {Close = 2.5M, Signal = false},
                new() {Close = 3.5M, Signal = true},
                new() {Close = 4.5M, Signal = true},
                new() {Close = 4.5M, Signal = false} ,
            };

            for (int i = 0; i < points.Count; i++)
            {
                var item = points[i];
                series1.Points.AddXY(i, item.Close);
                if (item.Signal == true)
                {
                    series1.Points[i].MarkerStyle = MarkerStyle.Triangle;
                    series1.Points[i].MarkerSize = 5;
                    series1.Points[i].MarkerColor = Color.Red;
                }
            }

            chart1.Invalidate();
        }

        private List<ZigZagObject> GetGraphData(string patternName)
        {
            var points = _fiboTester.GetData(patternName).Result;
            return points;
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ChartArea chartArea1 = new();
            Legend legend1 = new();
            chart1 = new Chart();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            chart1.Dock = DockStyle.Fill;
            chart1.Location = new Point(0, 0);
            chart1.Margin = new Padding(4, 5, 4, 5);
            chart1.Name = "chart1";
            chart1.Size = new Size(379, 403);
            chart1.TabIndex = 0;
            chart1.Click += chart1_Click;
            // 
            // Graph
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(379, 403);
            Controls.Add(chart1);
            Margin = new Padding(4, 5, 4, 5);
            Text = "Graph";
            Load += LoadData;
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            ResumeLayout(false);
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
