namespace ClassLibrary
{
    public class SplineParameters
    {
        // число узлов равномерной сетки
        public int ArgLengthUniform { get; set; }
        // вторые производные на границах отрезка первого сплайна
        public double[] FirstSplineSecondDerivatives { get; set; }
        // вторые производные на границах отрезка второго сплайна
        public double[] SecondSplineSecondDerivatives { get; set; }

        public SplineParameters(InputParams input)
        {
            ArgLengthUniform = input.UniformLength;
            FirstSplineSecondDerivatives = new double[2] { input.Der1Left, input.Der1Right };
            SecondSplineSecondDerivatives = new double[2] { input.Der2Left, input.Der2Right};
        }

        public void UpdateParams(InputParams input)
        {
            ArgLengthUniform = input.UniformLength;
            FirstSplineSecondDerivatives = new double[2] { input.Der1Left, input.Der1Right };
            SecondSplineSecondDerivatives = new double[2] { input.Der2Left, input.Der2Right };
        }
    }
}
