using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.Vector
{
    [Serializable]
    public abstract class FxVector<T> where T : struct, IEquatable<T>
    {

        #region Size Parameters

        /// <summary>
        /// Gets the Width
        /// </summary>
        public Int32 Size;

        #endregion


        #region Internal Variables

        public T []Data;

        #endregion


        #region Constructors

        public FxVector( int Size )
        {
            #region Exceptions
            if ( Size <= 0 ) {
                throw new ArgumentOutOfRangeException( "Size must be positive" );
            }
            #endregion

            // allocate the data
            Data = new T[Size];

            // set the internal size
            this.Size = Size;
        }

        /// <summary>
        /// Allocate the data for Vector
        /// </summary>
        /// <returns></returns>
        protected abstract FxVector<T> AllocateVector( int Size );

        /// <summary>
        /// Padding the internal data with specific value
        /// </summary>
        /// <param name="NumValues"></param>
        /// <param name="inEnd"></param>
        /// <param name="value"></param>
        public void Padding( int NumValues, Boolean inEnd , T value )
        {
            // store the old data
            T[] tmpData = Data;

            //  create a new data with the new size
            Data = new T[NumValues + tmpData.Length];

            // set the new size of the vector
            this.Size = Data.Length;

            // calc the start and end point 
            int startPoint = ( inEnd ) ? NumValues : 0;
            int endPoint =   ( inEnd ) ? Data.Length : tmpData.Length;

            // copy the old data to the new one
            for ( int i=startPoint; i < endPoint; i++ ) {
                Data[i] = tmpData[i - startPoint];
            }

            // reset the data to new areas
            startPoint = ( inEnd ) ? 0 : tmpData.Length;
            endPoint = (( inEnd ) ? NumValues : Data.Length);
            for ( int i=startPoint; i < endPoint; i++ ) {
                Data[i] = value;
            }
        }

        #endregion


        #region Get/Set values

        /// <summary>
        /// Get The Value in specific position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public virtual T GetValue( int index )
        {
            return this.Data[index];
        }

        /// <summary>
        /// Set The Value in specific position.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="Value"></param>
        public virtual void SetValue( int index, T Value )
        {
            this.Data[index] = Value;
        }

        /// <summary>
        /// Set all the data to specific value
        /// </summary>
        /// <param name="value"></param>
        public void SetValue( T value )
        {
            this.Data.Fill<T>(value);
        }

        /// <summary>
        /// Set the values from array of the same size
        /// </summary>
        /// <param name="data"></param>
        public void SetValue( T[] data )
        {
            int copySize = (data.Length > Size) ? Size : data.Length;
            Array.Copy(data, this.Data, copySize);
        }


        /// <summary>
        /// Set the values from other vector of the same size
        /// </summary>
        /// <param name="data"></param>
        public void SetValue( FxVector<T> data )
        {
            int copySize = (data.Size > Size) ? Size : data.Size;
            Array.Copy(data.Data, this.Data, copySize);
        }

        /// <summary>
        /// Get the values in array of the same size
        /// </summary>
        /// <param name="data"></param>
        public void GetValue( ref T[] data )
        {
            int copySize = (data.Length > Size) ? Size : data.Length;
            Array.Copy(this.Data, data, copySize);
        }


        /// <summary>
        /// Fill the data from external list.
        /// </summary>
        /// <typeparam name="K">The type of given list</typeparam>
        /// <param name="list"></param>
        /// <param name="func">A function that transform the list object to T</param>
        public void Fill<K>(List<K> list, Func<K, T> func)
        {
            int count = (list.Count < Data.Length) ? list.Count : Data.Length;
            for (int i = 0; i < count; i++)
            {
                this.Data[i] = func(list[i]);
            }
        }


        /// <summary>
        /// Fill the data from external list.
        /// </summary>
        /// <typeparam name="K">The type of given list</typeparam>
        /// <param name="list"></param>
        /// <param name="func">A function that transform the list object to T</param>
        /// <param name="offset">Offset to the data index of vector</param>
        public void Fill<K>(List<K> list, Func<K, T> func, int offset)
        {
            int count = (list.Count + offset < Data.Length) ? list.Count : Data.Length - offset;
            for (int i = offset; i < count; i++)
            {
                this.Data[i] = func(list[i]);
            }
        }

        /// <summary>
        /// Set/Get internal values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public virtual T this[int index]
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


        public void SetValue(FxVector<T> data, int startIndex)
        {
            int copySize = (data.Size > Size - startIndex) ? Size - startIndex : data.Size;
            Array.Copy(data.Data, 0, this.Data, startIndex, copySize);
        }

        #endregion


        #region Math Functions


        #region Add
        /// <summary>
        /// Adds another Vector to this Vector.
        /// </summary>
        /// <param name="other">The Vector to add to this Vector.</param>
        /// <param name="result">The Vector to store the result of the addition.</param>
        protected abstract void DoAdd( FxVector<T> other, FxVector<T> result );

        /// <summary>
        /// Adds another Vector to this Vector.
        /// Change this Vector.
        /// </summary>
        /// <param name="other">The Vector to add to this Vector.</param>
        protected abstract void DoAdd(FxVector<T> other);
        protected abstract void DoAdd(double value);
        protected abstract void DoAdd( float value );
        protected abstract void DoAdd( int result );

        /// <summary>
        /// Adds another Vector to this Vector.
        /// </summary>
        /// <param name="other">The Vector to add to this Vector.</param>
        public virtual void Add( FxVector<T> other )
        {
            #region Exceptions
            if ( other == null ) {
                throw new ArgumentNullException( "other" );
            }

            if ( other.Size != Size ) {
                throw new ArgumentOutOfRangeException( "Vector Dimensions" );
            }
            #endregion

            DoAdd( other );
        }

        /// <summary>
        /// Adds float value to this Vector.
        /// </summary>
        /// <param name="value">The float value to add to this Vector.</param>
        public virtual void Add( float value )
        {
            DoAdd( value );
        }

        /// <summary>
        /// Adds int value to this Vector.
        /// </summary>
        /// <param name="value">The int value to add to this Vector.</param>
        public virtual void Add( int value )
        {
            DoAdd( value );
        }
        #endregion


        #region Subtract

        #region With Vector

        /// <summary>
        /// Subtracts another Vector to this Vector.
        /// </summary>
        /// <param name="other">The Vector to Subtract to this Vector.</param>
        /// <param name="result">The Vector to store the result of the Subtractition.</param>
        protected abstract void DoSubtract( FxVector<T> other, FxVector<T> result );

        /// <summary>
        /// Subtracts another Vector to this Vector.
        /// Change this Vector.
        /// </summary>
        /// <param name="other">The Vector to Subtract to this Vector.</param>
        protected abstract void DoSubtract( FxVector<T> other );

        /// <summary>
        /// Subtracts another Vector to this Vector.
        /// </summary>
        /// <param name="other">The Vector to Subtract to this Vector.</param>
        public virtual void Subtract( FxVector<T> other )
        {
            #region Exceptions
            if ( other == null ) {
                throw new ArgumentNullException( "other" );
            }

            if ( other.Size != Size ) {
                throw new ArgumentOutOfRangeException( "Vector Dimensions" );
            }
            #endregion

            DoSubtract( other );
        }
        #endregion

        #region With Scalar

        protected abstract void DoSubtract(float value);
        protected abstract void DoSubtract(int result);

        /// <summary>
        /// Subtracts float value to this Vector.
        /// </summary>
        /// <param name="value">The float value to Subtract to this Vector.</param>
        public virtual void Subtract( float value )
        {
            DoSubtract( value );
        }

        /// <summary>
        /// Subtracts int value to this Vector.
        /// </summary>
        /// <param name="value">The int value to Subtract to this Vector.</param>
        public virtual void Subtract( int value )
        {
            DoSubtract( value );
        }

        #endregion

        #endregion


        #region Multiply


        #region Pointwise
        /// <summary>
        /// Pointwise multiplies this vector with another vector and store the result in this vector.
        /// </summary>
        /// <param name="other">The vector to pointwise multiply with this one.</param>
        public abstract void MultiplyPointwise(FxVector<T> other);

        #endregion


        #region With Scalar

        /// <summary>
        /// Multiply the vector with scalar.
        /// </summary>
        /// <param name="value"></param>
        public abstract void Multiply(float value);

        /// <summary>
        /// Multiply the vector with scalar.
        /// </summary>
        /// <param name="value"></param>
        public abstract void Multiply(int value);

        #endregion

        #endregion


        #region Divide


        #region Pointwise
        /// <summary>
        /// Pointwise division this vector with another vector and store the result in this vector.
        /// </summary>
        /// <param name="other">The vector to pointwise Divide with this one.</param>
        public abstract void DividePointwise( FxVector<T> other );

        #endregion


        #region With Scalar

        /// <summary>
        /// Divide the vector with scalar.
        /// </summary>
        /// <param name="value"></param>
        public abstract void Divide( float value );

        /// <summary>
        /// Divide the vector with scalar.
        /// </summary>
        /// <param name="value"></param>
        public abstract void Divide( int value );

        #endregion

        #endregion


        #region Negate

        /// <summary>
        /// Negate each element of this Vector.
        /// </summary>
        public abstract void Negate();

        #endregion


        #endregion


        #region Statistics

        /// <summary>
        /// Get the sum of all elements
        /// </summary>
        /// <returns></returns>
        public abstract T Sum();

        /// <summary>
        /// Get the mean value of all elements
        /// </summary>
        /// <returns></returns>
        public abstract double Mean();

        /// <summary>
        /// Get the standard deviation of all elements
        /// </summary>
        /// <returns></returns>
        public abstract double STD();

        /// <summary>
        /// Get the variance of all elements
        /// </summary>
        /// <returns></returns>
        public abstract double Variance();

        /// <summary>
        /// Get the Max value of the Vector
        /// </summary>
        /// <returns></returns>
        public abstract T Max();

        /// <summary>
        /// Get the Min value of the Vector
        /// </summary>
        /// <returns></returns>
        public abstract T Min();

        /// <summary>
        /// Get the Max value of the Vector
        /// </summary>
        /// /// <param name="MaxIndex"></param>
        /// <returns></returns>
        public abstract T Max(out int MaxIndex);

        /// <summary>
        /// Get the Min value of the Vector
        /// </summary>
        /// <param name="MinIndex"></param>
        /// <returns></returns>
        public abstract T Min(out int MinIndex);

        #endregion


        #region Norms

        /// <summary>
        /// Get the normal of specific type.
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public abstract double Norms( NormVectorType Type );

        /// <summary>
        /// Generic norms of given p
        /// p-norm, p>=1
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public abstract double Norms( double p );

        #endregion


        #region Lenght/Dist

        /// <summary>
        /// Get the distance between this vector and other one.
        /// </summary>
        /// <param name="Other"></param>
        /// <returns></returns>
        public abstract double Distance( FxVector<T> Other );

        /// <summary>
        /// Get the lenght of the vector
        /// </summary>
        /// <returns></returns>
        public virtual double Lenght()
        {
            return Norms( NormVectorType.Euclidean );
        }

        #endregion


        #region To String 

        public override string ToString()
        {
            StringBuilder strB = new StringBuilder();

            int maxPrint = ( Size < 32 ) ? Size : 32;

            for ( int i=0; i < maxPrint; i++ )
                strB.Append( Data[i].ToString() + "\t" );

            return strB.ToString();
        }

        #endregion


        #region Copy/Sub

        /// <summary>
        /// Get a memory copy of the vector
        /// </summary>
        /// <returns></returns>
        public FxVector<T> GetCopy()
        {
            // allocate a new vector with the same size
            FxVector<T> newVector = AllocateVector(this.Size);

            // copy the data
            for ( int i=0; i < Size; i++ ) {
                newVector[i] = Data[i];
            }

            return newVector;
        }


        /// <summary>
        /// Get a part of the vector
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public FxVector<T> GetSubVector( int startIndex, int endIndex )
        {

            #region Exceptions
            if ( startIndex < 0 || startIndex > endIndex ) {
                throw new ArgumentOutOfRangeException( "The start Index must be possitive and lower from endIndex." );
            }

            if ( endIndex > Size ) {
                throw new ArgumentOutOfRangeException( "The endIndex must be lower from the size of vector." );
            }
            #endregion


            // allocate a new vector with the desare size
            FxVector<T> newVector = AllocateVector( endIndex - startIndex);

            // copy the data
            for ( int i=startIndex; i < endIndex; i++ ) {
                newVector[i - startIndex] = Data[i];
            }

            return newVector;

        }
        #endregion


    }
}
