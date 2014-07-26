using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct2D1;
using FxMaths.GMaps;
using FxMaths.Vector;

namespace FxMaths.Geometry
{
    public class Rectangle:IBaseGeometry
    {



        #region Variables

        /// <summary>
        /// Start of the rectangle.
        /// </summary>
        private FxVector2f m_start;

        /// <summary>
        /// End of the recatngle
        /// </summary>
        private FxVector2f m_end;


        /// <summary>
        /// Use the default color from the Geometry.
        /// </summary>
        private Boolean m_useDefaultColor = true;


        /// <summary>
        /// The color of the line.
        /// To use that we must set false the "useDefaultColor"
        /// </summary>
        private SharpDX.Color4 m_lineColor;


        /// <summary>
        /// The width of the line
        /// </summary>
        private float m_lineWidth = 0.5f;


        private SolidColorBrush m_lineColorBrush = null;
        private Boolean m_isLineColorBrushDirty = false;
        private PathGeometry m_sharpGeometry = null;
        private Boolean isGeometryDirty = true;


        #endregion




        #region Properties


        /// <summary>
        /// Set/Get the start point
        /// </summary>
        public FxVector2f StartPoint
        {
            get { return m_start; }
            set { m_start = value; isGeometryDirty = true; }
        }

        /// <summary>
        /// Set/Get the End point
        /// </summary>
        public FxVector2f EndPoint
        {
            get { return m_end; }
            set { m_end = value; isGeometryDirty = true; }
        }

        /// <summary>
        /// Use the default color from the Geometry
        /// </summary>
        public Boolean UseDefaultColor
        {
            get { return m_useDefaultColor; }
            set { m_useDefaultColor = value; }
        }

        /// <summary>
        /// The color of the line.
        /// To use that we must set false the "useDefaultColor"
        /// </summary>
        public SharpDX.Color4 LineColor
        {
            get { return m_lineColor; }
            set { m_lineColor = value; m_isLineColorBrushDirty = true; }
        }

        /// <summary>
        /// The width of the lines 
        /// </summary>
        public float LineWidth
        {
            get { return m_lineWidth; }
            set { m_lineWidth = value; }
        }

        #endregion




        #region Constructors
        public Rectangle(FxVector2f start, FxVector2f end)
        {
            m_start = start;
            m_end = end;
        }

        public Rectangle(float start_x, float start_y, float end_x, float end_y)
        {
            m_start = new FxVector2f(start_x, start_y);
            m_end = new FxVector2f(end_x, end_y);
        } 
        #endregion




        #region Draw

        public void Render2D(RenderTarget renderTarget, Brush brush)
        {
            #region Update Color Brush
            // if the brush is dirty renew it
            if (m_isLineColorBrushDirty && !m_useDefaultColor)
            {
                // clean the color brush
                if (m_lineColorBrush != null)
                    m_lineColorBrush.Dispose();

                // allocate a new one
                m_lineColorBrush = new SolidColorBrush(renderTarget, m_lineColor);

                // clean the flag
                m_isLineColorBrushDirty = false;
            }
            #endregion

            if(isGeometryDirty)
            {
                if (m_sharpGeometry != null)
                    m_sharpGeometry.Dispose();

                m_sharpGeometry = new PathGeometry(renderTarget.Factory);

                using(GeometrySink Geo_Sink = m_sharpGeometry.Open())
                {
                    // draw the rectangle
                    Geo_Sink.BeginFigure(m_start.GetVector2(), FigureBegin.Filled);
                    Geo_Sink.AddLine(new SharpDX.Vector2(m_start.x, m_end.y));
                    Geo_Sink.AddLine(m_end.GetVector2());
                    Geo_Sink.AddLine(new SharpDX.Vector2(m_end.x, m_start.y));
                    Geo_Sink.EndFigure(FigureEnd.Closed);
                    Geo_Sink.Close();
                }

                isGeometryDirty = false;
            }


            // check if we use other color
            if (m_useDefaultColor || m_lineColorBrush == null)
            {
                renderTarget.DrawGeometry(m_sharpGeometry, brush, m_lineWidth);
            }
            else
            {
                renderTarget.DrawGeometry(m_sharpGeometry, m_lineColorBrush, m_lineWidth);
            }

        } 


        #endregion




        #region Dispose

        public void Dispose()
        {

        }

        #endregion
    }
}
