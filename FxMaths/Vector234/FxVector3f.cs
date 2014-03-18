using FxMaths.GMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.Vector
{
    public struct FxVector3f : IEquatable<FxVector3f>, IVertex<float>
    {

        #region Public Variables

        /// <summary>
        /// Gets or sets the X component of the vector.
        /// </summary>
        /// <value>The X component of the vector.</value>
        public float x;

        /// <summary>
        /// Gets or sets the Y component of the vector.
        /// </summary>
        /// <value>The Y component of the vector.</value>
        public float y;

        /// <summary>
        /// Gets or sets the Z component of the vector.
        /// </summary>
        /// <value>The Z component of the vector.</value>
        public float z;

        #endregion



        #region Public Color Variables
        public float R { get { return x; } set { x = value; } }
        public float G { get { return y; } set { y = value; } }
        public float B { get { return z; } set { z = value; } }
        #endregion



        #region Contractors

        /// <summary>
        /// Init the vector with the value
        /// </summary>
        /// <param name="value"></param>
        public FxVector3f( float value )
        {
            x = value;
            y = value;
            z = value;
        }

        /// <summary>
        /// Init the vector with the value
        /// </summary>
        /// <param name="value"></param>
        public FxVector3f( float x, float y, float z )
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Init the vector with the color values
        /// The color will be normalize with 255.
        /// </summary>
        public FxVector3f( System.Drawing.Color color )
        {
            x = color.R / 255.0f;
            y = color.G / 255.0f;
            z = color.B / 255.0f;
        }
        #endregion



        #region Get/Set Values

        public float this[int index]
        {
            get
            {
                switch ( index ) {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                }

                return 0;
            }

            set
            {
                switch ( index ) {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                }
            }

        }

        #endregion



        #region Math calcuations



        #region Add

        #region Static Function
        public static FxVector3f Add(ref FxVector3f left, ref FxVector3f right)
        {
            FxVector3f r;
            r.x = left.x + right.x;
            r.y = left.y + right.y;
            r.z = left.z + right.z;
            return r;
        }

        public static void Add(ref FxVector3f left, ref FxVector3f right, out FxVector3f r)
        {
            r.x = left.x + right.x;
            r.y = left.y + right.y;
            r.z = left.z + right.z;
        }

        public static FxVector3f Add(ref FxVector3f vec, float value)
        {
            FxVector3f r;
            r.x = vec.x + value;
            r.y = vec.y + value;
            r.z = vec.z + value;
            return r;
        }

        public static void Add(ref FxVector3f vec, float value, out FxVector3f r)
        {
            r.x = vec.x + value;
            r.y = vec.y + value;
            r.z = vec.z + value;
        }


        public static FxVector3f Add(ref FxVector3f vec, int value)
        {
            FxVector3f r;
            r.x = vec.x + value;
            r.y = vec.y + value;
            r.z = vec.z + value;
            return r;
        }

        public static void Add(ref FxVector3f vec, int value, out FxVector3f r)
        {
            r.x = vec.x + value;
            r.y = vec.y + value;
            r.z = vec.z + value;
        }

        public static FxVector3f Add(ref FxVector3f vec, byte value)
        {
            FxVector3f r;
            r.x = vec.x + value;
            r.y = vec.y + value;
            r.z = vec.z + value;
            return r;
        }

        public static void Add(ref FxVector3f vec, byte value, out FxVector3f r)
        {
            r.x = vec.x + value;
            r.y = vec.y + value;
            r.z = vec.z + value;
        }
        #endregion

        #region Local Function
        public void Add(ref FxVector3f value)
        {
            x += value.x;
            y += value.y;
            z += value.z;
        }

        public void Add(ref IVertex<float> value)
        {
            x += value.X;
            y += value.Y;
            z += value.Z;
        }


        public void Add(float value)
        {
            x += value;
            y += value;
            z += value;
        }

        public void Add(int value)
        {
            x += value;
            y += value;
            z += value;
        }

        public void Add(byte value)
        {
            x += value;
            y += value;
            z += value;
        }
        #endregion

        #endregion



        #region Subtract
        #region Static Function
        public static FxVector3f Subtract(ref FxVector3f left, ref FxVector3f right)
        {
            FxVector3f r;
            r.x = left.x - right.x;
            r.y = left.y - right.y;
            r.z = left.z - right.z;
            return r;
        }

        public static void Subtract(ref FxVector3f left, ref FxVector3f right, out FxVector3f r)
        {
            r.x = left.x - right.x;
            r.y = left.y - right.y;
            r.z = left.z - right.z;
        }


        public static FxVector3f Subtract(ref FxVector3f vec, float value)
        {
            FxVector3f r;
            r.x = vec.x - value;
            r.y = vec.y - value;
            r.z = vec.z - value;
            return r;
        }

        public static void Subtract(ref FxVector3f vec, float value, out FxVector3f r)
        {
            r.x = vec.x - value;
            r.y = vec.y - value;
            r.z = vec.z - value;
        }


        public static FxVector3f Subtract(ref FxVector3f vec, int value)
        {
            FxVector3f r;
            r.x = vec.x - value;
            r.y = vec.y - value;
            r.z = vec.z - value;
            return r;
        }

        public static void Subtract(ref FxVector3f vec, int value, out FxVector3f r)
        {
            r.x = vec.x - value;
            r.y = vec.y - value;
            r.z = vec.z - value;
        }


        public static FxVector3f Subtract(ref FxVector3f vec, byte value)
        {
            FxVector3f r;
            r.x = vec.x - value;
            r.y = vec.y - value;
            r.z = vec.z - value;
            return r;
        }

        public static void Subtract(ref FxVector3f vec, byte value, out FxVector3f r)
        {
            r.x = vec.x - value;
            r.y = vec.y - value;
            r.z = vec.z - value;
        }
        #endregion

        #region Local Function
        public void Subtract(ref FxVector3f value)
        {
            x -= value.X;
            y -= value.Y;
            z -= value.Z;
        }
        public void Subtract(ref IVertex<float> value)
        {
            x -= value.X;
            y -= value.Y;
            z -= value.Z;
        }

        public void Subtract(float value)
        {
            x -= value;
            y -= value;
            z -= value;
        }

        public void Subtract(int value)
        {
            x -= value;
            y -= value;
            z -= value;
        }

        public void Subtract(byte value)
        {
            x -= value;
            y -= value;
            z -= value;
        }

        #endregion
        #endregion



        #region Modulate

        #region static Functions
        public static FxVector3f Modulate(ref FxVector3f left,ref FxVector3f right )
        {
            FxVector3f r;
            r.x = left.x * right.x;
            r.y = left.y * right.y;
            r.z = left.z * right.z;
            return r;
        }


        public static void Modulate(ref FxVector3f left, ref FxVector3f right, out FxVector3f r)
        {
            r.x = left.x * right.x;
            r.y = left.y * right.y;
            r.z = left.z * right.z;
        }
        #endregion

        #region Local Functions
        public void Modulate(ref FxVector3f value )
        {
            x *= value.x;
            y *= value.y;
            z *= value.z;
        }
        #endregion

        #endregion



        #region Multiply

        #region static Functions
        public static FxVector3f Multiply(ref FxVector3f left, float scale )
        {
            FxVector3f r;
            r.x = left.x * scale;
            r.y = left.y * scale;
            r.z = left.z * scale;
            return r;
        }

        public static FxVector3f Multiply(ref FxVector3f left, int scale)
        {
            FxVector3f r;
            r.x = left.x * scale;
            r.y = left.y * scale;
            r.z = left.z * scale;
            return r;
        }

        public static FxVector3f Multiply(ref FxVector3f left, byte scale)
        {
            FxVector3f r;
            r.x = left.x * scale;
            r.y = left.y * scale;
            r.z = left.z * scale;
            return r;
        }
        public static void Multiply(ref FxVector3f left, float scale, out FxVector3f r)
        {
            r.x = left.x * scale;
            r.y = left.y * scale;
            r.z = left.z * scale;
        }

        public static void Multiply(ref FxVector3f left, int scale, out FxVector3f r)
        {
            r.x = left.x * scale;
            r.y = left.y * scale;
            r.z = left.z * scale;
        }

        public static void Multiply(ref FxVector3f left, byte scale, out FxVector3f r)
        {
            r.x = left.x * scale;
            r.y = left.y * scale;
            r.z = left.z * scale;
        }
        #endregion


        #region Local Function
        public void Multiply( float scale )
        {
            x *= scale;
            y *= scale;
            z *= scale;
        }

        public void Multiply( int scale )
        {
            x *= scale;
            y *= scale;
            z *= scale;
        }

        public void Multiply( byte scale )
        {
            x *= scale;
            y *= scale;
            z *= scale;
        }
        #endregion

        #endregion



        #region Divide

        #region Static Functions
        public static FxVector3f Divide(ref FxVector3f left, float scale )
        {
            FxVector3f r;
            r.x = left.x / scale;
            r.y = left.y / scale;
            r.z = left.z / scale;
            return r;
        }

        public static FxVector3f Divide(ref FxVector3f left, int scale)
        {
            FxVector3f r;
            r.x = left.x / scale;
            r.y = left.y / scale;
            r.z = left.z / scale;
            return r;
        }

        public static FxVector3f Divide(ref FxVector3f left, byte scale)
        {
            FxVector3f r;
            r.x = left.x / scale;
            r.y = left.y / scale;
            r.z = left.z / scale;
            return r;
        }
        public static void Divide(ref FxVector3f left, float scale, out FxVector3f r)
        {
            r.x = left.x / scale;
            r.y = left.y / scale;
            r.z = left.z / scale;
        }

        public static void Divide(ref FxVector3f left, int scale, out FxVector3f r)
        {
            r.x = left.x / scale;
            r.y = left.y / scale;
            r.z = left.z / scale;
        }

        public static void Divide(ref FxVector3f left, byte scale, out FxVector3f r)
        {
            r.x = left.x / scale;
            r.y = left.y / scale;
            r.z = left.z / scale;
        }
        #endregion

        #region Local Functions
        public void Divide( float scale )
        {
            x /= scale; y /= scale;
            z /= scale;
        }

        public void Divide( int scale )
        {
            x /= scale;y /= scale;
            z /= scale;
        }

        public void Divide( byte scale )
        {
            x /= scale; y /= scale;
            z /= scale;
        }
        #endregion

        #endregion


        #region Nagate

        #region static Function
        public static FxVector3f Negate( FxVector3f value )
        {
            return new FxVector3f( -value.x, -value.y, -value.z);
        }
        #endregion

        #region Local Function
        public void Negate()
        {
            x = -x; y = -y; z = -z; 
        }
        #endregion

        #endregion


        #region Dot

        #region  Static Func
        public static float Dot(ref FxVector3f left, ref FxVector3f right)
        {
            return (left.x * right.x + left.y * right.y + left.z * right.z);
        }
        #endregion

        #region Local Func

        public float Dot(ref IVertex<float> vec)
        {
            return (x * vec.X + y * vec.Y + z * vec.Z);
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
        public FxVector3f Clamp( FxVector3f value, FxVector3f min, FxVector3f max )
        {
            float x = value.x;
            x = ( x > max.x ) ? max.x : x;
            x = ( x < min.x ) ? min.x : x;

            float y = value.y;
            y = ( y > max.y ) ? max.y : y;
            y = ( y < min.y ) ? min.y : y;

            float z = value.z;
            z = ( z > max.z ) ? max.z : z;
            z = ( z < min.z ) ? min.z : z;

            return new FxVector3f( x, y, z);
        }

        /// <summary>
        /// Restricts the vector to be within a specified range.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public void Clamp( FxVector3f min, FxVector3f max )
        {
            x = ( x > max.x ) ? max.x : x;
            x = ( x < min.x ) ? min.x : x;

            y = ( y > max.y ) ? max.y : y;
            y = ( y < min.y ) ? min.y : y;

            z = ( z > max.z ) ? max.z : z;
            z = ( z < min.z ) ? min.z : z;
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
        public static FxVector3f Lerp( FxVector3f start, FxVector3f end, float factor )
        {
            return new FxVector3f( start.x + ( ( end.x - start.x ) * factor ), 
                                   start.y + ( ( end.y - start.y ) * factor ),
                                   start.z + ( ( end.z - start.z ) * factor ));
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
        public static float Distance(ref FxVector3f start, ref FxVector3f end )
        {
            float x = start.x - end.x;
            float y = start.y - end.y;
            float z = start.z - end.z;

            return (float)Math.Sqrt( ( x * x ) + ( y * y ) + ( z * z ) );
        }


        /// <summary>
        ///  Calculates the distance between two vectors and one other.
        /// </summary>
        /// <param name="start">The first vector.</param>
        /// <param name="endX">The x value of the second vector.</param>
        /// <param name="endY">The y value of the second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public static float Distance(ref FxVector2f start, float endX, float endY, float endZ)
        {
            float x = start.x - endX;
            float y = start.y - endY;
            float z = start.y - endZ;

            return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
        }
        #endregion

        #region Local Func
        /// <summary>
        /// Calculates the distance between this vector and one other.
        /// </summary>
        /// <param name="end">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float Distance( FxVector3f end )
        {
            float x = this.x - end.x;
            float y = this.y - end.y;
            float z = this.z - end.z;

            return (float)Math.Sqrt( ( x * x ) + ( y * y ) + ( z * z ));
        }

        /// <summary>
        /// Calculates the distance between this vector and one other.
        /// </summary>
        /// <param name="end">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float Distance(ref IVertex<float> end)
        {
            float x = this.x - end.X;
            float y = this.y - end.Y;
            float z = this.z - end.Z;

            return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
        }

        /// <summary>
        ///  Calculates the distance between this vector and one other.
        /// </summary>
        /// <param name="endX">The x value of the second vector.</param>
        /// <param name="endY">The y value of the second vector.</param>
        /// <param name="endZ">The z value of the second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float Distance(float endX, float endY, float endZ)
        {
            float x = this.x - endX;
            float y = this.y - endY;
            float z = this.z - endZ;

            return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
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
        public static float DistanceSquared(ref FxVector3f start, ref FxVector3f end)
        {
            float x = start.x - end.x;
            float y = start.y - end.y;
            float z = start.z - end.z;

            return ( x * x ) + ( y * y ) + ( z * z ) ;
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
        public float DistanceSquared(ref FxVector3f end )
        {
            float x = this.x - end.x;
            float y = this.y - end.y;
            float z = this.z - end.z;

            return ( x * x ) + ( y * y ) + ( z * z );
        }
        #endregion

        #endregion

        #region Dot

        #region  Static Func
        public static float Dot( FxVector3f left, FxVector3f right )
        {
            return ( left.x * right.x + left.y * right.y + left.z * right.z );
        }
        #endregion

        #region Local Func
        public float Dot( FxVector3f vec )
        {
            return ( x * vec.x + y * vec.y + z * vec.z );
        }
        #endregion

        #endregion

        #region Normalize

        #region Static Func
        public static FxVector3f Normalize( FxVector3f vector )
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
            x *= num;
            y *= num;
            z *= num;
        }
        #endregion

        #endregion

        #region Length

        public float Length()
        {
            return (float)Math.Sqrt( x * x + y * y + z * z );
        }

        #endregion

        #region LengthSquared

        public float LengthSquared()
        {
            return x * x + y * y + z * z;
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
        public static FxVector3f operator +( FxVector3f left, FxVector3f right )
        {
            left.x += right.x;
            left.y += right.y;
            left.z += right.z;
            return left;
        }


        /// <summary>
        /// Increase by one the 2 elements
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static FxVector3f operator ++(FxVector3f vec)
        {
            vec.x++;
            vec.y++;
            vec.z++;
            return vec;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static FxVector3f operator -( FxVector3f left, FxVector3f right )
        {
            left.x -= right.x;
            left.y -= right.y;
            left.z -= right.z;
            return left;
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static FxVector3f operator -( FxVector3f value )
        {
            value.x = -value.x;
            value.y = -value.y;
            value.z = -value.z;
            return value;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector3f operator *( FxVector3f vector, float scale )
        {
            vector.x *= scale;
            vector.y *= scale;
            vector.z *= scale;
            return vector;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector3f operator *( FxVector3f vector, int scale )
        {
            vector.x *= scale;
            vector.y *= scale;
            vector.z *= scale;
            return vector;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector3f operator *( FxVector3f vector, byte scale )
        {
            vector.x *= scale;
            vector.y *= scale;
            vector.z *= scale;
            return vector;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector3f operator *( float scale, FxVector3f vector )
        {
            vector.x *= scale;
            vector.y *= scale;
            vector.z *= scale;
            return vector;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector3f operator /( FxVector3f vector, float scale )
        {
            vector.x /= scale;
            vector.y /= scale;
            vector.z /= scale;
            return vector;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector3f operator /( FxVector3f vector, int scale )
        {
            vector.x /= scale;
            vector.y /= scale;
            vector.z /= scale;
            return vector;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector3f operator /( FxVector3f vector, byte scale )
        {
            vector.x /= scale;
            vector.y /= scale;
            vector.z /= scale;
            return vector;
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==( FxVector3f left, FxVector3f right )
        {
            return Equals( left, right );
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=( FxVector3f left, FxVector3f right )
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
            return x.GetHashCode() + y.GetHashCode() + z.GetHashCode();

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

            return Equals( (FxVector3f)( value ) );
        }

        /// <summary>
        /// Returns a value that indicates whether the current instance is equal to the specified object. 
        /// </summary>
        /// <param name="other">Object to make the comparison with.</param>
        /// <returns><c>true</c> if the current instance is equal to the specified object; <c>false</c> otherwise.</returns>
        public bool Equals( FxVector3f value )
        {
            return ( x == value.x && y == value.y && z == value.z );
        }

        /// <summary>
        /// Determines whether the specified object instances are considered equal. 
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="value1"/> is the same instance as <paramref name="value2"/> or 
        /// if both are <c>null</c> references or if <c>value1.Equals(value2)</c> returns <c>true</c>; otherwise, <c>false</c>.</returns>
        public static bool Equals( FxVector3f value1, FxVector3f value2 )
        {
            return ( value1.x == value2.x && value1.y == value2.y && value1.z == value2.z );
        }
        #endregion




        #region String Utils

        public override string ToString()
        {
            return "(" + x.ToString().Replace(',', '.') + "," + y.ToString().Replace(',', '.') + "," + z.ToString().Replace(',', '.') + ")";
        }

        public string ToString(String Format)
        {
            return "(" + x.ToString(Format).Replace(',', '.') + "," + y.ToString(Format).Replace(',', '.') + "," + z.ToString(Format).Replace(',', '.') + ")";
        }



        #endregion




        #region IVertex Members



        #region Set/Get
        public float X
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

        public float Y
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

        public float Z
        {
            get
            {
                return z;
            }
            set
            {
                this.z = value;
            }
        } 
        #endregion





        #region Data stream
        /// <summary>
        /// Write the vertex to the data stream
        /// </summary>
        /// <param name="dataStream"></param>
        public void WriteToDataStream(SharpDX.DataStream dataStream)
        {
            // write the data to the stream
            dataStream.Write<float>(x);
            dataStream.Write<float>(y);
            dataStream.Write<float>(z);
        }

        /// <summary>
        /// Read the vertex from the data stream
        /// </summary>
        /// <param name="dataStream"></param>
        public void ReadFromDataStream(SharpDX.DataStream dataStream)
        {
            // read the data from the stream
            x = dataStream.Read<float>();
            y = dataStream.Read<float>();
            z = dataStream.Read<float>();
        } 
        #endregion



        public IVertex<float> Copy()
        {
            return new FxVector3f(x, y, z);
        }



        #region Equals Functions

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public int GetHashCode(IVertex<float> obj)
        {
            return obj.GetHashCode();
        }


        public bool Equals(IVertex<float> value)
        {
            return (Math.Abs(value.X - x) < 0.0001 && Math.Abs(y - value.Y) < 0.0001 && Math.Abs(z - value.Z) < 0.0001);
        }

        /// <summary>
        /// Determines whether the specified object instances are considered equal. 
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="value1"/> is the same instance as <paramref name="value2"/> or 
        /// if both are <c>null</c> references or if <c>value1.Equals(value2)</c> returns <c>true</c>; otherwise, <c>false</c>.</returns>
        public bool Equals(IVertex<float> value1, IVertex<float> value2)
        {
            return (Math.Abs(value1.X - value2.X) < 0.0001 && Math.Abs(value1.Y - value2.Y) < 0.0001 && Math.Abs(value1.Z - value2.Z) < 0.0001);
        }

        #endregion




        #endregion


    }

    public static class FxVector3fUtils
    {
        #region Random
        public static FxVector3f NextFxVector3f(this Random random, FxVector3f min, FxVector3f max)
        {
            FxVector3f r;
            r.x = (float)(random.NextDouble() * max.x + min.x);
            r.y = (float)(random.NextDouble() * max.y + min.y);
            r.z = (float)(random.NextDouble() * max.z + min.z);
            return r;
        }

        public static FxVector3f NextFxVector3f(this Random random)
        {
            FxVector3f r;
            r.x = (float)(random.NextDouble());
            r.y = (float)(random.NextDouble());
            r.z = (float)(random.NextDouble());
            return r;
        }
        #endregion
    }
}
