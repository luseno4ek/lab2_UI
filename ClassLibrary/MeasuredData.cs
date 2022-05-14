using System;
using System.Collections.ObjectModel;

namespace ClassLibrary
{
    public class MeasuredData
    {
        // число узлов неравномерной сетки
        public int ArgLength { get; set; }
        // концы отрезка
        public double[] Interval { get; set; }
        // функция, которую надо интерполировать
        public SPf Function { get; set; }
        // массив узлов неравномерной сетки
        public double[] Grid { get; set; }
        // массив "измеренных" значений в узлах неравномерной сетки (Grid)
        public double[] FunctionValues { get; set; }
        // строки для вывода информации на экран
        public ObservableCollection<string> DataForListBox { get; set; }


        public MeasuredData(InputParams input)
        {
            ArgLength = input.Length;
            Interval = new double[2] { input.Left, input.Right };
            Function = input.Function;

            DataForListBox = new();
        }

        public void UpdateData(InputParams input)
        {
            ArgLength = input.Length;
            Interval = new double[2] { input.Left, input.Right };
            Function = input.Function;
        }


        public void InitGrid()
        {
            Grid = new double[ArgLength];

            Grid[0] = Interval[0];
            Grid[ArgLength - 1] = Interval[1];

            var rand = new Random();

            for (int i = 1; i < ArgLength - 1; i++)
            {
                double randval = Interval[0]; 
                while(randval <= Interval[0])
                {
                    if (Interval[0] < 0)
                    {
                        randval = Interval[1] * rand.NextDouble() * Math.Pow(-1, i);
                    }
                    else
                    {
                        randval = Interval[1] * rand.NextDouble();
                    }
                }
                Grid[i] = randval;
            }

            Array.Sort(Grid);
        }

        public void FillFunctionValues()
        {
            FunctionValues = new double[ArgLength];

            if (Function == SPf.Cosine)
            {
                for (int i = 0; i < ArgLength; i++)
                {
                    FunctionValues[i] = Math.Cos(Grid[i] * Math.PI / 180);
                }
            }
            else if (Function == SPf.Cubic)
            {
                for (int i = 0; i < ArgLength; i++)
                {
                    FunctionValues[i] = Math.Pow(Grid[i], 3) + 2 * Math.Pow(Grid[i], 2);
                }
            }
            else if (Function == SPf.Random)
            {
                var rand = new Random();
                for (int i = 0; i < ArgLength; i++)
                {
                    FunctionValues[i] = 15 * rand.NextDouble();
                }
            }

            DataForListBox.Clear();
            for (int i = 0; i < ArgLength; i++)
            {
                DataForListBox.Add($"Point: {Grid[i]}\nFunction Value: {FunctionValues[i]}\n");
            }
        }
    }
}
