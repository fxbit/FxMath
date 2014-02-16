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
    public class Path:IBaseGeometry
    {

        #region Variables
        /// <summary>
        /// The path...
        /// </summary>
        private List<FxVector2f> m_path;


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




        public Path(List<FxVector2f> Path)
        {
            // set the local path
            m_path = Path;


        }


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

            if (m_path.Count > 0)
            {
                // Update geometry
                if (isGeometryDirty)
                {
                    if (m_sharpGeometry != null)
                        m_sharpGeometry.Dispose();

                    m_sharpGeometry = new PathGeometry(renderTarget.Factory);

                    using (GeometrySink Geo_Sink = m_sharpGeometry.Open())
                    {
                        int count = m_path.Count;

                        // create the path
                        Geo_Sink.BeginFigure(m_path[0].GetVector2(), FigureBegin.Filled);
                        for (int i = 1; i < count; i++)
                        {
                            Geo_Sink.AddLine(m_path[i].GetVector2());
                        }
                        Geo_Sink.EndFigure(FigureEnd.Open);
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

        }

        #endregion



        #region Dispose

        public void Dispose()
        {

        } 

        #endregion
    }
}
