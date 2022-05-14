using System;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace Lab_2
{
    public class ChartData
    {
        public SeriesCollection Plot { get; set; }
        public Func<double, string> Formatter { get; set; }

        public ChartData(double[] inLables)
        {
            Plot = new SeriesCollection();

            Formatter = value => value.ToString("F4");
        }

        public void AddSeries(double[] points, double[] values, string title, int mode)
        {
            ChartValues<ObservablePoint> Values = new ChartValues<ObservablePoint>();
            for (int i = 0; i < values.Length; i++)
            {
                Values.Add(new(points[i], values[i]));
            }

            if(mode == 0)
            {
                Plot.Add(new ScatterSeries
                {
                    Title = title,
                    Values = Values,
                    Fill = Brushes.SeaGreen,
                    MinPointShapeDiameter = 5,
                    MaxPointShapeDiameter = 5
                });
            }
            else if(mode == 1)
            {
                Plot.Add(new LineSeries
                {
                    Title = title,
                    Values = Values,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Aquamarine,
                    PointGeometry = null, 
                    LineSmoothness = 0
                });
            }
            else if (mode == 2)
            {
                Plot.Add(new LineSeries
                {
                    Title = title,
                    Values = Values,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Coral,
                    PointGeometry = null,
                    LineSmoothness = 0
                });
            }
        }

        public void ClearCollection()
        {
            Plot.Clear();
        }
    }
}
