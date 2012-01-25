using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using FXImages;

namespace FxMaths.Matrix
{
    public partial class FxMatrixF
    {
        /// <summary>
        /// Load the input image to the matrix
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static FxMatrixF Load( Bitmap image , ColorSpace space)
        {
            int Width = image.Width;
            int Height = image.Height;
            // create a new matrix for the image
            FxMatrixF result = new FxMatrixF( image.Width, image.Height );

            switch ( space ) {

                    // base on 
                case ColorSpace.Grayscale: {

                        int grayScale;
                        FxImages image_in = FxTools.FxImages_safe_constructors( image );

                        // lock the input image
                        image_in.LockImage();

                        for ( int y = 0; y < Height; y++ ) {

                            for ( int x = 0; x < Width; x++ ) {
                                /// this number based in wikipedia!!! : http://en.wikipedia.org/wiki/Grayscale
                                grayScale = (int)( ( image_in[x, y, RGB.R] * 0.3 ) + ( image_in[x, y, RGB.G] * 0.59 ) + ( image_in[x, y, RGB.B] * 0.11 ) );
                                result[x, y] = grayScale / 256.0f;
                            }

                        }

                        // unlock the input image
                        image_in.UnLockImage();
                    }
                    break;

                case ColorSpace.RGB:
                    throw new ArgumentNullException( "Not supported Yet" );
                    //break;

                case ColorSpace.HSV:
                    throw new ArgumentNullException( "Not supported Yet" );
                    //break;
            }


            return result;
        }

        /// <summary>
        /// Load the input file to the matrix
        /// The file must be image
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="space"></param>
        /// <returns></returns>
        public static FxMatrixF Load( String FileName, ColorSpace space )
        {
            Bitmap image = new Bitmap( FileName );

            return Load( image, space );
        }
    }
}
