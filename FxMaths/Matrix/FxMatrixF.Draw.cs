using FxMaths.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxMaths.Matrix
{
    public partial class FxMatrixF
    {
        /// <summary>
        /// Draw a line with specific value
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void DrawLine(FxVector2f start, FxVector2f end, float value)
        {
            var dx = Math.Abs(end.x - start.x);
            var dy = Math.Abs(end.y - start.y);
            var sx = (start.x < end.x) ? 1 : -1;
            var sy = (start.y < end.y) ? 1 : -1;
            var err = dx - dy;
            int x0 = (int)start.x;
            int y0 = (int)start.y;
            int x1 = (int)end.x;
            int y1 = (int)end.y;
            while(!((x0 == x1) && (y0 == y1)))
            {
                this[x0, y0] = value;
                var e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if ((x0 == x1) && (y0 == y1))
                {
                    this[x0, y0] = value;
                    break;
                }

                if (e2 < -dx)
                {
                    err -= dx;
                    y0 += sy;
                }
            }
        }

        /// <summary>
        /// Draw Rectangle.
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="Size"></param>
        public void DrawRect(FxVector2f start, FxVector2f size, float value)
        {
            DrawLine(start, new FxVector2f(start.x + size.x, start.y), value);
            DrawLine(new FxVector2f(start.x + size.x, start.y), new FxVector2f(start.x + size.x, start.y + size.y), value);
            DrawLine(new FxVector2f(start.x + size.x, start.y + size.y), new FxVector2f(start.x, start.y + size.y), value);
            DrawLine(new FxVector2f(start.x, start.y + size.y), start, value);
        }



        /// <summary>
        /// Draw Circle
        /// </summary>
        /// <param name="Center"></param>
        /// <param name="Radius"></param>
        /// <param name="Value"></param>
        public void DrawCircle(Vector.FxVector2f center, float radius, float Value)
        {
            const float step = (float)Math.PI / 300.0f;

            for (float rad = 0; rad < 2 * Math.PI; rad += step)
            {
                int x = (int)Math.Round(center.x + radius * Math.Cos(rad));
                int y = (int)Math.Round(center.y + radius * Math.Sin(rad));
                if (x >= 0 && x < Width && y >= 0 && y < Height)
                    this[x, y] = Value;
            }
        }

        /// <summary>
        /// The method that we are going to 
        /// use for the interpolation.
        /// </summary>
        public enum DrawInterpolationMethod
        {
            NearestNeighbor,
            Linear,
            Cubic
        }

        /// <summary>
        /// Draw external matrix to this matrix.
        /// </summary>
        /// <param name="matrix">The matrix that store the copied image</param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        public void DrawMatrix(FxMatrixF matrix, FxVector2f start, FxVector2f size, DrawInterpolationMethod method)
        {
            int h = matrix.Height;
            int w = matrix.Width;

            float stepW = (float)w / size.x;
            float stepH = (float)h / size.y;

            FxVector2f end = start + size;
            if (end.x > Width)
                end.x = Width - 1;
            if (end.y > Height)
                end.y = Height - 1;

            switch (method)
            {
                case DrawInterpolationMethod.NearestNeighbor:

                    Parallel.For((int)start.x, (int)end.x, (i) =>
                    {
                        int xp = (int)Math.Floor((i - start.x) * stepW);
                        for (int j = (int)start.y, jy = 0; j < end.y ; j++, jy++)
                        {
                            int yp = (int)Math.Floor(jy * stepH);
                            this[i, j] = matrix[xp, yp];
                        }
                    });

                    break;

                case DrawInterpolationMethod.Linear:

                    Parallel.For((int)start.x, (int)end.x, (i) =>
                    {
                        float xp = (i - start.x) * stepW;
                        for (int j = (int)start.y, jy = 0; j < end.y; j++, jy++)
                        {
                            float yp = jy * stepH;
                            if (yp < matrix.Height - 1)
                                this[i, j] = matrix.Sample(xp, yp);
                        }
                    });

                    break;


                case DrawInterpolationMethod.Cubic:

                    Parallel.For((int)start.x, (int)end.x, (i) =>
                    {
                        float xp = (i - start.x) * stepW;
                        for (int j = (int)start.y, jy = 0; j < end.y; j++, jy++)
                        {
                            float yp = jy * stepH;
                            if (yp < matrix.Height - 1)
                                this[i, j] = matrix.SampleCubic(xp, yp);
                        }
                    });

                    break;
            }
        }
    }
}


