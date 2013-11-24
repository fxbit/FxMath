using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxMaths.Matrix
{
    [Serializable]
    public class FxMatrixMask
    {
        public bool[] Data;
        public int Width { get; set; }
        public int Height { get; set; }



        #region Constructors

        /// <summary>
        /// Init the mask with all data to false.
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public FxMatrixMask(int Width, int Height)
        {
            // init the boolean array as a continue array
            this.Data = new bool[Width * Height];

            this.Width = Width;
            this.Height = Height;
        }

        /// <summary>
        /// Ini the mask matrix with define value.
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="value"></param>
        public FxMatrixMask(int Width, int Height, Boolean value)
        {
            // init the boolean array as a continue array
            this.Data = new bool[Width * Height];

            this.Width = Width;
            this.Height = Height;

            for (int i = 0; i < Width * Height; i++)
                this.Data[i] = value;
        }

        public FxMatrixMask Copy()
        {
            FxMatrixMask result = new FxMatrixMask(this.Width, this.Height);
            result.Or(this);
            return result;
        }

        #endregion


        #region Get/Set values

        /// <summary>
        /// Get The Value in specific position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool GetValue(int x, int y)
        {
            return this.Data[y * Width + x];
        }

        /// <summary>
        /// Set The Value in specific position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Value"></param>
        public void SetValue(int x, int y, bool Value)
        {
            this.Data[y * Width + x] = Value;
        }


        /// <summary>
        /// Set/Get internal values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool this[int x, int y]
        {
            get
            {
                return this.Data[y * Width + x];
            }

            set
            {
                this.Data[y * Width + x] = value;
            }
        }

        /// <summary>
        /// Set/Get internal values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool this[int index]
        {
            get
            {
                return this.Data[index];
            }

            set
            {
                this.Data[index] = value;
            }
        }

        /// <summary>
        /// Set each internal data based on external function.
        /// </summary>
        /// <param name="func"></param>
        public void SetValueFunc(Func<int, int, bool> func)
        {
            Parallel.For(0, Height, (y) =>
            {
                for (int x = 0; x < Width; x++)
                {
                    this[x, y] = func(x, y);
                }
            });
        }

        #endregion



        #region Bitwise calculations

        public void Or(FxMatrixMask mask)
        {
            if (this.Width == mask.Width && this.Height == mask.Height)
            {
                Parallel.For(0, Height, (y) =>
                {
                    int offsetEnd = (y + 1) * Width;
                    for (int x = y * Width; x < offsetEnd; x++)
                    {
                        Data[x] = mask.Data[x] | Data[x];
                    }
                });
            }
        }

        public void And(FxMatrixMask mask)
        {
            if (this.Width == mask.Width && this.Height == mask.Height)
            {
                Parallel.For(0, Height, (y) =>
                {
                    int offsetEnd = (y + 1) * Width;
                    for (int x = y * Width; x < offsetEnd; x++)
                    {
                        Data[x] = mask.Data[x] & Data[x];
                    }
                });
            }
        }


        public void Not()
        {
            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    Data[x] = !Data[x];
                }
            });
        }


        public void Xor(FxMatrixMask mask)
        {
            if (this.Width == mask.Width && this.Height == mask.Height)
            {
                Parallel.For(0, Height, (y) =>
                {
                    int offsetEnd = (y + 1) * Width;
                    for (int x = y * Width; x < offsetEnd; x++)
                    {
                        Data[x] = mask.Data[x] ^ Data[x];
                    }
                });
            }
        }

        public static FxMatrixMask operator |(FxMatrixMask mask1, FxMatrixMask mask2)
        {
            FxMatrixMask result = mask1.Copy();
            result.Or(mask2);
            return result;
        }

        public static FxMatrixMask operator &(FxMatrixMask mask1, FxMatrixMask mask2)
        {
            FxMatrixMask result = mask1.Copy();
            result.And(mask2);
            return result;
        }

        public static FxMatrixMask operator ^(FxMatrixMask mask1, FxMatrixMask mask2)
        {
            FxMatrixMask result = mask1.Copy();
            result.Xor(mask2);
            return result;
        }

        public static FxMatrixMask operator !(FxMatrixMask mask1)
        {
            FxMatrixMask result = mask1.Copy();
            result.Not();
            return result;
        }
        #endregion



        #region From Matrix calculations


        public static FxMatrixMask GreaderThan<T>(FxMatrixF mat1, T value) where T : struct, IComparable
        {
            FxMatrixMask result = new FxMatrixMask(mat1.Width, mat1.Height);
            int Height = mat1.Height;
            int Width = mat1.Width;
            float fValue = (dynamic)value;

            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    result.Data[x] = mat1.Data[x] > fValue;
                }
            });
            return result;
        }

        public static FxMatrixMask LessThan<T>(FxMatrixF mat1, T value) where T : struct, IComparable
        {
            FxMatrixMask result = new FxMatrixMask(mat1.Width, mat1.Height);
            int Height = mat1.Height;
            int Width = mat1.Width;
            float fValue = (dynamic)value;

            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    result.Data[x] = mat1.Data[x] < fValue;
                }
            });
            return result;
        }


        public static FxMatrixMask Equal<T>(FxMatrixF mat1, T value) where T : struct, IComparable
        {
            FxMatrixMask result = new FxMatrixMask(mat1.Width, mat1.Height);
            int Height = mat1.Height;
            int Width = mat1.Width;
            float fValue = (dynamic)value;

            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    result.Data[x] = mat1.Data[x] == fValue;
                }
            });
            return result;
        }

        public static FxMatrixMask NotEqual<T>(FxMatrixF mat1, T value) where T : struct, IComparable
        {
            FxMatrixMask result = new FxMatrixMask(mat1.Width, mat1.Height);
            int Height = mat1.Height;
            int Width = mat1.Width;
            float fValue = (dynamic)value;

            Parallel.For(0, Height, (y) =>
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    result.Data[x] = mat1.Data[x] != fValue;
                }
            });
            return result;
        }

        public static FxMatrixF operator *(FxMatrixMask mask, float f)
        {
            FxMatrixF mat = new FxMatrixF(mask.Width, mask.Height);
            Parallel.For(0, mask.Height, (y) =>
            {
                int offsetEnd = (y + 1) * mask.Width;
                for (int x = y * mask.Width; x < offsetEnd; x++)
                {
                    mat.Data[x] = mask[x] ? f : 0;
                }
            });
            return mat;
        }

        #endregion



        #region Convert to FxMatrixF

        public FxMatrixF ToFxMatrixF()
        {
            FxMatrixF newMat = new FxMatrixF(Width, Height);
            // pass all the data and add the new data
            for (int y = 0; y < Height; y++)
            {
                int offsetEnd = (y + 1) * Width;
                for (int x = y * Width; x < offsetEnd; x++)
                {
                    newMat[x] = (this.Data[x]) ? 1f : 0;
                }
            }
            return newMat;
        }

        #endregion



        #region Median Filtering
        public FxMatrixMask MedianFilt(int sx = 3, int sy = 3)
        {
            FxMatrixMask result = new FxMatrixMask(Width, Height);
            int sx2 = (int)Math.Floor(sx / 2.0);
            int sy2 = (int)Math.Floor(sy / 2.0);
            int midPoint = (int)Math.Floor(sx * sy / 2.0);

            Parallel.For(0, Height, (y) =>
            {
                int iy_start = (y > sy2) ? y - sy2 : 0;
                int iy_end = (y + sy2 + 1 < Height) ? y + sy2 + 1 : Height;
                for (int x = 0; x < Width; x++)
                {
                    int ix_start = (x > sx2) ? x - sx2 : 0;
                    int ix_end = (x + sx2 + 1 < Width) ? x + sx2 + 1 : Width;
                    int sum = 0;
                    for (int iy = iy_start; iy < iy_end; iy++)
                        for (int ix = ix_start; ix < ix_end; ix++)
                        {
                            if (this[ix, iy])
                                sum++;
                        }
                    result[x, y] = sum >= midPoint;
                }
            });

            return result;
        }
        #endregion



        #region Labeling algorithm

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

        public FxMatrixF Labeling(out int labelCount)
        {
            int maskSize = Width * Height;
            var remainMask = this.Copy();
            var labelMap = new FxMatrixF(Width, Height);
            var stack = new Stack<Tuple<int, int>>(1000);

            labelCount = 1;
            for (int i = 0; i < maskSize; i++)
            {
                /* find the next start point */
                if (remainMask[i])
                {
                    int x;
                    int y = Math.DivRem(i, Width, out x);
                    remainMask[i] = false;
                    labelMap[x, y] = labelCount;

                    /* propacate the search in sub pixels */
                    Labeling_addStack(stack, x, y);
                    while (stack.Count > 0)
                    {
                        var dxy = stack.Pop();
                        x = dxy.Item1;
                        y = dxy.Item2;

                        if (x < 0 || x >= Width || y < 0 || y >= Height)
                            continue;

                        if (remainMask[x, y])
                        {
                            labelMap[x, y] = labelCount;
                            remainMask[x, y] = false;
                            Labeling_addStack(stack, x, y);
                        }
                    }
                    labelCount++;
                }
            }
            return labelMap;
        }
        #endregion



        public Bitmap ToBitmap()
        {
            Bitmap bitmap = new Bitmap(Width, Height);

            FxMaths.Images.FxImages fi = FxMaths.Images.FxTools.FxImages_safe_constructors(bitmap);
            fi.Load(this.ToFxMatrixF().Gradient(), new Images.ColorMap(Images.ColorMapDefaults.Gray));

            return bitmap;
        }



        #region Misc Functions

        /// <summary>
        /// Count the non zero (true) elements in the mask.
        /// </summary>
        /// <returns></returns>
        public int NumNZ()
        {
            int count = 0;
            int end = Width * Height;
            for (int i = 0; i < end; i++)
            {
                if (Data[i])
                    count++;
            }

            return count;
        }


        /// <summary>
        /// Count the non zero (true) elements in the mask.
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="Size"></param>
        /// <returns></returns>
        public int NumNZ(Vector.FxVector2f start, Vector.FxVector2f size)
        {
            if (start.x > Width || start.y > Height)
                return 0;

            int count = 0;
            int startX = (int)((start.x < 0) ? 0 : start.x);
            int startY = (int)((start.y < 0) ? 0 : start.y);
            int endX = (int)((startX + size.x > Width) ? Width - startX : size.x);
            int endY = (int)((startY + size.y > Height) ? Height - startY : size.y);
            for (int y = startY; y < endY; y++)
                for (int x = startX; x < endX; x++)
                {
                    if (this[x, y])
                        count++;
                }

            return count;
        } 


        #endregion
    }
}
