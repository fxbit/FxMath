using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct2D;

namespace FxMaths.Geometry
{
    public class Triangle : IBaseGeometry
    {
        #region Variables

        private Vector.FxVector2f m_P1,m_P2,m_P3;

        private System.Drawing.PointF mp_P1,mp_P2,mp_P3;
        private Boolean isGeometryDirty;


        private Boolean fillTheTriangle=false;

        private SlimDX.Color4 fillColor;
        private SolidColorBrush fillColorBrush;
        private Boolean fillColorDirty;
        private PathGeometry TriangleGeometry;

        private float _LineWidth = 1f;

        #endregion

        #region Properties

        #region Points

        /// <summary>
        /// The Point 1 of the triangle
        /// </summary>
        public Vector.FxVector2f P1
        {
            get { return m_P1; }
            set
            {
                m_P1 = value;
                mp_P1 = new System.Drawing.PointF( value.x, value.y );
            }
        }

        /// <summary>
        /// The Point 2 of the triangle
        /// </summary>
        public Vector.FxVector2f P2
        {
            get { return m_P2; }
            set
            {
                m_P2 = value;
                mp_P2 = new System.Drawing.PointF( value.x, value.y );
            }
        }

        /// <summary>
        /// The Point 3 of the triangle
        /// </summary>
        public Vector.FxVector2f P3
        {
            get { return m_P3; }
            set
            {
                m_P3 = value;
                mp_P3 = new System.Drawing.PointF( value.x, value.y );
            }
        }

        #endregion

        #region Filling

        /// <summary>
        /// Fill the triangle.
        /// </summary>
        public Boolean FillTheTriangle
        {
            get { return fillTheTriangle; }
            set { fillTheTriangle = value; }
        }

        /// <summary>
        /// The color of the filling.
        /// </summary>
        public SlimDX.Color4 FillColor
        {
            get { return fillColor; }
            set { fillColor = value; fillColorDirty = true; }
        }

        #endregion

        public float LineWidth
        {
            get { return _LineWidth; }
            set { _LineWidth = value; }
        }

        #endregion

        #region Contruction

        public Triangle( Vector.FxVector2f p1, Vector.FxVector2f p2 , Vector.FxVector2f p3)
        {
            // set the local positions
            m_P1 = p1;
            m_P2 = p2;
            m_P3 = p3;

            mp_P1 = new System.Drawing.PointF( m_P1.x, m_P1.y );
            mp_P2 = new System.Drawing.PointF( m_P2.x, m_P2.y );
            mp_P3 = new System.Drawing.PointF( m_P3.x, m_P3.y );

            // set that the geometry is dirty
            isGeometryDirty = true;
        }

        public Triangle(GMaps.IVertex<float> vec1, GMaps.IVertex<float> vec2, GMaps.IVertex<float> vec3)
        {
            // set the local positions
            m_P1 = vec1 as Vector.FxVector2f? ?? new Vector.FxVector2f(vec1.X, vec1.Y);
            m_P2 = vec2 as Vector.FxVector2f? ?? new Vector.FxVector2f(vec2.X, vec2.Y);
            m_P3 = vec3 as Vector.FxVector2f? ?? new Vector.FxVector2f(vec3.X, vec3.Y);

            mp_P1 = new System.Drawing.PointF(m_P1.x, m_P1.y);
            mp_P2 = new System.Drawing.PointF(m_P2.x, m_P2.y);
            mp_P3 = new System.Drawing.PointF(m_P3.x, m_P3.y);

            // set that the geometry is dirty
            isGeometryDirty = true;
        }

        #endregion

        #region Draw

        /// <summary>
        /// render the circle to specific render target of direct2D
        /// </summary>
        /// <param name="renderTarget"></param>
        public void Render2D(RenderTarget renderTarget, SlimDX.Direct2D.Brush brush)
        {
            // check if the geometry is dirty
            if (isGeometryDirty)
            {
                // dispose the old geometry
                if (TriangleGeometry != null)
                {
                    TriangleGeometry.Dispose();
                }

                // create a new one
                TriangleGeometry = new PathGeometry(renderTarget.Factory);

                // fill the geometry struct
                using (GeometrySink Geo_Sink = TriangleGeometry.Open())
                {
                    // create the figure
                    Geo_Sink.BeginFigure(mp_P1, FigureBegin.Filled);
                    Geo_Sink.AddLine(mp_P2);
                    Geo_Sink.AddLine(mp_P3);
                    Geo_Sink.EndFigure(FigureEnd.Closed);
                    
                    // close the mesh
                    Geo_Sink.Close();
                }

                // set the geometry that is the final
                isGeometryDirty = false;
            }

            // draw the wireframe of the triangle
            renderTarget.DrawGeometry(TriangleGeometry, brush, _LineWidth);

            if (fillTheTriangle)
            {
                // check if we must renew the brush
                if (fillColorDirty)
                {
                    // dispose the old brush
                    if (fillColorBrush != null)
                        fillColorBrush.Dispose();

                    // create the new one 
                    fillColorBrush = new SolidColorBrush(renderTarget, fillColor);

                    // set that the color is the correct 
                    fillColorDirty = false;
                }

                // fill the triangle
                renderTarget.FillGeometry(TriangleGeometry, fillColorBrush);
            }

        }

        #endregion

        #region Dispose
        public void Dispose()
        {

            // clean memory
            if (TriangleGeometry != null)
                TriangleGeometry.Dispose();
            if (fillColorBrush != null)
                fillColorBrush.Dispose();

        }
        #endregion
    }
}
