using System;
using System.ComponentModel;

namespace ClassLibrary
{
    public class InputParams : IDataErrorInfo
    {
        private int _length;
        public int Length
        {
            get => _length;
            set
            {
                _length = value;
                Error1 = false;
            }
        }

        private int _uniform_length;
        public int UniformLength
        {
            get => _uniform_length;
            set
            {
                _uniform_length = value;
                Error2 = false;
            }
        }

        private double _left;
        public double Left
        {
            get => _left;
            set
            {
                _left = value;
                Error1 = false;
            }
        }
        private double _right;
        public double Right
        {
            get => _right;
            set
            {
                _right = value;
                Error1 = false;
            }
        }
        private double _der1Left;
        public double Der1Left
        {
            get => _der1Left;
            set
            {
                _der1Left = value;
                Error2 = false;
            }
        }

        private double _der1Right;
        public double Der1Right
        {
            get => _der1Right;
            set
            {
                _der1Right = value;
                Error2 = false;
            }
        }
        private double _der2Left;
        public double Der2Left
        {
            get => _der2Left;
            set
            {
                _der2Left = value;
                Error2 = false;
            }
        }
        private double _der2Right;
        public double Der2Right
        {
            get => _der2Right;
            set
            {
                _der2Right = value;
                Error2 = false;
            }
        }

        public SPf Function { get; set; }
        public bool Error1 { get; set; }
        public bool Error2 { get; set; }

        public InputParams(int inLength1, int inLength2, double inLeft, double inRight, 
            SPf inFunction, double der1Left, double der1Right, double der2Left, double der2Right)
        {
            Length = inLength1;
            UniformLength = Length * 10;
            Left = inLeft;
            Right = inRight;
            Function = inFunction;
            Der1Left = der1Left;
            Der1Right = der1Right;
            Der2Left = der2Left;
            Der2Right = der2Right;
        }

        // IDataErrorInfo
        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case "Length":
                        if ((Length < 3) || (Length > 100000))
                        {
                            error = "Length";
                            Error1 = true;
                        }
                        break;
                    case "Right":
                    case "Left":
                        if (Right < Left)
                        {
                            error = "Borders";
                            Error1 = true;
                        }
                        break;
                    case "UniformLength":
                        if ((UniformLength < 3) || (UniformLength > 100000))
                        {
                            error = "UniformLength";
                            Error2 = true;
                        }
                        break;
                    default:
                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
    }
}
