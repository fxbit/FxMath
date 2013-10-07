using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FxMaths.Images
{
    unsafe class FxImages_24bppRgb : FxImages
    {

        public Bitmap localImage;
        public BitmapData ImageData;
        public int Stride;
        byte* Scan0;

        public FxImages_24bppRgb(Bitmap Image)
        {
            if (Image.PixelFormat != PixelFormat.Format24bppRgb
                &&
                Image.PixelFormat != PixelFormat.Format32bppRgb
                )
            {
                throw new ArgumentException("The input image must be 24bppRgb");
            }

            // set the pilex format
            FXPixelFormat = new RGB[3];
            FXPixelFormat[0] = RGB.B;
            FXPixelFormat[1] = RGB.G;
            FXPixelFormat[2] = RGB.R;

            // set the internal image
            localImage = Image;
        }

        public override void LockImage()
        {
            unsafe
            {
                ImageData = localImage.LockBits(new Rectangle(0, 0, localImage.Width, localImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                Stride = ImageData.Stride;
                Scan0 = (byte*)(ImageData.Scan0);
            }
        }

        public override void LockImage(PixelFormat customFormat)
        {
            unsafe
            {
                ImageData = localImage.LockBits(new Rectangle(0, 0, localImage.Width, localImage.Height), ImageLockMode.ReadWrite, customFormat);
                Stride = ImageData.Stride;
                Scan0 = (byte*)(ImageData.Scan0);
            }
        }

        public override void UnLockImage()
        {
            unsafe
            {
                localImage.UnlockBits(ImageData);
            }
        }

        /// <summary>
        /// Get the color of the specific pixel
        /// slower than: this[x,y,RGB]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override Color GetPixel(int x, int y)
        {
            byte* ptr = Scan0 + (y * Stride) + (x * 3);
            return Color.FromArgb(*(ptr + 2), *(ptr + 1), *(ptr));
        }


        /// <summary>
        /// Get the color of the specific pixel
        /// slower than: this[x,y,RGB]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override int GetPixel_int(int x, int y)
        {
            throw new NotImplementedException();
        }

        public override void Copy_to_Array(ref byte[] dest, ColorChannels numChannels)
        {
            int size = localImage.Height * localImage.Width;
            byte* ptr = Scan0;
            unsafe {
                switch(numChannels) {
                    case ColorChannels.Gray:
                        fixed(byte *d = dest) {
                            byte *pDest = d;
                            byte *pDestEnd = d + dest.Length;
                            for(; pDest < pDestEnd; pDest++) {
                                *pDest = (byte)((*ptr++ * 0.3) + (*ptr++ * 0.59) + (*ptr++ * 0.11));
                            }
                        }
                        break;
                    case ColorChannels.RGB:
                        Marshal.Copy(ImageData.Scan0, dest, 0, dest.Length);
                        break;
                    case ColorChannels.RGBA:
                        fixed(byte *d = dest) {
                            byte *pDest = d;
                            for(int i=0; i < size; i++) {
                                *(pDest++) = *ptr++;// B
                                *(pDest++) = *ptr++;// G
                                *(pDest++) = *ptr++;// R
                                *(pDest++) = 1;// A
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public override void Copy_to_Array(ref int[] dest, ColorChannels numChannels)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set the color of the specific pixel
        /// slower than: this[x,y,RGB]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public override void SetPixel(int x, int y, Color color)
        {
            byte* ptr = Scan0 + (y * Stride) + (x * 3);
            *(ptr + 2) = (byte)color.R;
            *(ptr + 1) = (byte)color.G;
            *(ptr) = (byte)color.B;
        }

        /// <summary>
        /// get the color for the specific position
        /// slower than: this[x,y,RGB]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override Color this[int x, int y]
        {
            get
            {
                unsafe
                {
                    byte* ptr = Scan0 + (y * Stride) + (x * 3);
                    return Color.FromArgb(*(ptr + 2), *(ptr + 1), *(ptr));
                }
            }
            set
            {
                unsafe
                {
                    byte* ptr = Scan0 + (y * Stride) + (x * 3);
                    *(ptr + 2) = (byte)value.R;
                    *(ptr + 1) = (byte)value.G;
                    *(ptr) = (byte)value.B;
                }
            }
        }


        /// <summary>
        /// get specific pixel and specific color
        /// </summary>
        /// <param name="x">x position of pixel</param>
        /// <param name="y">y position of pixel</param>
        /// <param name="index">index of the color blue:0 green:1 red:2 !!!</param>
        /// <returns>byte for the color of pixel</returns>
        public override byte this[int x, int y, RGB index]
        {
            get {
                lock (this)
                {
                    unsafe
                    {
                        return *(Scan0 + (y * Stride) + (x * 3) + (int)index);
                    }
                }
            }
            set
            {
                lock (this)
                {
                    unsafe
                    {
                        *(Scan0 + (y * Stride) + (x * 3) + (int)index) = (byte)value;
                    }
                }
            }
       
        }

        public override Bitmap getimage()
        {
            return localImage;
        }


        public override BitmapData get_ImageData()
        {
            return ImageData;
        }


        /// <summary>
        /// The internal bitmap
        /// </summary>
        public override Bitmap Image { get { return localImage; } }

        public override void Load(Matrix.FxMatrixF mat, ColorMap map)
        {
            if (mat.Height != Image.Height || mat.Width != Image.Width)
                return;

            this.LockImage();

            byte* pDst = Scan0;
            fixed(float* src = mat.Data) {
                float* pSrc = src;
                float* pSrcEnd = pSrc + mat.Size;
                for(; pSrc < pSrcEnd; pSrc++) {
                    byte id = (byte)(*(pSrc) * 255);
                    *(pDst++) = map[id, 2];
                    *(pDst++) = map[id, 1];
                    *(pDst++) = map[id, 0];
                }
            }

            this.UnLockImage();
        }
    }
}
