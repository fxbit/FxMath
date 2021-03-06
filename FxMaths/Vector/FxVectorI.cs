﻿using FxMaths.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.Vector
{
    public class FxVectorI : FxVector<int>
    {


        #region Constructors

        public FxVectorI( int Size )
            : base( Size )
        {
            // init the datas
            for ( int i=0; i < Size; i++ ) {
                this.Data[i] = 0;
            }
        }

        public FxVectorI( int Size, int value )
            : base( Size )
        {
            // init the datas
            for ( int i=0; i < Size; i++ ) {
                this.Data[i] = value;
            }
        }

        protected override FxVector<int> AllocateVector( int Size )
        {
            return new FxVectorI( Size );
        }

        #endregion


        #region Math Functions

        #region Add

        protected override void DoAdd( FxVector<int> other, FxVector<int> result )
        {
            // pass all the data and add the new data
            for ( int i=0; i < Size; i++ ) {
                result[i] = other[i] + Data[i];
            }
        }

        protected override void DoAdd( FxVector<int> other )
        {
            // pass all the data and add the new data
            for ( int i=0; i < Size; i++ ) {
                Data[i] += other[i];
            }
        }

        protected override void DoAdd(double value)
        {
            int ivalue = (int)value;
            // pass all the data and add the value
            for (int i = 0; i < Size; i++)
            {
                Data[i] += ivalue;
            }
        }

        protected override void DoAdd( float value )
        {
            int ivalue = (int)value;
            // pass all the data and add the value
            for ( int i=0; i < Size; i++ ) {
                Data[i] += ivalue;
            }
        }

        protected override void DoAdd( int value )
        {
            // pass all the data and add the value
            for ( int i=0; i < Size; i++ ) {
                Data[i] += value;
            }
        }
        #endregion


        #region Subtract

        protected override void DoSubtract( FxVector<int> other, FxVector<int> result )
        {
            // pass all the data and Subtract the new data
            for ( int i=0; i < Size; i++ ) {
                result[i] = Data[i] - other[i];
            }
        }

        protected override void DoSubtract( FxVector<int> other )
        {
            // pass all the data and Subtract the new data
            for ( int i=0; i < Size; i++ ) {
                Data[i] -= other[i];
            }
        }

        protected override void DoSubtract( float value )
        {
            // pass all the data and Subtract the value
            for ( int i=0; i < Size; i++ ) {
                Data[i] -= (int)value;
            }
        }

        protected override void DoSubtract( int value )
        {
            // pass all the data and Subtract the value
            for ( int i=0; i < Size; i++ ) {
                Data[i] -= value;
            }
        }
        #endregion


        #region Multiply


        #region Pointwise

        public override void MultiplyPointwise( FxVector<int> other )
        {
            // pass all the data and multiply the new data
            for ( int i = 0; i < Size; i++ ) {
                Data[i] *= other[i];
            }
        }

        #endregion


        #region With Scalar

        /// <summary>
        /// Multiply the vector with scalar.
        /// </summary>
        /// <param name="value"></param>
        public override void Multiply( float value )
        {
            // pass all the data and Subtract the value
            for ( int i = 0; i < Size; i++ ) {
                Data[i] *= (int)value;
            }
        }

        /// <summary>
        /// Multiply the vector with scalar.
        /// </summary>
        /// <param name="value"></param>
        public override void Multiply( int value )
        {
            // pass all the data and Subtract the value
            for ( int i = 0; i < Size; i++ ) {
                Data[i] *= value;
            }
        }

        #endregion

        #endregion


        #region Divide


        #region Pointwise

        public override void DividePointwise( FxVector<int> other )
        {
            // pass all the data and Divide the new data
            for ( int i = 0; i < Size; i++ ) {
                Data[i] /= other[i];
            }
        }

        #endregion


        #region With Scalar

        /// <summary>
        /// Divide the vector with scalar.
        /// </summary>
        /// <param name="value"></param>
        public override void Divide( float value )
        {
            // pass all the data and Subtract the value
            for ( int i = 0; i < Size; i++ ) {
                Data[i] /= (int)value;
            }
        }

        /// <summary>
        /// Divide the vector with scalar.
        /// </summary>
        /// <param name="value"></param>
        public override void Divide( int value )
        {
            // pass all the data and Subtract the value
            for ( int i = 0; i < Size; i++ ) {
                Data[i] /= value;
            }
        }

        #endregion

        #endregion


        #region Negate

        /// <summary>
        /// Negate each element of this Vector.
        /// </summary>
        public override void Negate()
        {
            // pass all the data and negate them
            for ( int i=0; i < Size; i++ ) {
                Data[i] = -Data[i];
            }
        }

        #endregion


        #endregion


        #region Statistics

        public override int Sum()
        {
            int sum=0;

            // add all the elements
            for ( int i=0; i < Size; i++ ) {
                sum += Data[i];
            }

            // return the result
            return sum;
        }

        /// <summary>
        /// Get the mean value of all elements
        /// </summary>
        /// <returns></returns>
        public override double Mean()
        {
            return Sum() / Size;
        }

        /// <summary>
        /// Get the standard deviation of all elements
        /// </summary>
        /// <returns></returns>
        public override double STD()
        {
            return Math.Sqrt( Variance() );
        }

        /// <summary>
        /// Get the variance of all elements
        /// </summary>
        /// <returns></returns>
        public override double Variance()
        {
            double mean = Mean();
            double var=0;

            // pass all the values
            for ( int x= 0; x < Size; x++ ) {
                var += ( Data[x] - mean ) * ( Data[x] - mean );
            }

            return var;
        }

        /// <summary>
        /// Get the Max value of the Vector
        /// </summary>
        /// <returns></returns>
        public override int Max()
        {
            int result = int.MinValue;

            // comp all the elements
            for ( int i=0; i < Size; i++ ) {
                result = ( Data[i] > result ) ? Data[i] : result;
            }

            return result;
        }

        /// <summary>
        /// Get the Min value of the Vector
        /// </summary>
        /// <returns></returns>
        public override int Min()
        {
            int result = int.MaxValue;

            // comp all the elements
            for ( int i=0; i < Size; i++ ) {
                result = ( Data[i] < result ) ? Data[i] : result;
            }

            return result;
        }


        /// <summary>
        /// Get the Max value of the Vector
        /// </summary>
        /// <param name="maxIndex"></param>
        /// <returns></returns>
        public override int Max(out int maxIndex)
        {
            int result = int.MinValue;
            maxIndex = -1;

            // comp all the elements
            for (int i = 0; i < Size; i++)
            {
                if (Data[i] > result)
                {
                    result = Data[i];
                    maxIndex = i;
                }
            }

            return result;
        }

        /// <summary>
        /// Get the Min value of the Vector
        /// </summary>
        /// <param name="minIndex"></param>
        /// <returns></returns>
        public override int Min(out int minIndex)
        {
            int result = int.MaxValue;
            minIndex = -1;

            // comp all the elements
            for (int i = 0; i < Size; i++)
            {
                if (Data[i] < result)
                {
                    result = Data[i];
                    minIndex = i;
                }
            }

            return result;
        }

        #endregion


        #region Norms

        /// <summary>
        /// Get the normal of specific type.
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public override double Norms( NormVectorType Type )
        {
            double result=0;

            switch ( Type ) {
                case NormVectorType.Euclidean:
                    // add all the elements
                    for ( int i=0; i < Size; i++ ) {
                        result += Math.Abs( Data[i] ) * Math.Abs( Data[i] );
                    }

                    result = Math.Sqrt( result );
                    break;

                case NormVectorType.Manhattan:
                    // add all the elements
                    for ( int i=0; i < Size; i++ ) {
                        result += Math.Abs( Data[i] );
                    }
                    break;

                case NormVectorType.Maximum:
                    // add all the elements
                    for ( int i=0; i < Size; i++ ) {
                        result = ( Data[i] > result ) ? Data[i] : result;
                    }
                    break;

            }

            return result;
        }

        /// <summary>
        /// Generic norms of given p
        /// p-norm
        /// </summary>
        /// <param name="p">p>0</param>
        /// <returns></returns>
        public override double Norms( double p )
        {
            double result=0;

            #region Exceptions
            if ( !( p >= 1 ) ) {
                throw new ArgumentNullException( "The P must be positive and greater than 1" );
            }
            #endregion

            // add all the elements
            for ( int i=0; i < Size; i++ ) {
                result += Math.Pow( Math.Abs( Data[i] ), p );
            }

            result = Math.Pow( result, 1.0 / p );

            return result;
        }

        #endregion


        #region Lenght/Distance

        public override double Distance( FxVector<int> Other )
        {
            double result=0;

            for ( int i=0; i < Size; i++ ) {
                result += ( Other[i] - Data[i] ) * ( Other[i] - Data[i] );
            }

            return Math.Sqrt( result );
        }

        #endregion


        #region Saves


        /// <summary>
        /// Save to CSV (Excel) format the Vector.
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveCsv(string fileName)
        {
            CsvRow row = new CsvRow();
            using (CsvFileWriter writer = new CsvFileWriter(fileName))
            {
                for (int j = 0; j < Data.Length; j++)
                {
                    row.Clear();
                    row.Add(Data[j].ToString().Replace(',', '.'));
                    writer.WriteRow(row);
                }
            }
        }

        #endregion




        #region Operators


        #region Subtract
        public static FxVectorI operator -(FxVectorI vec, int value)
        {
            var newVec = vec.GetCopy() as FxVectorI;
            newVec.DoSubtract(value);
            return newVec;
        }


        public static FxVectorI operator -(FxVectorI vec, float value)
        {
            var newVec = vec.GetCopy() as FxVectorI;
            newVec.DoSubtract(value);
            return newVec;
        }


        public static FxVectorI operator -(FxVectorI vec, FxVectorI value)
        {
            var newVec = vec.GetCopy() as FxVectorI;
            newVec.DoSubtract(value);
            return newVec;
        }

        #endregion



        #region Add

        public static FxVectorI operator +(FxVectorI vec, int value)
        {
            var newVec = vec.GetCopy() as FxVectorI;
            newVec.DoAdd(value);
            return newVec;
        }

        public static FxVectorI operator +(FxVectorI vec, float value)
        {
            var newVec = vec.GetCopy() as FxVectorI;
            newVec.DoAdd(value);
            return newVec;
        }

        public static FxVectorI operator +(FxVectorI vec, double value)
        {
            var newVec = vec.GetCopy() as FxVectorI;
            newVec.DoAdd(value);
            return newVec;
        }


        public static FxVectorI operator +(FxVectorI vec, FxVectorI value)
        {
            var newVec = vec.GetCopy() as FxVectorI;
            newVec.DoAdd(value);
            return newVec;
        }

        #endregion




        #region Multi

        public static FxVectorI operator *(FxVectorI vec, int value)
        {
            var newVec = vec.GetCopy() as FxVectorI;
            newVec.Multiply(value);
            return newVec;
        }

        public static FxVectorI operator *(FxVectorI vec, float value)
        {
            var newVec = vec.GetCopy() as FxVectorI;
            newVec.Multiply(value);
            return newVec;
        }

        public static FxVectorI operator *(FxVectorI vec, FxVectorI value)
        {
            var newVec = vec.GetCopy() as FxVectorI;
            newVec.MultiplyPointwise(value);
            return newVec;
        }

        #endregion



        #region Divide

        public static FxVectorI operator /(FxVectorI vec, int value)
        {
            var newVec = vec.GetCopy() as FxVectorI;
            newVec.Divide(value);
            return newVec;
        }

        public static FxVectorI operator /(FxVectorI vec, float value)
        {
            var newVec = vec.GetCopy() as FxVectorI;
            newVec.Divide(value);
            return newVec;
        }

        public static FxVectorI operator /(FxVectorI vec, FxVectorI value)
        {
            var newVec = vec.GetCopy() as FxVectorI;
            newVec.DividePointwise(value);
            return newVec;
        }

        #endregion


        #endregion


    }
}
