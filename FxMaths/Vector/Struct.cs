using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.Vector
{
    public enum NormVectorType
    {
        /// <summary>
        /// L-1
        /// </summary>
        Manhattan,

        /// <summary>
        /// L-2
        /// </summary>
        Euclidean,

        /// <summary>
        /// L-inf
        /// </summary>
        Maximum,

        /// <summary>
        /// L-inf
        /// </summary>
        AbsMaximum
    }
}
