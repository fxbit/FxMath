using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FxMaths.Vector;

namespace FxMaths.Complex
{
    public class ComplexTools
    {
        /// <summary>
        /// Complex Division. 
        /// Result=A/B=(realA+i*imagA)/(realB+i*imagB)
        /// </summary>
        /// <param name="realA"></param>
        /// <param name="imagA"></param>
        /// <param name="realB"></param>
        /// <param name="imagB"></param>
        /// <param name="resultReal"></param>
        /// <param name="resultImag"></param>
        public static void Division(FxVectorF realA, FxVectorF imagA, FxVectorF realB, FxVectorF imagB, out  FxVectorF resultReal, out FxVectorF resultImag)
        {
            int size = realA.Size;

            // allocate return result
            resultImag = new FxVectorF(size);
            resultReal = new FxVectorF(size);

            double bb;
            for (int i = 0; i < size; i++)
            {
                // calc the H=(realB + i * imagB) / (realA + i * imagA)
                // http://mathworld.wolfram.com/ComplexDivision.html
                bb = realB[i] * realB[i] + imagB[i] * imagB[i];
                resultReal[i] = (float)((realA[i] * realB[i] + imagA[i] * imagB[i]) / bb);
                resultImag[i] = (float)((imagA[i] * realB[i] - realA[i] * imagB[i]) / bb);

            }
        }
    }
}
