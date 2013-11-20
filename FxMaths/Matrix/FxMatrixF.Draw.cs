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
        /// <param name="start"></param>
        /// <param name="size"></param>
        public void DrawRect(FxVector2f start, FxVector2f size, float value)
        {
            DrawLine(start, new FxVector2f(start.x + size.x, start.y), value);
            DrawLine(new FxVector2f(start.x + size.x, start.y), new FxVector2f(start.x + size.x, start.y + size.y), value);
            DrawLine(new FxVector2f(start.x + size.x, start.y + size.y), new FxVector2f(start.x, start.y + size.y), value);
            DrawLine(new FxVector2f(start.x, start.y + size.y), start, value);
        }

    }
}
