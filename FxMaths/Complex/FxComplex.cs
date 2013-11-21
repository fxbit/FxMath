using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxMaths.Complex
{
    /// <summary>
    /// Complex Numbers
    /// </summary>
    [Serializable]
    public class FxComplex<T> where T : struct
    {
        #region Variables
        
        /// <summary>
        /// The real part
        /// </summary>
        public T r;

        /// <summary>
        /// The imaginary part
        /// </summary>
        public T i;

        #endregion



        #region Constractor
        
        /// <summary>
        /// Create a complex with real and imaginary part.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="i"></param>
        public FxComplex(T r, T i)
        {
            this.r = r;
            this.i = i;
        } 

        #endregion



        #region Misc Functions

        public override string ToString()
        {
            return r.ToString() + "+i" + i.ToString();
        }

        public FxComplex<T> Copy()
        {
            return new FxComplex<T>(r, i);
        } 

        #endregion



        #region Operators 

        public static FxComplex<T> operator +(FxComplex<T> x1, FxComplex<T> x2)
        {
            return FxComplex<T>.Add(x1, x2);
        }

        public static FxComplex<T> operator -(FxComplex<T> x1, FxComplex<T> x2)
        {
            return FxComplex<T>.Subtract(x1, x2);
        }

        public static FxComplex<T> operator *(FxComplex<T> x1, FxComplex<T> x2)
        {
            return FxComplex<T>.Multiply(x1, x2);
        }

        public static FxComplex<T> operator *(FxComplex<T> x1, float k)
        {
            return FxComplex<T>.Multiply(x1, k);
        }

        public static FxComplex<T> operator *(FxComplex<T> x1, double k)
        {
            return FxComplex<T>.Multiply(x1, k);
        }

        public static FxComplex<T> operator *(FxComplex<T> x1, int k)
        {
            return FxComplex<T>.Multiply(x1, k);
        }

        public static FxComplex<T> operator *(float k, FxComplex<T> x1)
        {
            return FxComplex<T>.Multiply(x1, k);
        }

        public static FxComplex<T> operator *(double k, FxComplex<T> x1)
        {
            return FxComplex<T>.Multiply(x1, k);
        }

        public static FxComplex<T> operator *(int k, FxComplex<T> x1)
        {
            return FxComplex<T>.Multiply(x1, k);
        } 
        #endregion



        #region Math functions
        public static FxComplex<T> Add(FxComplex<T> x1, FxComplex<T> x2) { throw new NotImplementedException(); }
        public static FxComplex<T> Subtract(FxComplex<T> x1, FxComplex<T> x2) { throw new NotImplementedException(); }
        public static FxComplex<T> Multiply(FxComplex<T> x1, FxComplex<T> x2) { throw new NotImplementedException(); }
        public static FxComplex<T> Multiply(FxComplex<T> x1, float k) { throw new NotImplementedException(); }
        public static FxComplex<T> Multiply(FxComplex<T> x1, double k) { throw new NotImplementedException(); }
        public static FxComplex<T> Multiply(FxComplex<T> x1, int k) { throw new NotImplementedException(); } 
        #endregion

    }

    public class FxComplexF : FxComplex<float>
    {


        #region Constactors

        public FxComplexF(float r, float i) :
            base(r, i)
        {

        }

        public FxComplexF(double r, double i) :
            base((float)r, (float)i)
        {

        }

        public static FxComplexF FromExp(double r, double angle)
        {
            return new FxComplexF(r * Math.Cos(angle), r * Math.Sin(angle));
        } 

        #endregion



        #region Math functions

        public void Add(FxComplexF x)
        {
            this.i += x.i;
            this.r += x.r;
        }

        public static FxComplexF Add(FxComplexF x1, FxComplexF x2)
        {
            return new FxComplexF(x1.r + x2.r, x1.i + x2.i);
        }
        public static FxComplexF Subtract(FxComplexF x1, FxComplexF x2)
        {
            return new FxComplexF(x1.r - x2.r, x1.i - x2.i);
        }

        public static FxComplexF Multiply(FxComplexF x1, FxComplexF x2)
        {
            return new FxComplexF(x1.r * x2.r - x1.i * x2.i, x1.i * x2.r + x1.r * x2.i);
        }

        public static FxComplexF Multiply(FxComplexF x1, float k)
        {
            return new FxComplexF(x1.r * k, x1.i * k);
        }

        public static FxComplexF Multiply(FxComplexF x1, double k)
        {
            return new FxComplexF((float)(x1.r * k), (float)(x1.i * k));
        }

        public static FxComplexF Multiply(FxComplexF x1, int k)
        {
            return new FxComplexF(x1.r * k, x1.i * k);
        } 
        #endregion



        #region Help functions
        public double Norma { get { return Math.Sqrt(r * r + i * i); } }
        public double NormaSquare { get { return (r * r + i * i); } }
        public double Angle { get { return Math.Atan2(i, r); } }
        public double CosAngle { get { return r / Math.Sqrt(r * r + i * i); } }

        public FxComplexF Rotate(double CosAngle, double SinAngle)
        {
            return new FxComplexF(CosAngle * r - SinAngle * i, SinAngle * r + CosAngle * i);
        }

        public FxComplexF Rotate(double Angle)
        {
            var CosAngle = Math.Cos(Angle);
            var SinAngle = Math.Sin(Angle);
            return new FxComplexF(CosAngle * r - SinAngle * i, SinAngle * r + CosAngle * i);
        } 
        #endregion


        #region Operators

        public static FxComplexF operator +(FxComplexF x1, FxComplexF x2)
        {
            return FxComplexF.Add(x1, x2);
        }

        public static FxComplexF operator -(FxComplexF x1, FxComplexF x2)
        {
            return FxComplexF.Subtract(x1, x2);
        }

        public static FxComplexF operator *(FxComplexF x1, FxComplexF x2)
        {
            return FxComplexF.Multiply(x1, x2);
        }

        public static FxComplexF operator *(FxComplexF x1, float k)
        {
            return FxComplexF.Multiply(x1, k);
        }

        public static FxComplexF operator *(FxComplexF x1, double k)
        {
            return FxComplexF.Multiply(x1, k);
        }

        public static FxComplexF operator *(FxComplexF x1, int k)
        {
            return FxComplexF.Multiply(x1, k);
        }

        public static FxComplexF operator *(float k, FxComplexF x1)
        {
            return FxComplexF.Multiply(x1, k);
        }

        public static FxComplexF operator *(double k, FxComplexF x1)
        {
            return FxComplexF.Multiply(x1, k);
        }

        public static FxComplexF operator *(int k, FxComplexF x1)
        {
            return FxComplexF.Multiply(x1, k);
        }
        #endregion
    }
}
