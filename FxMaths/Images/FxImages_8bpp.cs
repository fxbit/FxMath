using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FxMaths.Images
{
	unsafe class FxImages_8bpp : FxImages
	{

		public Bitmap localImage;
		public BitmapData ImageData;
		public int Stride;
		byte* Scan0;


		public FxImages_8bpp(Bitmap Image)
		{
			if (Image.PixelFormat != PixelFormat.Format8bppIndexed)
			{
				throw new ArgumentException("The input image must be 8 Bit per pixel");
			}

			// set the pilex format
			FXPixelFormat = new RGB[1];
			FXPixelFormat[0] = RGB.B;

			// set the internal image
			localImage = Image;

			/// make the grayscale palette
			ColorPalette greyPal = Image.Palette;
			for (int i = 0; i < greyPal.Entries.Length; i++)
				greyPal.Entries[i] = Color.FromArgb(i, i, i);
			Image.Palette = greyPal;
		}

		public override void LockImage()
		{
			unsafe
			{
				ImageData = localImage.LockBits(new Rectangle(0, 0, localImage.Width, localImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
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
			byte* ptr = Scan0 + (y * Stride) + x;
			return Color.FromArgb(*(ptr), *(ptr), *(ptr));
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
			unsafe {
				switch(numChannels) {
					case ColorChannels.Gray:
						Marshal.Copy(ImageData.Scan0, dest, 0, size);
						//Array.Copy((byte[])ImageData.Scan0, dest, size);
						break;
					case ColorChannels.RGB: {
							byte *pSrc = Scan0;
							byte *pSrcEnd = Scan0 + size;
							fixed(byte *d = dest) {
								byte *pDest = d;
								for(; pSrc < pSrcEnd; pSrc++) {
									byte value = *pSrc;
									*(pDest++) = value;// B
									*(pDest++) = value;// G
									*(pDest++) = value;// R
								}
							}
						}
						break;
					case ColorChannels.RGBA: {
							byte *pSrc = Scan0;
							byte *pSrcEnd = Scan0 + size;
							fixed(byte *d = dest) {
								byte *pDest = d;
								for(; pSrc < pSrcEnd; pSrc++) {
									byte value = *pSrc;
									*(pDest++) = value;// B
									*(pDest++) = value;// G
									*(pDest++) = value;// R
									*(pDest++) = 1;// A
								}
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
			byte* ptr = Scan0 + (y * Stride) + x;
			*(ptr) = (byte)color.R;
			*(ptr) = (byte)color.G;
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
					byte* ptr = Scan0 + (y * Stride) + x;
					return Color.FromArgb(*(ptr), *(ptr), *(ptr));
				}
			}
			set
			{
				unsafe
				{
					byte* ptr = Scan0 + (y * Stride) + x;
					*(ptr) = (byte)value.R;
					*(ptr) = (byte)value.G;
					*(ptr) = (byte)value.B;
				}
			}
		}


		/// <summary>
		/// get specific pixel and specific color
		/// </summary>
		/// <param name="x">x position of pixel</param>
		/// <param name="y">y position of pixel</param>
		/// <param name="index">index of the color blue:0 green:1 red:2 !!! ignore for grayscale</param>
		/// <returns>byte for the color of pixel</returns>
		public override byte this[int x, int y, RGB index]
		{
			get {
				unsafe
				{
					return *(Scan0 + (y * Stride) + x);
				}
			}
			set
			{
				unsafe
				{
					*(Scan0 + (y * Stride) + x) = (byte)value;
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
			throw new NotImplementedException();
		}
	}
}
