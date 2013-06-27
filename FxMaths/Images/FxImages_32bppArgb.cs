using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace FxMaths.Images
{
    unsafe public class FxImages_32bppArgb : FxImages
    {

        public Bitmap localImage;
        public BitmapData ImageData;
        public int Stride;
        byte* Scan0;


        public FxImages_32bppArgb(Bitmap Image)
        {
            if (Image.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("The input image must 32bppArgb");
            }

            // set the pilex format
            FXPixelFormat = new RGB[4];
            FXPixelFormat[0] = RGB.B;
            FXPixelFormat[1] = RGB.G;
            FXPixelFormat[2] = RGB.R;
            FXPixelFormat[3] = RGB.A;

            // set the internal image
            localImage = Image;
        }

        public override void LockImage()
        {
            unsafe
            {
                ImageData = localImage.LockBits(new Rectangle(0, 0, localImage.Width, localImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
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
            byte* ptr = Scan0 + (y * Stride) + (x * 4);
            return Color.FromArgb(*(ptr + 3), *(ptr + 2), *(ptr + 1), *(ptr));
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
            int ret = 0;
            byte* ptr_ret = (byte*)&ret;
            byte* ptr = Scan0 + (y * Stride) + (x * 4);
            //copy..
            *(ptr_ret + 3) = *(ptr + 3);
            * (ptr_ret + 2) = *(ptr + 2);
            * (ptr_ret + 1) = *(ptr + 1);
            * (ptr_ret) = *(ptr);
            return ret;
        }



        public override void Copy_to_Array(ref byte[] dest)
        {
            byte* ptr = Scan0;
            for (int n = 0; n < dest.Length; n++)
                dest[n] = *ptr++;
        }

        public override void Copy_to_Array(ref int[] dest)
        {
            byte* ptr = Scan0;
            int* dest_ptr = (int*)ptr;
            for (int n = 0; n < dest.Length; n++)
                dest[n] = *dest_ptr++;
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
            byte* ptr = Scan0 + (y * Stride) + (x * 4);
            *(ptr + 3) = (byte)color.A;
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
                    byte* ptr = Scan0 + (y * Stride) + (x * 4);
                    return Color.FromArgb(*(ptr + 3), *(ptr + 2), *(ptr + 1), *(ptr));
                }
            }
            set
            {
                unsafe
                {
                    byte* ptr = Scan0 + (y * Stride) + (x * 4);
                    *(ptr + 3) = (byte)value.A;
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
                unsafe
                {
                    return *(Scan0 + (y * Stride) + (x * 4) + (int)index);
                }
            }
            set
            {
                unsafe
                {
                    *(Scan0 + (y * Stride) + (x * 4) + (int)index) = (byte)value;
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
}
}
