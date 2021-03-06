﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

using SharpDX;
using FxMaths.GMaps;

namespace FxMaths.Vector
{
    [TypeConverter(typeof(FxVector2fConverter))]
    public struct FxVector2f : IEquatable<IVertex<float>>, IEqualityComparer<IVertex<float>>, IVertex<float>
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

        #endregion



        #region Contractors

        /// <summary>
        /// Init the vector with the value
        /// </summary>
        /// <param name="value"></param>
        public FxVector2f(float value)
        {
            x = value;
            y = value;
        }

        /// <summary>
        /// Init the vector with the value
        /// </summary>
        /// <param name="value"></param>
        public FxVector2f(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        #endregion



        #region Get/Set Values

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                }

                return 0;
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                }
            }

        }

        #endregion



        #region Math calcuations

        #region Add

        #region Static Function
        public static FxVector2f Add(ref FxVector2f left,ref FxVector2f right)
        {
            FxVector2f r;
            r.x = left.x + right.x;
            r.y = left.y + right.y;
            return r;
        }

        public static void Add(ref FxVector2f left, ref FxVector2f right, out FxVector2f r)
        {
            r.x = left.x + right.x;
            r.y = left.y + right.y;
        }

        public static FxVector2f Add(ref FxVector2f vec, float value)
        {
            FxVector2f r;
            r.x = vec.x + value;
            r.y = vec.y + value;
            return r;
        }

        public static void Add(ref FxVector2f vec, float value, out FxVector2f r)
        {
            r.x = vec.x + value;
            r.y = vec.y + value;
        }


        public static FxVector2f Add(ref FxVector2f vec, int value)
        {
            FxVector2f r;
            r.x = vec.x + value;
            r.y = vec.y + value;
            return r;
        }

        public static void Add(ref FxVector2f vec, int value, out FxVector2f r)
        {
            r.x = vec.x + value;
            r.y = vec.y + value;
        }

        public static FxVector2f Add(ref FxVector2f vec, byte value)
        {
            FxVector2f r;
            r.x = vec.x + value;
            r.y = vec.y + value;
            return r;
        }

        public static void Add(ref FxVector2f vec, byte value, out FxVector2f r)
        {
            r.x = vec.x + value;
            r.y = vec.y + value;
        }
        #endregion

        #region Local Function
        public void Add(ref FxVector2f value)
        {
            x += value.x;
            y += value.y;
        }

        public void Add(ref IVertex<float> value)
        {
            x += value.X;
            y += value.Y;
        }


        public void Add(float value)
        {
            x += value;
            y += value;
        }

        public void Add(int value)
        {
            x += value;
            y += value;
        }

        public void Add(byte value)
        {
            x += value;
            y += value;
        }
        #endregion

        #endregion




        #region Subtract
        #region Static Function
        public static FxVector2f Subtract(ref FxVector2f left, ref FxVector2f right)
        {
            FxVector2f r;
            r.x = left.x - right.x;
            r.y = left.y - right.y;
            return r;
        }

        public static void Subtract(ref FxVector2f left, ref FxVector2f right, out FxVector2f r)
        {
            r.x = left.x - right.x;
            r.y = left.y - right.y;
        }


        public static FxVector2f Subtract(ref FxVector2f vec, float value)
        {
            FxVector2f r;
            r.x = vec.x - value;
            r.y = vec.y - value;
            return r;
        }

        public static void Subtract(ref FxVector2f vec, float value, out FxVector2f r)
        {
            r.x = vec.x - value;
            r.y = vec.y - value;
        }


        public static FxVector2f Subtract(ref FxVector2f vec, int value)
        {
            FxVector2f r;
            r.x = vec.x - value;
            r.y = vec.y - value;
            return r;
        }

        public static void Subtract(ref FxVector2f vec, int value, out FxVector2f r)
        {
            r.x = vec.x - value;
            r.y = vec.y - value;
        }


        public static FxVector2f Subtract(ref FxVector2f vec, byte value)
        {
            FxVector2f r;
            r.x = vec.x - value;
            r.y = vec.y - value;
            return r;
        }

        public static void Subtract(ref FxVector2f vec, byte value, out FxVector2f r)
        {
            r.x = vec.x - value;
            r.y = vec.y - value;
        }
        #endregion

        #region Local Function
        public void Subtract(ref FxVector2f value)
        {
            x -= value.X;
            y -= value.Y;
        }
        public void Subtract(ref IVertex<float> value)
        {
            x -= value.X;
            y -= value.Y;
        }

        public void Subtract(float value)
        {
            x -= value;
            y -= value;
        }

        public void Subtract(int value)
        {
            x -= value;
            y -= value;
        }

        public void Subtract(byte value)
        {
            x -= value;
            y -= value;
        }

        #endregion
        #endregion




        #region Modulate

        #region static Functions
        public static FxVector2f Modulate(ref FxVector2f left, ref FxVector2f right)
        {
            FxVector2f r;
            r.x = left.x * right.x;
            r.y = left.y * right.y;
            return r;
        }

        public static void Modulate(ref FxVector2f left, ref FxVector2f right, out FxVector2f r)
        {
            r.x = left.x * right.x;
            r.y = left.y * right.y;
        }
        #endregion

        #region Local Functions
        public void Modulate(ref FxVector2f value)
        {
            x *= value.x;
            y *= value.y;
        }
        #endregion

        #endregion




        #region Multiply

        #region static Functions
        public static FxVector2f Multiply(ref FxVector2f left, float scale)
        {
            FxVector2f r;
            r.x = left.x * scale;
            r.y = left.y * scale;
            return r;
        }

        public static FxVector2f Multiply(ref FxVector2f left, int scale)
        {
            FxVector2f r;
            r.x = left.x * scale;
            r.y = left.y * scale;
            return r;
        }

        public static FxVector2f Multiply(ref FxVector2f left, byte scale)
        {
            FxVector2f r;
            r.x = left.x * scale;
            r.y = left.y * scale;
            return r;
        }

        public static void Multiply(ref FxVector2f left, float scale, out FxVector2f r)
        {
            r.x = left.x * scale;
            r.y = left.y * scale;
        }

        public static void Multiply(ref FxVector2f left, int scale, out FxVector2f r)
        {
            r.x = left.x * scale;
            r.y = left.y * scale;
        }

        public static void Multiply(ref FxVector2f left, byte scale, out FxVector2f r)
        {
            r.x = left.x * scale;
            r.y = left.y * scale;
        }
        #endregion

        #region Local Function
        public void Multiply(float scale)
        {
            x *= scale;
            y *= scale;
        }

        public void Multiply(int scale)
        {
            x *= scale;
            y *= scale;
        }

        public void Multiply(byte scale)
        {
            x *= scale;
            y *= scale;
        }
        #endregion

        #endregion




        #region Divide

        #region Static Functions
        public static FxVector2f Divide(ref FxVector2f left, float scale)
        {
            FxVector2f r;
            r.x = left.x / scale;
            r.y = left.y / scale;
            return r;
        }

        public static FxVector2f Divide(FxVector2f left, int scale)
        {
            FxVector2f r;
            r.x = left.x / scale;
            r.y = left.y / scale;
            return r;
        }

        public static FxVector2f Divide(FxVector2f left, byte scale)
        {
            FxVector2f r;
            r.x = left.x / scale;
            r.y = left.y / scale;
            return r;
        }

        public static void Divide(ref FxVector2f left, float scale, out FxVector2f r)
        {
            r.x = left.x / scale;
            r.y = left.y / scale;
        }

        public static void Divide(ref FxVector2f left, int scale, out FxVector2f r)
        {
            r.x = left.x / scale;
            r.y = left.y / scale;
        }

        public static void Divide(ref FxVector2f left, byte scale, out FxVector2f r)
        {
            r.x = left.x / scale;
            r.y = left.y / scale;
        }
        #endregion

        #region Local Functions
        public void Divide(float scale)
        {
            x /= scale;
            y /= scale;
        }

        public void Divide(int scale)
        {
            x /= scale;
            y /= scale;
        }

        public void Divide(byte scale)
        {
            x /= scale;
            y /= scale;
        }
        #endregion

        #endregion




        #region Nagate

        #region static Function
        public static FxVector2f Negate(FxVector2f value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }
        #endregion

        #region Local Function
        public void Negate()
        {
            x = -x;
            y = -y;
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
        public FxVector2f Clamp(FxVector2f value, FxVector2f min, FxVector2f max)
        {
            float x = value.x;
            x = (x > max.x) ? max.x : x;
            x = (x < min.x) ? min.x : x;

            float y = value.y;
            y = (y > max.y) ? max.y : y;
            y = (y < min.y) ? min.y : y;

            return new FxVector2f(x, y);
        }

        /// <summary>
        /// Restricts the vector to be within a specified range.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public void Clamp(FxVector2f min, FxVector2f max)
        {
            x = (x > max.x) ? max.x : x;
            x = (x < min.x) ? min.x : x;

            y = (y > max.y) ? max.y : y;
            y = (y < min.y) ? min.y : y;
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
        public static FxVector2f Lerp(FxVector2f start, FxVector2f end, float factor)
        {
            return new FxVector2f(start.x + ((end.x - start.x) * factor), start.y + ((end.y - start.y) * factor));
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
        public static float Distance(ref FxVector2f start, ref FxVector2f end)
        {
            float x = start.x - end.x;
            float y = start.y - end.y;

            return (float)Math.Sqrt((x * x) + (y * y));
        }

        /// <summary>
        ///  Calculates the distance between two vectors and one other.
        /// </summary>
        /// <param name="start">The first vector.</param>
        /// <param name="endX">The x value of the second vector.</param>
        /// <param name="endY">The y value of the second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public static float Distance(ref FxVector2f start, float endX, float endY)
        {
            float x = start.x - endX;
            float y = start.y - endY;

            return (float)Math.Sqrt((x * x) + (y * y));
        }
        #endregion

        #region Local Func
        /// <summary>
        /// Calculates the distance between this vector and one other.
        /// </summary>
        /// <param name="end">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float Distance(ref IVertex<float> end)
        {
            float x = this.X - end.X;
            float y = this.Y - end.Y;

            return (float)Math.Sqrt((x * x) + (y * y));
        }

        /// <summary>
        /// Calculates the distance between this vector and one other.
        /// </summary>
        /// <param name="end">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float Distance(ref FxVector2f end)
        {
            float x = this.X - end.X;
            float y = this.Y - end.Y;

            return (float)Math.Sqrt((x * x) + (y * y));
        }

        /// <summary>
        ///  Calculates the distance between this vector and one other.
        /// </summary>
        /// <param name="endX">The x value of the second vector.</param>
        /// <param name="endY">The y value of the second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float Distance(float endX, float endY)
        {
            float x = this.X - endX;
            float y = this.Y - endY;

            return (float)Math.Sqrt((x * x) + (y * y));
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
        public static float DistanceSquared(FxVector2f start, FxVector2f end)
        {
            float x = start.x - end.x;
            float y = start.y - end.y;

            return (x * x) + (y * y);
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
        public float DistanceSquared(FxVector2f end)
        {
            float x = this.x - end.x;
            float y = this.y - end.y;

            return (x * x) + (y * y);
        }
        #endregion

        #endregion


        #region Dot

        #region  Static Func
        public static float Dot(ref FxVector2f left,ref FxVector2f right)
        {
            return (left.x * right.x + left.y * right.y);
        }
        #endregion

        #region Local Func

        public float Dot(ref IVertex<float> vec)
        {
            return (x * vec.X + y * vec.Y);
        }


        public float Dot(ref FxVector2f vec)
        {
            return (x * vec.X + y * vec.Y);
        }

        #endregion

        #endregion


        #region Normalize

        #region Static Func
        public static FxVector2f Normalize(FxVector2f vector)
        {
            vector.Normalize();
            return vector;
        }
        #endregion

        #region Local Func
        public void Normalize()
        {
            float length = Length();
            if (length == 0)
                return;
            float num = 1 / length;
            x *= num;
            y *= num;
        }
        #endregion

        #endregion


        #region Length

        public float Length()
        {
            return (float)Math.Sqrt(x * x + y * y);
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
        public static FxVector2f operator +(FxVector2f left, FxVector2f right)
        {
            left.x += right.x;
            left.y += right.y;
            return left;
        }

        /// <summary>
        /// Increase by one the 2 elements
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static FxVector2f operator ++(FxVector2f vec)
        {
            vec.x++;
            vec.y++;
            return vec;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static FxVector2f operator -(FxVector2f left, FxVector2f right)
        {
            left.x -= right.x;
            left.y -= right.y;
            return left;
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static FxVector2f operator -(FxVector2f value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2f operator *(FxVector2f vector, float scale)
        {
            vector.x *= scale;
            vector.y *= scale;
            return vector;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2f operator *(FxVector2f vector, int scale)
        {
            vector.x *= scale;
            vector.y *= scale;
            return vector;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2f operator *(FxVector2f vector, byte scale)
        {
            vector.x *= scale;
            vector.y *= scale;
            return vector;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2f operator *(float scale, FxVector2f vector)
        {
            vector.x *= scale;
            vector.y *= scale;
            return vector;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2f operator /(FxVector2f vector, float scale)
        {
            vector.x /= scale;
            vector.y /= scale;
            return vector;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2f operator /(FxVector2f vector, int scale)
        {
            vector.x /= scale;
            vector.y /= scale;
            return vector;
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="vector">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2f operator /(FxVector2f vector, byte scale)
        {
            vector.x /= scale;
            vector.y /= scale;
            return vector;
        }


        /// <summary>
        /// Scales a vector by the given vector.
        /// </summary>
        /// <param name="left">The vector to scale.</param>
        /// <param name="right">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2f operator *(FxVector2f left, FxVector2f right)
        {
            left.x *= right.x;
            left.y *= right.y;
            return left;
        }


        /// <summary>
        /// Scales a vector by the given vector.
        /// </summary>
        /// <param name="left">The vector to scale.</param>
        /// <param name="right">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static FxVector2f operator /(FxVector2f left, FxVector2f right)
        {
            left.x /= right.x;
            left.y /= right.y;
            return left;
        }


        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(FxVector2f left, FxVector2f right)
        {
            return left.Equals( right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(FxVector2f left, FxVector2f right)
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
        public int GetHashCode(IVertex<float> obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Returns a value that indicates whether the current instance is equal to a specified object. 
        /// </summary>
        /// <param name="value">Object to make the comparison with.</param>
        /// <returns><c>true</c> if the current instance is equal to the specified object; <c>false</c> otherwise.</returns>
        public override bool Equals(Object value)
        {
            if (value == null)
                return false;

            if (value.GetType() != GetType())
                return false;

            return Equals((IVertex<float>)(value));
        }

        /// <summary>
        /// Returns a value that indicates whether the current instance is equal to the specified object. 
        /// </summary>
        /// <param name="value">Object to make the comparison with.</param>
        /// <returns><c>true</c> if the current instance is equal to the specified object; <c>false</c> otherwise.</returns>
        public bool Equals(IVertex<float> value)
        {
            return (Math.Abs(value.X - x) < 0.0001 && Math.Abs(y - value.Y) < 0.0001);
            //return (x == value.X && y == value.Y);
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
            //return (value1.X == value2.X && value1.Y == value2.Y);
            return (Math.Abs(value1.X - value2.X) < 0.0001 && Math.Abs(value1.Y - value2.Y) < 0.0001);
        }
        #endregion



        #region IVertex Members

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

        [Browsable(false)]
        public float Z
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
        public void WriteToDataStream(DataStream dataStream)
        {

            // write the data to the stream
            dataStream.Write<float>(this.X);
            dataStream.Write<float>(this.Y);

        }


        /// <summary>
        /// Read the vertex from the data stream
        /// </summary>
        /// <param name="dataStream"></param>
        public void ReadFromDataStream(DataStream dataStream)
        {
            // read the data from the stream
            x = dataStream.Read<float>();
            y = dataStream.Read<float>();
        }

        /// <summary>
        /// Read the vertex from the data stream
        /// </summary>
        /// <param name="dataStream"></param>
        public static IVertex<float> sReadFromDataStream(DataStream dataStream)
        {
            return new FxVector2f(dataStream.Read<float>(), dataStream.Read<float>());
        }

        /// <summary>
        /// Return a copy of the vertex.
        /// </summary>
        /// <returns></returns>
        public FxMaths.GMaps.IVertex<float> Copy()
        {
            return new FxVector2f(this.x, this.y);
        }

        #endregion


        public override string ToString()
        {
            return "(" + x.ToString("0.0000000000").Replace(',', '.') + "," + y.ToString("0.0000000000").Replace(',', '.') + ")";
        }


        public string ToString(String Format)
        {
            return "(" + x.ToString(Format).Replace(',', '.') + "," + y.ToString(Format).Replace(',', '.') + ")";
        }

        public static FxVector2f Parse(String str)
        {
            string s = (string)str;
            // parse the format "(X,Y)
            //
            char[] splitChar = { ' ', ',', ')', '(' };

            // split the string to the elements
            string[] elements = s.Trim().Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

            // check if we have only 2 elementex   (x,y)
            if (elements.Length == 2)
            {
                float X_result;
                if (float.TryParse(elements[0].Replace('.',','), out X_result))
                {
                    float Y_result;
                    if (float.TryParse(elements[1].Replace('.', ','), out Y_result))
                    {
                        return new FxVector2f(X_result, Y_result);
                    }
                }
            }

            // if we got this far, complain that we
            // couldn't parse the string
            //
            throw new ArgumentException(
               "Can not convert '" + (string)str +
                        "' to type FxVector2f");
        }

        /// <summary>
        /// Rotate the vector.
        /// </summary>
        /// <param name="directionAngleChange">Angle in radian.</param>
        public void Rotation(float directionAngleChange)
        {
            if (float.IsNaN(directionAngleChange))
                throw new ArithmeticException();

            float cs = (float)Math.Cos(directionAngleChange);
            float sn = (float)Math.Sin(directionAngleChange);
            float px = x * cs - y * sn;
            y = x * sn + y * cs;
            x = px;
        }

        public Vector2 GetVector2()
        {
            return new Vector2(x, y);
        }




        public double Angle(ref FxVector2f vec)
        {
            double val =  Math.Acos(this.Dot(ref vec) / (vec.Length() * this.Length()));
            return val;
        }
    }




    #region Converter

    public class FxVector2fConverter : ExpandableObjectConverter
    {

        public override bool CanConvertFrom(
         ITypeDescriptorContext context, Type t )
        {

            if ( t == typeof( string ) ) {
                return true;
            }
            return base.CanConvertFrom( context, t );
        }

        public override object ConvertFrom(
         ITypeDescriptorContext context,
         CultureInfo info,
          object value )
        {

            if ( value is string ) {

                string s = (string)value;
                // parse the format "(X,Y)
                //
                char []splitChar = { ' ', ',', ')', '(' };

                // split the string to the elements
                string[] elements = s.Trim().Split( splitChar, StringSplitOptions.RemoveEmptyEntries );

                // check if we have only 2 elementex   (x,y)
                if ( elements.Length == 2 ) {
                    float X_result;
                    if ( float.TryParse( elements[0], out X_result ) ) {
                        float Y_result;
                        if ( float.TryParse( elements[1], out Y_result ) ) {
                            return new FxVector2f( X_result, Y_result );
                        }
                    }
                }

                // if we got this far, complain that we
                // couldn't parse the string
                //
                throw new ArgumentException(
                   "Can not convert '" + (string)value +
                            "' to type FxVector2f" );

            }

            // if we got this far, complain that we
            // couldn't parse the string
            //
            throw new ArgumentException(
               "Can not convert '" + (string)value +
                        "' to type FxVector2f" );
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destType )
        {
            if ( destType == typeof( string ) && value is FxVector2f ) {
                FxVector2f vec = (FxVector2f)value;
                // simply build the string as "Last, First (Age)"
                return vec.ToString();
            }
            return base.ConvertTo( context, culture, value, destType );
        }
    }

    #endregion



    public static class FxVector2fUtils
    {
        #region Random
        public static FxVector2f NextFxVector2f(this Random random, FxVector2f min, FxVector2f max)
        {
            FxVector2f r;
            r.x = (float)(random.NextDouble() * max.x + min.x);
            r.y = (float)(random.NextDouble() * max.y + min.y);
            return r;
        }

        public static FxVector2f NextFxVector2f(this Random random)
        {
            FxVector2f r;
            r.x = (float)(random.NextDouble());
            r.y = (float)(random.NextDouble());
            return r;
        }
        #endregion
    }

}
