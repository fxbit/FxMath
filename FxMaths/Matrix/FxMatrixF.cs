using FxMaths.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FxMaths.Matrix
{

    [Serializable]
    public partial class FxMatrixF : FxMatrix<float>
    {

        #region Constructors

        public FxMatrixF( int width, int height )
            : base( width, height )
        {
            // allocate the data array
            this.Data = new float[Size];

            // init the datas
            this.Data.Fill(0);
        }

        public FxMatrixF( int width, int height, float value )
            : base( width, height )
        {
            // allocate the data array
            this.Data = new float[Size];

            // init the datas
            this.Data.Fill(value);
        }

        protected override FxMatrix<float> AllocateMatrix()
        {
            return new FxMatrixF( Width, Height );
        }


        protected override FxMatrix<float> AllocateCopyMatrix()
        {
            FxMatrixF newMat = new FxMatrixF(Width, Height);
            //Buffer.BlockCopy(this.Data, 0, newMat.Data, 0, this.Size * 4 /* num of Bytes */);
            Array.Copy(this.Data, 0, newMat.Data, 0, this.Size);
            return newMat;
        }

        protected override FxMatrix<float> AllocateMatrix( int _Width, int _Height )
        {
            return new FxMatrixF( _Width, _Height );
        }

        protected override Vector.FxVector<float> AllocateVector( int Size )
        {
            return new Vector.FxVectorF( Size );
        }

        /// <summary>
        /// Make a copy of the matrix.
        /// </summary>
        /// <returns></returns>
        public new FxMatrixF Copy()
        {
            return AllocateCopyMatrix() as FxMatrixF;
        }
        #endregion




        #region Get/Set

        public void SetValue(int value)
        {
            // init the datas
            this.Data.Fill(value);
        }

        public void SetValue(float value)
        {
            // init the datas
            this.Data.Fill(value);
        }

        public void SetValue(FxMatrixMask mask, float value)
        {
            // pass all the data and add the new data
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    if (mask[x])
                        this.Data[x] = value;
                }
            });
        }

        public void SetValue(FxMatrixMask mask, int value)
        {
            // pass all the data and add the new data
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    if (mask[x])
                        this.Data[x] = value;
                }
            });
        }


        public FxMatrixF this[FxMatrixMask mask]
        {
            get
            {
                FxMatrixF newMat = new FxMatrixF(Width, Height);
                // pass all the data and add the new data
                Parallel.For(0, Height, (y) =>
                {
                    int offsetEnd = (y + 1) * Width;
                    for (int x = y * Width; x < offsetEnd; x++)
                    {
                        if (mask[x])
                            newMat.Data[x] = this.Data[x];
                    }
                });

                return newMat;
            }

            set
            {
                // pass all the data and add the new data
                Parallel.For(0, Height, (y) =>
                {
                    int offsetEnd = (y + 1) * Width;
                    for (int x = y * Width; x < offsetEnd; x++)
                    {
                        if (mask[x])
                            this.Data[x] = value[x];
                    }
                });
            }

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

        protected override void DoAdd(double value)
        {
            float fvalue = (float)value;
            // pass all the data and add the value
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    Data[x] += fvalue;
                }
            });
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

        protected void Add(FxMatrix<float> other, FxMatrixMask mask)
        {
            // pass all the data and add the new data
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    Data[x] += mask[x] ? other[x] : 0;
                }
            });
        }

        protected void DoAdd(float value, FxMatrixMask mask)
        {
            // pass all the data and add the value
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    Data[x] += mask[x] ? value : 0;
                }
            });
        }

        protected void DoAdd(int value, FxMatrixMask mask)
        {
            // pass all the data and add the value
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    Data[x] += mask[x] ? value : 0;
                }
            });
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



        public void Add(int sx, int sy, FxMatrixF mat)
        {
            if(sx > this.Width || sy > this.Height)
                return;

            int startX = (sx < 0) ? 0 : sx;
            int startY = (sy < 0) ? 0 : sy;
            int endX = (sx + mat.Width > this.Width) ? this.Width : sx + mat.Width;
            int endY = (sy + mat.Height > this.Height) ? this.Height : sy + mat.Height;
            int offsetMatX = (sx > 0) ? 0 : sx;
            int offsetMatY = (sy > 0) ? 0 : sy;
            // pass all the data and add the value
            Parallel.For(startY, endY, (y) => {
                var deltaY =  y * Width;
                for(int x = startX; x < endX; x++) {
                    Data[x + deltaY] += mat[x - startX - offsetMatX, y - startY - offsetMatY];
                }
            });
        }



        public static FxMatrixF operator +(FxMatrixF mat, float value)
        {
            var newMat = mat.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoAdd(value);
            return newMat;
        }


        public static FxMatrixF operator +(float value, FxMatrixF mat)
        {
            var newMat = mat.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoAdd(value);
            return newMat;
        }

        public static FxMatrixF operator +(FxMatrixF mat, int value)
        {
            var newMat = mat.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoAdd(value);
            return newMat;
        }

        public static FxMatrixF operator +(int value, FxMatrixF mat)
        {
            var newMat = mat.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoAdd(value);
            return newMat;
        }

        public static FxMatrixF operator +(FxMatrixF mat1, FxMatrixF mat2)
        {
            var newMat = mat1.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoAdd(mat2);
            return newMat;
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

        protected override void DoSubtract(double value)
        {
            float fvalue = (float)value;
            // pass all the data and Subtract the value
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    Data[x] -= fvalue;
                }
            });
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

        public static FxMatrixF operator -(FxMatrixF mat, float value)
        {
            var newMat = mat.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoSubtract(value);
            return newMat;
        }


        public static FxMatrixF operator -(float value, FxMatrixF mat)
        {
            var newMat = mat.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoAdd(-value);
            return newMat;
        }

        public static FxMatrixF operator -(FxMatrixF mat, int value)
        {
            var newMat = mat.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoSubtract(value);
            return newMat;
        }

        public static FxMatrixF operator -(int value, FxMatrixF mat)
        {
            var newMat = mat.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoAdd(-value);
            return newMat;
        }

        public static FxMatrixF operator -(FxMatrixF mat1, FxMatrixF mat2)
        {
            var newMat = mat1.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoSubtract(mat2);
            return newMat;
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

        protected override void DoMultiply(double value)
        {
            float fvalue = (float)value;
            // pass all the data and Multiply the value
            Parallel.For(0, Height, (y) =>
            {
                int end = (y + 1) * Width;
                for (int x = y * Width; x < end; x++)
                {
                    Data[x] *= fvalue;
                }
            });
        }

        protected override void DoMultiply(float value)
        {
            // pass all the data and Multiply the value
            Parallel.For(0, Height, (y) => {
                int end = (y+1) * Width;
                for(int x=y * Width; x < end; x++) {
                    Data[x] *= value;
                }
            });
        }

        protected override void DoMultiply( int value )
        {
            // pass all the data and Multiply the value
            Parallel.For( 0, Height, ( y ) => {
                int end = (y + 1) * Width;
                for(int x=y * Width; x < Width; x++) {
                    Data[x] *= value;
                }
            } );
        }


        public static FxMatrixF operator *(FxMatrixF mat, float value)
        {
            var newMat  = mat.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoMultiply(value);
            return newMat;
        }

        public static FxMatrixF operator *(float value, FxMatrixF mat)
        {
            var newMat = mat.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoMultiply(value);
            return newMat;
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

        public static FxMatrixF operator *(FxMatrixF mat1, FxMatrixF mat2)
        {
            var newMat = mat1.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoMultiplyPointwise(mat2);
            return newMat;
        }
        #endregion


        #endregion




        #region Divide


        #region With scalar

        protected override void DoDivide(double value)
        {
#if true
            DoMultiply(1.0f / value);
#else
            // pass all the data and Divide the value
            Parallel.For( 0, Height, ( y ) => {
                int offset = y * Width;
                for ( int x = 0; x < Width; x++ ) {
                    Data[offset + x] /= value;
                }
           } );
#endif
        }

        protected override void DoDivide( float value )
        {
#if true
            DoMultiply(1.0f / value);
#else
            // pass all the data and Divide the value
            Parallel.For( 0, Height, ( y ) => {
                int offset = y * Width;
                for ( int x = 0; x < Width; x++ ) {
                    Data[offset + x] /= value;
                }
           } );
#endif
        }

        protected override void DoDivide(int value)
        {
#if true
            DoMultiply(1.0f / value);
#else
            // pass all the data and Divide the value
            Parallel.For( 0, Height, ( y ) => {
                int offset = y * Width;
                for ( int x = 0; x < Width; x++ ) {
                    Data[offset + x] /= value;
                }
           } );
#endif
        }




        public static FxMatrixF operator /(FxMatrixF mat, float value)
        {
            var newMat = mat.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoDivide(value);
            return newMat;
        }

        public static FxMatrixF operator /(FxMatrixF mat, int value)
        {
            var newMat = mat.AllocateCopyMatrix() as FxMatrixF;
            newMat.DoDivide(value);
            return newMat;
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


        public void Normalize()
        {
            this.Divide(Norm(NormMatrixType.Frobenius));
        }

        public void Normalize(NormMatrixType type)
        {
            this.Divide(Norm(type));
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



        #region Determinal

        public override float Determinal()
        {
            if (Width != Height)
                return 0;

            switch (Width)
            {
                case 0: return 0;
                case 1: return Data[0];
                case 2: return (Data[0] * Data[3] - Data[1] * Data[2]);
                case 3: return
                    (Data[0] * ((Data[8] * Data[4]) - (Data[7] * Data[5]))) -
                    (Data[3] * ((Data[8] * Data[1]) - (Data[7] * Data[2]))) +
                    (Data[6] * ((Data[5] * Data[1]) - (Data[4] * Data[2])));

                // only supporting 1x1, 2x2 and 3x3
                default: return 0;
            }
        } 

        #endregion



        #region Inverse

        public override FxMatrix<float> Inverse()
        {
            // check if we can compute inverse
            float det = this.Determinal();
            if (det == 0) 
                return null;

            // create the result matrix
            FxMatrix<float> result = this.AllocateMatrix(Width, Height);
            switch (Width)
            {
                case 1:
                    result[0] = 1 / Data[0];
                    break;

                case 2:
                    result[0] = Data[3] / det;
                    result[1] = -Data[1] / det;
                    result[2] = -Data[2] / det;
                    result[3] = Data[0] / det;
                    break;

                case 3:
                    result[0] = ((Data[8] * Data[4]) - (Data[7] * Data[5])) / det;
                    result[1] = -((Data[8] * Data[1]) - (Data[7] * Data[2])) / det;
                    result[2] = ((Data[5] * Data[1]) - (Data[4] * Data[2])) / det;

                    result[3] = -((Data[8] * Data[3]) - (Data[6] * Data[5])) / det;
                    result[4] = ((Data[8] * Data[0]) - (Data[6] * Data[2])) / det;
                    result[5] = -((Data[5] * Data[0]) - (Data[3] * Data[2])) / det;

                    result[6] = ((Data[7] * Data[3]) - (Data[6] * Data[4])) / det;
                    result[7] = -((Data[7] * Data[0]) - (Data[6] * Data[2])) / det;
                    result[8] = ((Data[4] * Data[0]) - (Data[3] * Data[1])) / det;
                    break;
            }
            return result;
        }

        #endregion


        #endregion




        #region Sample Functions

        /// <summary>
        /// Return interpolated value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override float Sample(float x, float y)
        {
            // for now we are support only Bilinear sampling
            int lowX = (int)Math.Floor(x);
            int lowY = (int)Math.Floor(y);
            float lowlow = this[lowX, lowY];
            float lowhi = this[lowX, lowY+1];
            float hilow = this[lowX+1, lowY];
            float hihi = this[lowX+1, lowY+1];
            x=x-lowX;
            y=y-lowY;
            //return lowlow + (hilow - lowlow) * x + (lowhi - lowlow) * y + (lowlow - hilow - lowhi + hihi) * x * y;
            return lowlow * (1 - x) * (1 - y) + hilow * x * (1 - y) + lowhi * (1 - x) * y + hihi * x * y;
        }

        #endregion




        #region From mask calculation from FxMatrix




        #region With other matrix

        public static FxMatrixMask operator >(FxMatrixF mat1, FxMatrixF mat2)
        {
            FxMatrixMask result = new FxMatrixMask(mat1.Width, mat1.Height);
            int Height = mat1.Height;
            int Width = mat1.Width;

            Parallel.For(0, Height, (y) => {
                int offsetEnd = (y + 1) * Width;
                for(int x= y * Width; x < offsetEnd; x++) {
                    result.Data[x] = mat1.Data[x] > mat2.Data[x];
                }
            });
            return result;
        }

        public static FxMatrixMask operator <(FxMatrixF mat1, FxMatrixF mat2)
        {
            FxMatrixMask result = new FxMatrixMask(mat1.Width, mat1.Height);
            int Height = mat1.Height;
            int Width = mat1.Width;

            Parallel.For(0, Height, (y) => {
                int offsetEnd = (y + 1) * Width;
                for(int x= y * Width; x < offsetEnd; x++) {
                    result.Data[x] = mat1.Data[x] < mat2.Data[x];
                }
            });
            return result;
        }

        public static FxMatrixMask operator ==(FxMatrixF mat1, FxMatrixF mat2)
        {
            FxMatrixMask result = new FxMatrixMask(mat1.Width, mat1.Height);
            int Height = mat1.Height;
            int Width = mat1.Width;

            Parallel.For(0, Height, (y) => {
                int offsetEnd = (y + 1) * Width;
                for(int x= y * Width; x < offsetEnd; x++) {
                    result.Data[x] = mat1.Data[x] == mat2.Data[x];
                }
            });
            return result;
        }


        public static FxMatrixMask operator !=(FxMatrixF mat1, FxMatrixF mat2)
        {
            FxMatrixMask result = new FxMatrixMask(mat1.Width, mat1.Height);
            int Height = mat1.Height;
            int Width = mat1.Width;

            Parallel.For(0, Height, (y) => {
                int offsetEnd = (y + 1) * Width;
                for(int x= y * Width; x < offsetEnd; x++) {
                    result.Data[x] = mat1.Data[x] == mat2.Data[x];
                }
            });
            return result;
        }
        
        #endregion




        #region With float scalar

        public static FxMatrixMask operator >(FxMatrixF mat1, float value) { return FxMatrixMask.GreaderThan<float>(mat1, value); }
        public static FxMatrixMask operator <(FxMatrixF mat1, float value) { return FxMatrixMask.LessThan<float>(mat1, value); }
        public static FxMatrixMask operator ==(FxMatrixF mat1, float value) { return FxMatrixMask.Equal<float>(mat1, value); }
        public static FxMatrixMask operator !=(FxMatrixF mat1, float value) { return FxMatrixMask.NotEqual<float>(mat1, value); }

        #endregion




        #region With int scalar

        public static FxMatrixMask operator >(FxMatrixF mat1, int value) { return FxMatrixMask.GreaderThan<int>(mat1, value); }
        public static FxMatrixMask operator <(FxMatrixF mat1, int value) { return FxMatrixMask.LessThan<int>(mat1, value); }
        public static FxMatrixMask operator ==(FxMatrixF mat1, int value) { return FxMatrixMask.Equal<int>(mat1, value); }
        public static FxMatrixMask operator !=(FxMatrixF mat1, int value) { return FxMatrixMask.NotEqual<int>(mat1, value); }

        #endregion

        #endregion




        #region Equal test

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is FxMatrixF)
            {
                FxMatrixF mat = obj as FxMatrixF;
                Boolean equal = true;
                for (int i = 0; i < Size; i++)
                    if (Data[i] != mat.Data[i])
                    {
                        equal = false;
                        break;
                    }
                return equal;

            }
            return false;
        } 

        #endregion




        #region Save Matrix in Files

        public void SaveImage(string fileName, FxMaths.Images.ColorMap map)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(Width, Height);
            FxMaths.Images.FxImages im =  FxMaths.Images.FxTools.FxImages_safe_constructors(bitmap);
            im.Load(this, map);
            im.Image.Save(fileName);
            im.Dispose();
            bitmap.Dispose();
        }


        /// <summary>
        /// Save the Matrix in CSV form.
        /// This filetype can be load from matlab.
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveCsv(string fileName)
        {
            CsvRow row = new CsvRow();
            using(CsvFileWriter writer = new CsvFileWriter(fileName)) {
                for(int j=0; j < Height; j++) {
                    row.Clear();
                    for(int i=0; i < Width; i++) {
                        row.Add(this[i, j].ToString().Replace(',', '.'));
                    }
                    writer.WriteRow(row);
                }
            }
        }

        /// <summary>
        /// Load a Matrix from CSV form
        /// This filetype can be saved from matlab
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static public FxMatrixF LoadCsv(string fileName)
        {
            FxMatrixF mat = new FxMatrixF(10,10);
            // Read sample data from CSV file
            using (CsvFileReader reader = new CsvFileReader(fileName))
            {
                List<CsvRow> listRow = new List<CsvRow>();
                CsvRow row = new CsvRow();
                int oldWidth = -1;
                while (reader.ReadRow(row))
                {
                    // save the row for later
                    listRow.Add(row);

                    // the first time update the old width with the row count
                    if (oldWidth == -1)
                        oldWidth = row.Count;

                    if (oldWidth != row.Count)
                    {
                        Console.WriteLine("The lines are not have the same lenght");
                        return null;
                    }

                    oldWidth = row.Count;


                    // create a new one that is going to be readed from file.
                    row = new CsvRow();
                }


                // create the matrix
                mat = new FxMatrixF(oldWidth, listRow.Count);
                int i, j = 0;
                foreach (CsvRow r in listRow)
                {
                    for (i = 0; i < oldWidth; i++)
                    {
                        mat[i, j] = float.Parse(r[i].Replace('.', ','));
                    }
                    j++;
                }
            }
            return mat;
        }

        #endregion




        #region Calculate Gradient

        public enum GradientMethod
        {
            Sobel,
            Prewitt,
            Roberts,
            Scharr,
            CentralDifference
        }

        /// <summary>
        /// Calculate the gradient of the matrix.
        /// Use Sobel method.
        /// </summary>
        /// <returns></returns>
        public FxMatrixF Gradient()
        {
            return Gradient(GradientMethod.Sobel);
        }

        /// <summary>
        /// Calculate the gradient of the matrix.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public FxMatrixF Gradient(GradientMethod method)
        {
            var result = new FxMatrixF(Width, Height);

            switch(method) {
                case GradientMethod.Prewitt:
                    Parallel.For(1, Height - 1, (y) => {
                        for(int x= 1; x < Width - 1; x++) {
                            var dx  = this[x - 1, y - 1] - this[x + 1, y - 1];
                            dx += this[x - 1, y] - this[x + 1, y];
                            dx += this[x - 1, y + 1] - this[x + 1, y + 1];

                            var dy  = this[x - 1, y - 1] - this[x - 1, y + 1];
                            dy += this[x, y - 1] - this[x, y + 1];
                            dy += this[x + 1, y - 1] - this[x + 1, y + 1];

                            result[x, y] = (float)Math.Sqrt(dx * dx + dy * dy);
                        }
                    });
                    break;


                case GradientMethod.Sobel:
                    Parallel.For(1, Height - 1, (y) => {
                        for(int x= 1; x < Width - 1; x++) {
                            var dx  = this[x - 1, y - 1] - this[x + 1, y - 1];
                            dx += 2 * this[x - 1, y] - 2 * this[x + 1, y];
                            dx += this[x - 1, y + 1] - this[x + 1, y + 1];

                            var dy  = this[x - 1, y - 1] - this[x - 1, y + 1];
                            dy += 2 * this[x, y - 1] - 2 * this[x, y + 1];
                            dy += this[x + 1, y - 1] - this[x + 1, y + 1];

                            result[x, y] = (float)Math.Sqrt(dx * dx + dy * dy);
                        }
                    });
                    break;


                case GradientMethod.Roberts:
                    Parallel.For(0, Height - 1, (y) => {
                        for(int x= 0; x < Width - 1; x++) {
                            var dx  = this[x, y] - this[x + 1, y + 1];
                            var dy  = this[x + 1, y] - this[x, y + 1];

                            result[x, y] = (float)Math.Sqrt(dx * dx + dy * dy);
                        }
                    });
                    break;

                case GradientMethod.Scharr:
                    Parallel.For(1, Height - 1, (y) => {
                        for(int x= 1; x < Width - 1; x++) {
                            var dx  = 3 * this[x - 1, y - 1] - 3 * this[x + 1, y - 1];
                            dx += 10 * this[x - 1, y] - 10 * this[x + 1, y];
                            dx += 3 * this[x - 1, y + 1] - 3 * this[x + 1, y + 1];

                            var dy  = 3 * this[x - 1, y - 1] - 3 * this[x - 1, y + 1];
                            dy += 10 * this[x, y - 1] - 10 * this[x, y + 1];
                            dy += 3 * this[x + 1, y - 1] - 3 * this[x + 1, y + 1];

                            result[x, y] = (float)Math.Sqrt(dx * dx + dy * dy);
                        }
                    });
                    break;



                case GradientMethod.CentralDifference:
                    Parallel.For(1, Height - 1, (y) => {
                        for(int x= 1; x < Width - 1; x++) {
                            var dx  = (this[x - 1, y] - this[x + 1, y]) * 0.5f;
                            var dy  = (this[x, y - 1] - this[x, y + 1]) * 0.5f;
                            result[x, y] = (float)Math.Sqrt(dx * dx + dy * dy);
                        }
                    });
                    break;
            }

            return result;
        } 

        #endregion



        #region Misc Utils


        #region Clamp
        /// <summary>
        /// Clamped the value of the matrix to 
        /// the zero or max based on min max.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void Clamp(float min, float max, float minReplace = 0)
        {
            // pass all the data and add the new data
            Parallel.For(0, Height, (y) => {
                int offsetEnd = (y + 1) * Width;
                for(int x= y * Width; x < offsetEnd; x++) {
                    Data[x] = (Data[x] < min) ? minReplace : (Data[x] > max) ? max : Data[x];
                }
            });
        } 
        #endregion


        
        #endregion




        #region Resize

        public FxMatrixF Resize(int width, int height)
        {
            var newMat = new FxMatrixF(width, height);
            float lx = Width / (float)width;
            float ly = Height / (float)height;

            Parallel.For(0, height, (y) => {
                int offsetEnd = (y + 1) * width;
                int offsetX = y * width;
                for(int x= offsetX; x < offsetEnd; x++) {
                    newMat.Data[x] = this.Sample((x - offsetX) * lx, y * ly);
                }
            });

            return newMat;
        } 

        #endregion



        #region Median Filter 

        public FxMatrixF MedianFilt(int sx = 3, int sy = 3)
        {
            FxMatrixF result = new FxMatrixF(Width, Height);
            int sx2 = (int)Math.Floor(sx / 2.0);
            int sy2 = (int)Math.Floor(sy / 2.0);
            int midPoint = (int)Math.Floor(sx * sy / 2.0);

            Parallel.For(0, Height, (y) =>
            {
                int iy_start = (y > sy2) ? y - sy2 : 0;
                int iy_end = (y + sy2 + 1 < Height) ? y + sy2 + 1 : Height;
                FxMaths.Vector.FxVectorF vec = new Vector.FxVectorF(sx * sy);
                for (int x = 0; x < Width; x++)
                {
                    int ix_start = (x > sx2) ? x - sx2 : 0;
                    int ix_end = (x + sx2 + 1 < Width) ? x + sx2 + 1 : Width;
                    int i = 0;
                    for (int iy = iy_start; iy < iy_end; iy++)
                        for (int ix = ix_start; ix < ix_end; ix++)
                        {
                            vec[i] = this[ix, iy];
                            i++;
                        }
                    vec.Sort();
                    result[x, y] = vec[midPoint];
                }
            });

            return result;
        }

        #endregion
    }
}
