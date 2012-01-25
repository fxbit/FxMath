using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX.Direct2D;
using FxMaths.GMaps;

namespace FxMaths.Geometry
{
    public class Circle : IBaseGeometry
    {
        #region Variables

        IVertex<float> m_Center;
        
        private float m_Radius;

        /// <summary>
        /// local variable for the rendering
        /// </summary>
        private Ellipse m_Ellipse;

        /// <summary>
        /// Use the default color from the Geometry
        /// </summary>
        private Boolean m_useDefaultColor = true;

        /// <summary>
        /// The color of the line.
        /// To use that we must set false the "useDefaultColor"
        /// </summary>
        private SlimDX.Color4 m_circleColor;

        private float m_lineWidth = 3;
        private SolidColorBrush m_lineColorBrush = null;
        private Boolean m_isLineColorBrushDirty = false;
        #endregion

        #region Properties

        /// <summary>
        /// The center of the circle
        /// </summary>
        public IVertex<float> Center
        {
            get { return m_Center; }
            set
            {
                m_Center = value;
                m_Ellipse.Center = new System.Drawing.PointF( m_Center.X, m_Center.Y );
            }
        }

        /// <summary>
        /// The radius of the circle
        /// </summary>
        public float Radius
        {
            get { return m_Radius; }
            set
            {
                m_Radius = value;
                m_Ellipse.RadiusX = value;
                m_Ellipse.RadiusY = value;
            }
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
        public SlimDX.Color4 LineColor
        {
            get { return m_circleColor; }
            set { m_circleColor = value; m_isLineColorBrushDirty = true; }
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

        #region Contruction

        public Circle( IVertex<float> Center, float Radius )
        {
            // set the local radius
            m_Center = Center;
            m_Radius = Radius;

            // allocate the ellipse for the render
            m_Ellipse = new Ellipse();

            // init the ellipse
            m_Ellipse.Center = new System.Drawing.PointF( m_Center.X, m_Center.Y );
            m_Ellipse.RadiusX = Radius;
            m_Ellipse.RadiusY = Radius;
        }

        #endregion

        #region Draw

        /// <summary>
        /// render the circle to specific render target of direct2D
        /// </summary>
        /// <param name="renderTarget"></param>
        public void Render2D( RenderTarget renderTarget , Brush brush)
        {
            // if the brush is dirty renew it
            if ( m_isLineColorBrushDirty ) {
                // clean the color brush
                if ( m_lineColorBrush != null )
                    m_lineColorBrush.Dispose();

                // allocate a new one
                m_lineColorBrush = new SolidColorBrush( renderTarget, m_circleColor );

                // clean the flag
                m_isLineColorBrushDirty = false;
            }

            // check if we use other color
            if ( m_useDefaultColor || m_lineColorBrush == null) {
                renderTarget.DrawEllipse( brush, m_Ellipse );
            } else {
                // draw
                renderTarget.DrawEllipse( m_lineColorBrush, m_Ellipse ,m_lineWidth);
            }
            
        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            if (m_lineColorBrush!=null)
                m_lineColorBrush.Dispose();
        }
        #endregion
    }
}
