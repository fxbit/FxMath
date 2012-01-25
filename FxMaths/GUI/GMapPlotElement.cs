using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxMaths.GMaps;
using SlimDX.Direct2D;
using System.Drawing;
using SlimDX;

namespace FxMaths.GUI
{
    public class GMapPlotElement : CanvasElements
    {
        #region Private variables

        /// <summary>
        /// internal mesh
        /// </summary>
        private DataStructHE mesh;


        private GeometrySink meshGeo_Sink;
        private GeometrySink meshGeo_Sink_Color;

        /// <summary>
        /// Geometry of the mesh
        /// </summary>
        private PathGeometry meshGeo;

        /// <summary>
        /// Geometry of the mesh
        /// </summary>
        private PathGeometry meshGeo_Color;

        /// <summary>
        /// Flag for any change of the geometry
        /// </summary>
        private Boolean IsGeomrtryDirty=true;

        /// <summary>
        /// set the brush style of lines
        /// </summary>
        private SolidColorBrush lineBrush;

        /// <summary>
        /// set the brush style of lines
        /// </summary>
        private SolidColorBrush lineBrush_Color;

        /// <summary>
        /// The color that the user have set
        /// </summary>
        private Color4 LineColor;

        /// <summary>
        /// The width of the line
        /// </summary>
        private float StrokeWidth;

        /// <summary>
        /// Flag to show or hide the vertex positions.
        /// </summary>
        private Boolean ShowVertex;

        #endregion

        #region construct

        public GMapPlotElement( DataStructHE data, Color4 LineColor, float StrokeWidth = 1, Boolean ShowVertex = false )
        {
            // link the mesh
            this.mesh        = data;
            this.Position    = new Vector.FxVector2f( 0 );
            this.Size        = new Vector.FxVector2f( 512, 256 );
            this.LineColor   = LineColor;
            this.ShowVertex  = ShowVertex;
            this.StrokeWidth = StrokeWidth;
        }

        #endregion

        #region Canvas Specific Functions

        public override void Render( CanvasRenderArguments args, System.Drawing.SizeF Zoom )
        {
            if ( IsGeomrtryDirty ) {
                Load( args );
            }

            // plot the mesh
            args.renderTarget.DrawGeometry( meshGeo, lineBrush, StrokeWidth );

            // plot the mesh
            args.renderTarget.DrawGeometry( meshGeo_Color, lineBrush_Color, StrokeWidth );
        }

        public override void Load( CanvasRenderArguments args )
        {
            // dispose the prev geometry
            if ( meshGeo != null )
                meshGeo.Dispose();
            if (meshGeo_Color != null)
                meshGeo_Color.Dispose();
            if (lineBrush_Color != null)
                lineBrush_Color.Dispose();
            if (lineBrush != null)
                lineBrush.Dispose();

            // init the lines brushs
            lineBrush = new SolidColorBrush(args.renderTarget, LineColor);

            // init the lines brushs
            lineBrush_Color = new SolidColorBrush(args.renderTarget, new Color4(0.98f, 0.99f, 0.09f));


            // init the geometryes
            meshGeo = new PathGeometry( args.factory );

            // fill the mesh geo
            meshGeo_Sink = meshGeo.Open();


            // init the geometryes
            meshGeo_Color = new PathGeometry(args.factory);

            // fill the mesh geo
            meshGeo_Sink_Color = meshGeo_Color.Open();

            // draw all the faces
            mesh.FaceFunctionExec(new FaceFunction(DrawFace));

            // close the geo
            meshGeo_Sink.Close();

            // free the memory for the sink
            meshGeo_Sink.Dispose();

            // close the geo
            meshGeo_Sink_Color.Close();

            // free the memory for the sink
            meshGeo_Sink_Color.Dispose();

            IsGeomrtryDirty = false;
        }

        internal override void InternalMove( Vector.FxVector2f delta )
        {
            // nothing to do here
        }

        public override void Dispose()
        {
            // clean the memmory
            lineBrush.Dispose();
            lineBrush_Color.Dispose();

            if ( meshGeo != null )
                meshGeo.Dispose();

            if (meshGeo_Color != null)
                meshGeo_Color.Dispose();
        }

        #endregion

        #region Drawing 

        PointF point= new PointF();

        private void DrawFace( Face fe )
        {
            Half_Edge he = fe.HalfEnd;

            if (false && (he.TwinEdge == null || he.NextEdge.TwinEdge == null || he.NextEdge.NextEdge.TwinEdge == null))
            {
                // start the figure
                point.X = he.StartVertex.X;
                point.Y = he.StartVertex.Y;
                meshGeo_Sink_Color.BeginFigure(point, FigureBegin.Hollow);
                he = he.NextEdge;

                // add the first edge
                point.X = he.StartVertex.X;
                point.Y = he.StartVertex.Y;
                meshGeo_Sink_Color.AddLine(point);
                he = he.NextEdge;

                // add the sec edge
                point.X = he.StartVertex.X;
                point.Y = he.StartVertex.Y;
                meshGeo_Sink_Color.AddLine(point);

                // close the face
                meshGeo_Sink_Color.EndFigure(FigureEnd.Closed);

            }
            else
            {
                // start the figure
                point.X = he.StartVertex.X;
                point.Y = he.StartVertex.Y;
                meshGeo_Sink.BeginFigure(point, FigureBegin.Hollow);
                he = he.NextEdge;

                // add the first edge
                point.X = he.StartVertex.X;
                point.Y = he.StartVertex.Y;
                meshGeo_Sink.AddLine(point);
                he = he.NextEdge;

                // add the sec edge
                point.X = he.StartVertex.X;
                point.Y = he.StartVertex.Y;
                meshGeo_Sink.AddLine(point);

                // close the face
                meshGeo_Sink.EndFigure(FigureEnd.Closed);
            }

            if ( ShowVertex ) {
                DrawVertex( he.StartVertex );
                DrawVertex( he.NextEdge.StartVertex );
                DrawVertex( he.NextEdge.NextEdge.StartVertex );
            }

        }

        private void DrawVertex( IVertex<float> vert )
        {
            point.X = vert.X - 10;
            point.Y = vert.Y - 10;
            meshGeo_Sink.BeginFigure( point, FigureBegin.Hollow );

            point.X = vert.X + 10;
            point.Y = vert.Y - 10;
            meshGeo_Sink.AddLine( point );

            point.X = vert.X + 10;
            point.Y = vert.Y + 10;
            meshGeo_Sink.AddLine( point );

            point.X = vert.X - 10;
            point.Y = vert.Y + 10;
            meshGeo_Sink.AddLine( point );

            meshGeo_Sink.EndFigure( FigureEnd.Closed );
        }

        #endregion

        #region Update/Refresh

        /// <summary>
        /// Update the internal Geometry base on mesh
        /// </summary>
        public void Update(Boolean ReDraw = true)
        {
            IsGeomrtryDirty = true;

            if (ReDraw)
                Parent.ReDraw();
        }

        #endregion
    }
}
