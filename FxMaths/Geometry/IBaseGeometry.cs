using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct2D1;

namespace FxMaths.Geometry
{
    public interface IBaseGeometry
    {
         
        #region Draw function

        void Render2D(RenderTarget renderTarget, Brush brush);

        #endregion

        #region Dispose Function

        /// <summary>
        /// Clean the memory
        /// </summary>
        void Dispose();

        #endregion
    }
}
