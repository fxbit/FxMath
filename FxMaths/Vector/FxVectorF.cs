using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.Vector
{
    // Declare a delegate type for processing a book:
    public delegate float Fx1DGeneratorF( float x );

    public class FxVectorF : FxVector<float>
    {


        #region Constructors

        public FxVectorF( int Size )
            : base( Size )
        {
            // init the datas
            for ( int i=0; i < Size; i++ ) {
                this.Data[i] = 0;
            }
        }

        public FxVectorF( int Size, float value )
            : base( Size )
        {
            // init the datas
            for ( int i=0; i < Size; i++ ) {
                this.Data[i] = value;
            }
        }

        public FxVectorF( int Size, Fx1DGeneratorF Generator,float Step )
            : base( Size )
        {
            // init the datas
            for ( int i=0; i < Size; i++ ) {
                this.Data[i] = Generator(i*Step);
            }
        }

        public FxVectorF( byte[] data)
            : base( data.Length/4 )
        {
            // init the datas
            for ( int i=0; i < Size; i++ ) {
                float num = (float)BitConverter.ToInt32(data,4*i);
                this.Data[i] = num;// data[2 * i + 1];//(float)( num / (float)short.MaxValue);
            }
        }

        public FxVectorF( float[] data)
            : base( data.Length )
        {
            // init the datas
            for ( int i=0; i < Size; i++ ) {
                this.Data[i] = data[i];
            }
        }

        public FxVectorF( float[] data ,int size)
            : base( size )
        {
            // init the datas
            for ( int i=0; i < Size; i++ ) {
                this.Data[i] = data[i];
            }
        }

        protected override FxVector<float> AllocateVector( int Size )
        {
            return new FxVectorF( Size );
        }

        #endregion


        #region Math Functions

        #region Add

        protected override void DoAdd( FxVector<float> other, FxVector<float> result )
        {
            // pass all the data and add the new data
            for ( int i=0; i < Size; i++ ) {
                result[i] = other[i] + Data[i];
            }
        }

        protected override void DoAdd( FxVector<float> other )
        {
            // pass all the data and add the new data
            for ( int i=0; i < Size; i++ ) {
                Data[i] += other[i];
            }
        }

        protected override void DoAdd( float value )
        {
            // pass all the data and add the value
            for ( int i=0; i < Size; i++ ) {
                Data[i] += value;
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

        protected override void DoSubtract( FxVector<float> other, FxVector<float> result )
        {
            // pass all the data and Subtract the new data
            for ( int i=0; i < Size; i++ ) {
                result[i] = Data[i] - other[i];
            }
        }

        protected override void DoSubtract( FxVector<float> other )
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
                Data[i] -= value;
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

        public override void MultiplyPointwise( FxVector<float> other )
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
                Data[i] *= value;
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

        public override void DividePointwise( FxVector<float> other )
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
                Data[i] /= value;
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

        public override float Sum()
        {
            float sum=0;

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
        public override float Max()
        {
            float result = float.MinValue;

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
        public override float Min()
        {
            float result = float.MaxValue;

            // comp all the elements
            for ( int i=0; i < Size; i++ ) {
                result = ( Data[i] < result ) ? Data[i] : result;
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
                    result = double.MinValue;
                    // comp all the elements
                    for ( int i=0; i < Size; i++ ) {
                        result = ( Data[i] > result ) ? Data[i] : result;
                    }
                    break;

                case NormVectorType.AbsMaximum:
                    // comp all the elements
                    for ( int i=0; i < Size; i++ ) {
                        result = ( Math.Abs(Data[i]) > result ) ? Math.Abs(Data[i]) : result;
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

        public override double Distance( FxVector<float> Other )
        {
            double result=0;

            for ( int i=0; i < Size; i++ ) {
                result += ( Other[i] - Data[i] ) * ( Other[i] - Data[i] );
            }

            return Math.Sqrt( result );
        }

        #endregion

    }
}
