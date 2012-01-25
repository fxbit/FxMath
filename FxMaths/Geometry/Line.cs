using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SlimDX.Direct2D;
using System.Drawing;


namespace FxMaths.Geometry
{
    /// <summary>
    /// Line Geometry 
    /// </summary>
    public class Line : IBaseGeometry
    {
        #region Variables

        GMaps.IVertex<float> m_Start;
        GMaps.IVertex<float> m_End;

        private PointF m_pStart;
        private PointF m_pEnd;
        private float m_lineWidth = 2;

        /// <summary>
        /// Use the default color from the Geometry
        /// </summary>
        private Boolean m_useDefaultColor=true;

        /// <summary>
        /// The color of the line.
        /// To use that we must set false the "useDefaultColor"
        /// </summary>
        private SlimDX.Color4 m_lineColor;

        private SolidColorBrush m_lineColorBrush = null;
        private Boolean m_isLineColorBrushDirty = false;

        #endregion

        #region Properties

        /// <summary>
        /// The Start of the Line
        /// </summary>
        public FxMaths.GMaps.IVertex<float> Start
        {
            get { return m_Start; }
            set
            {
                m_Start = value;
                m_pStart = new PointF( value.X, value.Y );
            }
        }

        /// <summary>
        /// The End of the Line
        /// </summary>
        public FxMaths.GMaps.IVertex<float> End
        {
            get { return m_End; }
            set
            {
                m_End = value;
                m_pEnd = new PointF( value.X, value.Y );
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

        #region Contruction

        public Line( Vector.FxVector2f Start, Vector.FxVector2f End )
        {
            // set the local radius
            m_Start = Start;
            m_End = End;

            m_pStart = new PointF(m_Start.X, m_Start.Y);
            m_pEnd = new PointF(m_End.X, m_End.Y);
        }

        public Line(GMaps.IVertex<float> Start, GMaps.IVertex<float> End)
        {
            // set the local radius
            m_Start = Start;
            m_End = End;

            m_pStart = new PointF(m_Start.X, m_Start.Y);
            m_pEnd = new PointF(m_End.X, m_End.Y);
        }
        
        #endregion

        #region Draw

        /// <summary>
        /// render the circle to specific render target of direct2D
        /// </summary>
        /// <param name="renderTarget"></param>
        public void Render2D( RenderTarget renderTarget, SlimDX.Direct2D.Brush brush )
        {
            // if the brush is dirty renew it
            if ( m_isLineColorBrushDirty ) {
                // clean the color brush
                if ( m_lineColorBrush != null )
                    m_lineColorBrush.Dispose();

                // allocate a new one
                m_lineColorBrush = new SolidColorBrush( renderTarget, m_lineColor );

                // clean the flag
                m_isLineColorBrushDirty = false;
            }

            if (m_useDefaultColor || m_lineColorBrush == null)
            {
                renderTarget.DrawLine(brush, m_pStart, m_pEnd, m_lineWidth);
            }
            else
            {
                // draw
                renderTarget.DrawLine(m_lineColorBrush, m_pStart, m_pEnd, m_lineWidth);
            }
        }

        #endregion

        #region Dispose
        public void Dispose(){
            
            // clean memory
            if (m_lineColorBrush != null)
                m_lineColorBrush.Dispose();
        }
        #endregion
    }
}
