using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace FxMaths.Images
{
    public enum ColorSpace { RGB, HSV, HSL, HSI };

    public class FxTools
    {
        /// <summary>
        /// function for choice the correct FxImage base on PixelFormat
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static FxImages FxImages_safe_constructors(Bitmap image)
        {
            if (image.PixelFormat == PixelFormat.Format32bppArgb)
            {
                return new FxImages_32bppArgb(image);
            }
            else if (image.PixelFormat == PixelFormat.Format24bppRgb)
            {
                return new FxImages_24bppRgb(image);
            }
            else if (image.PixelFormat == PixelFormat.Format32bppRgb)
            {
                return new FxImages_24bppRgb(image);
            }
            else if (image.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                return new FxImages_8bpp(image);
            }
            else
            {
                throw new ArgumentException("The format of input image is not supported!!");
            }
        }

        #region Bininear interpolation
        /// <summary>
        /// interpolate the input image in the specific position 
        /// and specific color, return byte
        /// </summary>
        /// <param name="image"></param>
        /// <param name="xf"></param>
        /// <param name="yf"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static byte Bilinear(FxImages image, float xf, float yf, RGB color)
        {
            if (yf + 1>= image.Image.Height)
                yf = image.Image.Height - 2;

            if (xf + 1 >= image.Image.Width)
                xf = image.Image.Width - 2;

            // get the integer part of the position
            int x = (int)Math.Floor(xf);
            int y = (int)Math.Floor(yf);

            // get the ratio part
            double x_ratio = xf - x;
            double y_ratio = yf - y;

            // the inverse ratio
            double x_opposite = 1 - x_ratio;
            double y_opposite = 1 - y_ratio;

            byte result = 0;

            // interpolate on x
            if (y_opposite > 0)
            {
                if (x_opposite > 0)
                    result += (byte)(image[x, y, color] * x_opposite * y_opposite);

                if (x_ratio > 0)
                    result += (byte)(image[x + 1, y, color] * x_ratio * y_opposite);
            }

            // interpolate on y
            if (y_ratio > 0)
            {
                if (x_opposite > 0)
                    result += (byte)(image[x, y + 1, color] * x_opposite * y_ratio);

                if (x_ratio > 0)
                    result += (byte)(image[x + 1, y + 1, color] * x_ratio * y_ratio);
            }
            return result;
        }

        /// <summary>
        /// Supplicant the input image in the specific position 
        /// and for the colors and store the results in the outImage
        /// </summary>
        /// <param name="image"></param>
        /// <param name="xf"></param>
        /// <param name="yf"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static void Bilinear(FxImages InImage, float x_In, float y_In, FxImages OutImage, int x_Out, int y_Out)
        {
            // get the integer part of the position
            int x = (int)Math.Floor(x_In);
            int y = (int)Math.Floor(y_In);

            // get the ratio part
            double x_ratio = x_In - x;
            double y_ratio = y_In - y;

            // the inverse ratio
            double x_opposite = 1 - x_ratio;
            double y_opposite = 1 - y_ratio;

            double result = 0;

            foreach (RGB i in InImage.FXPixelFormat)
            {
                result = 0;

                // interpolate on x
                if (y_opposite > 0)
                {
                    if (x_opposite > 0)
                        result += InImage[x, y, i] * x_opposite * y_opposite;

                    if (x_ratio > 0)
                        result += InImage[x + 1, y, i] * x_ratio * y_opposite;
                }

                // interpolate on y
                if (y_ratio > 0)
                {
                    if (x_opposite > 0)
                        result += InImage[x, y + 1, i] * x_opposite * y_ratio;

                    if (x_ratio > 0)
                        result += InImage[x + 1, y + 1, i] * x_ratio * y_ratio;
                }

                // store the result
                OutImage[x_Out, y_Out, i] = (byte)result;
            }
        }

        #endregion

        #region Color Space Convertions
        /// <summary>
        /// Convert input Image to other ColorSpace
        /// </summary>
        /// <param name="image"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void ColorSpaceConverter(FxImages image, ColorSpace from, ColorSpace to)
        {
            switch (from)
            {
                case ColorSpace.RGB:
                    switch (to)
                    {
                        case ColorSpace.HSV:
                            ColorSpaceConverter_FromRGB_ToHSV(image);
                            break;
                    }

                    break;
                case ColorSpace.HSV:
                    switch (to)
                    {
                        case ColorSpace.RGB:
                            ColorSpaceConverter_FromHSV_ToRGB(image);
                            break;
                    }
                    break;
            }


        }

        private static void ColorSpaceConverter_FromRGB_ToHSV(FxImages image)
        {
            // base on http://en.wikipedia.org/wiki/HSL_and_HSV

            // get the size of the image
            int ImWidth = image.Image.Width;
            int ImHeight = image.Image.Height;

            // lock the input memory
            image.LockImage();


            // temp variables
            double R, G, B, H, S, V, C, min, MAX;
            for (int i = 0; i < ImWidth; i++)
            {
                for (int j = 0; j < ImHeight; j++)
                {
                    // set the local RGB
                    R = image[i, j, RGB.R];
                    G = image[i, j, RGB.G];
                    B = image[i, j, RGB.B];

                    // find the min and max colors
                    min = Math.Min(Math.Min(R, G), B);
                    MAX = Math.Max(Math.Max(R, G), B);

                    // find the diff 
                    C = MAX - min;

                    // set the V
                    V = MAX;

                    // check if the max is zero
                    if (V == 0)
                    {
                        S = 0;
                        H = 0;
                    }
                    else
                    {
                        // calc the S
                        S = 255 * C / V;

                        // check if S is zero
                        if (S == 0)
                        {
                            H = 0;
                        }
                        else
                        {
                            // calc the H
                            if (MAX == R)
                            {
                                H = 42.5 * (((G - B) / C) % 6);
                            }
                            else if (MAX == G)
                            {
                                H = 42.5 * ((B - R) / C + 2);
                            }
                            else /* MAX == B */
                            {
                                H = 42.5 * ((R - G) / C + 4);
                            }
                        }

                    }

                    // store the result
                    image[i, j, RGB.R] = (byte)H;
                    image[i, j, RGB.G] = (byte)S;
                    image[i, j, RGB.B] = (byte)V;

                }
            }

            // unlock the input and output image
            image.UnLockImage();

        }

        private static void ColorSpaceConverter_FromHSV_ToRGB(FxImages image)
        {
            // base on http://en.wikipedia.org/wiki/HSL_and_HSV

            // get the size of the image
            int ImWidth = image.Image.Width;
            int ImHeight = image.Image.Height;

            // lock the input memory
            image.LockImage();


            // temp variables
            double R, G, B, H, S, V, C, X,diff;
            for (int i = 0; i < ImWidth; i++)
            {
                for (int j = 0; j < ImHeight; j++)
                {
                    // set the local RGB
                    H = image[i, j, RGB.R];
                    S = image[i, j, RGB.G] / 255.0;
                    V = image[i, j, RGB.B] / 255.0;

                    // find the chroma
                    C = V * S;

                    // find H'
                    H = H / 42.5;
                    X = C * (1 - Math.Abs(H % 2 - 1));

                    // calc the diff between V and C
                    diff = V - C;

                    if (H < 1)
                    {
                        R = C + diff;
                        G = X + diff;
                        B = diff;
                    }
                    else if (H < 2)
                    {
                        R = X + diff;
                        G = C + diff;
                        B = diff;
                    }
                    else if (H < 3)
                    {
                        R = diff;
                        G = C + diff;
                        B = X + diff;
                    }
                    else if (H < 4)
                    {
                        R = diff;
                        G = X + diff;
                        B = C + diff;
                    }
                    else if (H < 5)
                    {
                        R = X + diff;
                        G = diff;
                        B = C + diff;
                    }
                    else if (H < 6)
                    {
                        R = C + diff;
                        G = diff;
                        B = X + diff;
                    }
                    else
                    {
                        R = diff;
                        G = diff;
                        B = diff;
                    }

                    // store the result
                    image[i, j, RGB.R] = (byte)(R * 255.0);
                    image[i, j, RGB.G] = (byte)(G * 255.0);
                    image[i, j, RGB.B] = (byte)(B * 255.0);

                }
            }

            // unlock the input and output image
            image.UnLockImage();

        }

        #endregion


    }
}
