using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SlimDX;
using System.Drawing;
using SlimDX.Direct2D;
using FxMaths.Images;

namespace FxMaths.GUI
{
    public class ImageElement : CanvasElements
    {
        SlimDX.Direct2D.Bitmap mImageBitmap;

        int Width,Height,Pitch;
        byte []internalImage;

        public ImageElement(FxImages image)
        {

            // set the position and the size of the element
            Position = new Vector.FxVector2f(0);
            Size = new Vector.FxVector2f(image.Image.Width, image.Image.Height);

            // get the size of the image
            Width = image.Image.Width;
            Height = image.Image.Height;

            // allocate the memory for the internal image
            internalImage = new byte[Width * Height * 4];

            Pitch = Width * 4;
            int size = Width * Height;
            image.LockImage();

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    for (int g = 0; g < image.FXPixelFormat.Length; g++)
                    {
                        internalImage[i * 4 + j * Pitch + g] = image[i,j,(RGB)g];

                    }
                    internalImage[i * 4 + j * Pitch + 3] = 255;
                }
            }
            image.UnLockImage();
        }

        public ImageElement( Matrix.FxMatrixF image )
        {

            // set the position and the size of the element
            Position = new Vector.FxVector2f( 0 );
            Size = new Vector.FxVector2f( image.Width, image.Height );

            // get the size of the image
            Width = image.Width;
            Height = image.Height;

            // allocate the memory for the internal image
            internalImage = new byte[Width * Height * 4];

            Pitch = Width * 4;
            int size= Width*Height;
            
            // load the data in image form
            for ( int i=0; i < size; i ++ ) {
                internalImage[i * 4] = (byte)( 256 * image[i] );
                internalImage[i * 4 + 1] = (byte)( 256 * image[i] );
                internalImage[i * 4 + 2] = (byte)( 256 * image[i] );
                internalImage[i * 4 + 3] = 255;
            }
        }

        public override void Render( CanvasRenderArguments args, SizeF Zoom )
        {
            // check if we have set the image
            if ( mImageBitmap != null ) {
                //RectangleF rect= new RectangleF( Position.X, Position.Y, Size.X, Size.Y );
                // render the image
                args.renderTarget.DrawBitmap( mImageBitmap );
            }
        }

        public override void Load( CanvasRenderArguments args )
        {
            // make the data stream of the matrix
            DataStream dataStream = new DataStream( internalImage, true, false );

            // set the properties of the image
            BitmapProperties bitmapProps = new BitmapProperties();
            bitmapProps.PixelFormat = new SlimDX.Direct2D.PixelFormat( SlimDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied );

            if ( mImageBitmap != null ) {
                // write to the specific bitmap not create a new one
                mImageBitmap.FromStream( dataStream, Width, Width*Height );
            } else {
                // make the bitmap for direct2d
                mImageBitmap = new SlimDX.Direct2D.Bitmap( args.renderTarget, new Size( Width, Height ), dataStream, Pitch, bitmapProps );
            }
        }

        internal override void InternalMove( Vector.FxVector2f delta )
        {
            // do nothing
        }

        public override void Dispose()
        {
            // clean the memmory
            mImageBitmap.Dispose();
        }
    }
}
