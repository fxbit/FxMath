using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace FxMaths.Images
{
    public enum RGB { B = 0, G, R, A }

    public abstract class FxImages
    {

        public RGB[] FXPixelFormat;

        /// <summary>
        /// The internal bitmap
        /// </summary>
        public abstract  Bitmap Image 
        {
            get;            
        }

        public abstract BitmapData get_ImageData();

        public abstract void LockImage();
        public abstract void LockImage(PixelFormat customFormat);
        public abstract void UnLockImage();

        /// <summary>
        /// Get the color of the specific pixel
        /// slower than: this[x,y,RGB]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public abstract Color GetPixel(int x, int y);
        public abstract int GetPixel_int(int x, int y);
        public abstract void Copy_to_Array(ref int[] dest);

        /// <summary>
        /// Set the color of the specific pixel
        /// slower than: this[x,y,RGB]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public abstract void SetPixel(int x, int y, Color color);

        /// <summary>
        /// get the color for the specific position
        /// slower than: this[x,y,RGB]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public abstract Color this[int x, int y]
        {
            get;
            set;
        }


        /// <summary>
        /// get specific pixel and specific color
        /// </summary>
        /// <param name="x">x position of pixel</param>
        /// <param name="y">y position of pixel</param>
        /// <param name="index">index of the color blue:0 green:1 red:2 !!!</param>
        /// <returns>byte for the color of pixel</returns>
        public abstract byte this[int x, int y, RGB index]
        {
            get;
            set;

        }

        public abstract Bitmap getimage();



    }
}
