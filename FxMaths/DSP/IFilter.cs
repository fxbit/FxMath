using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FxMaths.Vector;

namespace FxMaths.DSP
{
    public interface IFilter
    {
        #region Transform

        void Transform( FxVectorF inBuffer, FxVectorF outBuffer );

        void Transform( FxVectorF inBuffer, out FxVectorF outBuffer );

        void Transform( float[] inBuffer, float[] outBuffer );

        #endregion

        #region Frequency Response

        FxVectorF GetFrequencyResponse(int resolution);

        #endregion
    }
}
