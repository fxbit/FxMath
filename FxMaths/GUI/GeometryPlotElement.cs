using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FxMaths;
using FxMaths.Geometry;

using SlimDX.Direct2D;
using SlimDX;

namespace FxMaths.GUI
{

    public class GeometryPlotElement : CanvasElements
    {
        /// <summary>
        /// Variable that show the list of the 
        /// </summary>
        private List<IBaseGeometry> listGeometry;

        /// <summary>
        /// Flag for any change of the geometry
        /// </summary>
        private Boolean IsGeomrtryDirty=true;

        /// <summary>
        /// set the brush style of lines
        /// </summary>
        SolidColorBrush lineBrush;
        PathGeometry Geo_Axes;

        /// <summary>
        /// Set the position of the origin in the plot
        /// </summary>
        Vector.FxVector2f OriginPosition;

        public Vector.FxVector2f Origin
        {
            get { return OriginPosition; }
            set { OriginPosition = value; IsGeomrtryDirty = true; Parent.ReDraw(); }
        }


        #region Constructor

        public GeometryPlotElement()
        {
            // init the lists
            listGeometry = new List<IBaseGeometry>();

            // set the position and the size of the element
            this.Position = new Vector.FxVector2f( 0 );
            this.Size = new Vector.FxVector2f( 10, 10 );

            // set the origin position 
            OriginPosition = new Vector.FxVector2f( 0 );
        }

        #endregion



        #region Add/Remove Plots

        /// <summary>
        /// add a new base geometry to the plot
        /// </summary>
        /// <param name="geo"></param>
        /// <param name="Redraw">Set if we want to redraw all elements</param>
        public void AddGeometry(IBaseGeometry geo, Boolean Redraw = true)
        {
            // add the plot to the list
            listGeometry.Add(geo);

            // set the the geometry is dirty
            IsGeomrtryDirty = true;

            // check if the load have be called before of add
            if (Parent != null && Redraw)
                // redraw to see the result
                Parent.ReDraw();
        }

        /// <summary>
        /// Remove all the internal geometry
        /// </summary>
        public void ClearGeometry()
        {
            // remove all the geometry from the list
            listGeometry.Clear();

            // set the the geometry is dirty
            IsGeomrtryDirty = true;

            // check if the load have be called before of add
            if (Parent != null)
                // redraw to see the result
                Parent.ReDraw();
        }
        #endregion



        #region Canvas Specific Functions

        Matrix3x2 newMAtrix;

        public override void Render( CanvasRenderArguments args, System.Drawing.SizeF Zoom )
        {
            if ( IsGeomrtryDirty ) {
                Load( args );
            }

            // plot the plots
            foreach (IBaseGeometry geo in listGeometry)
            {
                newMAtrix = args.renderTarget.Transform;
                newMAtrix.M31 += OriginPosition.x * Zoom.Width;
                newMAtrix.M32 -= OriginPosition.y * Zoom.Height;
                args.renderTarget.Transform = newMAtrix;

                geo.Render2D( args.renderTarget, lineBrush );

                newMAtrix.M31 -= OriginPosition.x * Zoom.Width;
                newMAtrix.M32 += OriginPosition.y * Zoom.Height;
                args.renderTarget.Transform = newMAtrix;

            }

            // plot the axes
            args.renderTarget.DrawGeometry( Geo_Axes, lineBrush, 1 );
        }

        public override void Load( CanvasRenderArguments args )
        {
            // init the lines brushes
            if(lineBrush!=null)
                lineBrush.Dispose();
            lineBrush = new SolidColorBrush( args.renderTarget, new Color4( 0.08f, 0.40f, 0.93f ) );

            // refresh the geometries 
            RefreshGeometry( args.renderTarget );
        }

        public override void Dispose()
        {
            // clean all the subgeometry
            foreach (IBaseGeometry bg in listGeometry)
                bg.Dispose();

            // clean the memory 
            lineBrush.Dispose();
            Geo_Axes.Dispose();

        }

        #endregion



        #region Geometrys

        void RefreshGeometry( RenderTarget renderTarget )
        {
            // dispose the axes geometry
            if ( Geo_Axes != null )
                Geo_Axes.Dispose();

            // init the geometryes
            Geo_Axes = new PathGeometry( renderTarget.Factory );

            // fill the geometry struct
            using ( GeometrySink Geo_Axes_Sink = Geo_Axes.Open() ) {

                if ( OriginPosition.y > 0 && OriginPosition.y < this.Size.y ) {
                    // add x axes
                    Geo_Axes_Sink.BeginFigure( new System.Drawing.PointF( 0, this.Size.y - OriginPosition.y ), FigureBegin.Filled );
                    Geo_Axes_Sink.AddLine( new System.Drawing.PointF( this.Size.x, this.Size.y - OriginPosition.y ) );
                    Geo_Axes_Sink.EndFigure( FigureEnd.Open );
                }

                if ( OriginPosition.x > 0 && OriginPosition.x < this.Size.x ) {
                    // add y axes
                    Geo_Axes_Sink.BeginFigure( new System.Drawing.PointF( OriginPosition.x, 0 ), FigureBegin.Filled );
                    Geo_Axes_Sink.AddLine( new System.Drawing.PointF( OriginPosition.x, this.Size.y ) );
                    Geo_Axes_Sink.EndFigure( FigureEnd.Open );
                }

                // close the mesh
                Geo_Axes_Sink.Close();
            }


            IsGeomrtryDirty = false;
        }

        #endregion



        internal override void InternalMove( Vector.FxVector2f delta )
        {
            // change the position of the origin
            OriginPosition.x += delta.x;
            OriginPosition.y -= delta.y;

            // set that the geometry is dirty
            IsGeomrtryDirty = true;

            // force the element to redraw
            //Parent.ReDraw();
        }
    }
}
