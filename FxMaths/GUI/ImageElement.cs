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
        
        private byte[] internalImage;
        private int Width;
        private int Height;
		private int Pitch;
		
        #region Constructor

        public ImageElement(FxImages image)
        {

            // set the position and the size of the element
            Position = new Vector.FxVector2f(0);
            Size = new Vector.FxVector2f(image.Image.Width, image.Image.Height);

            // set the size of the image
            Width = image.Image.Width;
            Height = image.Image.Height;

            // allocate the memory for the internal image
            internalImage = new byte[Width * Height * 4];

            Pitch = Width * 4;
            image.LockImage();
            image.Copy_to_Array(ref internalImage, ColorChannels.RGBA);
            image.UnLockImage();
        }

        public ImageElement(Matrix.FxMatrixF mat)
        {

            // set the position and the size of the element
            Position = new Vector.FxVector2f(0);
            Size = new Vector.FxVector2f(mat.Width, mat.Height);
            
            // set the size of the image
            Width = mat.Width;
            Height = mat.Height;

            // allocate the memory for the internal image
            internalImage = new byte[mat.Width * mat.Height * 4];
            Pitch = mat.Width * 4;

            UpdateInternalImage(mat);
        }

        public ImageElement(Matrix.FxMatrixF mat, ColorMap map)
        {

            // set the position and the size of the element
            Position = new Vector.FxVector2f(0);
            Size = new Vector.FxVector2f(mat.Width, mat.Height);

            // set the size of the image
            Width = mat.Width;
            Height = mat.Height;

            // allocate the memory for the internal image
            internalImage = new byte[mat.Width * mat.Height * 4];
            Pitch = mat.Width * 4;

            UpdateInternalImage(mat, map);
        }

        #endregion




        #region Rendering


        public override void Render(CanvasRenderArguments args, SizeF Zoom)
        {
            // check if we have set the image
            if(mImageBitmap != null) {
                //RectangleF rect= new RectangleF( Position.X, Position.Y, Size.X, Size.Y );
                // render the image
                args.renderTarget.DrawBitmap(mImageBitmap, 1, BitmapInterpolationMode.Linear);
            }
        }
        
        #endregion




        #region Load functions

        public override void Load(CanvasRenderArguments args)
        {
            // set the properties of the image
            BitmapProperties bitmapProps = new BitmapProperties();
            bitmapProps.PixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied);

            if(mImageBitmap == null) {
                // make the bitmap for Direct2D1
                mImageBitmap = new SharpDX.Direct2D1.Bitmap(args.renderTarget,
                    new Size2(Width, Height),
                    bitmapProps);
            }

            // write to the specific bitmap not create a new one
            mImageBitmap.CopyFromMemory(internalImage, Pitch);
        }

        
        #endregion




        #region Update Internal Image
        public void UpdateInternalImage(FxImages image)
        {
            int size = Width * Height;
            image.LockImage();
            image.Copy_to_Array(ref internalImage, ColorChannels.RGBA);
            image.UnLockImage();

            // write to the specific bitmap not create a new one
            mImageBitmap.CopyFromMemory(internalImage, Pitch);
        }



        public void UpdateInternalImage(byte[] image)
        {
            int size = Width * Height;

            for(int i = 0; i < size; i++) {
                internalImage[i * 4] = image[i * 3];
                internalImage[i * 4 + 1] = image[i * 3 + 1];
                internalImage[i * 4 + 2] = image[i * 3 + 2];
                internalImage[i * 4 + 3] = 255;
            }

            // write to the specific bitmap not create a new one
            mImageBitmap.CopyFromMemory(internalImage, Pitch);
        }



        public void UpdateInternalImage(Matrix.FxMatrixF mat)
        {
            unsafe {
                try {
                    int size = Width * Height;
                    fixed(byte*dst = internalImage) {
                        fixed(float *src = mat.Data) {
                            byte *pDst = dst;
                            float *pSrc = src;
                            float *pSrcEnd = pSrc + mat.Size;
                            for(; pSrc < pSrcEnd; pSrc++) {
                                byte value = (byte)(*(pSrc) * 255);
                                *(pDst++) = value;
                                *(pDst++) = value;
                                *(pDst++) = value;
                                *(pDst++) = 255;
                            }
                        }
                    }

                    // write to the specific bitmap not create a new one
                    mImageBitmap.CopyFromMemory(internalImage, Width * 4);
                } catch(Exception ex) { Console.WriteLine(ex.Message); }
            }
        }

        public void UpdateInternalImage(Matrix.FxMatrixF mat, ColorMap map, bool useInvMap = false)
        {
            unsafe {
                try {
                    int size = Width * Height;
                    fixed(byte* dst = internalImage) {
                        fixed(float* src = mat.Data) {
                            byte* pDst = dst;
                            float* pSrc = src;
                            float* pSrcEnd = pSrc + mat.Size;
                            if(useInvMap) {
                                for(; pSrc < pSrcEnd; pSrc++) {
                                    byte id = (byte)(255 - *(pSrc) * 255);
                                    *(pDst++) = map[id, 2];
                                    *(pDst++) = map[id, 1];
                                    *(pDst++) = map[id, 0];
                                    *(pDst++) = 255;
                                }
                            } else {
                                for(; pSrc < pSrcEnd; pSrc++) {
                                    byte id = (byte)(*(pSrc) * 255);
                                    *(pDst++) = map[id, 2];
                                    *(pDst++) = map[id, 1];
                                    *(pDst++) = map[id, 0];
                                    *(pDst++) = 255;
                                }

                            }
                        }
                    }

                    // write to the specific bitmap not create a new one
                    mImageBitmap.CopyFromMemory(internalImage, Width * 4);
                } catch(Exception ex) { Console.WriteLine(ex.Message); }
            }
        }
        #endregion

        

        public override void Dispose()
        {
            // clean the memmory
            mImageBitmap.Dispose();
        }



        #region Mouse Events


        internal override void InternalMove(Vector.FxVector2f delta)
        {
            // do nothing
        }


        internal override void MouseClick(Vector.FxVector2f location)
        {
            int x = (int)Math.Ceiling(location.x);
            int y = (int)Math.Ceiling(location.y);
            Console.WriteLine(location.ToString());
            if(x >= 0 && x < Width && y >= 0 && y < Height) {
                Console.WriteLine(internalImage[x+y*Pitch]);
                OnMouseClickEvent(this, location);
            }
            
        }

        #endregion
    }
}
