using FxMaths.Complex;
using FxMaths.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxMaths.Matrix
{
    public class FxContour
    {

        public List<FxContourChain> ChainList;
        public int NumChains { get { return ChainList.Count; } }

        private void Labeling_addStack(Stack<Tuple<int, int>> stack, int x, int y)
        {
            stack.Push(Tuple.Create(x, y - 1));
            stack.Push(Tuple.Create(x, y + 1));

            stack.Push(Tuple.Create(x + 1, y - 1));
            stack.Push(Tuple.Create(x + 1, y));
            stack.Push(Tuple.Create(x + 1, y + 1));

            stack.Push(Tuple.Create(x - 1, y - 1));
            stack.Push(Tuple.Create(x - 1, y));
            stack.Push(Tuple.Create(x - 1, y + 1));
        }

        private bool check(FxMatrixMask mask, int x, int y)
        {
            return !(mask[x, y - 1] &&
                     mask[x, y + 1] &&
                     mask[x + 1, y - 1] &&
                     mask[x + 1, y] &&
                     mask[x + 1, y + 1] &&
                     mask[x - 1, y - 1] &&
                     mask[x - 1, y] &&
                     mask[x - 1, y + 1]);
        }

        public FxContour(FxMatrixMask mask)
        {
            int maskSize = mask.Width * mask.Height;
            var stack = new Stack<Tuple<int, int>>(1000);
            var remainMask = mask.Copy();
            var labelMap = new FxMatrixF(mask.Width, mask.Height);
            ChainList = new List<FxContourChain>();

            int labelCount = 1;
            var maskG = mask.ToFxMatrixF().Gradient(FxMatrixF.GradientMethod.Roberts) > 0;
            for (int i = 1; i < maskSize - 1; i++)
            {
                /* check if we have edge */
                if (maskG[i])
                {
                    int x;
                    int y = Math.DivRem(i, mask.Width, out x);
                    labelMap[x, y] = labelCount;
                    maskG[x, y] = false;

                    // create a new chain 
                    FxContourChain newChain = new FxContourChain(x, y);

                    /* propacate the search in sub pixels */
                    Labeling_addStack(stack, x, y);
                    while (stack.Count > 0)
                    {
                        var dxy = stack.Pop();
                        x = dxy.Item1;
                        y = dxy.Item2;

                        if (x < 1 || (x >= mask.Width - 1) || y < 1 || (y >= mask.Height - 1))
                            continue;

                        if (maskG[x, y] && check(maskG, x, y))
                        {
                            // add the labeling
                            labelMap[x, y] = labelCount;
                            maskG[x, y] = false;

                            // add the new point to the chain
                            newChain.PushPoint(x, y);

                            Labeling_addStack(stack, x, y);
                        }
                    }
                    labelCount++;

                    // calc boundary rect

                    // add the new chain to the list 
                    ChainList.Add(newChain);
                }
            }
        }

        public FxMatrixF ToFxMatrixF(int Width, int Height)
        {
            FxMatrixF result = new FxMatrixF(Width, Height);
            int i = 0;
            foreach(FxContourChain chain in ChainList)
            {
                //chain.Equalization(20);
                i++;
                FxVector2f prevPoint = chain.StartPoint;
                foreach (FxComplexF c in chain.ListComplex)
                {
                    if (prevPoint.x >= Width)
                        prevPoint.x = Width-1;
                    if (prevPoint.y >= Height)
                        prevPoint.y = Height-1;
                    result[(int)prevPoint.x, (int)prevPoint.y] = i;

                    // calc the new prev point
                    prevPoint.x += c.r;
                    prevPoint.y += c.i;
                }

                // Draw boundrary rect
                result.DrawRect(chain.RectStart, chain.RectSize, i);
            }

            result.Divide(result.Max());

            return result;
        }
    }

    /// <summary>
    /// The chain with the points of the contour.
    /// </summary>
    public class FxContourChain
    {
        /// <summary>
        /// List of the points
        /// </summary>
        public List<FxComplexF> ListComplex;

        /// <summary>
        /// The start point of the chaib
        /// </summary>
        public FxVector2f StartPoint;


        /// <summary>
        /// The start of the rect boundary.
        /// </summary>
        public FxVector2f RectStart;

        /// <summary>
        /// The size of the rect.
        /// </summary>
        public FxVector2f RectSize;

        /// <summary>
        /// The previous added point
        /// </summary>
        private FxVector2f prevPoint;

        public int Count { get { return ListComplex.Count; } }

        public FxComplexF this[int i]
        {
            get { return ListComplex[i]; }
            set { ListComplex[i] = value; }
        }

        public FxContourChain(float x, float y)
        {
            // set the start point 
            StartPoint = new FxVector2f(x, y);
            prevPoint = new FxVector2f(x, y);

            // create the complex list 
            ListComplex = new List<FxComplexF>();

            RectStart = new FxVector2f(x, y);
            RectSize = new FxVector2f(0, 0);
        }

        public void PushPoint(float x, float y)
        {
            lock (ListComplex)
            {
                ListComplex.Add(new FxComplexF(x - prevPoint.x, y - prevPoint.y));
            }
            prevPoint.x = x;
            prevPoint.y = y;


            // Renew rect boundary
            if (RectStart.x > x)
            {
                RectSize.x += RectStart.x - x;
                RectStart.x = x;
            }
            else if (RectStart.x + RectSize.x < x)
            {
                RectSize.x = x - RectStart.x;
            }

            if (RectStart.y > y)
            {
                RectSize.y += RectStart.y - y;
                RectStart.y = y;
            }
            else if (RectStart.y + RectSize.y < y)
            {
                RectSize.y = y - RectStart.y;
            }
        }

        public FxVector2f PopPoint()
        {
            FxComplexF diff;
            FxVector2f result = prevPoint;

            // get and remove the last point
            lock (ListComplex)
            {
                diff = ListComplex[ListComplex.Count - 1];
                ListComplex.RemoveAt(ListComplex.Count);
            }

            // calc the new prev point
            prevPoint.x += diff.r;
            prevPoint.y += diff.i;

            // return the old prev point 
            return result;
        }

        public void Equalization(int newCount)
        {
            if(ListComplex.Count<newCount)
            {
                EqualizationUp(newCount);
            }
            else
            {
                EqualizationDown(newCount);
            }
        }

        private void EqualizationUp(int newCount)
        {
            List<FxComplexF> newList = new List<FxComplexF>();

            for (int i = 0; i < newCount; i++)
            {
                float index = 1f * i * Count / newCount;
                int j = (int)index;
                float k = index - j;
                if (j == Count - 1)
                    newList.Add(this[j]);
                else
                    newList.Add(this[j] * (1 - k) + this[j + 1] * k);
            }

            ListComplex.Clear();
            ListComplex = newList;
        }


        private void EqualizationDown(int newCount)
        {
            FxComplexF[] newPoint = new FxComplexF[newCount];
            
            for (int i = 0; i < newCount; i++)
            {
                newPoint[newCount] = new FxComplexF(0,0);
            }

            for (int i = 0; i < Count; i++)
            {
                newPoint[i * newCount / Count] += this[i];
            }

            ListComplex.Clear();
            ListComplex.AddRange(newPoint);
        }

#if false
        public float GetArea()
        {
            float area = 0;

            // using image moments
            for (int i = 0; i < Count; i++)
            {
                area += (ListComplex[i].r - ListComplex[i].i);
            }

            return area / 2.0f;
        }
#endif

        public FxVector2f GetCentroid()
        {
            float m00 = 0;
            float m10 = 0;
            float m01 = 0;
            FxVector2f tmpPoint = StartPoint;

            // using image moments
            for (int i = 0; i < Count; i++)
            {
                m10 += tmpPoint.x;
                m01 += tmpPoint.y;
                tmpPoint.x += ListComplex[i].r;
                tmpPoint.y += ListComplex[i].i;
            }
            m00 = Count;
            return new FxVector2f(m10/m00, m01/m00);
        }



        /// <summary>
        /// Get the maximum distance from specific point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetMaxDist(FxVector2f point)
        {
            float maxDist = 0;
            FxVector2f tmpPoint = StartPoint;

            for (int i = 0; i < Count; i++)
            {
                var dist = tmpPoint.Distance(point);
                if (dist > maxDist)
                    maxDist = dist;
                tmpPoint.x += ListComplex[i].r;
                tmpPoint.y += ListComplex[i].i;
            }

            return maxDist;
        }



        /// <summary>
        /// Get the minimum distance from specific point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetMinDist(FxVector2f point)
        {
            float minDist = float.MaxValue;
            FxVector2f tmpPoint = StartPoint;

            for (int i = 0; i < Count; i++)
            {
                var dist = tmpPoint.Distance(point);
                if (dist < minDist)
                    minDist = dist;
                tmpPoint.x += ListComplex[i].r;
                tmpPoint.y += ListComplex[i].i;
            }

            return minDist;
        }
    }
}
