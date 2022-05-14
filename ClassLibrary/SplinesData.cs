using System.Linq;
using System.Runtime.InteropServices;

namespace ClassLibrary
{
    public class SplinesData
    {
        public SplineParameters SplineParams { get; }
        public MeasuredData Data { get; }
        public double[] FirstCubicSpline { get; set; }
        public double[] SecondCubicSpline { get; set; }
        public double[] FirstSplineDerivatives { get; set; } = new double[4];
        public double[] SecondSplineDerivatives { get; set; } = new double[4];


        public SplinesData(MeasuredData md, SplineParameters sp)
        {
            Data = md;
            SplineParams = sp;
        }

        [DllImport("..\\..\\..\\..\\x64\\Debug\\MKL_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double InterpolateMKL(int length1, int length2, double[] points, 
            double[] func, double[] derivatives1, double[] derivatives2, double[] spline1, double[] spline2);

        public double Interpolate()
        {
            double[] FirstInterpolationResults = new double[2 * SplineParams.ArgLengthUniform];
            double[] SecondInterpolationResults = new double[2 * SplineParams.ArgLengthUniform];

            double error = InterpolateMKL(Data.ArgLength, SplineParams.ArgLengthUniform, Data.Grid, Data.FunctionValues, 
                SplineParams.FirstSplineSecondDerivatives, SplineParams.SecondSplineSecondDerivatives,
                FirstInterpolationResults, SecondInterpolationResults);

            if (error == 0)
            {
                FirstCubicSpline = new double[SplineParams.ArgLengthUniform];
                SecondCubicSpline = new double[SplineParams.ArgLengthUniform];
                for (int i = 0; i < SplineParams.ArgLengthUniform; i++)
                {
                    FirstCubicSpline[i] = FirstInterpolationResults[2 * i];
                    SecondCubicSpline[i] = SecondInterpolationResults[2 * i];
                }

                FirstSplineDerivatives[0] = FirstInterpolationResults[1];
                FirstSplineDerivatives[1] = FirstInterpolationResults[3];
                FirstSplineDerivatives[2] = FirstInterpolationResults[2 * SplineParams.ArgLengthUniform - 3];
                FirstSplineDerivatives[3] = FirstInterpolationResults[2 * SplineParams.ArgLengthUniform - 1];

                SecondSplineDerivatives[0] = SecondInterpolationResults[1];
                SecondSplineDerivatives[1] = SecondInterpolationResults[3];
                SecondSplineDerivatives[2] = SecondInterpolationResults[2 * SplineParams.ArgLengthUniform - 3];
                SecondSplineDerivatives[3] = SecondInterpolationResults[2 * SplineParams.ArgLengthUniform - 1];

                return 0;
            }
            else
            {
                return error;
            }
        }
    }
}
