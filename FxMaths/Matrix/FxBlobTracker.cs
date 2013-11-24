using FxMaths.Vector;
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


        /// <summary>
        /// List with tracking blobs.
        /// </summary>
        public List<FxBlob> ListBlobs;

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


        public int step_w;
        public int step_h;
        private int cG_thd;
        private int numProcessingFrames;
        private const int G_small_width = 64;
        private const int G_small_height = 48;
        
        private const int smallerRadius = 8;

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

            ListBlobs = new List<FxBlob>();

            numProcessingFrames = 0;
        }


        public void Process(FxMatrixF NextFrame)
        {
            // evrey 100 frames reset the s;
            if (numProcessingFrames % 100 ==0)
            {
                s = new FxMatrixF(NextFrame.Width, NextFrame.Height, 0.05f);
            }


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

            // create the contours of the current frame
            var contours = new FxContour(G_small);
            var listContour = contours.ChainList;


            // update the contours
            foreach (FxBlob b in ListBlobs)
            {
                Stack<FxContourChain> chainStack = new Stack<FxContourChain>();
                FxVector2f newCenter = b.Center;
                float newRadius = 0;
                FxContourChain selected = null;

                // find all the chains that are common with the old blob
                foreach (FxContourChain c in listContour)
                {
                    var c_center = c.GetCentroid();
                    var c_radius = c.GetMaxDist(c_center);

                    if (c_center.Distance(b.Center) < c_radius + b.Radius / 2)
                    {
                        chainStack.Push(c);
                        newCenter.x = (newCenter.x + c_center.x) / 2.0f;
                        newCenter.y = (newCenter.y + c_center.y) / 2.0f;
                    }
                }

                // find the biger radius
                foreach (FxContourChain c in chainStack)
                {
                    var r = c.GetMaxDist(newCenter);
                    if (newRadius < r)
                    {
                        selected = c;
                        newRadius = r;
                    }
                }

                // remove the chains that we have find that can be used
                listContour.RemoveAll(c => chainStack.Contains(c));

                if (newRadius > 2*smallerRadius)
                    continue;

                // update the blob
                if (selected != null)
                {
                    b.LastContour = selected;
                    b.Center = newCenter;
                    if (b.Radius < newRadius)
                        b.Radius = (newRadius > smallerRadius) ? smallerRadius : newRadius;
                    
                    b.NumOfElements = G_small.NumNZ(selected.RectStart, selected.RectSize);
                    b.UnSeenCount = 0;
                }

                chainStack.Clear();
            }


            // remove old blobs
            Stack<FxBlob> oldBlob = new Stack<FxBlob>();
            foreach (FxBlob b in ListBlobs)
            {
                if (G_small.NumNZ(b.Center, b.Radius) == 0)
                    b.UnSeenCount++;
                else
                    b.UnSeenCount = 0;

                if (b.UnSeenCount > 5)
                    oldBlob.Push(b);
            }
            ListBlobs.RemoveAll(c => oldBlob.Contains(c));



            // add a new blobs
            ListBlobs.AddRange(
                listContour.Where(x =>
                            {
                                var size = x.RectSize.x * x.RectSize.y;
                                return (size > 40 && size < 200);
                            })
                           .Select(x =>
                           {
                               var b = new FxBlob();
                               b.LastContour = x;
                               b.Center = x.GetCentroid();
                               b.Radius = x.GetMaxDist(b.Center);
                               if (b.Radius > smallerRadius)
                                   b.Radius = smallerRadius;
                               b.NumOfElements = G_small.NumNZ(x.RectStart, x.RectSize);
                               b.UnSeenCount = 0;
                               return b;
                           })
                           );


            // increase the counter
            numProcessingFrames++;
        }



    }

    public class FxBlob
    {
        /// <summary>
        /// The last see contour.
        /// </summary>
        public FxContourChain LastContour;

        /// <summary>
        /// The number of the last see elements in the region that include this blob.
        /// </summary>
        public int NumOfElements;


        /// <summary>
        /// The center of the blob.
        /// </summary>
        public FxVector2f Center;


        /// <summary>
        /// The radius of the blob.
        /// </summary>
        public float Radius;

        /// <summary>
        /// The number of frames that we havent see any movement in the region of blob.
        /// </summary>
        public int UnSeenCount;
    }
}
