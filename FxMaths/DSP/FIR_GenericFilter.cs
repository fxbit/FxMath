using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FxMaths.Vector;

namespace FxMaths.DSP
{
    public class FIR_GenericFilter : IFilter
    {

        private FxVectorF p_Impulse;
        private FxVectorF p_ImpulseFFT_Real,p_ImpulseFFT_Imag;

        #region Properties
        public FxVectorF Impulse
        {
            get { return p_Impulse; }
        }

        public FxVectorF ImpulseFFT_Real
        {
            get { return p_ImpulseFFT_Real; }
        }

        public FxVectorF ImpulseFFT_Imag
        {
            get { return p_ImpulseFFT_Imag; }
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Create a new filter base on existing data 
        /// </summary>
        /// <param name="filter"></param>
        public FIR_GenericFilter( FxVectorF filter )
        {
            p_Impulse = filter.GetCopy() as FxVectorF;

            // normalize the filter
            p_Impulse.Divide( (float)p_Impulse.Norms( NormVectorType.Manhattan ) );

            // check that is power of 2 ( we do that for speed )
            if ( Math.Pow( 2, Math.Log( p_Impulse.Size, 2 ) ) != p_Impulse.Size ) {
                // calc the size of log2 extence
                int sizeLog2 = (int)Math.Floor( Math.Log( p_Impulse.Size, 2 ) ) + 1;

                // calc the new maxSize
                int newSize = (int)Math.Pow( 2, sizeLog2 );

                // increase the size of Impulse
                p_Impulse.Padding( newSize - p_Impulse.Size, true, 0 );
            }

            // calc the fft of impulse
            TransformImpulseToFFT();
        }

        #endregion

        #region IFilter Members

        #region Trasform

        /// <summary>
        /// Use the filter in the input data
        /// </summary>
        /// <param name="inBuffer"></param>
        /// <param name="outBuffer"></param>
        public void Transform( FxVectorF inBuffer, out FxVectorF outBuffer )
        {
            // make the convolution
            SignalTools.Convolution_FFT_F( inBuffer, p_Impulse, out outBuffer );
        }


        public void Transform( FxVectorF inBuffer, FxVectorF outBuffer )
        {

            FxVectorF outBufferTmp;
            FxVectorF inBuffer_local = inBuffer;

            if ( inBuffer.Size > p_Impulse.Size ) {
                // add zeros to the end of the impulse
                p_Impulse.Padding( inBuffer.Size - p_Impulse.Size, true, 0 );

                // update fft
                TransformImpulseToFFT();
            } 
            
            if ( inBuffer.Size < p_Impulse.Size ) {
                // copy the input buffer local
                inBuffer_local = inBuffer.GetCopy() as FxVectorF;

                // add zero sto teh end of the input buffer
                inBuffer_local.Padding( p_Impulse.Size - inBuffer_local.Size, true, 0 );
            }

            // make the convolution
            SignalTools.Convolution_FFT_F( inBuffer_local, p_ImpulseFFT_Real, p_ImpulseFFT_Imag, out outBufferTmp );

            if ( inBuffer_local != inBuffer ) {
                // copy the result to the data
                outBuffer.SetValue( outBufferTmp.GetSubVector( inBuffer_local.Size - inBuffer.Size, outBufferTmp.Size ) );
            } else {
                // copy the result to the data
                outBuffer.SetValue( outBufferTmp );
            }

        }

        public void Transform( float[] inBuffer, float[] outBuffer )
        {
            FxVectorF outBufferTmp,inBufferTmp;

            // copy the input data to vector
            inBufferTmp = new FxVectorF( inBuffer );

            // make the convolution
            SignalTools.Convolution_FFT_F( inBufferTmp, p_Impulse, out outBufferTmp );

            // copy the result to the data
            outBufferTmp.GetValue(ref outBuffer );
        }

        #endregion

        #region Freq Resp

        public FxVectorF GetFrequencyResponse(int resolution)
        {
            FxVectorF inFilter = new FxVectorF(4096);
            FxVectorF FreqResponse = new FxVectorF(resolution);
            FxVectorF tmpImag, tmpReal, power;
            FxVectorF filter;

            // calc the sinc for the size of the filter
            for (int i = 0; i < inFilter.Size; i++)
            {
                if (i == inFilter.Size / 2)
                    inFilter[i] = 1;
                else
                    inFilter[i] = 0;
            }

            // calc the impulse of the filter
            this.Transform(inFilter, out filter);

            power = new FxVectorF(256);
            int mod = (int)Math.Ceiling(filter.Size / ((double)power.Size * 2));


            // calc the fft of the filter
            SignalTools.FFT_Safe_F(filter, null, true, out tmpReal, out tmpImag);


            for (int i = 0; i < mod; i++)
            {
                // calc the power of the filter
                for (int j = 0; j < power.Size; j++)
                {
                    FreqResponse[j] += (float)(Math.Sqrt(tmpReal[j * mod + i] * tmpReal[j * mod + i] + tmpImag[j * mod + i] * tmpImag[j * mod + i]));
                }
            }


            // normalize
            FreqResponse.Divide((float)(mod * Math.Sqrt(2)));

            return FreqResponse;
        }

        #endregion

        #endregion

        #region Utils

        private void TransformImpulseToFFT()
        {
            // check that is power of 2 ( we do that for speed )
            if ( Math.Pow( 2, Math.Log( p_Impulse.Size, 2 ) ) != p_Impulse.Size ) {
                // calc the size of log2 extence
                int sizeLog2 = (int)Math.Floor( Math.Log( p_Impulse.Size, 2 ) ) + 1;

                // calc the new maxSize
                int newSize = (int)Math.Pow( 2, sizeLog2 );

                // increase the size of Impulse
                p_Impulse.Padding( newSize - p_Impulse.Size, true, 0 );
            }

            // calc the DFT of the signal
            SignalTools.FFT_F( p_Impulse,null, true, out p_ImpulseFFT_Real, out p_ImpulseFFT_Imag );

        }

        #endregion
    }
}
