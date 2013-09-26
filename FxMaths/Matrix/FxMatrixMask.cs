using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxMaths.Matrix
{
    public class FxMatrixMask
    {
        public bool []Data;
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

            for(int i=0; i < Width * Height; i++)
                this.Data[i] = value;
        }

        public FxMatrixMask Copy()
        {
            FxMatrixMask result = new FxMatrixMask(this.Width, this.Height);
            result.Or(this);
            return result;
        }

        #endregion



        #region Bitwise calculations

        public void Or(FxMatrixMask mask)
        {
            if(this.Width == mask.Width && this.Height == mask.Height) {
                Parallel.For(0, Height, (y) => {
                    int offsetEnd = (y + 1) * Width;
                    for(int x= y * Width; x < offsetEnd; x++) {
                        Data[x] = mask.Data[x] | Data[x];
                    }
                });
            }
        }

        public void And(FxMatrixMask mask)
        {
            if(this.Width == mask.Width && this.Height == mask.Height) {
                Parallel.For(0, Height, (y) => {
                    int offsetEnd = (y + 1) * Width;
                    for(int x= y * Width; x < offsetEnd; x++) {
                        Data[x] = mask.Data[x] & Data[x];
                    }
                });
            }
        }


        public void Not()
        {
            Parallel.For(0, Height, (y) => {
                int offsetEnd = (y + 1) * Width;
                for(int x= y * Width; x < offsetEnd; x++) {
                    Data[x] = !Data[x];
                }
            });
        }


        public void Xor(FxMatrixMask mask)
        {
            if(this.Width == mask.Width && this.Height == mask.Height) {
                Parallel.For(0, Height, (y) => {
                    int offsetEnd = (y + 1) * Width;
                    for(int x= y * Width; x < offsetEnd; x++) {
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

            Parallel.For(0, Height, (y) => {
                int offsetEnd = (y + 1) * Width;
                for(int x= y * Width; x < offsetEnd; x++) {
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

            Parallel.For(0, Height, (y) => {
                int offsetEnd = (y + 1) * Width;
                for(int x= y * Width; x < offsetEnd; x++) {
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

            Parallel.For(0, Height, (y) => {
                int offsetEnd = (y + 1) * Width;
                for(int x= y * Width; x < offsetEnd; x++) {
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

            Parallel.For(0, Height, (y) => {
                int offsetEnd = (y + 1) * Width;
                for(int x= y * Width; x < offsetEnd; x++) {
                    result.Data[x] = mat1.Data[x] != fValue;
                }
            });
            return result;
        }


        #endregion


    }
}
