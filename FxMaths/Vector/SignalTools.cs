using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxMaths.Vector
{
    public static class SignalTools
    {

        #region DFT

        /// <summary>
        /// DFT with real input
        /// </summary>
        /// <param name="real"></param>
        /// <param name="imag"></param>
        /// <param name="Forward"></param>
        /// <param name="resultReal"></param>
        /// <param name="resultImag"></param>
        static public void DFT_F( FxVectorF real, Boolean Forward, out FxVectorF resultReal, out  FxVectorF resultImag )
        {
            // set the direction of the DFT
            double dir = ( Forward ) ? -1 : 1;
            int size = real.Size;

            // allocate result
            FxVectorF resultRealTmp = new FxVectorF( size );
            FxVectorF resultImagTmp = new FxVectorF( size );

            // pass all the datas
            Parallel.For( 0, size, ( i ) => {
                //for ( int i=0; i < size; i++ ) {
                double arg = -dir * 2.0 * Math.PI * (double)i / size;

                for ( int k = 0; k < size; k++ ) {
                    double cosarg = Math.Cos( k * arg );
                    double sinarg = Math.Sin( k * arg );
                    resultRealTmp[i] += (float)( real[k] * cosarg );
                    resultImagTmp[i] += (float)( real[k] * sinarg );
                }
            } );

            resultReal = resultRealTmp;
            resultImag = resultImagTmp;

            // normalize when we have reverce
            if ( !Forward ) {
                resultReal.Divide( size );
                resultImag.Divide( size );
            }

        }

        /// <summary>
        /// DFT 
        /// </summary>
        /// <param name="real"></param>
        /// <param name="imag"></param>
        /// <param name="Forward"></param>
        /// <param name="resultReal"></param>
        /// <param name="resultImag"></param>
        static public void DFT_F( FxVectorF real, FxVectorF imag, Boolean Forward, out FxVectorF resultReal, out  FxVectorF resultImag )
        {
            // set the direction of the DFT
            double dir = ( Forward ) ? -1 : 1;
            double arg, cosarg,sinarg;
            int size = real.Size;

            // allocate result
            FxVectorF resultRealTmp = new FxVectorF( size );
            FxVectorF resultImagTmp = new FxVectorF( size );

            // pass all the datas
            Parallel.For( 0, size, ( i ) => {
                arg = -dir * 2.0 * Math.PI * (double)i / size;

                for ( int k = 0; k < size; k++ ) {
                    cosarg = Math.Cos( k * arg );
                    sinarg = Math.Sin( k * arg );
                    resultRealTmp[i] += (float)( real[k] * cosarg - imag[k] * sinarg );
                    resultImagTmp[i] += (float)( real[k] * sinarg + imag[k] * cosarg );
                }
            } );

            // pass the result to the output
            resultReal = resultRealTmp;
            resultImag = resultImagTmp;

            // normalize when we have reverce
            if ( !Forward ) {
                resultReal.Divide( size );
                resultImag.Divide( size );
            }

        }

        #endregion


        #region Convolution

        /// <summary>
        /// Result = A (*) B
        /// By using direct form for calculations
        /// faster for small size filter
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="Result"></param>
        static public void Convolution_F( FxVectorF A, FxVectorF B, out FxVectorF Result )
        {

            // get the max size from A,B
            int maxSize = ( A.Size > B.Size ) ? A.Size : B.Size;
            int minSize = ( A.Size < B.Size ) ? A.Size : B.Size;

            // allocate  for result
            Result = new FxVectorF( A.Size + B.Size - 1 );

            int n_lo,n_hi;
            for ( int i=0; i < Result.Size; i++ ) {
                float s=0;
                n_lo = ( 0 > ( i - B.Size + 1 ) ) ? 0 : i - B.Size + 1;
                n_hi = ( A.Size - 1 < i ) ? A.Size - 1 : i;
                for ( int n=n_lo; n <= n_hi; n++ ) {
                    s += A[n_lo] * B[i - n_lo];
                    n_lo++;
                }
                Result[i] = s;
            }


        }
                
    


        /// <summary>
        /// Result = A (*) B
        /// By using FFT for calculations
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="Result"></param>
        static public void Convolution_FFT_F( FxVectorF A, FxVectorF B, out FxVectorF Result )
        {
            FxVectorF A_dftReal,A_dftImag,B_dftReal,B_dftImag;

            // copy local the inputs
            FxVectorF A_local = A.GetCopy() as FxVectorF;
            FxVectorF B_local = B.GetCopy() as FxVectorF;

            // get the max size from A,B
            int maxSize = ( A_local.Size > B_local.Size ) ? A_local.Size : B_local.Size;

            // check that is power of 2
            if ( Math.Pow( 2, Math.Log( maxSize, 2 ) ) != maxSize ) {
                // calc the size of log2 extence
                int sizeLog2 = (int)Math.Floor( Math.Log( maxSize, 2 ) ) + 1;

                // calc the new maxSize
                maxSize = (int)Math.Pow( 2, sizeLog2 );

                // increase the size of A,B
                A_local.Padding( maxSize - A_local.Size, true, 0 );
                B_local.Padding( maxSize - B_local.Size, true, 0 );
            }

            // allocate temp file
            FxVectorF tmp = new FxVectorF( maxSize );

            // check for padding
            if ( A_local.Size == maxSize && B_local.Size != maxSize ) {
                // increase the size of B
                B_local.Padding( maxSize - B_local.Size, true, 0 );
            } else if ( B_local.Size == maxSize && A_local.Size != maxSize ) {
                A_local.Padding( maxSize - A_local.Size, true, 0 );
            }

            // calc the DFT of the signal
            SignalTools.FFT_F( A_local, tmp, true, out A_dftReal, out A_dftImag );

            // calc the DFT of the signal
            SignalTools.FFT_F( B_local, tmp, true, out B_dftReal, out B_dftImag );

            // allocate result vector
            Result = new FxVectorF( maxSize );

            float real,imag;

            // do the multiplication
            for ( int i=0; i < maxSize; i++ ) {
                // complex multiplication
                real = A_dftReal[i] * B_dftReal[i] - A_dftImag[i] * B_dftImag[i];
                imag = A_dftReal[i] * B_dftImag[i] + A_dftImag[i] * B_dftReal[i];

                // set the new values
                A_dftReal[i] = real;
                A_dftImag[i] = imag;
            }


            // calc the DFT of the signal
            SignalTools.FFT_F( A_dftReal, A_dftImag, false, out Result, out B_dftImag );

            // cat the padding
            int maxInputSize = ( A.Size > B.Size ) ? A.Size : B.Size;
            Result = Result.GetSubVector( maxSize - maxInputSize, Result.Size) as FxVectorF;

        }

        /// <summary>
        /// Result = A (*) B
        /// By using FFT for calculations
        /// The second vector is allready in fft form
        /// The two vectors must have the same size and be power of 2
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B_dftReal"></param>
        /// <param name="B_dftImag"></param>
        /// <param name="Result"></param>
        public static void Convolution_FFT_F( FxVectorF A, FxVectorF B_dftReal, FxVectorF B_dftImag, out FxVectorF Result )
        {
            FxVectorF A_dftReal,A_dftImag;

            // copy local the inputs
            FxVectorF A_local = A.GetCopy() as FxVectorF;

            // get the max size from A,B
            int maxSize = ( A_local.Size > B_dftReal.Size ) ? A_local.Size : B_dftReal.Size;


            // calc the DFT of the signal
            SignalTools.FFT_F( A_local, null, true, out A_dftReal, out A_dftImag );

            // allocate result vector
            Result = new FxVectorF( maxSize );

            float real,imag;

            // do the multiplication
            for ( int i=0; i < maxSize; i++ ) {
                // complex multiplication
                real = A_dftReal[i] * B_dftReal[i] - A_dftImag[i] * B_dftImag[i];
                imag = A_dftReal[i] * B_dftImag[i] + A_dftImag[i] * B_dftReal[i];

                // set the new values
                A_dftReal[i] = real;
                A_dftImag[i] = imag;
            }

            // calc the DFT of the signal
            SignalTools.FFT_F( A_dftReal, A_dftImag, false, out Result, out B_dftImag );

            // cat the padding
            int maxInputSize = ( A.Size > B_dftReal.Size ) ? A.Size : B_dftReal.Size;
            Result = Result.GetSubVector( maxSize - maxInputSize, Result.Size ) as FxVectorF;
        }

        /// <summary>
        /// Result = A (*) B
        /// By using DFT for calculations
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="Result"></param>
        static public void Convolution_DFT_F( FxVectorF A, FxVectorF B, out FxVectorF Result )
        {
            FxVectorF A_dftReal,A_dftImag,B_dftReal,B_dftImag;

            TimeStatistics.ClockLap( "Enter" );

            // get the max size from A,B
            int maxSize = ( A.Size > B.Size ) ? A.Size : B.Size;

            // check for padding
            if ( A.Size == maxSize && B.Size != maxSize ) {
                // increase the size of B
                B.Padding( maxSize - B.Size, true, 0 );
            } else if ( B.Size == maxSize && A.Size != maxSize ) {
                A.Padding( maxSize - A.Size, true, 0 );
            }

            TimeStatistics.ClockLap( "AfterPadding" );

            // calc the DFT of the signal
            SignalTools.DFT_F( A, true, out A_dftReal, out A_dftImag );

            TimeStatistics.ClockLap( "After DFT A" );

            // calc the DFT of the signal
            SignalTools.DFT_F( B, true, out B_dftReal, out B_dftImag );

            TimeStatistics.ClockLap( "After DFT B" );

            // allocate result vector
            Result = new FxVectorF( maxSize );

            float real,imag;

            // do the multiplication
            for ( int i=0; i < maxSize; i++ ) {
                // complex multiplication
                real = A_dftReal[i] * B_dftReal[i] - A_dftImag[i] * B_dftImag[i];
                imag = A_dftReal[i] * B_dftImag[i] + A_dftImag[i] * B_dftReal[i];

                // set the new values
                A_dftReal[i] = real;
                A_dftImag[i] = imag;
            }

            TimeStatistics.ClockLap( "After multiplication" );

            // calc the DFT of the signal
            SignalTools.DFT_F( A_dftReal, A_dftImag, false, out Result, out B_dftImag );

            TimeStatistics.ClockLap( "After iDFT" );
        }

       
        #endregion


        #region FFT

        /// <summary>
        /// Radix-2 Step 
        /// </summary>
        /// <param name="real">The real part of input</param>
        /// <param name="imag">The imag part of input</param>
        /// <param name="exponentSign">Fourier series exponent sign.</param>
        /// <param name="levelSize">Level Group Size.</param>
        /// <param name="k">Index inside of the level.</param>
        private static void Radix2Step( FxVectorF real, FxVectorF imag, int exponentSign, int levelSize, int k )
        {
            // Twiddle Factor
            var exponent = ( exponentSign * k ) * Math.PI / levelSize;
            float wR = (float)Math.Cos( exponent );
            float wI = (float)Math.Sin( exponent );

            var step = levelSize << 1;
            for ( var i = k; i < real.Size; i += step ) {
                float aiR = real[i];
                float aiI = imag[i];

                // complex multiplication
                float tR = wR * real[i + levelSize] - wI * imag[i + levelSize];
                float tI = wR * imag[i + levelSize] + wI * real[i + levelSize];

                real[i] = aiR + tR;
                real[i + levelSize] = aiR - tR;

                imag[i] = aiI + tI;
                imag[i + levelSize] = aiI - tI;
            }
        }

        /// <summary>
        /// FFT algorithm
        /// </summary>
        /// <param name="real">The real part of input</param>
        /// <param name="imag">The imag part of input</param>
        /// <param name="Forward">The </param>
        /// <param name="resultReal">The real part of the result</param>
        /// <param name="resultImag">The real imag of the result</param>
        static public void FFT_F( FxVectorF real, FxVectorF imag, Boolean Forward, out FxVectorF resultReal, out  FxVectorF resultImag )
        {
            // find the size and log2(size)
            int size = real.Size;
            int sizeLog2 = (int)Math.Log( size, 2 );

            // allocate result
            FxVectorF resultRealTmp = new FxVectorF( size );
            FxVectorF resultImagTmp = new FxVectorF( size );

            // Do the bit reversal
            int i2 = size >> 1;
            var j = 0;
            for ( var i = 0; i < size - 1; i++ ) {
                if ( i < j ) {
                    resultRealTmp[i] = real[j];
                    resultRealTmp[j] = real[i];

                    if ( imag == null ) {
                        resultImagTmp[i] = 0;
                        resultImagTmp[j] = 0;
                    } else {
                        resultImagTmp[i] = imag[j];
                        resultImagTmp[j] = imag[i];
                    }
                }

                var m = size;

                do {
                    m >>= 1;
                    j ^= m;
                }
                while ( ( j & m ) == 0 );
            }

            // set the sign of exponents
            int exponentSign = ( Forward ) ? 1 : -1;

            // pass all the level of fft tree and compute the fourier
            for ( var levelSize = 1; levelSize < size; levelSize *= 2 ) {
                Parallel.For(
                    0,
                    levelSize,
                    index => Radix2Step( resultRealTmp, resultImagTmp, exponentSign, levelSize, index ) );
            }

            // set the out result
            resultReal = resultRealTmp;
            resultImag = resultImagTmp;

            // normalize when we have reverce
            if ( !Forward ) {
                resultReal.Divide( size );
                resultImag.Divide( size );
            }
        }

        /// <summary>
        /// FFT algorithm with internal padding to power of 2
        /// </summary>
        /// <param name="real">The real part of input</param>
        /// <param name="imag">The imag part of input</param>
        /// <param name="Forward">The </param>
        /// <param name="resultReal">The real part of the result</param>
        /// <param name="resultImag">The real imag of the result</param>
        static public void FFT_Safe_F( FxVectorF real, FxVectorF imag, Boolean Forward, out FxVectorF resultReal, out  FxVectorF resultImag )
        {
            // copy local the inputs
            FxVectorF real_local = real.GetCopy() as FxVectorF;
            FxVectorF imag_local = ( imag != null ) ? imag.GetCopy() as FxVectorF : null;

            // get the max size from A,B
            int maxSize;
            if ( imag_local != null )
                maxSize = ( real_local.Size > imag_local.Size ) ? real_local.Size : imag_local.Size;
            else
                maxSize = real_local.Size;

            // check that is power of 2
            if ( Math.Pow( 2, Math.Floor( Math.Log( maxSize, 2 ) ) ) != maxSize ) {
                // calc the size of log2 extence
                int sizeLog2 = (int)Math.Floor( Math.Log( maxSize, 2 ) ) + 1;

                // calc the new maxSize
                maxSize = (int)Math.Pow( 2, sizeLog2 );

                // increase the size of A,B
                real_local.Padding( maxSize - real_local.Size, true, 0 );

                if ( imag_local != null )
                    imag_local.Padding( maxSize - imag_local.Size, true, 0 );
            }

            // call the unsafe fft
            FFT_F( real_local, imag_local, Forward, out resultReal, out resultImag );
        }


        #endregion


    }
}
