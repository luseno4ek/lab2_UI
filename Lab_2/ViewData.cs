using System;
using System.ComponentModel;
using ClassLibrary;

namespace Lab_2
{
    public class ViewData
    {
        public InputParams InputData { get; set; }
        public SplinesData SplineData { get; set; }
        public ChartData Graphics { get; set; }

        public ViewData()
        {
            InputData = new(20, 20 * 10, -200, 200, SPf.Cosine, 10, 10, -10, -10);
            InputData.Error1 = false;
            InputData.Error2 = false;

            SplineData = new(new(InputData), new(InputData));

            Graphics = new(SplineData.Data.Grid);
        }

        public void UpdateData()
        {
            SplineData.Data.UpdateData(InputData);
            SplineData.SplineParams.UpdateParams(InputData);
        }

        public double Interpolate()
        {
            return SplineData.Interpolate();
        }
    }
}
