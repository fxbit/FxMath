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
            for (int i = 1; i < maskSize -1; i++)
            {
                /* check if we have edge */
                if((mask[i] ^ mask[i+1]) && (labelMap[i] + labelMap[i+1] == 0))
                {
                    int x;
                    int y = Math.DivRem(i, mask.Width, out x);
                    labelMap[x, y] = labelCount;
                    remainMask[x, y] = false;

                    // create a new chain 
                    FxContourChain newChain = new FxContourChain(x, y);

#if true
                    /* propacate the search in sub pixels */
                    Labeling_addStack(stack, x, y);
                    while (stack.Count > 0)
                    {
                        var dxy = stack.Pop();
                        x = dxy.Item1;
                        y = dxy.Item2;

                        if (x < 1 || (x >= mask.Width - 1) || y < 1 || (y >= mask.Height - 1))
                            continue;

                       // if (newChain.StartPoint.x == x && newChain.StartPoint.y == y)
                        //    break;

                        if (remainMask[x, y] && check(mask, x, y))
                        {
                            // add the labeling
                            labelMap[x, y] = labelCount;
                            remainMask[x, y] = false;
                            
                            // add the new point to the chain
                            newChain.PushPoint(x, y);

                            Labeling_addStack(stack, x, y);
                        }
                    }
#else
                    // the first point that we are finding is having only from x + 1, y and clockwise 

                    int oldX = x, oldY = y;
                    int oldPath = 0; // we have 8 paths
                    bool cw = false;

                    if (mask[x + 1, y] == true) { x++; oldPath = 8; }
                    else if (mask[x + 1, y + 1] == true) { x++; y++; oldPath = 1; }
                    else if (mask[x, y + 1] == true) { y++; oldPath = 2; }
                    else if (mask[x - 1, y + 1] == true) { x--; y++; oldPath = 3; }
                    else throw new Exception();
                    
                    while (true)
                    {
                        if (x < 1 || (x >= mask.Width - 1) || y < 1 || (y >= mask.Height - 1))
                            continue;

                        if (newChain.StartPoint.x == x && newChain.StartPoint.y == y)
                            break;

                        // add the labeling
                        labelMap[x, y] = labelCount;
                        remainMask[x, y] = false;

                        // add the new point to the chain
                        newChain.PushPoint(x, y);

                        // calc the new point

                        // 8 cases
                        switch(oldPath)
                        {
                            case 1:
                                // select the if we move clockwise
                                if (mask[oldX + 1, oldY])
                                    cw = true;
                                else if (mask[oldX, oldY + 1])
                                    cw = false;
                                else
                                    throw new Exception();

                                if(cw)
                                {
                                    if (mask[x - 1, y - 1] == true) { x++; oldPath = 8; }
                                    else if (mask[x + 1, y + 1] == true) { x++; y++; oldPath = 1; }
                                    else if (mask[x, y + 1] == true) { y++; oldPath = 2; }
                                    else if (mask[x - 1, y + 1] == true) { x--; y++; oldPath = 3; }
                                    else throw new Exception();
                                }
                                else
                                {

                                }



                                break;
                            case 2:

                                break;
                            case 3:

                                break;
                            case 4:

                                break;
                            case 5:

                                break;
                            case 6:

                                break;
                            case 7:

                                break;
                            case 8:

                                break;
                        }


                        // save old points
                        oldX = x; oldY = y;
                    }
#endif
                    labelCount++;

                    // add the new chain to the list 
                    ChainList.Add(newChain);
                }
            }

        }

        public FxMatrixF ToFxMatrixF(int Width, int Height)
        {
            FxMatrixF result = new FxMatrixF(Width, Height);

            for (int i = 0; i < NumChains; i++)
            {
                FxVector2f prevPoint = ChainList[i].StartPoint;
                foreach (FxComplexF c in ChainList[i].ListComplex)
                {
                    result[(int)prevPoint.x, (int)prevPoint.y] = i;

                    // calc the new prev point
                    prevPoint.x += c.r;
                    prevPoint.y += c.i;
                }
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
        /// The previous added point
        /// </summary>
        private FxVector2f prevPoint;

        public FxContourChain(float x, float y)
        {
            // set the start point 
            StartPoint = new FxVector2f(x, y);
            prevPoint = new FxVector2f(x, y);

            // create the complex list 
            ListComplex = new List<FxComplexF>();
        }

        public void PushPoint(float x, float y)
        {
            lock (ListComplex)
            {
                ListComplex.Add(new FxComplexF(x - prevPoint.x, y - prevPoint.y));
            }
            prevPoint.x = x;
            prevPoint.y = y;
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
    }
}
