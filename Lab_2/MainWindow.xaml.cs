using System;
using System.Windows;
using System.Windows.Input;

namespace Lab_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public ViewData Data { get; set; }
        public bool IsMeasured { get; set; }
        public bool IsSplined { get; set; }

        public MainWindow()
        {
            try
            {
                Data = new();

                DataContext = this;
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            InitializeComponent();

            Func.ItemsSource = Enum.GetValues(typeof(ClassLibrary.SPf));
        }

        private void MeasuredData_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !Data.InputData.Error1;
        }
        private void MeasuredData_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                textBlock_Der_1rst.Text = "";
                textBlock_Der_2nd.Text = "";
                textBlock_Spl1.Text = "";
                textBlock_Spl2.Text = "";

                Data.UpdateData();

                Data.SplineData.Data.InitGrid();
                Data.SplineData.Data.FillFunctionValues();

                IsMeasured = true;
                IsSplined = false;

                Data.Graphics.ClearCollection();
                Data.Graphics.AddSeries(Data.SplineData.Data.Grid, 
                    Data.SplineData.Data.FunctionValues, "Measured", 0);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Splines_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (!Data.InputData.Error2) && IsMeasured && (!IsSplined);
        }

        private void Splines_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                IsSplined = true;

                double status = Data.Interpolate();

                if (status == 0)
                {
                    textBlock_Der_1rst.Text = $"First spline second derivatives:\n" +
                        $"a :   {Data.SplineData.FirstSplineDerivatives[0]:0.00};   a + h :    " +
                        $"{Data.SplineData.FirstSplineDerivatives[1]:0.00};\n" +
                        $"b :   {Data.SplineData.FirstSplineDerivatives[3]:0.00};   b - h :    " +
                        $"{Data.SplineData.FirstSplineDerivatives[2]:0.00};";
                    textBlock_Der_2nd.Text = $"Second spline second derivatives:\n" +
                        $"a :   {Data.SplineData.SecondSplineDerivatives[0]:0.00};   a + h :   " +
                        $"{Data.SplineData.SecondSplineDerivatives[1]:0.00};\n" +
                        $"b :   {Data.SplineData.SecondSplineDerivatives[3]:0.00};   b - h :   " +
                        $"{Data.SplineData.SecondSplineDerivatives[2]:0.00};";
                    textBlock_Spl1.Text = $"First spline values:\n" +
                        $"a :   {Data.SplineData.FirstCubicSpline[0]:0.00};   a + h :    " +
                        $"{Data.SplineData.FirstCubicSpline[1]:0.00};\n" +
                        $"b :   {Data.SplineData.FirstCubicSpline[Data.SplineData.SplineParams.ArgLengthUniform - 1]:0.00};   b - h :    " +
                        $"{Data.SplineData.FirstCubicSpline[Data.SplineData.SplineParams.ArgLengthUniform - 2]:0.00};";
                    textBlock_Spl2.Text = $"Second spline values:\n" +
                        $"a :   {Data.SplineData.SecondCubicSpline[0]:0.00};   a + h :    " +
                        $"{Data.SplineData.SecondCubicSpline[1]:0.00};\n" +
                        $"b :   {Data.SplineData.SecondCubicSpline[Data.SplineData.SplineParams.ArgLengthUniform - 1]:0.00};   b - h :    " +
                        $"{Data.SplineData.SecondCubicSpline[Data.SplineData.SplineParams.ArgLengthUniform - 2]:0.00};";

                    double[] GridUniform = new double[Data.SplineData.SplineParams.ArgLengthUniform];
                    double step = (Data.SplineData.Data.Interval[1] - Data.SplineData.Data.Interval[0]) / (Data.SplineData.SplineParams.ArgLengthUniform - 1);
                    for (int i = 0; i < Data.SplineData.SplineParams.ArgLengthUniform; i++)
                    {
                        GridUniform[i] = Data.SplineData.Data.Interval[0] + (i * step);
                    }

                    Data.Graphics.ClearCollection();
                    Data.Graphics.AddSeries(GridUniform, Data.SplineData.FirstCubicSpline, " First Spline", 1);
                    Data.Graphics.AddSeries(GridUniform, Data.SplineData.SecondCubicSpline, "Second Spline", 2);
                    Data.Graphics.AddSeries(Data.SplineData.Data.Grid,
                        Data.SplineData.Data.FunctionValues, "Measured", 0);
                }
                else
                {
                    MessageBox.Show($"Error in Interpolate(): {status}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public static class CustomCommands
    {
        public static readonly RoutedUICommand MeasuredData = new
            (
                "MeasuredData",
                "MeasuredData",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D1, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand Splines = new
            (
                "Splines",
                "Splines",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D2, ModifierKeys.Control)
                }
            );
    }
}
