using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxMaths.Vector;
using System.Threading.Tasks;

namespace FxMaths.Matrix
{
    public abstract partial class FxMatrix<T> where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {

        #region Size Parameters

        /// <summary>
        /// Gets the Width
        /// </summary>
        public Int32 Width;

        /// <summary>
        /// Gets the height
        /// </summary>
        public Int32 Height;

        /// <summary>
        /// Number of values.
        /// </summary>
        public Int32 Size;

        #endregion




        #region Internal Variables

        public T []Data;

        #endregion





        #region Constraction

        /// <summary>
        /// Initializes a new instance of the Matrix class.
        /// </summary>
        /// <param name="height">
        /// The number of rows.
        /// </param>
        /// <param name="width">
        /// The number of columns.
        /// </param>
        public FxMatrix( int width, int height )
        {
            #region Exceptions
            if ( width <= 0 ) {
                throw new ArgumentOutOfRangeException( "Width must be positive" );
            }

            if ( height <= 0 ) {
                throw new ArgumentOutOfRangeException( "Height must be positive" );
            }
            #endregion

            // set the internal sizes
            Width = width;
            Height = height;
            Size = Width * Height;
        }

        /// <summary>
        /// Allocate the data array
        /// </summary>
        /// <returns></returns>
        protected abstract FxMatrix<T> AllocateMatrix();

                /// <summary>
        /// Allocate the data array
        /// </summary>
        /// <returns></returns>
        protected abstract FxMatrix<T> AllocateCopyMatrix();


        /// <summary>
        /// Allocate the data array
        /// </summary>
        /// <returns></returns>
        protected abstract FxMatrix<T> AllocateMatrix(int Width,int Height);
        

        /// <summary>
        /// Allocate the data for Vector
        /// </summary>
        /// <returns></returns>
        protected abstract FxVector<T> AllocateVector( int Size );

        /// <summary>
        /// Make a copy of the matrix.
        /// </summary>
        /// <returns></returns>
        public FxMatrix<T> Copy()
        {
            return AllocateCopyMatrix();
        }

        #endregion




        #region Fill


        /// <summary>
        /// Return a new matrix in identity form
        /// </summary>
        /// <returns></returns>
        public virtual FxMatrix<T> Identity()
        {
            FxMatrix<T> newMatrix = AllocateMatrix();

            newMatrix.FillIdentity();

            return newMatrix;
        }

        /// <summary>
        /// Reset the matrix to identity form
        /// </summary>
        public abstract void FillIdentity();

        #endregion



        #region Get/Set values

        /// <summary>
        /// Get The Value in specific position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public T GetValue( int x, int y )
        {
            return this.Data[y * Width + x];
        }

        /// <summary>
        /// Set The Value in specific position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Value"></param>
        public void SetValue( int x, int y, T Value )
        {
            this.Data[y * Width + x] = Value;
        }


        /// <summary>
        /// Set/Get internal values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public T this[int x, int y]
        {
            get
            {
                return this.Data[y * Width + x];
            }

            set
            {
                this.Data[y * Width + x] = value;
            }
        }

        /// <summary>
        /// Set/Get internal values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return this.Data[index];
            }

            set
            {
                this.Data[index] = value;
            }
        }


        #endregion
      


        #region Get/Set Row/Columns

        #region Get

        #region Row

        /// <summary>
        /// Get a specific Row of the Matrix
        /// </summary>
        /// <param name="RowIndex">The index of the row</param>
        /// <returns></returns>
        public FxVector<T> GetRow( int RowIndex )
        {
            // allocate a row vector
            FxVector<T> row = AllocateVector( Width );

            // calc the offset for the row
            int RowOffset = RowIndex * Width;

            // copy the row
            for ( int i=0; i < Width; i++ ) {
                row[i] = Data[RowOffset + i];
            }

            // return the new row
            return row;
        }

        /// <summary>
        /// Get a specific Row of the Matrix
        /// </summary>
        /// <param name="RowIndex">The index of the row</param>
        /// <returns></returns>
        public void GetRow( int RowIndex, FxVector<T> row )
        {

            #region Exceptions
            if ( row == null ) {
                throw new ArgumentNullException( "other" );
            }

            if ( row.Size != Height ) {
                throw new ArgumentOutOfRangeException( "Matrix/Vector Dimensions" );
            }
            #endregion


            // copy the row
            for ( int i=0; i < Width; i++ ) {
                row[i] = Data[RowIndex + i];
            }
        }
        #endregion

        #region Collumn

        /// <summary>
        /// Get a specific collumn of the Matrix
        /// </summary>
        /// <param name="ColIndex">The index of the collumn</param>
        /// <returns></returns>
        public FxVector<T> GetCol( int ColIndex )
        {
            // allocate a col vector
            FxVector<T> col = AllocateVector( Height );

            // copy the col
            for ( int i=0; i < Height; i++ ) {
                col[i] = Data[i * Width + ColIndex];
            }

            // return the new col
            return col;
        }

        /// <summary>
        /// Get a specific collumn of the Matrix
        /// </summary>
        /// <param name="ColIndex">The index of the collumn</param>
        /// <returns></returns>
        public void GetCol( int ColIndex, FxVector<T> col )
        {

            #region Exceptions
            if ( col == null ) {
                throw new ArgumentNullException( "other" );
            }

            if ( col.Size != Height ) {
                throw new ArgumentOutOfRangeException( "Matrix/Vector Dimensions" );
            }
            #endregion

            // copy the col
            for ( int i=0; i < Height; i++ ) {
                col[i] = Data[i * Width + ColIndex];
            }
        }

        #endregion

        #region Diagonal

        /// <summary>
        /// Get a specific collumn of the Matrix
        /// </summary>
        /// <param name="ColIndex">The index of the collumn</param>
        /// <returns></returns>
        public FxVector<T> GetDiagonal( )
        {
            // find the size of diagonal
            int diaSize = ( Height > Width ) ? Width : Height;

            // allocate a col vector
            FxVector<T> dia = AllocateVector( diaSize );

            // copy the col
            for ( int i=0; i < diaSize; i++ ) {
                dia[i] = Data[i * Width + i];
            }

            // return the new col
            return dia;
        }

        /// <summary>
        /// Get the diagonal of the matrix
        /// </summary>
        /// <returns></returns>
        public void GetDiagonal( FxVector<T> dia )
        {
            // find the size of diagonal
            int diaSize = (Height>Width)?Width:Height;

            #region Exceptions
            if ( dia == null ) {
                throw new ArgumentNullException( "other" );
            }

            if ( dia.Size != diaSize ) {
                throw new ArgumentOutOfRangeException( "Matrix/Vector Dimensions" );
            }
            #endregion

            // copy the col
            for ( int i=0; i < diaSize; i++ ) {
                dia[i] = Data[i * Width + i];
            }
        }

        #endregion 

        #region SubMatrix

        /// <summary>
        /// Get a copy of a submatrix
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        public FxMatrix<T> GetSubMatrix( int startX, int startY, int endX, int endY )
        {
            // allocate the return matrix
            FxMatrix<T> sub = AllocateMatrix( endX - startX + 1, endY - startY + 1 );

            // copy the submatrix
            for ( int x=startX; x < endX; x++ ) {
                for ( int y=startY; y < endY; y++ ) {
                    sub[x, y] = this[x, y];
                }
            }

            return sub;
        }

        /// <summary>
        /// copy the values of the matrix in the given submatrix
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        public void GetSubMatrix( FxMatrix<T> sub, int startX, int startY, int endX, int endY )
        {
            #region Exceptions
            if (object.ReferenceEquals(null, sub))
            {
                throw new ArgumentNullException( "other" );
            }

            if ( sub.Height <= Height && sub.Width <= Width ) {
                throw new ArgumentOutOfRangeException( "Matrix Dimensions" );
            }
            #endregion

            // copy the submatrix
            for ( int x=startX; x < endX; x++ ) {
                for ( int y=startY; y < endY; y++ ) {
                    sub[x, y] = this[x, y];
                }
            }
        }

        #endregion

        #endregion

        #region Set

        #region Row

        /// <summary>
        /// Set a specific Row
        /// </summary>
        /// <param name="row">The Row vector</param>
        /// <param name="RowIndex">The index of the Row</param>
        public void SetRow( FxVector<T> row, int RowIndex )
        {
            // calc the offset for the row
            int RowOffset = RowIndex * Width;

            // copy the row
            for ( int i=0; i < Width; i++ ) {
                Data[RowOffset + i] = row[i];
            }
        }

        #endregion

        #region Collumn

        /// <summary>
        /// Set a specific Collumn
        /// </summary>
        /// <param name="col">The collumn vector</param>
        /// <param name="ColIndex">The index of the Collumn</param>
        public void SetCol( FxVector<T> col, int ColIndex )
        {
            // copy the col
            for ( int i=0; i < Height; i++ ) {
                Data[i * Width + ColIndex] = col[i];
            }
        }

        #endregion

        #region Diagonal

        /// <summary>
        /// Set the diagonal of the matrix
        /// </summary>
        /// <returns></returns>
        public void SetDiagonal( FxVector<T> dia )
        {
            // find the size of diagonal
            int diaSize = ( Height > Width ) ? Width : Height;

            #region Exceptions
            if ( dia == null ) {
                throw new ArgumentNullException( "other" );
            }

            if ( dia.Size != diaSize ) {
                throw new ArgumentOutOfRangeException( "Matrix/Vector Dimensions" );
            }
            #endregion

            // copy the col
            for ( int i=0; i < diaSize; i++ ) {
                Data[i * Width + i] = dia[i];
            }
        }

        /// <summary>
        /// Set the diagonal of the matrix
        /// with value
        /// </summary>
        /// <returns></returns>
        public void SetDiagonal( T value)
        {
            // find the size of diagonal
            int diaSize = ( Height > Width ) ? Width : Height;

            // copy the col
            for ( int i=0; i < diaSize; i++ ) {
                Data[i * Width + i] = value;
            }
        }

        #endregion

        #region SubMatrix


        /// <summary>
        /// copy the values of the given submatrix in the matrix
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        public void SetSubMatrix( FxMatrix<T> sub, int startX, int startY, int endX, int endY )
        {
            #region Exceptions
            if (object.ReferenceEquals(null, sub))
            {
                throw new ArgumentNullException( "other" );
            }

            if ( sub.Height <= Height && sub.Width <= Width ) {
                throw new ArgumentOutOfRangeException( "Matrix Dimensions" );
            }
            #endregion

            // copy the submatrix
            for ( int x=startX; x < endX; x++ ) {
                for ( int y=startY; y < endY; y++ ) {
                    this[x, y] = sub[x, y];
                }
            }
        }

        #endregion


        #endregion

        #endregion



        #region Math Functions

        #region Add
        /// <summary>
        /// Adds another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to add to this matrix.</param>
        /// <param name="result">The matrix to store the result of the addition.</param>
        protected abstract void DoAdd( FxMatrix<T> other, FxMatrix<T> result );

        /// <summary>
        /// Adds another matrix to this matrix.
        /// Change this Matrix.
        /// </summary>
        /// <param name="other">The matrix to add to this matrix.</param>
        protected abstract void DoAdd( FxMatrix<T> other );
        protected abstract void DoAdd( float value );
        protected abstract void DoAdd( int result );

        /// <summary>
        /// Add Vector to specific row
        /// </summary>
        /// <param name="vector"></param>
        protected abstract void DoAddRow( FxMaths.Vector.FxVector<T> vector, int RowIndex );

        /// <summary>
        /// Add Vector to specific Column
        /// </summary>
        /// <param name="vector"></param>
        protected abstract void DoAddCol( FxMaths.Vector.FxVector<T> vector, int ColIndex );

        /// <summary>
        /// Adds another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to add to this matrix.</param>
        public virtual void Add( FxMatrix<T> other )
        {
            #region Exceptions
            if ( object.ReferenceEquals(null, other) ) {
                throw new ArgumentNullException( "other" );
            }

            if ( other.Width != Width || other.Height != Height ) {
                throw new ArgumentOutOfRangeException( "Matrix Dimensions" );
            }
            #endregion

            DoAdd( other );
        }

        /// <summary>
        /// Adds float value to this matrix.
        /// </summary>
        /// <param name="value">The float value to add to this matrix.</param>
        public virtual void Add( float value )
        {
            DoAdd( value );
        }

        /// <summary>
        /// Adds int value to this matrix.
        /// </summary>
        /// <param name="value">The int value to add to this matrix.</param>
        public virtual void Add( int value )
        {
            DoAdd( value );
        }

        /// <summary>
        /// Add Vector to specific Row.
        /// </summary>
        /// <param name="vector">The Selected Row</param>
        /// <param name="RowIndex">The index of the row</param>
        public virtual void AddRow( FxMaths.Vector.FxVector<T> vector, int RowIndex )
        {
            DoAddRow( vector, RowIndex );
        }

        /// <summary>
        /// Add Vector to specific Column.
        /// </summary>
        /// <param name="vector">The Selected Column</param>
        /// <param name="RowIndex">The index of the Column</param>
        public virtual void AddCol( FxMaths.Vector.FxVector<T> vector, int ColIndex )
        {
            DoAddCol( vector, ColIndex );
        }


        #endregion





        #region Subtract
        /// <summary>
        /// Subtracts another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to Subtract to this matrix.</param>
        /// <param name="result">The matrix to store the result of the Subtractition.</param>
        protected abstract void DoSubtract( FxMatrix<T> other, FxMatrix<T> result );

        /// <summary>
        /// Subtracts another matrix to this matrix.
        /// Change this Matrix.
        /// </summary>
        /// <param name="other">The matrix to Subtract to this matrix.</param>
        protected abstract void DoSubtract( FxMatrix<T> other );
        protected abstract void DoSubtract( float value );
        protected abstract void DoSubtract( int result );

        /// <summary>
        /// Subtract Vector to specific row
        /// </summary>
        /// <param name="vector"></param>
        protected abstract void DoSubtractRow( FxMaths.Vector.FxVector<T> vector, int RowIndex );

        /// <summary>
        /// Subtract Vector to specific Column
        /// </summary>
        /// <param name="vector"></param>
        protected abstract void DoSubtractCol( FxMaths.Vector.FxVector<T> vector, int ColIndex );

        /// <summary>
        /// Subtracts another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to Subtract to this matrix.</param>
        public virtual void Subtract( FxMatrix<T> other )
        {
            #region Exceptions
            if (object.ReferenceEquals(null, other))
            {
                throw new ArgumentNullException( "other" );
            }

            if ( other.Width != Width || other.Height != Height ) {
                throw new ArgumentOutOfRangeException( "Matrix Dimensions" );
            }
            #endregion

            DoSubtract( other );
        }

        /// <summary>
        /// Subtracts float value to this matrix.
        /// </summary>
        /// <param name="value">The float value to Subtract to this matrix.</param>
        public virtual void Subtract( float value )
        {
            DoSubtract( value );
        }

        /// <summary>
        /// Subtracts int value to this matrix.
        /// </summary>
        /// <param name="value">The int value to Subtract to this matrix.</param>
        public virtual void Subtract( int value )
        {
            DoSubtract( value );
        }

        /// <summary>
        /// Subtract Vector to specific Row.
        /// </summary>
        /// <param name="vector">The Selected Row</param>
        /// <param name="RowIndex">The index of the row</param>
        public virtual void SubtractRow( Vector.FxVector<T> vector, int RowIndex )
        {
            DoSubtractRow( vector, RowIndex );
        }

        /// <summary>
        /// Subtract Vector to specific Column.
        /// </summary>
        /// <param name="vector">The Selected Column</param>
        /// <param name="RowIndex">The index of the Column</param>
        public virtual void SubtractCol( Vector.FxVector<T> vector, int ColIndex )
        {
            DoSubtractCol( vector, ColIndex );
        }
        #endregion





        #region Multiply


        #region Multiply with Matrix

        /// <summary>
        /// Multiplys another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to Multiply to this matrix.</param>
        /// <param name="result">The matrix to store the result of the Multiplyition.</param>
        protected abstract void DoMultiply( FxMatrix<T> other, FxMatrix<T> result );

        /// <summary>
        /// Multiplys another matrix to this matrix.
        /// The result is not inplace.
        /// </summary>
        /// <param name="other">The matrix to Multiply to this matrix.</param>
        /// <returns>The result of the multiplication</returns>
        public virtual FxMatrix<T> Multiply( FxMatrix<T> other )
        {
            #region Exceptions
            if (object.ReferenceEquals(null, other))
            {
                throw new ArgumentNullException( "other" );
            }

            if ( other.Height != Width ) {
                throw new ArgumentOutOfRangeException( "Matrix Dimensions" );
            }
            #endregion

            FxMatrix<T> result = AllocateMatrix(other.Width,Height);

            DoMultiply( other , result );

            return result;
        }

        /// <summary>
        /// Multiplys another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to Multiply to this matrix.</param>
        /// <param name="result">The result of the multiplication</param>
        public virtual void Multiply( FxMatrix<T> other , FxMatrix<T> result)
        {
            #region Exceptions
            if (object.ReferenceEquals(null, other))
            {
                throw new ArgumentNullException( "other" );
            }

            if ( other.Width != Width || other.Height != Height ) {
                throw new ArgumentOutOfRangeException( "Matrix Dimensions" );
            }
            #endregion

            DoMultiply( other, result );
        }

        #endregion




        #region Multiply with scalar

        protected abstract void DoMultiply( float value );
        protected abstract void DoMultiply( int result );

        /// <summary>
        /// Multiplys float value to this matrix.
        /// </summary>
        /// <param name="value">The float value to Multiply to this matrix.</param>
        public virtual void Multiply( float value )
        {
            DoMultiply( value );
        }

        /// <summary>
        /// Multiplys int value to this matrix.
        /// </summary>
        /// <param name="value">The int value to Multiply to this matrix.</param>
        public virtual void Multiply( int value )
        {
            DoMultiply( value );
        }
        #endregion




        #region Multiply with vector

        protected abstract void DoMultiply( FxMaths.Vector.FxVector<T> rightSide, FxMaths.Vector.FxVector<T> result );

        /// <summary>
        /// Right multiply  of vector to this matrix.
        /// (Result) = (Matrix)*(Vector)
        /// </summary>
        public virtual FxMaths.Vector.FxVector<T> Multiply( FxMaths.Vector.FxVector<T> rightSide )
        {
            #region Exceptions
            if ( rightSide == null ) {
                throw new ArgumentNullException( "other" );
            }

            if ( rightSide.Size != Width ) {
                throw new ArgumentOutOfRangeException( "Matrix/Vector Dimensions" );
            }
            #endregion

            // allocate the result vector
            FxMaths.Vector.FxVector<T> result = AllocateVector( Height );

            DoMultiply( rightSide, result );

            return result;
        }

        protected abstract void DoLeftMultiply( FxMaths.Vector.FxVector<T> leftSide, FxMaths.Vector.FxVector<T> result );

        /// <summary>
        /// Left multiply  of vector to this matrix.
        /// (Result) = (Vector)*(Matrix)
        /// </summary>
        public virtual FxMaths.Vector.FxVector<T> MultiplyLeft( FxMaths.Vector.FxVector<T> leftSide )
        {

            #region Exceptions
            if ( leftSide == null ) {
                throw new ArgumentNullException( "other" );
            }

            if ( leftSide.Size != Height) {
                throw new ArgumentOutOfRangeException( "Matrix/Vector Dimensions" );
            }
            #endregion


            // allocate the result vector
            FxMaths.Vector.FxVector<T> result = AllocateVector( Width );

            DoLeftMultiply( leftSide, result );

            return result;
        }

        #endregion




        #region Pointwise

        /// <summary>
        /// Pointwise multiplies this matrix with another matrix and return the result into new matrix.
        /// </summary>
        /// <param name="other">The matrix to pointwise multiply with this one.</param>
        /// <param name="result">The return matrix with the results</param>
        public virtual void MultiplyPointwise( FxMatrix<T> other, FxMatrix<T> result )
        {

            #region Exceptions
            if (object.ReferenceEquals(null, other))
            {
                throw new ArgumentNullException( "other" );
            }

            if ( other.Height != Height || other.Width != Width ) {
                throw new ArgumentOutOfRangeException( "Matrix Dimensions" );
            }
            #endregion

            DoMultiplyPointwise( other, result );
        }

        /// <summary>
        /// Pointwise multiplies this matrix with another matrix and store the result in this matrix.
        /// </summary>
        /// <param name="other">The matrix to pointwise multiply with this one.</param>
        public virtual void MultiplyPointwise( FxMatrix<T> other )
        {

            #region Exceptions
            if (object.ReferenceEquals(null, other))
            {
                throw new ArgumentNullException( "other" );
            }

            if ( other.Height != Height || other.Width != Width ) {
                throw new ArgumentOutOfRangeException( "Matrix Dimensions" );
            }
            #endregion

            DoMultiplyPointwise( other );
        }

        protected abstract void DoMultiplyPointwise( FxMatrix<T> other, FxMatrix<T> result );
        protected abstract void DoMultiplyPointwise( FxMatrix<T> other);

        #endregion


        #endregion






        #region Divide 

        
        #region With scalar

        protected abstract void DoDivide(float value);
        protected abstract void DoDivide(int result);

        /// <summary>
        /// Divides float value to this matrix.
        /// </summary>
        /// <param name="value">The float value to Divide to this matrix.</param>
        public virtual void Divide(float value)
        {
            DoDivide(value);
        }

        /// <summary>
        /// Divides int value to this matrix.
        /// </summary>
        /// <param name="value">The int value to Divide to this matrix.</param>
        public virtual void Divide(int value)
        {
            DoDivide(value);
        }


        public static FxMatrix<T> operator /(FxMatrix<T> mat, float value)
        {
            FxMatrix<T> newMat = mat.AllocateCopyMatrix();
            newMat.Divide(value);
            return newMat;
        }

        #endregion


        #region Pointwise

        /// <summary>
        /// Pointwise division this matrix with another matrix and return the result into new matrix.
        /// </summary>
        /// <param name="other">The matrix to pointwise Divide with this one.</param>
        /// <param name="result">The return matrix with the results</param>
        public virtual void DividePointwise(FxMatrix<T> other, FxMatrix<T> result)
        {

            #region Exceptions
            if ( object.ReferenceEquals(null, other) )
            {
                throw new ArgumentNullException("other");
            }

            if (other.Height != Height || other.Width != Width)
            {
                throw new ArgumentOutOfRangeException("Matrix Dimensions");
            }
            #endregion

            DoDividePointwise(other, result);
        }

        /// <summary>
        /// Pointwise division this matrix with another matrix and store the result in this matrix.
        /// </summary>
        /// <param name="other">The matrix to pointwise Divide with this one.</param>
        public virtual void DividePointwise(FxMatrix<T> other)
        {

            #region Exceptions
            if ( object.ReferenceEquals(null, other) )
            {
                throw new ArgumentNullException("other");
            }

            if (other.Height != Height || other.Width != Width)
            {
                throw new ArgumentOutOfRangeException("Matrix Dimensions");
            }
            #endregion

            DoDividePointwise(other);
        }

        protected abstract void DoDividePointwise(FxMatrix<T> other, FxMatrix<T> result);
        protected abstract void DoDividePointwise(FxMatrix<T> other);


        #endregion


        #endregion





        #region Negate

        protected abstract void DoNegate();

        /// <summary>
        /// Negate each element of this matrix.
        /// </summary>
        public virtual void Negate()
        {
            DoNegate();
        }

        #endregion





        #region Transpose

        /// <summary>
        /// Transpose the matrix
        /// </summary>
        public virtual void Transpose()
        {
            // allocate a new array for the data's
            T []NewData= new T[Size];

            // copy the data in tranpose 
            for ( int x=0; x < Width; x++ )
                for ( int y=0; y < Height; y++ )
                    NewData[x * Height + y] = Data[y * Width + x];

            // swap the sizes
            int oldW = Width;
            Width = Height;
            Height = oldW;

            // change the data
            Data = NewData;
        }

        #endregion



        #endregion



        #region Statistics

        /// <summary>
        /// Get the sum of all values
        /// </summary>
        /// <returns></returns>
        public abstract T Sum();

        /// <summary>
        /// Get the mean of all values
        /// </summary>
        /// <returns></returns>
        public abstract T Mean();

        /// <summary>
        /// Get the std of all values
        /// </summary>
        /// <returns></returns>
        public abstract T Std();

        /// <summary>
        /// Get the variamce of all values
        /// </summary>
        /// <returns></returns>
        public abstract T Variance();


        /// <summary>
        /// Get the normal of the matrix 
        /// base on different's metrics
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract T Norm( NormMatrixType type );

        #endregion



        #region Inverse/Determinal/LUFactor


        /// <summary>
        /// Computes the LUP factorization of A. P*A = L*U.
        /// The matrix is overwritten with the  the LU factorization on exit. The lower triangular factor L is 
        /// stored in under the diagonal of internal matrix (the diagonal is always 1.0
        /// for the L factor). The upper triangular factor U is stored on and above the diagonal of internal matrix
        /// </summary>
        /// <returns>It contains the pivot indices. The size of the vector is the order of the table</returns>
        public abstract Vector.FxVectorI LU();


        #endregion




        #region Override 
        public override string ToString()
        {
            StringBuilder strB = new StringBuilder();

            for (int y = 0; y < Height; y++)
            {
                int end = (y + 1) * Width;
                for (int i = y * Width; i < end; i++)
                    strB.Append(Data[i].ToString() + "\t");

                strB.Append("\r\n");
            }

            return strB.ToString();
        }

        #endregion

    }
}
