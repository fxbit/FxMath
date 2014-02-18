using FxMaths.GMaps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FxMaths.Vector
{
    public struct FxVector2b : IEquatable<FxVector2b>, IVertex<byte>
    {
        #region Public Variables

        /// <summary>
        /// Gets or sets the X component of the vector.
        /// </summary>
        /// <value>The X component of the vector.</value>
        public byte x;

        /// <summary>
        /// Gets or sets the Y component of the vector.
        /// </summary>
        /// <value>The Y component of the vector.</value>
        public byte y;

        #endregion


        #region Contractors

        /// <summary>
        /// Init the vector with the value
        /// </summary>
        public FxVector2b( byte value )
        {
            this.x = value;
            this.y = value;
        }

        /// <summary>
        /// Init the vector with the value
        /// </summary>
        public FxVector2b( byte x, byte y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Init the vector with the value
        /// </summary>
        public FxVector2b( int x, int y )
        {
            this.x = (byte)x;
            this.y = (byte)y;
        }
        #endregion

        #region Get/Set Values

        public byte this[int index]
        {
            get
            {
                switch ( index ) {
                    case 0: return X;
                    case 1: return Y;
                }

                return 0;
            }

            set
            {
                switch ( index ) {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                }
            }

        }

        #endregion

        #region Math calcuations

        #region Add

        #region Static Function
        public static FxVector2b Add( FxVector2b left, FxVector2b right )
        {
            return new FxVector2b( left.X + right.X, left.Y + right.Y);
        }
        #endregion

        #region Local Function
        public void Add( FxVector2b value )
        {
            X += value.X;
            Y += value.Y;
        }
        #endregion

        #endregion

        #region Subtract
        #region Static Function
        public static FxVector2b Subtract( FxVector2b left, FxVector2b right )
        {
            return new FxVector2b( left.X - right.X, left.Y - right.Y);
        }
        #endregion

        #region Local Function
        public void Subtract( FxVector2b value )
        {
            X -= value.X;
            Y -= value.Y;
        }
        #endregion
        #endregion

        #region Modulate

        #region static Functions
        public static FxVector2b Modulate( FxVector2b left, FxVector2b right )
        {
            return new FxVector2b( left.X * right.X, left.Y * right.Y );
        }
        #endregion

        #region Local Functions
        public void Modulate( FxVector2b value )
        {
            X *= value.X;
            Y *= value.Y;
        }
        #endregion

        #endregion

        #region Multiply

        #region static Functions
        public static FxVector2b Multiply( FxVector2b left, float scale )
        {
            return new FxVector2b( (byte)(left.X * scale), (byte)(left.Y * scale));
        }

        public static FxVector2b Multiply( FxVector2b left, int scale )
        {
            return new FxVector2b( left.X * scale, left.Y * scale);
        }

        public static FxVector2b Multiply( FxVector2b left, byte scale )
        {
            return new FxVector2b( left.X * scale, left.Y * scale);
        }
        #endregion

        #region Local Function
        public void Multiply( float scale )
        {
            X = (byte)( X * scale );
            Y = (byte)( Y * scale );
        }

        public void Multiply( int scale )
        {
            X = (byte)( X * scale );
            Y = (byte)( Y * scale );
        }

        public void Multiply( byte scale )
        {
            X *= scale;
            Y *= scale;
        }
        #endregion

        #endregion

        #region Divide

        #region Static Functions
        public static FxVector2b Divide( FxVector2b left, float scale )
        {
            return new FxVector2b( (byte)( left.X / scale ), (byte)( left.Y / scale ));
        }

        public static FxVector2b Divide( FxVector2b left, int scale )
        {
            return new FxVector2b( (byte)( left.X / scale ), (byte)( left.Y / scale ));
        }

        public static FxVector2b Divide( FxVector2b left, byte scale )
        {
            return new FxVector2b( (byte)( left.X / scale ), (byte)( left.Y / scale ));
        }
        #endregion

        #region Local Functions
        public void Divide( float scale )
        {
            X = (byte)( X / scale ); 
            Y = (byte)( Y / scale ); 
        }

        public void Divide( int scale )
        {
            X = (byte)( X / scale ); 
            Y = (byte)( Y / scale ); 
        }

        public void Divide( byte scale )
        {
            X /= scale; Y /= scale;
        }
        #endregion

        #endregion

        #region Nagate

        #region static Function
        public static FxVector2b Negate( FxVector2b value )
        {
            return new FxVector2b( -value.X, -value.Y);
        }
        #endregion

        #region Local Function
        public void Negate()
        {
            X = (byte)(-X); Y = (byte)(-Y);
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
        public FxVector2b Clamp( FxVector2b value, FxVector2b min, FxVector2b max )
        {
            byte x = value.X;
            x = ( x > max.X ) ? max.X : x;
            x = ( x < min.X ) ? min.X : x;

            byte y = value.Y;
            y = ( y > max.Y ) ? max.Y : y;
            y = ( y < min.Y ) ? min.Y : y;

            return new FxVector2b( x, y);
        }

        /// <summary>
        /// Restricts the vector to be within a specified range.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public void Clamp( FxVector2b min, FxVector2b max )
        {
            X = ( X > max.X ) ? max.X : X;
            X = ( X < min.X ) ? min.X : X;

            Y = ( Y > max.Y ) ? max.Y : Y;
            Y = ( Y < min.Y ) ? min.Y : Y;
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
        public static FxVector2b Lerp( FxVector2b start, FxVector2b end, float factor )
        {
            return new FxVector2b( (byte)( start.X + ( ( end.X - start.X ) * factor ) ),
                                   (byte)( start.Y + ( ( end.Y - start.Y ) * factor ) ));
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
        public static float Distance( FxVector2b start, FxVector2b end )
        {
            float x = start.X - end.X;
            float y = start.Y - end.Y;

            return (float)Math.Sqrt( ( x * x ) + ( y * y ));
        }
        #endregion

        #region Local Func
        /// <summary>
        /// Calculates the distance between this vector and one other.
        /// </summary>
        /// <param name="end">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float Distance( FxVector2b end )
        {
            float x = X - end.X;
            float y = Y - end.Y;

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
        public static float DistanceSquared( FxVector2b start, FxVector2b end )
        {
            float x = start.X - end.X;
            float y = start.Y - end.Y;

            return ( x * x ) + ( y * y ) ;
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
        public float DistanceSquared( FxVector2b end )
        {
            float x = X - end.X;
            float y = Y - end.Y;

            return ( x * x ) + ( y * y );
        }
        #endregion

        #endregion

        #region Dot

        #region  Static Func
        public static float Dot( FxVector2b left, FxVector2b right )
        {
            return ( left.X * right.X + left.Y * right.Y);
        }
        #endregion

        #region Local Func
        public float Dot( FxVector2b vec )
        {
            return ( X * vec.X + Y * vec.Y);
        }
        #endregion

        #endregion

        #region Normalize

        #region Static Func
        public static FxVector2b Normalize( FxVector2b vector )
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
            X = (byte)( X * num );
            Y = (byte)( Y *num);
        }
        #endregion

        #endregion

        #region Length

        public float Length()
        {
            return (float)Math.Sqrt( X * X + Y * Y );
        }

        #endregion

        #region LengthSquared

        public float LengthSquared()
        {
            return X * X + Y * Y ;
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
        public static FxVector2b operator +( FxVector2b left, FxVector2b right )
        {
            return Add( left, right );
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static FxVector2b operator -( FxVector2b left, FxVector2b right )
        {
            return Subtract( left, right );
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static FxVector2b operator -( FxVector2b value )
        {
            return Negate( value );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2b operator *( FxVector2b vector, float scale )
        {
            return Multiply( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2b operator *( FxVector2b vector, int scale )
        {
            return Multiply( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2b operator *( FxVector2b vector, byte scale )
        {
            return Multiply( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2b operator *( float scale, FxVector2b vector )
        {
            return Multiply( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2b operator /( FxVector2b vector, float scale )
        {
            return Divide( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2b operator /( FxVector2b vector, int scale )
        {
            return Divide( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2b operator /( FxVector2b vector, byte scale )
        {
            return Divide( vector, scale );
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==( FxVector2b left, FxVector2b right )
        {
            return Equals( left, right );
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=( FxVector2b left, FxVector2b right )
        {
            return !Equals( left, right );
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

            return Equals( (FxVector2b)( value ) );
        }

        /// <summary>
        /// Returns a value that indicates whether the current instance is equal to the specified object. 
        /// </summary>
        /// <param name="other">Object to make the comparison with.</param>
        /// <returns><c>true</c> if the current instance is equal to the specified object; <c>false</c> otherwise.</returns>
        public bool Equals( FxVector2b value )
        {
            return ( X == value.X && Y == value.Y );
        }

        /// <summary>
        /// Determines whether the specified object instances are considered equal. 
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="value1"/> is the same instance as <paramref name="value2"/> or 
        /// if both are <c>null</c> references or if <c>value1.Equals(value2)</c> returns <c>true</c>; otherwise, <c>false</c>.</returns>
        public static bool Equals( FxVector2b value1, FxVector2b value2 )
        {
            return ( value1.X == value2.X && value1.Y == value2.Y );
        }
        #endregion


        #region IVertex Members 
        
        /// <summary>
        /// Gets or sets the X component of the vector.
        /// </summary>
        /// <value>The X component of the vector.</value>
        public byte X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        /// <summary>
        /// Gets or sets the Y component of the vector.
        /// </summary>
        /// <value>The Y component of the vector.</value>
        public byte Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }


        [Browsable(false)]
        public byte Z
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
        public void WriteToDataStream(SharpDX.DataStream dataStream)
        {
            // write the data to the stream
            dataStream.Write<byte>(this.x);
            dataStream.Write<byte>(this.y);
        }


        /// <summary>
        /// Read the vertex from the data stream
        /// </summary>
        /// <param name="dataStream"></param>
        public void ReadFromDataStream(SharpDX.DataStream dataStream)
        {
            // read the data from the stream
            x = dataStream.Read<byte>();
            y = dataStream.Read<byte>();
        }


        /// <summary>
        /// Read the vertex from the data stream
        /// </summary>
        /// <param name="dataStream"></param>
        public IVertex<byte> Copy()
        {
            return new FxVector2b(x, y);
        }

        public float Dot(IVertex<byte> vec)
        {
            return (x * vec.X + y * vec.Y);
        }

        public void Subtract(IVertex<byte> vec)
        {
            x -= vec.X;
            y -= vec.Y;
        }

        public float Distance(IVertex<byte> end)
        {
            int x = this.X - end.X;
            int y = this.Y - end.Y;

            return (byte)Math.Sqrt((x * x) + (y * y));
        }

        public override string ToString()
        {
            return "(" + x.ToString() + "," + y.ToString() + ")";
        }


        public string ToString(string Format)
        {
            return "(" + x.ToString(Format) + "," + y.ToString(Format) + ")";
        }

        public bool Equals(IVertex<byte> other)
        {
            return other.X == x && other.Y == y;
        }

        public bool Equals(IVertex<byte> x, IVertex<byte> y)
        {
            return x.X == y.X && x.Y == y.Y;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public int GetHashCode(IVertex<byte> obj)
        {
            return obj.GetHashCode();
        }

        #endregion


    }
}

