using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxMaths.Matrix
{
    public partial class FxMatrixF : FxMatrix<float>
    {

        #region Constructors

        public FxMatrixF( int width, int height )
            : base( width, height )
        {
            // allocate the data array
            this.Data = new float[Size];

            // init the datas
            for ( int i=0; i < Size; i++ ) {
                this.Data[i] = 0;
            }
        }

        public FxMatrixF( int width, int height, float value )
            : base( width, height )
        {
            // allocate the data array
            this.Data = new float[Size];

            // init the datas
            for ( int i=0; i < Size; i++ ) {
                this.Data[i] = value;
            }
        }

        protected override FxMatrix<float> AllocateMatrix()
        {
            return new FxMatrixF( Width, Height );
        }

        protected override FxMatrix<float> AllocateMatrix( int _Width, int _Height )
        {
            return new FxMatrixF( _Width, _Height );
        }

        protected override Vector.FxVector<float> AllocateVector( int Size )
        {
            return new Vector.FxVectorF( Size );
        }
        #endregion

        #region Math Functions



        #region Add

        protected override void DoAdd( FxMatrix<float> other, FxMatrix<float> result )
        {
            // pass all the data and add the new data
            Parallel.For( 0, Height, ( y ) => {
                int offsetEnd = ( y + 1 ) * Width;
                for ( int x= y * Width; x < offsetEnd; x++ ) {
                    result[x] = other[x] + Data[x];
                }
            } );
        }

        protected override void DoAdd( FxMatrix<float> other )
        {
            // pass all the data and add the new data
            Parallel.For( 0, Height, ( y ) => {
                int offsetEnd = ( y + 1 ) * Width;
                for ( int x= y * Width; x < offsetEnd; x++ ) {
                    Data[x] += other[x];
                }
            } );
        }

        protected override void DoAdd( float value )
        {
            // pass all the data and add the value
            Parallel.For( 0, Height, ( y ) => {
                int offsetEnd = ( y + 1 ) * Width;
                for ( int x= y * Width; x < offsetEnd; x++ ) {
                    Data[x] += value;
                }
            } );
        }

        protected override void DoAdd( int value )
        {
            // pass all the data and add the value
            Parallel.For( 0, Height, ( y ) => {
                int offsetEnd = ( y + 1 ) * Width;
                for ( int x= y * Width; x < offsetEnd; x++ ) {
                    Data[x] += value;
                }
            } );
        }

        protected override void DoAddRow( FxMaths.Vector.FxVector<float> Row, int RowIndex )
        {
            // set the offset for the specific row
            int rowOffset = RowIndex * Width;

            // pass all the data and add the value
            for ( int i=0; i < Width; i++ ) {
                Data[rowOffset + i] += Row[i];
            }
        }

        protected override void DoAddCol( FxMaths.Vector.FxVector<float> Col, int ColIndex )
        {
            // pass all the data and add the value
            for ( int i=0; i < Height; i++ ) {
                Data[i * Width + ColIndex] += Col[i];
            }
        }

        public static FxMatrixF operator+(FxMatrixF mat1, FxMatrixF mat2)
        {
            int Height = mat1.Height;
            int Width = mat1.Width;
            FxMatrixF result = new FxMatrixF(Width, Height);

            // pass all the data and add the new data
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    result[x] = mat1[x] + mat2[x];
                }
            });

            return result;
        }

        public static FxMatrixF operator +(FxMatrixF mat1, float value)
        {
            int Height = mat1.Height;
            int Width = mat1.Width;
            FxMatrixF result = new FxMatrixF(Width, Height);

            // pass all the data and add the new data
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    result[x] = mat1[x] + value;
                }
            });

            return result;
        }

        public static FxMatrixF operator +(FxMatrixF mat1, int value)
        {
            int Height = mat1.Height;
            int Width = mat1.Width;
            FxMatrixF result = new FxMatrixF(Width, Height);

            // pass all the data and add the new data
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    result[x] = mat1[x] + value;
                }
            });

            return result;
        }

        public static FxMatrixF operator +(float value, FxMatrixF mat1)
        {
            return mat1 + value;
        }

        public static FxMatrixF operator +(int value, FxMatrixF mat1)
        {
            return mat1 + value;
        }

        #endregion




        #region Subtract

        protected override void DoSubtract( FxMatrix<float> other, FxMatrix<float> result )
        {
            // pass all the data and Subtract the new data
            Parallel.For( 0, Height, ( y ) => {
                int offsetEnd = ( y + 1 ) * Width;
                for ( int x= y * Width; x < offsetEnd; x++ ) {
                    result[x] = Data[x] - other[x];
                }
            } );
        }

        protected override void DoSubtract( FxMatrix<float> other )
        {
            // pass all the data and Subtract the new data
            Parallel.For( 0, Height, ( y ) => {
                int offsetEnd = ( y + 1 ) * Width;
                for ( int x= y * Width; x < offsetEnd; x++ ) {
                    Data[x] -= other[x];
                }
            } );
        }

        protected override void DoSubtract( float value )
        {
            // pass all the data and Subtract the value
            Parallel.For( 0, Height, ( y ) => {
                int offsetEnd = ( y + 1 ) * Width;
                for ( int x= y * Width; x < offsetEnd; x++ ) {
                    Data[x] -= value;
                }
            } );
        }

        protected override void DoSubtract( int value )
        {
            // pass all the data and Subtract the value
            Parallel.For( 0, Height, ( y ) => {
                int offsetEnd = ( y + 1 ) * Width;
                for ( int x= y * Width; x < offsetEnd; x++ ) {
                    Data[x] -= value;
                }
            } );
        }

        protected override void DoSubtractRow( FxMaths.Vector.FxVector<float> Row, int RowIndex )
        {
            // set the offset for the specific row
            int rowOffset = RowIndex * Width;

            // pass all the data and Subtract the value
            for ( int i=0; i < Width; i++ ) {
                Data[rowOffset + i] -= Row[i];
            }
        }

        protected override void DoSubtractCol( FxMaths.Vector.FxVector<float> Col, int ColIndex )
        {
            // pass all the data and Subtract the value
            for ( int i=0; i < Height; i++ ) {
                Data[i * Width + ColIndex] -= Col[i];
            }
        }

        public static FxMatrixF operator -(FxMatrixF mat1, FxMatrixF mat2)
        {
            int Height = mat1.Height;
            int Width = mat1.Width;
            FxMatrixF result = new FxMatrixF(Width, Height);

            // pass all the data and add the new data
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    result[x] = mat1[x] - mat2[x];
                }
            });

            return result;
        }

        public static FxMatrixF operator -(FxMatrixF mat1, float value)
        {
            int Height = mat1.Height;
            int Width = mat1.Width;
            FxMatrixF result = new FxMatrixF(Width, Height);

            // pass all the data and add the new data
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    result[x] = mat1[x] - value;
                }
            });

            return result;
        }

        public static FxMatrixF operator -(FxMatrixF mat1, int value)
        {
            int Height = mat1.Height;
            int Width = mat1.Width;
            FxMatrixF result = new FxMatrixF(Width, Height);

            // pass all the data and add the new data
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    result[x] = mat1[x] - value;
                }
            });

            return result;
        }


        public static FxMatrixF operator -(float value, FxMatrixF mat1)
        {
            return mat1 - value;
        }

        public static FxMatrixF operator -(int value, FxMatrixF mat1)
        {
            return mat1 - value;
        }

        #endregion




        #region Multiply


        #region Matric Mult

        protected override void DoMultiply(FxMatrix<float> other, FxMatrix<float> result)
        {
            #region test code
            /*
            // pass all the data and Multiply the new data
            Parallel.For( 0, Height, ( y ) => {
                int offsetR = y * result.Width;
                int offsetD = y * Width;
                float sum=0.0f;

                for ( int x=0; x < Width; x++ ) {
                    // init sum
                    sum = 0.0f;

                    // pass all the columns
                    for ( int i=0; i < Width; i++ ) {
                        sum += Data[offsetD + i] * other[i * other.Width + x];
                    }

                    // store the result
                    result[offsetR + x] = sum;
                }
            } );
             */
            #endregion

            unsafe
            {
                // store local the with of other
                int OWidth = other.Width;

                // pass all the data and Multiply the new data
                Parallel.For(0, OWidth, (x) =>
                {
                    // get the selected collumn
                    FxMaths.Vector.FxVector<float> vec = other.GetCol(x);
                    float sum = 0.0f;
                    int Width2 = Width - Width % 2;

                    // use pointer to access the data
                    fixed (float* DataX = Data)
                    {
                        fixed (float* ResultX = result.Data)
                        {

                            // get the pointer of the result and the local data
                            float* pData = DataX;
                            float* pResult = ResultX + x;

                            // pass all the rows
                            for (int y = 0; y < Height; y++)
                            {

                                // init sum
                                sum = 0.0f;

                                // pass all the columns
                                for (int i = 0; i < Width2; i += 2)
                                {
                                    sum += *(pData + i) * vec[i] + *(pData + i + 1) * vec[i + 1];
                                }

                                for (int i = Width2; i < Width; i++)
                                {
                                    sum += *(pData + i) * vec[i];
                                }

                                // store the result
                                *(pResult) = sum;

                                // move to the next data
                                pResult += OWidth;
                                pData += Width;
                            }
                        }
                    }
                });
            }
        }

        
        #endregion



        #region Multiply with scalar

        protected override void DoMultiply( float value )
        {
            // pass all the data and Multiply the value
            Parallel.For( 0, Height, ( y ) => {
                int offset = y * Width;
                for ( int x=0; x < Width; x++ ) {
                    Data[offset + x] *= value;
                }
            } );
        }

        protected override void DoMultiply( int value )
        {
            // pass all the data and Multiply the value
            Parallel.For( 0, Height, ( y ) => {
                int offset = y * Width;
                for ( int x=0; x < Width; x++ ) {
                    Data[offset + x] *= value;
                }
            } );
        }

        public static FxMatrixF operator *(FxMatrixF mat1, int value)
        {
            int Height = mat1.Height;
            int Width = mat1.Width;
            FxMatrixF result = new FxMatrixF(Width, Height);

            // pass all the data and add the new data
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    result[x] = mat1[x] * value;
                }
            });

            return result;
        }

        public static FxMatrixF operator *(FxMatrixF mat1, float value)
        {
            int Height = mat1.Height;
            int Width = mat1.Width;
            FxMatrixF result = new FxMatrixF(Width, Height);

            // pass all the data and add the new data
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    result[x] = mat1[x] * value;
                }
            });

            return result;
        }

        public static FxMatrixF operator *(float value, FxMatrixF mat1)
        {
            return mat1 * value ;
        }

        public static FxMatrixF operator *(int value, FxMatrixF mat1)
        {
            return mat1 * value;
        }
        #endregion




        #region Multiply with vector

        protected override void DoMultiply( FxMaths.Vector.FxVector<float> rightSide, FxMaths.Vector.FxVector<float> result )
        {
            // pass all the data and Multiply the new data
            Parallel.For( 0, Height, ( y ) => {

                // init the result
                float sum=0;
                int offset = y * Width;

                // multiply the row with the right vector
                for ( int i=0; i < Width; i++ ) {
                    sum += Data[offset + i] * rightSide[i];
                }

                // store the result
                result[y] = sum;
            } );

        }

        protected override void DoLeftMultiply( FxMaths.Vector.FxVector<float> leftSide, FxMaths.Vector.FxVector<float> result )
        {
            // pass all the data and Multiply the new data
            Parallel.For( 0, Width, ( x ) => {

                // init the result
                float sum=0;
                int offset = x;

                // multiply the row with the right vector
                for ( int i=0; i < Height; i++ ) {
                    sum += Data[offset] * leftSide[i];

                    // go to the next row
                    offset += Width;
                }

                // store the result
                result[x] = sum;
            } );

        }

        #endregion




        #region Pointwise
        protected override void DoMultiplyPointwise( FxMatrix<float> other, FxMatrix<float> result )
        {
            // pass all the data and Multiply the value
            Parallel.For( 0, Height, ( y ) => {
                int offsetEnd = ( y + 1 ) * Width;
                for ( int x= y * Width; x < offsetEnd; x++ ) {
                    result[x] = Data[x] * other[x];
                }
            } );
        }

        protected override void DoMultiplyPointwise( FxMatrix<float> other )
        {
            // pass all the data and Multiply the value
            Parallel.For( 0, Height, ( y ) => {
                int offsetEnd = ( y + 1 ) * Width;
                for ( int x= y * Width; x < offsetEnd; x++ ) {
                    Data[x] *= other[x];
                }
            } );
        }
        #endregion


        #endregion




        #region Divide


        #region With scalar

        protected override void DoDivide( float value )
        {
            // pass all the data and Divide the value
            Parallel.For( 0, Height, ( y ) => {
                int offset = y * Width;
                for ( int x = 0; x < Width; x++ ) {
                    Data[offset + x] /= value;
                }
            } );
        }

        protected override void DoDivide( int value )
        {
            // pass all the data and Divide the value
            Parallel.For( 0, Height, ( y ) => {
                int offset = y * Width;
                for ( int x = 0; x < Width; x++ ) {
                    Data[offset + x] /= value;
                }
            } );
        }

        #endregion


        #region Pointwise

        protected override void DoDividePointwise( FxMatrix<float> other, FxMatrix<float> result )
        {
            // pass all the data and Divide the value
            Parallel.For( 0, Height, ( y ) => {
                int offsetEnd = ( y + 1 ) * Width;
                for ( int x = y * Width; x < offsetEnd; x++ ) {
                    result[x] = Data[x] / other[x];
                }
            } );
        }

        protected override void DoDividePointwise( FxMatrix<float> other )
        {
            // pass all the data and Divide the value
            Parallel.For( 0, Height, ( y ) => {
                int offsetEnd = ( y + 1 ) * Width;
                for ( int x = y * Width; x < offsetEnd; x++ ) {
                    Data[x] /= other[x];
                }
            } );
        }
        #endregion


        #endregion




        #region Negate
        protected override void DoNegate()
        {
            // negate everything !!!
            // !! the parallel is slower than this !!
            for ( int i=0; i < Size; i++ )
                Data[i] = -Data[i];
        }
        #endregion


        #endregion





        #region Statistics

        public override float Sum()
        {
            float sum=0;

            // add all the numbers
            // !! the parallel is slower than this !!
            for ( int i=0; i < Size; i++ )
                sum += Data[i];

            // sum all the accumulate results
            return sum;
        }

        public override float Mean()
        {
            // sum all the accumulate results
            return Sum() / Size;
        }

        public override float Variance()
        {
            float mean = Mean();
            FxMaths.Vector.FxVectorF RowResult = new Vector.FxVectorF( Height, 0 );

            // pass all the data and add the new data
            Parallel.For( 0, Height, ( y ) => {
                int offsetEnd = ( y + 1 ) * Width;
                for ( int x= y * Width; x < offsetEnd; x++ ) {
                    RowResult[y] += ( Data[x] - mean ) * ( Data[x] - mean );
                }
            } );

            // sum all the accumulate results
            return RowResult.Sum() / Size;
        }

        public override float Std()
        {
            // calc the square root of the variance
            return (float)Math.Sqrt( Variance() );
        }

        #endregion

        #region Fill

        public override void FillIdentity()
        {
            // reset the data values to 0
            for ( int i=0; i < Size; i++ ) {
                Data[i] = 0;
            }

            // set the diagonal to 1
            SetDiagonal( 1 );
        }

        #endregion

        #region Norm

        public override float Norm( NormMatrixType type )
        {
            float value=0;

            switch ( type ) {
                case NormMatrixType.Frobenius: {
                        // variable for adding
                        double ret=0;

                        // add the sqr of abs of all values
                        for ( int i=0; i < Size; i++ ) {
                            ret += Math.Abs( Data[i] ) * Math.Abs( Data[i] );
                        }

                        // calc the sqrt
                        value = (float)Math.Sqrt( ret );
                    }
                    break;

                case NormMatrixType.MaximumAbsoluteRowSum: {

                        Double ret=0;

                        // pass all the rows
                        for ( int y=0; y < Height; y++ ) {

                            double sum=0;
                            int end = ( y + 1 ) * Width;

                            // sum the abs of all data
                            for ( int x=y * Width; x < end; x++ )
                                sum += Math.Abs( Data[x] );

                            ret = Math.Max( ret, sum );

                        }

                        value = (float)ret;
                    }
                    break;

                case NormMatrixType.MaximumAbsoluteColSum: {

                        Double ret=0;

                        // pass all the rows
                        for ( int x=0; x < Width; x++ ) {

                            double sum=0;
                            int offset=x;

                            // sum the abs of all data
                            for ( int y=0; y < Height; y++ ) {
                                sum += Math.Abs( Data[offset] );
                                offset += Width;
                            }

                            ret = Math.Max( ret, sum );

                        }

                        value = (float)ret;
                    }
                    break;
            }

            return value;
        }

        #endregion

        #region Inverse/Determinal/LUFactor


        #region LUFactor

        /// <summary>
        /// Computes the LUP factorization of A. P*A = L*U.
        /// The matrix is overwritten with the  the LU factorization on exit. The lower triangular factor L is 
        /// stored in under the diagonal of internal matrix (the diagonal is always 1.0
        /// for the L factor). The upper triangular factor U is stored on and above the diagonal of internal matrix
        /// </summary>
        /// <returns>It contains the pivot indices. The size of the vector is the order of the table</returns>
        public override Vector.FxVectorI LU()
        {
            #region Exceptions

            if ( this.Width != this.Height ) {
                throw new ArgumentNullException( "The matrix must be square" );
            }

            #endregion

            //  set the order of the matrix
            int order = this.Width;
            Vector.FxVectorI ipiv = new Vector.FxVectorI( order );

            // Initialize the pivot matrix to the identity permutation.
            for ( var i = 0; i < order; i++ ) {
                ipiv[i] = i;
            }

            var vecLUcolx = new float[order];

            unsafe {

                // use pointer to access the data
                fixed ( float* DataX = Data ) {

                    // get the pointer of the result and the local data
                    float *pData = DataX;

                    // Outer loop.
                    for ( var x = 0; x < order; x++ ) {

                        // Make a copy of the x-th column to localize references.
                        for ( var y = 0; y < order; y++ ) {
                            vecLUcolx[y] = this[x, y];
                        }

                        // reset the data pointer
                        pData = DataX;

                        // Apply previous transformations.
                        for ( var y = 0; y < order; y++ ) {
                            // Most of the time ys spent yn the following dot product.
                            var kmax = ( y > x ) ? x : y;
                            var s = 0.0f;                            
                            for ( var k = 0; k < kmax; k++ ) {
                                s += *(pData+k) * vecLUcolx[k];
                            }

                            vecLUcolx[y] -= s;
                            *( pData + x ) = vecLUcolx[y];

                            // change line
                            pData += order;
                        }

                        // Find pivot and exchange if necessary.
                        var p = x;
                        for ( var y = x + 1; y < order; y++ ) {
                            if ( Math.Abs( vecLUcolx[y] ) > Math.Abs( vecLUcolx[p] ) ) {
                                p = y;
                            }
                        }

                        // set the pivot
                        if ( p != x ) {
                            var indexkp = p * order;
                            var indexkx = x * order;

                            for ( var k = 0; k < order; k++ ) {
                                var indexk = k;
                                var temp = this[indexkp + indexk];
                                this[indexkp + indexk] = this[indexkx + indexk];
                                this[indexkx +indexk] = temp;
                            }

                            ipiv[x] = p;
                        }

                        // reset the data pointer
                        pData = DataX;

                        // Compute multiplyers.
                        if ( x < order & this[x, x] != 0.0 ) {
                            // get the diagonal value
                            float dataxx=this[x, x];

                            // go after the diagonal value
                            pData += ( x + 1 ) * order + x;

                            for ( var y = x + 1; y < order; y++ ) {
                                // divide all the down triangle with the diagonal value
                                *( pData ) /= dataxx;

                                // change line
                                pData += order;
                            }
                        }
                    }
                }
            }
            return ipiv;
        }
        #endregion



        #endregion


    }
}
