using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.Matrix
{
    public enum NormMatrixType
    {
        /// <summary>
        /// L-1
        /// </summary>
        MaximumAbsoluteColSum,

        /// <summary>
        /// A natural extension of a vector norm to matrices.
        /// </summary>
        Frobenius,

        /// <summary>
        /// L-inf
        /// </summary>
        MaximumAbsoluteRowSum
    }

    public enum ColorSpace
    {
        /// <summary>
        /// Grayscale space.
        /// </summary>
        Grayscale,

        /// <summary>
        /// Color scpace base on RGB
        /// </summary>
        RGB,

        /// <summary>
        /// HSV color space
        /// </summary>
        HSV

    }
}
