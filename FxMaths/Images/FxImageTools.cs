using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxMaths.Vector;
using FXImages;

namespace FxMaths.Images
{
    public class FxImageTools
    {

        #region Get histogram for specific area of the image

        /// <summary>
        /// Get the histogram of the image.
        /// The histogram is separate for each color channel
        /// </summary>
        /// <param name="image"></param>
        /// <param name="lanes"></param>
        /// <returns></returns>
        public static FxVector<float>[] GetHistogram(FxImages image)
        {
            return GetHistogram(image, new FxVector2i(0, 0), new FxVector2i(image.Image.Width, image.Image.Height));
        }

        /// <summary>
        /// Get the histogram of a specific area of the image.
        /// The histogram is separate for each color channel
        /// </summary>
        /// <param name="image"></param>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static FxVector<float>[] GetHistogram(FxImages image, FxVector2i Min, FxVector2i Max)
        {
            FxVector<float>[] hist = new FxVectorF[image.FXPixelFormat.Length];
            for (int i = 0; i < image.FXPixelFormat.Length; i++)
            {
                hist[i] = new FxVectorF(256,0);
            }

            int pixelCount = 0;

            // lock the input memory
            //image.LockImage();

            for (int i = Min.X; i < Max.X; i++)
            {
                for (int j = Min.Y; j < Max.Y; j++)
                {
                    for (int g = 0; g < image.FXPixelFormat.Length; g++)
                    {
                        hist[g][image[i, j, (RGB)g]]++;
                    }

                    // count the pixels
                    pixelCount++;
                }
            }

            // unlock the input and output image
            //image.UnLockImage();

            // normalize the image
            for (int i = 0; i < image.FXPixelFormat.Length; i++)
            {
                hist[i].Divide(pixelCount);
            }

            return hist;
        }

        #endregion


        #region Image Normalize

        /// <summary>
        /// Get the histogram of a specific area of the image.
        /// The histogram is separate for each color channel
        /// </summary>
        /// <param name="image"></param>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static void NormalizeImage(FxImages image)
        {
            FxVector<float>[] hist = new FxVectorF[image.FXPixelFormat.Length];
            for (int i = 0; i < image.FXPixelFormat.Length; i++)
            {
                hist[i] = new FxVectorF(256, 0);
            }

            int pixelCount = 0;

            // get the size of the image
            int ImWidth = image.Image.Width;
            int ImHeight = image.Image.Height;


            // lock the input memory
            image.LockImage();

            byte[] max = new byte[image.FXPixelFormat.Length];
            byte[] min = new byte[image.FXPixelFormat.Length];

            for (int g = 0; g < image.FXPixelFormat.Length; g++)
            {
                max[g] = Byte.MinValue;
                min[g] = Byte.MaxValue;
            }

            // find max min
            for (int i = 0; i < ImWidth; i++)
            {
                for (int j = 0; j < ImHeight; j++)
                {
                    for (int g = 0; g < image.FXPixelFormat.Length; g++)
                    {
                        if (min[g] > image[i, j, (RGB)g])
                            min[g] = image[i, j, (RGB)g];

                        if (max[g] < image[i, j, (RGB)g])
                            max[g] = image[i, j, (RGB)g];
                    }
                }

            }

            // normalize image
            for (int i = 0; i < ImWidth; i++)
            {
                for (int j = 0; j < ImHeight; j++)
                {
                    for (int g = 0; g < image.FXPixelFormat.Length; g++)
                    {
                        image[i, j, (RGB)g] = (byte)((float)(image[i, j, (RGB)g] - min[g]) / (float)(max[g] - min[g]));
                    }
                }
            }

            // unlock the input and output image
            image.UnLockImage();

        }

        #endregion
    }
}
