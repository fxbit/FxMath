using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FxMaths.Vector;

namespace FxMaths.DSP
{
    public class IIR_GenericFilter : IFilter
    {

        #region Variables

        /// <summary>
        /// Feedback filter coefficients
        /// </summary>
        private FxVectorF A_Data;

        /// <summary>
        /// Feedforward filter coefficients
        /// </summary>
        private FxVectorF B_Data;

        /// <summary>
        /// The offset for the filtering
        /// </summary>
        private int maxOffset;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new filter base on existing data 
        /// </summary>
        /// <param name="A_Data">Feedback filter coefficients</param
        /// <param name="B_Data">Feedforward filter coefficients</param>
        public IIR_GenericFilter(FxVectorF A_Data,FxVectorF B_Data)
        {
            // create copy of the data for internal use
            this.A_Data = A_Data.GetCopy() as FxVectorF;
            this.B_Data = B_Data.GetCopy() as FxVectorF;

            // normalize base on a0 coefficients
            this.A_Data.Divide(this.A_Data[0]);
            this.B_Data.Divide(this.A_Data[0]);

            // set the max offset
            maxOffset = (A_Data.Size > B_Data.Size) ? A_Data.Size : B_Data.Size;

            float maxManhattan = (float)this.A_Data.Norms(NormVectorType.Manhattan);
            if (maxManhattan < (float)this.B_Data.Norms(NormVectorType.Manhattan))
            {
                maxManhattan = (float)this.B_Data.Norms(NormVectorType.Manhattan);
            }

            // normalize base on a0 coefficients
            this.A_Data.Divide(maxManhattan);
            this.B_Data.Divide(maxManhattan);
        }

        #endregion

        #region IFilter methods


        #region Transform


        public void Transform(Vector.FxVectorF inBuffer, Vector.FxVectorF outBuffer)
        {
            float result = 0;

            /// cacl the filter in setup time
            for (int n = 0; n < maxOffset; n++)
            {
                // reset the result
                result = 0;

                // calc the window of calculations
                int maxOffset_B = (n > B_Data.Size) ? B_Data.Size : n;
                int maxOffset_A = (n > A_Data.Size) ? A_Data.Size : n;

                // calc the Feedback values
                for (int i = 0; i < maxOffset_B; i++)
                    result += B_Data[i] * inBuffer[n - i];

                // calc the Feedforward values
                for (int i = 1; i < maxOffset_A; i++)
                    result -= A_Data[i] * outBuffer[n - i];

                // update the outBuffer
                outBuffer[n] = result;
            }

            /// pass all the data and execute the iir filter
            for (int n = maxOffset; n < inBuffer.Size; n++)
            {
                // reset the result
                result = 0;

                // calc the Feedback values
                for (int i = 0; i < B_Data.Size; i++)
                    result += B_Data[i] * inBuffer[n - i];

                // calc the Feedforward values
                for (int i = 1; i < A_Data.Size; i++)
                    result -= A_Data[i] * outBuffer[n - i];

                // update the outBuffer
                outBuffer[n] = result;
            }
        }

        public void Transform(Vector.FxVectorF inBuffer, out Vector.FxVectorF outBuffer)
        {
            // allocate the output filter
            outBuffer = new FxVectorF(inBuffer.Size);

            this.Transform(inBuffer, outBuffer);
        }

        public void Transform(float[] inBuffer, float[] outBuffer)
        {
            float result = 0;

            /// cacl the filter in setup time
            for (int n = 0; n < maxOffset; n++)
            {
                result = 0;

                // calc the window of calculations
                int maxOffset_B = (n > B_Data.Size) ? B_Data.Size : n;
                int maxOffset_A = (n > A_Data.Size) ? A_Data.Size : n;

                // calc the Feedback values
                for (int i = 0; i < maxOffset_B; i++)
                    result += B_Data[i] * inBuffer[n - i];

                // calc the Feedforward values
                for (int i = 1; i < maxOffset_A; i++)
                    result -= A_Data[i] * outBuffer[n - i];

                // update the outBuffer
                outBuffer[n] = result;
            }

            /// pass all the data and execute the iir filter
            for (int n = maxOffset; n < inBuffer.Length; n++)
            {
                result = 0;

                // calc the Feedback values
                for (int i = 0; i < B_Data.Size; i++)
                    result += B_Data[i] * inBuffer[n - i];

                // calc the Feedforward values
                for (int i = 1; i < A_Data.Size; i++)
                    result -= A_Data[i] * outBuffer[n - i];

                // update the outBuffer
                outBuffer[n] = result;
            }
        }

        #endregion



        #region Freq Resp

        public FxVectorF GetFrequencyResponse(int resolution)
        {
            // copy local the coefficiens
            FxVectorF A_Data_Local = A_Data.GetCopy() as FxVectorF;
            FxVectorF B_Data_Local = B_Data.GetCopy() as FxVectorF;

            // padding A
            if (2 * resolution > A_Data_Local.Size)
                A_Data_Local.Padding(2 * resolution - A_Data_Local.Size, true, 0);

            // padding B
            if (2 * resolution > B_Data_Local.Size)
                B_Data_Local.Padding(2 * resolution - B_Data_Local.Size, true, 0);

            FxVectorF tmpRealA, tmpRealB, tmpImagA, tmpImagB,resultReal,resultImag;

            // fft of A
            SignalTools.FFT_F(A_Data_Local, null, true, out tmpRealA, out tmpImagA);

            // fft of A
            SignalTools.FFT_F(B_Data_Local, null, true, out tmpRealB, out tmpImagB);

            // complex division
            Complex.ComplexTools.Division(tmpRealB, tmpImagB, tmpRealA, tmpImagA, out resultReal, out resultImag);


            FxVectorF result = new FxVectorF(resolution);
            // calc the arg
            for (int j = 0; j < resolution; j++)
            {
                result[j] = (float)(Math.Sqrt(resultReal[j] * resultReal[j] + resultImag[j] * resultImag[j]));
            }

            return result;
        }

        #endregion



        #endregion


    }
}
