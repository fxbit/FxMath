using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SharpDX;
using System.Drawing;
using SharpDX.Direct2D1;
using FxMaths.Images;
using System.Threading.Tasks;

namespace FxMaths.GUI
{
    public class ImageElement : CanvasElements
    {
        SharpDX.Direct2D1.Bitmap mImageBitmap;

        private byte[] internalImage;
        private int Width;
        private int Height;
        private int Pitch;
        private FxMaths.Matrix.FxMatrixF internalMatrix;


        #region Constructor

        public ImageElement(FxImages image)
        {
            InitToolStrips();

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
            InitToolStrips();

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
            InitToolStrips();

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
            lock (this)
            {
                // check if we have set the image
                if (mImageBitmap != null)
                {

                    var rect = new SharpDX.RectangleF(0, 0, Size.X, Size.Y);
                    // render the image
                    args.renderTarget.DrawBitmap(mImageBitmap, rect, 1, BitmapInterpolationMode.Linear);
                }
            }
        }

        #endregion




        #region Load functions

        public override void Load(CanvasRenderArguments args)
        {
            // set the properties of the image
            BitmapProperties bitmapProps = new BitmapProperties();
            bitmapProps.PixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied);

            if (mImageBitmap == null)
            {
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

            for (int i = 0; i < size; i++)
            {
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
            // link the extran matrix with the internal
            internalMatrix = mat;


            // check if we need to create a new internal buffer 
            if (mat.Width != Width || mat.Height != Height)
            {
                lock (this)
                {
                    // set the properties of the image
                    BitmapProperties bitmapProps = new BitmapProperties();
                    bitmapProps.PixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied);

                    // set the size of the image
                    Width = mat.Width;
                    Height = mat.Height;

                    if (mImageBitmap == null)
                    {
                        mImageBitmap.Dispose();
                    }

                    // make the bitmap for Direct2D1
                    mImageBitmap = new SharpDX.Direct2D1.Bitmap(this.Parent.RenderVariables.renderTarget,
                                                                new Size2(Width, Height),
                                                                bitmapProps);
                }
            }

            unsafe
            {
                try
                {
                    int size = Width * Height;
                    fixed (byte* dst = internalImage)
                    {
                        fixed (float* src = mat.Data)
                        {
                            byte* pDst = dst;
                            float* pSrc = src;

#if false
                            float *pSrcEnd = pSrc + mat.Size;
                            for(; pSrc < pSrcEnd; pSrc++) {
                                byte value = (byte)(*(pSrc) * 255);
                                *(pDst++) = value;
                                *(pDst++) = value;
                                *(pDst++) = value;
                                *(pDst++) = 255;
                            }
#else
                            int step = size / 8;
                            Parallel.For(0, 8, (s) =>
                            {
                                int end = (s + 1) * step;
                                for (int i = s * step; i < end; i++)
                                {
                                    byte value = (byte)(*(pSrc) * 255);
                                    int i4 = i * 4;
                                    *(pDst + i4) = value;
                                    *(pDst + i4 + 1) = value;
                                    *(pDst + i4 + 2) = value;
                                    *(pDst + i4 + 3) = 255;

                                }
                            });
#endif
                        }
                    }

                    // write to the specific bitmap not create a new one
                    mImageBitmap.CopyFromMemory(internalImage, Width * 4);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }

        public void UpdateInternalImage(Matrix.FxMatrixF mat, ColorMap map, bool useInvMap = false)
        {
            // link the extran matrix with the internal
            internalMatrix = mat;

            // check if we need to create a new internal buffer 
            if (mat.Width != Width || mat.Height != Height)
            {
                lock (this)
                {
                    // set the properties of the image
                    BitmapProperties bitmapProps = new BitmapProperties();
                    bitmapProps.PixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied);

                    // set the size of the image
                    Width = mat.Width;
                    Height = mat.Height;

                    if (mImageBitmap == null)
                    {
                        mImageBitmap.Dispose();
                    }

                    // make the bitmap for Direct2D1
                    mImageBitmap = new SharpDX.Direct2D1.Bitmap(this.Parent.RenderVariables.renderTarget,
                                                                new Size2(Width, Height),
                                                                bitmapProps);
                }
            }

            unsafe
            {
                try
                {
                    int size = Width * Height;
                    fixed (byte* dst = internalImage)
                    {
                        fixed (float* src = mat.Data)
                        {
                            byte* pDst = dst;
                            float* pSrc = src;

                            if (useInvMap)
                            {
#if false
                                float* pSrcEnd = pSrc + mat.Size;
                                for (; pSrc < pSrcEnd; pSrc++)
                                {
                                    byte id = (byte)(255 - *(pSrc) * 255);
                                    *(pDst++) = map[id, 2];
                                    *(pDst++) = map[id, 1];
                                    *(pDst++) = map[id, 0];
                                    *(pDst++) = 255;
                                }
#else
                                int step = size / 8;
                                Parallel.For(0, 8, (s) =>
                                {
                                    int end = (s + 1) * step;
                                    for (int i = s * step; i < end; i++)
                                    {
                                        byte id = (byte)(255 - *(pSrc + i) * 255);
                                        int i4 = i * 4;
                                        *(pDst + i4) = map[id, 2];
                                        *(pDst + i4 + 1) = map[id, 1];
                                        *(pDst + i4 + 2) = map[id, 0];
                                        *(pDst + i4 + 3) = 255;

                                    }
                                });
#endif
                            }
                            else
                            {
#if false
                                
                                float* pSrcEnd = pSrc + mat.Size;
                                for(; pSrc < pSrcEnd; pSrc++) {
                                    byte id = (byte)(*(pSrc) * 255);
                                    *(pDst++) = map[id, 2];
                                    *(pDst++) = map[id, 1];
                                    *(pDst++) = map[id, 0];
                                    *(pDst++) = 255;
                                }
#else
                                int step = size / 8;
                                Parallel.For(0, 8, (s) =>
                                {
                                    int end = (s + 1) * step;
                                    for (int i = s * step; i < end; i++)
                                    {
                                        byte id = (byte)(*(pSrc + i) * 255);
                                        int i4 = i * 4;
                                        *(pDst + i4) = map[id, 2];
                                        *(pDst + i4 + 1) = map[id, 1];
                                        *(pDst + i4 + 2) = map[id, 0];
                                        *(pDst + i4 + 3) = 255;

                                    }
                                });
#endif
                            }
                        }
                    }

                    // write to the specific bitmap not create a new one
                    mImageBitmap.CopyFromMemory(internalImage, Width * 4);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
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
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                Console.WriteLine(internalImage[x + y * Pitch]);
                OnMouseClickEvent(this, location);
            }

        }

        #endregion



        #region ToolStrip

        private System.Windows.Forms.ToolStripButton toolStrip_ShowColor;

        private void InitToolStrips()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Canvas));

            // 
            // toolStripButton_propertieGrid
            // 
            toolStrip_ShowColor = new System.Windows.Forms.ToolStripButton();
            toolStrip_ShowColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStrip_ShowColor.Image = ((System.Drawing.Image)(resources.GetObject("color-picker.Image")));
            toolStrip_ShowColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStrip_ShowColor.Name = "Show Color";
            toolStrip_ShowColor.Size = new System.Drawing.Size(28, 28);
            toolStrip_ShowColor.Text = "Show Color";
            toolStrip_ShowColor.Click += toolStrip_ShowColor_Click;

        }

        void toolStrip_ShowColor_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripButton strip = (System.Windows.Forms.ToolStripButton)sender;

            if (strip.Checked)
            {
                strip.Checked = false;
                this.MouseClickEvent -= ImageElement_MouseClickEvent_ShowColor;
            }
            else
            {
                strip.Checked = true;
                this.MouseClickEvent += ImageElement_MouseClickEvent_ShowColor;
            }
        }

        void ImageElement_MouseClickEvent_ShowColor(CanvasElements m, Vector.FxVector2f location)
        {
            int position = (int)(location.x + location.y * this.Width);
            System.Windows.Forms.MessageBox.Show( "Postion : (" + location.x.ToString() + ", " + location.y.ToString() + ")\r\n"+
                "RGBA: (" + this.internalImage[position * 4].ToString() + "," +
                this.internalImage[position * 4 + 1].ToString() + "," +
                this.internalImage[position * 4 + 2].ToString() + "," +
                this.internalImage[position * 4 + 3].ToString() + ")\r\n" +
                "Matrix : " + this.internalMatrix[(int)location.x, (int)location.y]);
        }


        public override void FillToolStrip(System.Windows.Forms.ToolStrip toolStrip)
        {
            toolStrip.Items.Add(toolStrip_ShowColor);
        }


        #endregion
    }

}
