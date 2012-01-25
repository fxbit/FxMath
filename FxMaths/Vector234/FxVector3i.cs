using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.Vector
{
    public struct FxVector3i : IEquatable<FxVector3i>
    {
        #region Public Variables

        /// <summary>
        /// Gets or sets the X component of the vector.
        /// </summary>
        /// <value>The X component of the vector.</value>
        public int X;

        /// <summary>
        /// Gets or sets the Y component of the vector.
        /// </summary>
        /// <value>The Y component of the vector.</value>
        public int Y;

        /// <summary>
        /// Gets or sets the Z component of the vector.
        /// </summary>
        /// <value>The Z component of the vector.</value>
        public int Z;

        #endregion

        #region Public Color Variables
        public int R { get { return X; } set { X = value; } }
        public int G { get { return Y; } set { Y = value; } }
        public int B { get { return Z; } set { Z = value; } }
        #endregion

        #region Contractors

        /// <summary>
        /// Init the vector with the value
        /// </summary>
        public FxVector3i( int value )
        {
            X = value;
            Y = value;
            Z = value;
        }

        /// <summary>
        /// Init the vector with the value
        /// </summary>
        public FxVector3i( int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Init the vector with the value
        /// </summary>
        public FxVector3i( float x, float y, float z)
        {
            X = (int)x;
            Y = (int)y;
            Z = (int)z;
        }

        /// <summary>
        /// Init the vector with the color values
        /// The color will be normalize with 255.
        /// </summary>
        public FxVector3i( System.Drawing.Color color )
        {
            X = color.R;
            Y = color.G;
            Z = color.B;
        }
        #endregion

        #region Get/Set Values

        public int this[int index]
        {
            get
            {
                switch ( index ) {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                }

                return 0;
            }

            set
            {
                switch ( index ) {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                }
            }

        }

        #endregion

        #region Math calcuations

        #region Add

        #region Static Function
        public static FxVector3i Add( FxVector3i left, FxVector3i right )
        {
            return new FxVector3i( left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }
        #endregion

        #region Local Function
        public void Add( FxVector3i value )
        {
            X += value.X;
            Y += value.Y;
            Z += value.Z;
        }
        #endregion

        #endregion

        #region Subtract
        #region Static Function
        public static FxVector3i Subtract( FxVector3i left, FxVector3i right )
        {
            return new FxVector3i( left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }
        #endregion

        #region Local Function
        public void Subtract( FxVector3i value )
        {
            X -= value.X;
            Y -= value.Y;
            Z -= value.Z;
        }
        #endregion
        #endregion

        #region Modulate

        #region static Functions
        public static FxVector3i Modulate( FxVector3i left, FxVector3i right )
        {
            return new FxVector3i( left.X * right.X, left.Y * right.Y, left.Z * right.Z );
        }
        #endregion

        #region Local Functions
        public void Modulate( FxVector3i value )
        {
            X *= value.X;
            Y *= value.Y;
            Z *= value.Z;
        }
        #endregion

        #endregion

        #region Multiply

        #region static Functions
        public static FxVector3i Multiply( FxVector3i left, float scale )
        {
            return new FxVector3i( left.X * scale, left.Y * scale, left.Z * scale);
        }

        public static FxVector3i Multiply( FxVector3i left, int scale )
        {
            return new FxVector3i( left.X * scale, left.Y * scale, left.Z * scale);
        }

        public static FxVector3i Multiply( FxVector3i left, byte scale )
        {
            return new FxVector3i( left.X * scale, left.Y * scale, left.Z * scale);
        }
        #endregion

        #region Local Function
        public void Multiply( float scale )
        {
            X = (int)( X * scale );
            Y = (int)( Y * scale );
            Z = (int)( Z * scale );
        }

        public void Multiply( int scale )
        {
            X *= scale;
            Y *= scale;
            Z *= scale;
        }

        public void Multiply( byte scale )
        {
            X *= scale;
            Y *= scale;
            Z *= scale;
        }
        #endregion

        #endregion

        #region Divide

        #region Static Functions
        public static FxVector3i Divide( FxVector3i left, float scale )
        {
            return new FxVector3i( left.X / scale, left.Y / scale, left.Z / scale );
        }

        public static FxVector3i Divide( FxVector3i left, int scale )
        {
            return new FxVector3i( left.X / scale, left.Y / scale, left.Z / scale);
        }

        public static FxVector3i Divide( FxVector3i left, byte scale )
        {
            return new FxVector3i( left.X / scale, left.Y / scale, left.Z / scale);
        }
        #endregion

        #region Local Functions
        public void Divide( float scale )
        {
            X = (int)( X / scale ); Y = (int)( Y / scale );
            Z = (int)( Z / scale ); 
        }

        public void Divide( int scale )
        {
            X /= scale;Y /= scale;
            Z /= scale;
        }

        public void Divide( byte scale )
        {
            X /= scale; Y /= scale;
            Z /= scale;
        }
        #endregion

        #endregion

        #region Nagate

        #region static Function
        public static FxVector3i Negate( FxVector3i value )
        {
            return new FxVector3i( -value.X, -value.Y, -value.Z );
        }
        #endregion

        #region Local Function
        public void Negate()
        {
            X = -X; Y = -Y;
            Z = -Z; 
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
        public FxVector3i Clamp( FxVector3i value, FxVector3i min, FxVector3i max )
        {
            float x = value.X;
            x = ( x > max.X ) ? max.X : x;
            x = ( x < min.X ) ? min.X : x;

            float y = value.Y;
            y = ( y > max.Y ) ? max.Y : y;
            y = ( y < min.Y ) ? min.Y : y;

            float z = value.Z;
            z = ( z > max.Z ) ? max.Z : z;
            z = ( z < min.Z ) ? min.Z : z;

            return new FxVector3i( x, y, z);
        }

        /// <summary>
        /// Restricts the vector to be within a specified range.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public void Clamp( FxVector3i min, FxVector3i max )
        {
            X = ( X > max.X ) ? max.X : X;
            X = ( X < min.X ) ? min.X : X;

            Y = ( Y > max.Y ) ? max.Y : Y;
            Y = ( Y < min.Y ) ? min.Y : Y;

            Z = ( Z > max.Z ) ? max.Z : Z;
            Z = ( Z < min.Z ) ? min.Z : Z;
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
        public static FxVector3i Lerp( FxVector3i start, FxVector3i end, float factor )
        {
            return new FxVector3i( start.X + ( ( end.X - start.X ) * factor ), 
                                   start.Y + ( ( end.Y - start.Y ) * factor ),
                                   start.Z + ( ( end.Z - start.Z ) * factor ));
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
        public static float Distance( FxVector3i start, FxVector3i end )
        {
            float x = start.X - end.X;
            float y = start.Y - end.Y;
            float z = start.Z - end.Z;

            return (float)Math.Sqrt( ( x * x ) + ( y * y ) + ( z * z ));
        }
        #endregion

        #region Local Func
        /// <summary>
        /// Calculates the distance between this vector and one other.
        /// </summary>
        /// <param name="end">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float Distance( FxVector3i end )
        {
            float x = X - end.X;
            float y = Y - end.Y;
            float z = Z - end.Z;

            return (float)Math.Sqrt( ( x * x ) + ( y * y ) + ( z * z ) );
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
        public static float DistanceSquared( FxVector3i start, FxVector3i end )
        {
            float x = start.X - end.X;
            float y = start.Y - end.Y;
            float z = start.Z - end.Z;

            return ( x * x ) + ( y * y ) + ( z * z )  ;
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
        public float DistanceSquared( FxVector3i end )
        {
            float x = X - end.X;
            float y = Y - end.Y;
            float z = Z - end.Z;

            return ( x * x ) + ( y * y ) + ( z * z );
        }
        #endregion

        #endregion

        #region Dot

        #region  Static Func
        public static float Dot( FxVector3i left, FxVector3i right )
        {
            return ( left.X * right.X + left.Y * right.Y + left.Z * right.Z );
        }
        #endregion

        #region Local Func
        public float Dot( FxVector3i vec )
        {
            return ( X * vec.X + Y * vec.Y + Z * vec.Z);
        }
        #endregion

        #endregion

        #region Normalize

        #region Static Func
        public static FxVector3i Normalize( FxVector3i vector )
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
            return (float)Math.Sqrt( X * X + Y * Y + Z * Z );
        }

        #endregion

        #region LengthSquared

        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z ;
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
        public static FxVector3i operator +( FxVector3i left, FxVector3i right )
        {
            return Add( left, right );
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static FxVector3i operator -( FxVector3i left, FxVector3i right )
        {
            return Subtract( left, right );
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static FxVector3i operator -( FxVector3i value )
        {
            return Negate( value );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector3i operator *( FxVector3i vector, float scale )
        {
            return Multiply( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector3i operator *( FxVector3i vector, int scale )
        {
            return Multiply( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector3i operator *( FxVector3i vector, byte scale )
        {
            return Multiply( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector3i operator *( float scale, FxVector3i vector )
        {
            return Multiply( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector3i operator /( FxVector3i vector, float scale )
        {
            return Divide( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector3i operator /( FxVector3i vector, int scale )
        {
            return Divide( vector, scale );
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector3i operator /( FxVector3i vector, byte scale )
        {
            return Divide( vector, scale );
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==( FxVector3i left, FxVector3i right )
        {
            return Equals( left, right );
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=( FxVector3i left, FxVector3i right )
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
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();

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

            return Equals( (FxVector3i)( value ) );
        }

        /// <summary>
        /// Returns a value that indicates whether the current instance is equal to the specified object. 
        /// </summary>
        /// <param name="other">Object to make the comparison with.</param>
        /// <returns><c>true</c> if the current instance is equal to the specified object; <c>false</c> otherwise.</returns>
        public bool Equals( FxVector3i value )
        {
            return ( X == value.X && Y == value.Y && Z == value.Z  );
        }

        /// <summary>
        /// Determines whether the specified object instances are considered equal. 
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="value1"/> is the same instance as <paramref name="value2"/> or 
        /// if both are <c>null</c> references or if <c>value1.Equals(value2)</c> returns <c>true</c>; otherwise, <c>false</c>.</returns>
        public static bool Equals( FxVector3i value1, FxVector3i value2 )
        {
            return ( value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z );
        }
        #endregion
    }
}
