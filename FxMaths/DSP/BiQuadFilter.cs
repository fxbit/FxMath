// based on Cookbook formulae for audio EQ biquad filter coefficients
// http://www.musicdsp.org/files/Audio-EQ-Cookbook.txt
// by Robert Bristow-Johnson  <rbj@audioimagination.com>

//    alpha = sin(w0)/(2*Q)                                       (case: Q)
//          = sin(w0)*sinh( ln(2)/2 * BW * w0/sin(w0) )           (case: BW)
//          = sin(w0)/2 * sqrt( (A + 1/A)*(1/S - 1) + 2 )         (case: S)
// Q: (the EE kind of definition, except for peakingEQ in which A*Q is
// the classic EE Q.  That adjustment in definition was made so that
// a boost of N dB followed by a cut of N dB for identical Q and
// f0/Fs results in a precisely flat unity gain filter or "wire".)
//
// BW: the bandwidth in octaves (between -3 dB frequencies for BPF
// and notch or between midpoint (dBgain/2) gain frequencies for
// peaking EQ)
//
// S: a "shelf slope" parameter (for shelving EQ only).  When S = 1,
// the shelf slope is as steep as it can be and remain monotonically
// increasing or decreasing gain with frequency.  The shelf slope, in
// dB/octave, remains proportional to S for all other values for a
// fixed f0/Fs and dBgain.

using System;
using System.Collections.Generic;
using System.Text;
using FxMaths.Vector;

namespace FxMaths.DSP
{
    public enum BiQuadFilterType
    {
        Unused,
        LowPassFilter,
        HighPassFilter,
        BandPassFilterConstantSkirtGain,
        BandPassFilterConstantPeakGain,
        NotchFilter,
        AllPassFilter,
        PeakingEQ,
        LowShelf,
        HighShelf
    }

    public class BiQuadFilter : IFilter
    {
        // coefficients
        double a0;
        double a1;
        double a2;
        double b0;
        double b1;
        double b2;

        BiQuadFilterType _FilterType = BiQuadFilterType.Unused;

        public BiQuadFilterType FilterType
        {
            get { return _FilterType; }
        }


        private BiQuadFilter()
        {

        }

        #region IFilter Members


        #region Transform

        /// <summary>
        /// Use the filter in the input data
        /// </summary>
        /// <param name="inBuffer"></param>
        /// <param name="outBuffer"></param>
        public void Transform(FxVectorF inBuffer, FxVectorF outBuffer)
        {
            // calc the actual parameter of filter
            float h0 = (float)(b0 / a0);
            float h1 = (float)(b1 / a0);
            float h2 = (float)(b2 / a0);
            float h3 = (float)(a1 / a0);
            float h4 = (float)(a2 / a0);

            //  canv the filter
            for (int n = 2; n < inBuffer.Size; n++)
            {
                outBuffer[n] = h0 * inBuffer[n] + h1 * inBuffer[n - 1] + h2 * inBuffer[n - 2]
                    - h3 * outBuffer[n - 1] - h4 * outBuffer[n - 2];
            }
        }

        /// <summary>
        ///  Use the filter in the input data
        /// </summary>
        /// <param name="inBuffer"></param>
        /// <param name="outBuffer"></param>
        public void Transform(float[] inBuffer, float[] outBuffer)
        {
            float[] x = inBuffer;
            float[] y = outBuffer;

            // calc the actual parameter of filter
            float h0 = (float)(b0 / a0);
            float h1 = (float)(b1 / a0);
            float h2 = (float)(b2 / a0);
            float h3 = (float)(a1 / a0);
            float h4 = (float)(a2 / a0);

            //  canv the filter
            for (int n = 2; n < inBuffer.Length; n++)
            {
                y[n] = h0 * x[n] + h1 * x[n - 1] + h2 * x[n - 2]
                    - h3 * y[n - 1] - h4 * y[n - 2];
            }
        }

        public void Transform(FxVectorF inBuffer, out FxVectorF outBuffer)
        {
            // allocate the output filter
            outBuffer = new FxVectorF(inBuffer.Size);

            this.Transform(inBuffer, outBuffer);
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
            this.Transform(inFilter,out filter);

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
            FreqResponse.Divide((float)(mod*Math.Sqrt(2)));

            return FreqResponse;
        }

        #endregion

        #endregion

        #region Creation

        /// <summary>
        /// H(s) = 1 / (s^2 + s/Q + 1)
        /// </summary>
        public static BiQuadFilter LowPassFilter(float sampleRate, float cutoffFrequency, float q)
        {
            double w0 = 2 * Math.PI * cutoffFrequency / sampleRate;
            double cosw0 = Math.Cos(w0);
            double alpha = Math.Sin(w0) / (2 * q);

            BiQuadFilter filter = new BiQuadFilter();

            filter.b0 = (1 - cosw0) / 2;
            filter.b1 = 1 - cosw0;
            filter.b2 = (1 - cosw0) / 2;
            filter.a0 = 1 + alpha;
            filter.a1 = -2 * cosw0;
            filter.a2 = 1 - alpha;
            filter._FilterType = BiQuadFilterType.LowPassFilter;

            return filter;
        }

        /// <summary>
        /// H(s) = s^2 / (s^2 + s/Q + 1)
        /// </summary>
        public static BiQuadFilter HighPassFilter(float sampleRate, float cutoffFrequency, float q)
        {
            double w0 = 2 * Math.PI * cutoffFrequency / sampleRate;
            double cosw0 = Math.Cos(w0);
            double alpha = Math.Sin(w0) / (2 * q);

            BiQuadFilter filter = new BiQuadFilter();

            filter.b0 = (1 + Math.Cos(w0)) / 2;
            filter.b1 = -(1 + Math.Cos(w0));
            filter.b2 = (1 + Math.Cos(w0)) / 2;
            filter.a0 = 1 + alpha;
            filter.a1 = -2 * Math.Cos(w0);
            filter.a2 = 1 - alpha;
            filter._FilterType = BiQuadFilterType.HighPassFilter;

            return filter;
        }

        /// <summary>
        /// H(s) = s / (s^2 + s/Q + 1)  (constant skirt gain, peak gain = Q)
        /// </summary>
        public static BiQuadFilter BandPassFilterConstantSkirtGain(float sampleRate, float centreFrequency, float q)
        {
            double w0 = 2 * Math.PI * centreFrequency / sampleRate;
            double cosw0 = Math.Cos(w0);
            double sinw0 = Math.Sin(w0);
            double alpha = sinw0 / (2 * q);

            BiQuadFilter filter = new BiQuadFilter();
            filter.b0 = sinw0 / 2; // =   Q*alpha
            filter.b1 = 0;
            filter.b2 = -sinw0 / 2; // =  -Q*alpha
            filter.a0 = 1 + alpha;
            filter.a1 = -2 * cosw0;
            filter.a2 = 1 - alpha;
            filter._FilterType = BiQuadFilterType.BandPassFilterConstantSkirtGain;
            return filter;
        }

        /// <summary>
        /// H(s) = (s/Q) / (s^2 + s/Q + 1)      (constant 0 dB peak gain)
        /// </summary>
        public static BiQuadFilter BandPassFilterConstantPeakGain(float sampleRate, float centreFrequency, float q)
        {
            double w0 = 2 * Math.PI * centreFrequency / sampleRate;
            double cosw0 = Math.Cos(w0);
            double sinw0 = Math.Sin(w0);
            double alpha = sinw0 / (2 * q);

            BiQuadFilter filter = new BiQuadFilter();
            filter.b0 = alpha;
            filter.b1 = 0;
            filter.b2 = -alpha;
            filter.a0 = 1 + alpha;
            filter.a1 = -2 * cosw0;
            filter.a2 = 1 - alpha;
            filter._FilterType = BiQuadFilterType.BandPassFilterConstantPeakGain;
            return filter;
        }

        /// <summary>
        /// H(s) = (s^2 + 1) / (s^2 + s/Q + 1)
        /// </summary>
        public static BiQuadFilter NotchFilter(float sampleRate, float centreFrequency, float q)
        {
            double w0 = 2 * Math.PI * centreFrequency / sampleRate;
            double cosw0 = Math.Cos(w0);
            double sinw0 = Math.Sin(w0);
            double alpha = sinw0 / (2 * q);

            BiQuadFilter filter = new BiQuadFilter();
            filter.b0 = 1;
            filter.b1 = -2 * cosw0;
            filter.b2 = 1;
            filter.a0 = 1 + alpha;
            filter.a1 = -2 * cosw0;
            filter.a2 = 1 - alpha;
            filter._FilterType = BiQuadFilterType.NotchFilter;
            return filter;
        }

        /// <summary>
        /// H(s) = (s^2 - s/Q + 1) / (s^2 + s/Q + 1)
        /// </summary>
        public static BiQuadFilter AllPassFilter(float sampleRate, float centreFrequency, float q)
        {
            double w0 = 2 * Math.PI * centreFrequency / sampleRate;
            double cosw0 = Math.Cos(w0);
            double sinw0 = Math.Sin(w0);
            double alpha = sinw0 / (2 * q);

            BiQuadFilter filter = new BiQuadFilter();
            filter.b0 = 1 - alpha;
            filter.b1 = -2 * cosw0;
            filter.b2 = 1 + alpha;
            filter.a0 = 1 + alpha;
            filter.a1 = -2 * cosw0;
            filter.a2 = 1 - alpha;
            filter._FilterType = BiQuadFilterType.AllPassFilter;
            return filter;
        }

        /// <summary>
        /// H(s) = (s^2 + s*(A/Q) + 1) / (s^2 + s/(A*Q) + 1)
        /// </summary>
        public static BiQuadFilter PeakingEQ(float sampleRate, float centreFrequency, float q, float dbGain)
        {
            double w0 = 2 * Math.PI * centreFrequency / sampleRate;
            double cosw0 = Math.Cos(w0);
            double sinw0 = Math.Sin(w0);
            double alpha = sinw0 / (2 * q);
            double A = Math.Pow(10, dbGain / 40);     // TODO: should we square root this value?

            BiQuadFilter filter = new BiQuadFilter();
            filter.b0 = 1 + alpha * A;
            filter.b1 = -2 * cosw0;
            filter.b2 = 1 - alpha * A;
            filter.a0 = 1 + alpha / A;
            filter.a1 = -2 * cosw0;
            filter.a2 = 1 - alpha / A;
            filter._FilterType = BiQuadFilterType.PeakingEQ;
            return filter;
        }

        /// <summary>
        /// H(s) = A * (s^2 + (sqrt(A)/Q)*s + A)/(A*s^2 + (sqrt(A)/Q)*s + 1)
        /// </summary>
        /// <param name="sampleRate"></param>
        /// <param name="cutoffFrequency"></param>
        /// <param name="shelfSlope">a "shelf slope" parameter (for shelving EQ only).  
        /// When S = 1, the shelf slope is as steep as it can be and remain monotonically
        /// increasing or decreasing gain with frequency.  The shelf slope, in dB/octave, 
        /// remains proportional to S for all other values for a fixed f0/Fs and dBgain.</param>
        /// <param name="dbGain">Gain in decibels</param>
        public static BiQuadFilter LowShelf(float sampleRate, float cutoffFrequency, float shelfSlope, float dbGain)
        {
            double w0 = 2 * Math.PI * cutoffFrequency / sampleRate;
            double cosw0 = Math.Cos(w0);
            double sinw0 = Math.Sin(w0);
            double A = Math.Pow(10, dbGain / 40);     // TODO: should we square root this value?
            double alpha = sinw0 / 2 * Math.Sqrt((A + 1 / A) * (1 / shelfSlope - 1) + 2);
            double temp = 2 * Math.Sqrt(A) * alpha;
            BiQuadFilter filter = new BiQuadFilter();
            filter.b0 = A * ((A + 1) - (A - 1) * cosw0 + temp);
            filter.b1 = 2 * A * ((A - 1) - (A + 1) * cosw0);
            filter.b2 = A * ((A + 1) - (A - 1) * cosw0 - temp);
            filter.a0 = (A + 1) + (A - 1) * cosw0 + temp;
            filter.a1 = -2 * ((A - 1) + (A + 1) * cosw0);
            filter.a2 = (A + 1) + (A - 1) * cosw0 - temp;
            filter._FilterType = BiQuadFilterType.LowShelf;
            return filter;
        }

        /// <summary>
        /// H(s) = A * (A*s^2 + (sqrt(A)/Q)*s + 1)/(s^2 + (sqrt(A)/Q)*s + A)
        /// </summary>
        /// <param name="sampleRate"></param>
        /// <param name="cutoffFrequency"></param>
        /// <param name="shelfSlope"></param>
        /// <param name="dbGain"></param>
        /// <returns></returns>
        public static BiQuadFilter HighShelf(float sampleRate, float cutoffFrequency, float shelfSlope, float dbGain)
        {
            double w0 = 2 * Math.PI * cutoffFrequency / sampleRate;
            double cosw0 = Math.Cos(w0);
            double sinw0 = Math.Sin(w0);
            double A = Math.Pow(10, dbGain / 40);     // TODO: should we square root this value?
            double alpha = sinw0 / 2 * Math.Sqrt((A + 1 / A) * (1 / shelfSlope - 1) + 2);
            double temp = 2 * Math.Sqrt(A) * alpha;

            BiQuadFilter filter = new BiQuadFilter();
            filter.b0 = A * ((A + 1) + (A - 1) * cosw0 + temp);
            filter.b1 = -2 * A * ((A - 1) + (A + 1) * cosw0);
            filter.b2 = A * ((A + 1) + (A - 1) * cosw0 - temp);
            filter.a0 = (A + 1) - (A - 1) * cosw0 + temp;
            filter.a1 = 2 * ((A - 1) - (A + 1) * cosw0);
            filter.a2 = (A + 1) - (A - 1) * cosw0 - temp;
            filter._FilterType = BiQuadFilterType.HighShelf;
            return filter;
        }

        #endregion

        /// <summary>
        /// Copy the value of other filter to this filter
        /// </summary>
        /// <param name="fil"></param>
        public void UpdateValues(BiQuadFilter fil)
        {
            this.a0 = fil.a0;
            this.a1 = fil.a1;
            this.a2 = fil.a2;
            this.b0 = fil.b0;
            this.b1 = fil.b1;
            this.b2 = fil.b2;
            this._FilterType = fil._FilterType;
        }

    }
}

