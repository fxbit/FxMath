using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SharpDX;
using System.Drawing;
using SharpDX.Direct2D1;
using FxMaths.Images;

namespace FxMaths.GUI
{
    public class ImageElement : CanvasElements
    {
        SharpDX.Direct2D1.Bitmap mImageBitmap;

        int Width,Height,Pitch;
        DataStream internalImage;

        public ImageElement(FxImages image)
        {

            // set the position and the size of the element
            Position = new Vector.FxVector2f(0);
            Size = new Vector.FxVector2f(image.Image.Width, image.Image.Height);

            // get the size of the image
            Width = image.Image.Width;
            Height = image.Image.Height;

            // allocate the memory for the internal image
            internalImage = new DataStream( Width * Height * 4, true, true);
            byte[] internalImage_local = new byte[Width * Height * 4];

            Pitch = Width * 4;
            image.LockImage();
            image.Copy_to_Array(ref internalImage_local);
            image.UnLockImage();

            internalImage.WriteRange<byte>(internalImage_local);
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
            internalImage = new DataStream(Width * Height * 4, true, true);
            byte[] internalImage_local = new byte[Width * Height * 4];

            Pitch = Width * 4;
            int size= Width*Height;
            
            // load the data in image form
            for ( int i=0; i < size; i ++ ) {
                internalImage_local[i * 4] = (byte)(256 * image[i]);
                internalImage_local[i * 4 + 1] = (byte)(256 * image[i]);
                internalImage_local[i * 4 + 2] = (byte)(256 * image[i]);
                internalImage_local[i * 4 + 3] = 255;
            }
            internalImage.WriteRange<byte>(internalImage_local);
        }

        public override void Render( CanvasRenderArguments args, SizeF Zoom )
        {
            // check if we have set the image
            if ( mImageBitmap != null ) {
                //RectangleF rect= new RectangleF( Position.X, Position.Y, Size.X, Size.Y );
                // render the image
                args.renderTarget.DrawBitmap( mImageBitmap,1, BitmapInterpolationMode.Linear );
            }
        }

        public override void Load( CanvasRenderArguments args )
        {
            // set the properties of the image
            BitmapProperties bitmapProps = new BitmapProperties();
            bitmapProps.PixelFormat = new SharpDX.Direct2D1.PixelFormat( SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied );

            if ( mImageBitmap != null ) {
                // write to the specific bitmap not create a new one
                mImageBitmap.CopyFromStream(internalImage, Width, Width * Height);
            } else {
                // make the bitmap for Direct2D1
                mImageBitmap = new SharpDX.Direct2D1.Bitmap(args.renderTarget,
                    new Size2(Width, Height), 
                    new DataPointer(internalImage.DataPointer, (int)internalImage.Length), 
                    Pitch, 
                    bitmapProps);
            }
        }

        #region Update Internal Image
        public void UpdateInternalImage(FxImages image)
        {
            byte[] internalImage_local = new byte[Width * Height * 4];

            Pitch = Width * 4;
            int size = Width * Height;
            image.LockImage();
            image.Copy_to_Array(ref internalImage_local);
            image.UnLockImage();

            // write to the specific bitmap not create a new one
            mImageBitmap.CopyFromMemory(internalImage_local, Pitch);
        }



        public void UpdateInternalImage(byte[] image)
        {
            byte[] newIm = new byte[Width * Height * 4];

            Pitch = Width * 4;
            int size = Width * Height;

            for (int i = 0; i < size; i++)
            {
                newIm[i * 4] = image[i * 3];
                newIm[i * 4 + 1] = image[i * 3 + 1];
                newIm[i * 4 + 2] = image[i * 3 + 2];
                newIm[i * 4 + 3] = 255;
            }

            // write to the specific bitmap not create a new one
            mImageBitmap.CopyFromMemory(newIm, Pitch);
        }



        public void UpdateInternalImage(Matrix.FxMatrixF mat)
        {
            byte[] newIm = new byte[Width * Height * 4];
            int size = Width * Height;

            for (int i = 0; i < size; i++)
            {
                newIm[i * 4] = (byte)(mat[i] * 256);
                newIm[i * 4 + 1] = (byte)(mat[i] * 256);
                newIm[i * 4 + 2] = (byte)(mat[i] * 256);
                newIm[i * 4 + 3] = 255;
            }

            // write to the specific bitmap not create a new one
            mImageBitmap.CopyFromMemory(newIm, Width * 4);

        } 
        #endregion


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
