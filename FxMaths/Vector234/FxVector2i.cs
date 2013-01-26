using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using FxMaths.GMaps;

namespace FxMaths.Vector
{
    public struct FxVector2i : IEquatable<IVertex<int>>, IEqualityComparer<IVertex<int>>, IVertex<int>
    {
        #region Public Variables

        /// <summary>
        /// Gets or sets the X component of the vector.
        /// </summary>
        /// <value>The X component of the vector.</value>
        public int x;

        /// <summary>
        /// Gets or sets the Y component of the vector.
        /// </summary>
        /// <value>The Y component of the vector.</value>
        public int y;

        #endregion

        #region Contractors

        /// <summary>
        /// Init the vector with the value
        /// </summary>
        public FxVector2i( int value )
        {
            this.x = value;
            this.y = value;
        }

        /// <summary>
        /// Init the vector with the value
        /// </summary>
        public FxVector2i( int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Init the vector with the value
        /// </summary>
        public FxVector2i( float x, float y)
        {
            this.x = (int)x;
            this.y = (int)y;
        }

        #endregion

        #region Get/Set Values

        public int this[int index]
        {
            get
            {
                switch ( index ) {
                    case 0: return x;
                    case 1: return y;
                }

                return 0;
            }

            set
            {
                switch ( index ) {
                    case 0: x = value; break;
                    case 1: y = value; break;
                }
            }

        }

        #endregion

        #region Math calcuations

        #region Add

        #region Static Function
        public static FxVector2i Add( FxVector2i left, FxVector2i right )
        {
            return new FxVector2i( left.x + right.x, left.y + right.y);
        }
        #endregion

        #region Local Function
        public void Add( FxVector2i value )
        {
            x += value.x;
            y += value.y;
        }
        #endregion

        #endregion

        #region Subtract
        #region Static Function
        public static FxVector2i Subtract( FxVector2i left, FxVector2i right )
        {
            return new FxVector2i( left.x - right.x, left.y - right.y);
        }
        #endregion

        #region Local Function
        public void Subtract( IVertex<int> value )
        {
            x -= value.X;
            y -= value.Y;
        }
        #endregion
        #endregion

        #region Modulate

        #region static Functions
        public static FxVector2i Modulate( FxVector2i left, FxVector2i right )
        {
            return new FxVector2i( left.x * right.x, left.y * right.y );
        }
        #endregion

        #region Local Functions
        public void Modulate( FxVector2i value )
        {
            x *= value.x;
            y *= value.y;
        }
        #endregion

        #endregion

        #region Multiply

        #region static Functions
        public static FxVector2i Multiply( FxVector2i left, float scale )
        {
            return new FxVector2i( left.x * scale, left.y * scale);
        }

        public static FxVector2i Multiply( FxVector2i left, int scale )
        {
            return new FxVector2i( left.x * scale, left.y * scale);
        }

        public static FxVector2i Multiply( FxVector2i left, byte scale )
        {
            return new FxVector2i( left.x * scale, left.y * scale);
        }
        #endregion

        #region Local Function
        public void Multiply( float scale )
        {
            x = (int)( x * scale );
            y = (int)( y * scale );
        }

        public void Multiply( int scale )
        {
            x *= scale;
            y *= scale;
        }

        public void Multiply( byte scale )
        {
            x *= scale;
            y *= scale;
        }
        #endregion

        #endregion

        #region Divide

        #region Static Functions
        public static FxVector2i Divide( FxVector2i left, float scale )
        {
            return new FxVector2i( left.x / scale, left.y / scale );
        }

        public static FxVector2i Divide( FxVector2i left, int scale )
        {
            return new FxVector2i( left.x / scale, left.y / scale);
        }

        public static FxVector2i Divide( FxVector2i left, byte scale )
        {
            return new FxVector2i( left.x / scale, left.y / scale);
        }
        #endregion

        #region Local Functions
        public void Divide( float scale )
        {
            x = (int)( x / scale ); y = (int)( y / scale );
        }

        public void Divide( int scale )
        {
            x /= scale;y /= scale;
        }

        public void Divide( byte scale )
        {
            x /= scale; y /= scale;
        }
        #endregion

        #endregion

        #region Nagate

        #region static Function
        public static FxVector2i Negate( FxVector2i value )
        {
            return new FxVector2i( -value.x, -value.y );
        }
        #endregion

        #region Local Function
        public void Negate()
        {
            x = -x; y = -y;
        }
        #endregion

        #endregion

        #endregion

        #region Special Functions

        #region Clamp
        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public FxVector2i Clamp( FxVector2i value, FxVector2i min, FxVector2i max )
        {
            float x = value.x;
            x = ( x > max.x ) ? max.x : x;
            x = ( x < min.x ) ? min.x : x;

            float y = value.y;
            y = ( y > max.y ) ? max.y : y;
            y = ( y < min.y ) ? min.y : y;

            return new FxVector2i( x, y);
        }

        /// <summary>
        /// Restricts the vector to be within a specified range.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public void Clamp( FxVector2i min, FxVector2i max )
        {
            x = ( x > max.x ) ? max.x : x;
            x = ( x < min.x ) ? min.x : x;

            y = ( y > max.y ) ? max.y : y;
            y = ( y < min.y ) ? min.y : y;
        }
        #endregion

        #region Lerp
        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="start">Start vector.</param>
        /// <param name="end">End vector.</param>
        /// <param name="factor">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the linear interpolation of the two vectors.</param>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula.
        /// <code>start + (end - start) * factor</code>
        /// Passing <paramref name="factor"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned. 
        /// </remarks>
        public static FxVector2i Lerp( FxVector2i start, FxVector2i end, float factor )
        {
            return new FxVector2i( start.x + ( ( end.x - start.x ) * factor ), 
                                   start.y + ( ( end.y - start.y ) * factor ));
        }
        #endregion

        #endregion

        #region vector functions

        #region Distance

        #region Static Func
        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="start">The first vector.</param>
        /// <param name="end">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public static float Distance( FxVector2i start, FxVector2i end )
        {
            float x = start.x - end.x;
            float y = start.y - end.y;

            return (float)Math.Sqrt( ( x * x ) + ( y * y ));
        }
        #endregion

        #region Local Func
        /// <summary>
        /// Calculates the distance between this vector and one other.
        /// </summary>
        /// <param name="end">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float Distance(IVertex<int> end)
        {
            float x = this.X - end.X;
            float y = this.Y - end.Y;

            return (float)Math.Sqrt( ( x * x ) + ( y * y ) );
        }
        #endregion

        #endregion

        #region DistanceSquared

        #region Static Func
        /// <summary>
        /// Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="start">The first vector.</param>
        /// <param name="end">The second vector.</param>
        /// <returns>The squared distance between the two vectors.</returns>
        /// <remarks>Distance squared is the value before taking the square root. 
        /// </remarks>
        public static float DistanceSquared( FxVector2i start, FxVector2i end )
        {
            float x = start.x - end.x;
            float y = start.y - end.y;

            return ( x * x ) + ( y * y );
        }
        #endregion

        #region Local Func
        /// <summary>
        /// Calculates the squared distance between this vector and one other.
        /// </summary>
        /// <param name="end">The second vector.</param>
        /// <returns>The squared distance between the two vectors.</returns>
        /// <remarks>Distance squared is the value before taking the square root. 
        /// </remarks>
        public float DistanceSquared( FxVector2i end )
        {
            float x = this.x - end.x;
            float y = this.y - end.y;

            return ( x * x ) + ( y * y );
        }
        #endregion

        #endregion

        #region Dot

        #region  Static Func
        public static float Dot( FxVector2i left, FxVector2i right )
        {
            return ( left.x * right.x + left.y * right.y );
        }
        #endregion

        #region Local Func
        public float Dot( FxMaths.GMaps.IVertex<int> vec )
        {
            return ( x * vec.X + y * vec.Y);
        }
        #endregion

        #endregion

        #region Normalize

        #region Static Func
        public static FxVector2i Normalize( FxVector2i vector )
        {
            vector.Normalize();
            return vector;
        }
        #endregion

        #region Local Func
        public void Normalize()
        {
            float length = Length();
            if ( length == 0 )
                return;
            float num = 1 / length;
            Multiply( num );
        }
        #endregion

        #endregion

        #region Length

        public float Length()
        {
            return (float)Math.Sqrt( x * x + y * y);
        }

        #endregion

        #region LengthSquared

        public float LengthSquared()
        {
            return x * x + y * y;
        }

        #endregion

        #endregion

        #region Overite operations
        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static FxVector2i operator +( FxVector2i left, FxVector2i right )
        {
            return Add( left, right );
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static FxVector2i operator -( FxVector2i left, FxVector2i right )
        {
            return Subtract( left, right );
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static FxVector2i operator -( FxVector2i value )
        {
            return Negate( value );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2i operator *( FxVector2i vector, float scale )
        {
            return Multiply( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2i operator *( FxVector2i vector, int scale )
        {
            return Multiply( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2i operator *( FxVector2i vector, byte scale )
        {
            return Multiply( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2i operator *( float scale, FxVector2i vector )
        {
            return Multiply( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2i operator /( FxVector2i vector, float scale )
        {
            return Divide( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2i operator /( FxVector2i vector, int scale )
        {
            return Divide( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2i operator /( FxVector2i vector, byte scale )
        {
            return Divide( vector, scale );
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==( FxVector2i left, FxVector2i right )
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=( FxVector2i left, FxVector2i right )
        {
            return !left.Equals(right);
        }
        #endregion

        #region Equals Functions

        /// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() + y.GetHashCode();

        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public int GetHashCode(IVertex<int> obj)
        {
            return obj.GetHashCode();
        }


        /// <summary>
        /// Returns a value that indicates whether the current instance is equal to a specified object. 
        /// </summary>
        /// <param name="obj">Object to make the comparison with.</param>
        /// <returns><c>true</c> if the current instance is equal to the specified object; <c>false</c> otherwise.</returns>
        public override bool Equals( Object value )
        {
            if ( value == null )
                return false;

            if ( value.GetType() != GetType() )
                return false;

            return Equals( (FxVector2i)( value ) );
        }

        /// <summary>
        /// Returns a value that indicates whether the current instance is equal to the specified object. 
        /// </summary>
        /// <param name="other">Object to make the comparison with.</param>
        /// <returns><c>true</c> if the current instance is equal to the specified object; <c>false</c> otherwise.</returns>
        public bool Equals( IVertex<int> value )
        {
            return ( x == value.X && y == value.Y );
        }

        /// <summary>
        /// Determines whether the specified object instances are considered equal. 
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="value1"/> is the same instance as <paramref name="value2"/> or 
        /// if both are <c>null</c> references or if <c>value1.Equals(value2)</c> returns <c>true</c>; otherwise, <c>false</c>.</returns>
        public bool Equals( IVertex<int> value1, IVertex<int> value2 )
        {
            return ( value1.X == value2.X && value1.Y == value2.Y  );
        }
        #endregion

        #region IVertex<int> Members

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                this.x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                this.y = value;
            }
        }

        public int Z
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }

        /// <summary>
        /// Write the vertex to the data stream
        /// </summary>
        /// <param name="dataStream"></param>
        public void WriteToDataStream( DataStream dataStream )
        {

            // write the data to the stream
            dataStream.Write<int>( this.x );
            dataStream.Write<int>( this.y );

        }


        /// <summary>
        /// Read the vertex from the data stream
        /// </summary>
        /// <param name="dataStream"></param>
        public void ReadFromDataStream( DataStream dataStream )
        {
            // read the data from the stream
            x = dataStream.Read<int>();
            y = dataStream.Read<int>();
        }

        /// <summary>
        /// Read the vertex from the data stream
        /// </summary>
        /// <param name="dataStream"></param>
        public static IVertex<int> sReadFromDataStream(DataStream dataStream)
        {
            return new FxVector2i(dataStream.Read<int>(), dataStream.Read<int>());
        }


        /// <summary>
        /// Return a copy of the vertex.
        /// </summary>
        /// <returns></returns>
        public FxMaths.GMaps.IVertex<int> Copy()
        {
            return new FxVector2i( this.x, this.y );
        }

        #endregion

        #region String Utils

        public override string ToString()
        {
            return "(" + x.ToString().Replace(',', '.') + "," + y.ToString().Replace(',', '.') + ")";
        }


        public string ToString(String Format)
        {
            return "(" + x.ToString(Format).Replace(',', '.') + "," + y.ToString(Format).Replace(',', '.') + ")";
        }


        #endregion

    }
}
