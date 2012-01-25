﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths
{
    public static class MiscUtils
    {
        #region Generic Min/Max

        /// <summary>
        /// Get the max value of a number
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Object GetMaxValue<T>() where T : struct
        {
            Type t = typeof( T );

            if ( t == typeof( Double ) ) {
                return Double.MaxValue;
            } else if ( t == typeof( double ) ) {
                return double.MaxValue;
            } else if ( t == typeof( float ) ) {
                return float.MaxValue;
            } else if ( t == typeof( int ) ) {
                return int.MaxValue;
            } else if ( t == typeof( Int16 ) ) {
                return Int16.MaxValue;
            } else if ( t == typeof( Int32 ) ) {
                return Int32.MaxValue;
            }else if ( t == typeof( UInt64 ) ) {
                return UInt64.MaxValue;
            } else if ( t == typeof( uint ) ) {
                return uint.MaxValue;
            } else if ( t == typeof( UInt16 ) ) {
                return UInt16.MaxValue;
            } else if ( t == typeof( UInt32 ) ) {
                return UInt32.MaxValue;
            } else if ( t == typeof( UInt64 ) ) {
                return UInt64.MaxValue;
            } else if ( t == typeof( short ) ) {
                return short.MaxValue;
            } else if ( t == typeof( byte ) ) {
                return byte.MaxValue;
            } else if ( t == typeof( Byte ) ) {
                return Byte.MaxValue;
            }

            return 0;
        }

        /// <summary>
        /// Get the min value of a number 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Object GetMinValue<T>() where T : struct
        {
            Type t = typeof( T );

            if ( t == typeof( Double ) ) {
                return Double.MinValue;
            } else if ( t == typeof( double ) ) {
                return double.MinValue;
            } else if ( t == typeof( float ) ) {
                return float.MinValue;
            } else if ( t == typeof( int ) ) {
                return int.MinValue;
            } else if ( t == typeof( Int16 ) ) {
                return Int16.MinValue;
            } else if ( t == typeof( Int32 ) ) {
                return Int32.MinValue;
            } else if ( t == typeof( UInt64 ) ) {
                return UInt64.MinValue;
            } else if ( t == typeof( uint ) ) {
                return uint.MinValue;
            } else if ( t == typeof( UInt16 ) ) {
                return UInt16.MinValue;
            } else if ( t == typeof( UInt32 ) ) {
                return UInt32.MinValue;
            } else if ( t == typeof( UInt64 ) ) {
                return UInt64.MinValue;
            } else if ( t == typeof( short ) ) {
                return short.MinValue;
            } else if ( t == typeof( byte ) ) {
                return byte.MinValue;
            } else if ( t == typeof( Byte ) ) {
                return Byte.MinValue;
            }

            return 0;
        }

        #endregion
    }
}
