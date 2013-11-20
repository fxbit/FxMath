using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxMaths.Matrix
{
    /// <summary>
    /// Track a blob in specific sequence of FxMatrixF.
    /// </summary>
    public class FxBlobTracker
    {
        // Reference:
        // http://en.wikipedia.org/wiki/Standard_deviation#Rapid_calculation_methods

        #region Properties of the tracker.

        /// <summary>
        /// A FG mask of the blobs.
        /// </summary>
        public FxMatrixMask FGMask { get { return G; } }

        public float a { get; set; }

        #endregion

        #region Private variables.

        /// <summary>
        /// The average of the matrix sequences.
        /// </summary>
        public FxMatrixF m;

        /// <summary>
        /// The std of the matrix sequences
        /// </summary>
        public FxMatrixF s;

        /// <summary>
        /// A mask matrix.
        /// </summary>
        private FxMatrixMask G;


        private int step_w;
        private int step_h;
        private int cG_thd;
        private const int G_small_width = 64;
        private const int G_small_height = 48;
        public FxMatrixMask G_small;
        #endregion

        public FxBlobTracker(FxMatrixF firstFrame)
        {
            m = firstFrame.Copy();
            s = new FxMatrixF(firstFrame.Width, firstFrame.Height,0.05f);
            G = m != -1; /* force a mask with all 1 */
            a = 0.005f;

            G_small = new FxMatrixMask(G_small_width, G_small_height);
            step_w = (int)Math.Ceiling(G.Width / (float)G_small_width);
            step_h = (int)Math.Ceiling(G.Height / (float)G_small_height);
            cG_thd = step_w * step_h / 2;
        }


        public void Process(FxMatrixF NextFrame)
        {
#if false
            // create the boolean image 
            m = (1 - a) * m + a * NextFrame;
            var diff = NextFrame - m;
            s = (1 - a) * s + a * diff * diff;
            G = diff*diff > s;
#else
            int Width = NextFrame.Width;
            int Height = NextFrame.Height;
            Parallel.For(0, Height, (y) =>
            {
                var diff2 = 0f;
                for (int x = 0; x < Width; x++)
                {
                    m[x, y] = (1 - a) * m[x, y] + a * NextFrame[x, y];
                    diff2 = NextFrame[x, y] - m[x, y];
                    diff2 *= diff2;
                    s[x, y] = (1 - a) * s[x, y] + a * diff2;
                    G[x, y] = diff2 > s[x, y];
                }
            });
#endif

            // create a resize value
            G_small.SetValueFunc((x, y) =>
            {
                int sum = 0;
                for (int ix = x * step_w; ix < x * step_w + step_w; ix++)
                {
                    for (int iy = y * step_h; iy < y * step_h + step_h; iy++)
                    {
                        sum += G[ix, iy] ? 1 : 0;
                        if (sum >= cG_thd)
                            return true;
                    }
                }
                return false;
            });

            // filter out any small points 
            G_small = G_small.MedianFilt();
        }



    }

    public class FxBlob
    {

    }
}
