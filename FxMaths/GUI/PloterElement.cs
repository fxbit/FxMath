﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FxMaths;

using SharpDX.Direct2D1;
using SharpDX;

namespace FxMaths.GUI
{
    public enum PlotType
    {
        Stem,
        Lines,
        Bars
    }

    public enum XStepType
    {
        Range,
        ZeroToMax,
        VectorDefine
    }

    class PlotGeometry
    {
        /// <summary>
        /// The vector with the points of the plot
        /// </summary>
        public Vector.FxVectorF ScaledVectorX;
        
        /// <summary>
        /// The vector with the points of the plot
        /// </summary>
        public Vector.FxVectorF ScaledVectorY;

        /// <summary>
        /// The vector with the points of the plot
        /// </summary>
        public Vector.FxVectorF OrigVectorX;

        /// <summary>
        /// The vector with the points of the plot
        /// </summary>
        public Vector.FxVectorF OrigVectorY;

        /// <summary>
        ///  Store the geometry of the plot
        /// </summary>
        public PathGeometry Geometry;

        /// <summary>
        /// The color of the plot
        /// </summary>
        public Color4 Color;

        /// <summary>
        /// the brush for the drawing of the plot
        /// </summary>
        public SolidColorBrush Brush;

        /// <summary>
        /// The type of the plot
        /// </summary>
        public PlotType Type;

        /// <summary>
        /// Set how we set the steps of X
        /// </summary>
        public XStepType StepType;

    }

    public class PloterElement : CanvasElements
    {
        /// <summary>
        /// Variable that show the list of the 
        /// </summary>
        private List<PlotGeometry> listPlotsGeometry;

        /// <summary>
        /// Flag for any change of the geometry
        /// </summary>
        private Boolean IsGeomrtryDirty=true;

        /// <summary>
        /// set the brush style of lines
        /// </summary>
        SolidColorBrush lineBrush;
        PathGeometry Geo_Axes;

        private SharpDX.DirectWrite.TextFormat DW_textFormat;
        private TextElementFormat _TextFormat;
        private RectangleF textRectangle;


        /// <summary>
        /// Set the position of the origine in the plot
        /// </summary>
        Vector.FxVector2f OriginPosition;

        public Vector.FxVector2f Origin
        {
            get { return OriginPosition; }
            set { OriginPosition = value; FitPlots(); IsGeomrtryDirty = true; if(Parent!=null) Parent.ReDraw(); }
        }

        private Color _AxesColor = Color.NavajoWhite;
        public Color AxesColor
        {
            get { return _AxesColor; }
            set { _AxesColor = value; IsGeomrtryDirty = true; if (Parent != null) Parent.ReDraw(); }
        }

        /// <summary>
        /// The distance bitween the steps in the x axis.
        /// </summary>
        private float X_Space = 10;

        private FxMaths.Vector.FxVector2f _scale;

        /// <summary>
        /// The scale of the plot
        /// </summary>
        public FxMaths.Vector.FxVector2f Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;           
                // normalize the positions
                foreach ( PlotGeometry p in listPlotsGeometry ) {
                    p.ScaledVectorY = (FxMaths.Vector.FxVectorF)p.OrigVectorY.GetCopy();
                    p.ScaledVectorY.Multiply( _scale.y );
                }
                IsGeomrtryDirty = true; Parent.ReDraw();
            }
        }

        /// <summary>
        /// The type of plot
        /// </summary>
        PlotType _plotType;

        /// <summary>
        /// The type of the plot
        /// </summary>
        public PlotType plotType
        {
            get { return _plotType; }
            set { _plotType = value; IsGeomrtryDirty = true; Parent.ReDraw(); }
        }

        public PloterElement(Vector.FxVector<float> vec)
        {
            InitPlotter(vec as Vector.FxVectorF, PlotType.Bars, Color.OrangeRed);
        }


        public PloterElement( Vector.FxVectorF vec )
        {
            InitPlotter(vec, PlotType.Lines, Color.OrangeRed);
        }


        public PloterElement(Vector.FxVectorF vec, PlotType plotType)
        {
            InitPlotter(vec, plotType, Color.OrangeRed);
        }

        public PloterElement(Vector.FxVectorF vec, PlotType plotType, Color color)
        {
            InitPlotter(vec, plotType, color);
        }


        public PloterElement(Vector.FxVectorF vec, PlotType plotType, System.Drawing.Color color)
        {
            InitPlotter(vec, plotType, new Color(color.R, color.G, color.B, color.A));
        }

        private void InitPlotter(Vector.FxVectorF vec, PlotType plotType, Color color)
        {
            // init the lists
            listPlotsGeometry = new List<PlotGeometry>();

            // set the position and the size of the element
            this.Position = new Vector.FxVector2f(0);
            this.Size = new Vector.FxVector2f(750, 500);

            // add the vector to the list 
            PlotGeometry plot = new PlotGeometry();
            plot.OrigVectorY = vec;
            plot.Color = color.ToColor4();
            plot.Type = plotType;
            plot.StepType = XStepType.ZeroToMax;

            // add the plot to the list
            listPlotsGeometry.Add(plot);

            // set the origin position 
            {
                float max = vec.Max();
                float min = vec.Min();
                float orig_y = Size.Y / 2;
                if(max >0 && min<0)
                {
                    orig_y = Size.Y * (min / (max - min));
                }

                if(max >0 && min >= 0)
                {
                    orig_y = -min;
                }

                if(max <=0 && min <0)
                {
                    orig_y = Size.Y + max;
                }
                OriginPosition = new Vector.FxVector2f(5, orig_y);
            }

            // allocate scale
            _scale = new Vector.FxVector2f(1.0f);

            // fit the plots to the view
            FitPlots();

            // set the x_step base on the size of vec and the width
            X_Space = this.Size.x / vec.Size;


            // init format 
            _TextFormat = new TextElementFormat();
            _TextFormat.familyName = "Calibri";
            _TextFormat.weight = SharpDX.DirectWrite.FontWeight.Black;
            _TextFormat.fontStyle = SharpDX.DirectWrite.FontStyle.Normal;
            _TextFormat.fontStretch = SharpDX.DirectWrite.FontStretch.Normal;
            _TextFormat.fontSize = 8.0f;


            // init toolstrip
            InitToolStrips();
        }

        public void FitPlots()
        {
            // set the scale
            float tmpScaleY=int.MaxValue;
            _scale.y = int.MaxValue;

            // find the scale for this vector
            foreach ( PlotGeometry p in listPlotsGeometry ) {
                // check if the origine is insite of the window
                if ( OriginPosition.y < Size.y && OriginPosition.y > 0 ) {

                    float min = p.OrigVectorY.Min();
                    float max = p.OrigVectorY.Max();

                    float upDist = Size.y - OriginPosition.y;
                    float downDist = OriginPosition.y;

                    float upScale = (max == 0)? int.MaxValue : upDist / (max*1.1f);
                    float downScale = (min == 0) ? int.MaxValue : downDist / (min*1.1f);

                    if (Math.Abs(upScale) < Math.Abs(downScale))
                        tmpScaleY = Math.Abs(upScale);
                    else
                        tmpScaleY = Math.Abs(downScale);

                } else {
                    tmpScaleY = Size.y / p.OrigVectorY.Max();
                }

                if ( _scale.y > tmpScaleY )
                    _scale.y = tmpScaleY;
            }

            // normalize the positions
            foreach ( PlotGeometry p in listPlotsGeometry ) {
                p.ScaledVectorY = (FxMaths.Vector.FxVectorF)p.OrigVectorY.GetCopy();
                p.ScaledVectorY.Multiply( _scale.y );
            }

            // set the geometry dirty
            IsGeomrtryDirty = true;
        }


        public void CenterYOrigin()
        {
            float orig_y = Size.Y / 2;

            float max = float.MinValue;
            float min = float.MaxValue;

            // find the scale for this vector
            foreach (PlotGeometry p in listPlotsGeometry)
            {
                float maxP =  p.OrigVectorY.Max();
                float minP =  p.OrigVectorY.Min();
                if (maxP > max)
                    max = maxP;
                if (minP < min)
                    min = minP;
            }

            if (max > 0 && min < 0)
            {
                orig_y = -Size.Y * (min / (max - min));
            }

            if (max > 0 && min >= 0)
            {
                orig_y = -min;
            }

            if (max <= 0 && min < 0)
            {
                orig_y = Size.Y + max;
            }

            OriginPosition.y = orig_y;

            // set the geometry dirty
            IsGeomrtryDirty = true;
        }


        #region Add/Remove Plots

        public void AddPlot( Vector.FxVectorF vecX, Vector.FxVectorF vecY, PlotType type, System.Drawing.Color plotColor )
        {
            // add the vector to the list 
            PlotGeometry plot = new PlotGeometry();
            
            // add the vectors
            plot.OrigVectorX = vecX;
            plot.OrigVectorY = vecY;

            // set the color
            plot.Color = new Color4(plotColor.R, plotColor.G, plotColor.B, 1.0f);
            
            // set the plot type
            plot.Type = type;

            // because he set vecY and vecX we set the vector define
            plot.StepType = XStepType.VectorDefine;

            // add the plot to the list
            listPlotsGeometry.Add( plot );

            // fit the plots to the view
            FitPlots();

            // set the the geometry is dirty
            IsGeomrtryDirty = true;

            // check if the load have be called before of add
            if ( Parent != null )
                // redraw to see the result
                Parent.ReDraw();
        }

        public void AddPlot( Vector.FxVectorF vecY, PlotType type, System.Drawing.Color plotColor )
        {
            AddPlot(vecY, type, new Color4(plotColor.R, plotColor.G, plotColor.B, plotColor.A));
        }

        public void AddPlot(Vector.FxVectorF vecY, PlotType type, Color4 plotColor)
        {
            // add the vector to the list 
            PlotGeometry plot = new PlotGeometry();

            // add the vectors
            plot.OrigVectorY = vecY;

            // set the color
            plot.Color = plotColor;

            // set the plot type
            plot.Type = type;

            // because he set vecY and vecX we set the vector define
            plot.StepType = XStepType.ZeroToMax;

            // add the plot to the list
            listPlotsGeometry.Add(plot);

            // fit the plots to the view
            FitPlots();

            // set the the geometry is dirty
            IsGeomrtryDirty = true;

            // check if the load have be called before of add
            if (Parent != null)
                // redraw to see the result
                Parent.ReDraw();
        }

        public void RefreshPlot( Vector.FxVectorF vecY, int index )
        {
            // add the vector to the list 
            PlotGeometry plot = listPlotsGeometry[index];

            // add the vectors
            plot.OrigVectorY = vecY;

            // fit the plots to the view
            //FitPlots();

            // normalize the positions
            foreach ( PlotGeometry p in listPlotsGeometry ) {
                p.ScaledVectorY = (FxMaths.Vector.FxVectorF)p.OrigVectorY.GetCopy();
                p.ScaledVectorY.Multiply( _scale.y );
            }

            // set the x_step base on the size of vec and the width
            X_Space = this.Size.x / vecY.Size;

            // set the the geometry is dirty
            IsGeomrtryDirty = true;

            
            // check if the load have be called before of add
            if ( Parent != null ){
               // Parent.ReDraw();
            }
             
        }

        #endregion




        #region Canvas Specific Functions

        public override void Render( CanvasRenderArguments args, System.Drawing.SizeF Zoom )
        {
            float maxValue = 0;
            float minValue = 0;
            float maxOrigValue = 0;

            if ( IsGeomrtryDirty ) {
                Load( args );
            }

            float maxSize = (Size.y - OriginPosition.y > OriginPosition.y) ? Size.y - OriginPosition.y : OriginPosition.y;

            int offset = (int)((Size.y) / 10.0);
            float PositiveMaxValue = (Size.y - OriginPosition.y) / _scale.y;

            // plot the plots
            foreach (PlotGeometry geo in listPlotsGeometry)
            {
                args.renderTarget.DrawGeometry(geo.Geometry, geo.Brush, 1);
            }

            // plot the axes
            args.renderTarget.DrawGeometry(Geo_Axes, lineBrush, 1);

            // draw possitive values
            for(int i=0;i<Size.y - OriginPosition.y;i+=offset)
            {
                textRectangle.X = OriginPosition.x;
                textRectangle.Y = (Size.y - OriginPosition.y) - i  - textRectangle.Height;

                if (textRectangle.X < 0 ||
                    textRectangle.Y < 0 ||
                    textRectangle.X > Size.x ||
                    textRectangle.Y > Size.y)
                    continue;

                args.renderTarget.DrawText(((float)i / _scale.y).ToString(), DW_textFormat, textRectangle, lineBrush);
            }


            // draw Negative values
            for (int i = offset; i < OriginPosition.y; i += offset)
            {
                textRectangle.X = OriginPosition.x;
                textRectangle.Y = (Size.y - OriginPosition.y) + i - textRectangle.Height;

                if (textRectangle.X < 0 ||
                    textRectangle.Y < 0 ||
                    textRectangle.X > Size.x ||
                    textRectangle.Y > Size.y)
                    continue;

                args.renderTarget.DrawText(((float)-i / _scale.y).ToString(), DW_textFormat, textRectangle, lineBrush);
            }
        }

        public override void Load( CanvasRenderArguments args )
        {
            // dispose the old brush
            if (lineBrush != null && !lineBrush.IsDisposed)
                lineBrush.Dispose();


            if (DW_textFormat != null && !DW_textFormat.IsDisposed)
                DW_textFormat.Dispose();

            // init the lines brushs
            lineBrush = new SolidColorBrush( args.renderTarget, _AxesColor.ToColor4() );


            _TextFormat.fontCollection = args.WriteFactory.GetSystemFontCollection(false);

            // init the text format
            DW_textFormat = new SharpDX.DirectWrite.TextFormat(args.WriteFactory,
                                                            _TextFormat.familyName,
                                                            _TextFormat.fontCollection,
                                                            _TextFormat.weight,
                                                            _TextFormat.fontStyle,
                                                            _TextFormat.fontStretch,
                                                            _TextFormat.fontSize,
                                                            "en-us");

            // get the size of the string
            SharpDX.DirectWrite.TextLayout textL = new SharpDX.DirectWrite.TextLayout(args.WriteFactory, "(0,0)", DW_textFormat, 1500, 1500);
            
            // init text rectangle
            textRectangle = new RectangleF(0, 0, textL.GetFontSize(0) * 10, textL.GetFontSize(0));
            textL.Dispose();

            // refresh the geometrys
            RefreshGeometry( args.renderTarget );
        }

        public override void Dispose()
        {
            // clean the memmory
            if (lineBrush!=null)
                lineBrush.Dispose();
            if (Geo_Axes!=null)
                Geo_Axes.Dispose();

            if (DW_textFormat != null && !DW_textFormat.IsDisposed)
                DW_textFormat.Dispose();

            // clean all the plots
            foreach ( PlotGeometry p in listPlotsGeometry ) {
                p.Brush.Dispose();
                p.Geometry.Dispose();
            }
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
                    Geo_Axes_Sink.BeginFigure( new Vector2( 0, this.Size.y - OriginPosition.y ), FigureBegin.Filled );
                    Geo_Axes_Sink.AddLine( new Vector2( this.Size.x, this.Size.y - OriginPosition.y ) );
                    Geo_Axes_Sink.EndFigure( FigureEnd.Open );
                }

                if ( OriginPosition.x > 0 && OriginPosition.x < this.Size.x ) {
                    // add y axes
                    Geo_Axes_Sink.BeginFigure(new Vector2(OriginPosition.x, 0), FigureBegin.Filled);
                    Geo_Axes_Sink.AddLine(new Vector2(OriginPosition.x, this.Size.y));
                    Geo_Axes_Sink.EndFigure( FigureEnd.Open );
                }

                // close the mesh
                Geo_Axes_Sink.Close();
            }

            // dispose all the datd pefore the cleaning
            foreach ( PlotGeometry Geo in listPlotsGeometry ) {

                // update the geometry
                RefreshPlotGeometry( renderTarget, Geo);

                // create the brush for the plot
                Geo.Brush = new SolidColorBrush( renderTarget, Geo.Color );
            }
        }

        /// <summary>
        /// Update the geometry base on the input vector and type
        /// </summary>
        /// <param name="vec"></param>
        void RefreshPlotGeometry( RenderTarget renderTarget , PlotGeometry Geo)
        {
            float X_point=0;
            int X_Index=0;

            // dispose the old one
            if ( Geo.Geometry != null )
                Geo.Geometry.Dispose();

            // init the geometryes
            Geo.Geometry = new PathGeometry( renderTarget.Factory );

            // fill the geometry struct
            using ( GeometrySink Geo_Sink = Geo.Geometry.Open() ) {

                switch ( Geo.Type ) {
                    case PlotType.Lines: {
                            //////////////////////////////////////////////////////////////////////////////////////////// Plot Type Lines
                            Boolean IsFigureBegined = false;
                            Boolean IsPointDrawable = true;
                            float Y_Point,X_View_Point,Y_Prev_Point=0,X_Prev_point=0;

                            // pass all the points
                            for ( int i = 0; i < Geo.ScaledVectorY.Size; i++ ) {

                                IsPointDrawable = true;
                                X_View_Point = X_point + OriginPosition.x;

                                // check that we are insite of draw area
                                if ( X_View_Point > 0 ) {

                                    // set the y point 
                                    Y_Point = this.Size.y - Geo.ScaledVectorY[i] - OriginPosition.y;

                                    if ( i > 0 ) {
                                        Y_Prev_Point = this.Size.y - Geo.ScaledVectorY[i - 1] - OriginPosition.y;
                                        switch ( Geo.StepType ) {
                                            case XStepType.ZeroToMax:
                                                X_Prev_point = X_View_Point - X_Space;
                                                break;
                                            case XStepType.VectorDefine:
                                                X_Prev_point = Geo.ScaledVectorX[X_Index-1];
                                                break;
                                        }
                                    } else {
                                        Y_Prev_Point = this.Size.y - Geo.ScaledVectorY[0] - OriginPosition.y;
                                        switch ( Geo.StepType ) {
                                            case XStepType.ZeroToMax:
                                                X_Prev_point = X_point;
                                                break;
                                            case XStepType.VectorDefine:
                                                X_Prev_point = Geo.ScaledVectorX[0];
                                                break;
                                        }
                                    }

                                    
                                    // check the Y point if is insite of the plot area
                                    if ( Y_Point > this.Size.y ) {

                                        // check if and the prev point was outside of the draw area
                                        if ( Y_Prev_Point > this.Size.y )
                                            IsPointDrawable=false;

                                        // calc the new x value with linear interpolation
                                        X_View_Point = X_Prev_point + ( X_View_Point - X_Prev_point ) * (this.Size.y - Y_Prev_Point) / ( Y_Point - Y_Prev_Point );
                                        Y_Point = this.Size.y;
                                    }

                                    // check the Y point if is insite of the plot area
                                    if ( Y_Point <0 ) {

                                        // check if and the prev point was outside of the draw area
                                        if ( Y_Prev_Point <0)
                                            IsPointDrawable = false;

                                        // calc the new x value with linear interpolation
                                        X_View_Point = X_Prev_point + ( X_View_Point - X_Prev_point ) * ( Y_Prev_Point ) / ( Y_Prev_Point - Y_Point);
                                        Y_Point = 0;
                                    }

                                    // check if we must draw the point
                                    if ( IsPointDrawable ) {

                                        // check if this is the first point of the figure
                                        if ( IsFigureBegined ) {

                                            // add the i'st point of the plot
                                            Geo_Sink.AddLine( new Vector2( X_View_Point, Y_Point ) );

                                        } else {

                                            // add the start of the plot
                                            Geo_Sink.BeginFigure(new Vector2(X_View_Point, Y_Point), FigureBegin.Filled);

                                            // set that  we have begin the figure
                                            IsFigureBegined = true;

                                        }

                                    } else {
                                        if ( IsFigureBegined ) {
                                            // close the plot
                                            Geo_Sink.EndFigure( FigureEnd.Open );

                                            // and set it for a new figure
                                            IsFigureBegined = false;
                                        }
                                    }

                                }

                                // increese the x position
                                switch ( Geo.StepType ) {
                                    case XStepType.ZeroToMax:
                                        X_point += X_Space;
                                        break;
                                    case XStepType.VectorDefine:
                                        X_point = Geo.ScaledVectorX[X_Index];
                                        X_Index++;
                                        break;
                                }

                                // check if we are out of the view area
                                if ( X_point + OriginPosition.x > this.Size.x )
                                    break;
                            }

                            if ( IsFigureBegined ) {
                                // end the plot
                                Geo_Sink.EndFigure( FigureEnd.Open );
                            }
                        }
                        break;

                    case PlotType.Stem: {
                            //////////////////////////////////////////////////////////////////////////////////////////// Plot Type Stems

                            float Y_Start,Y_End;

                            // pass all the points
                            for ( int i = 0; i < Geo.ScaledVectorY.Size; i++ ) {

                                // check that we are insite of draw area
                                if ( X_point + OriginPosition.x > 0 ) {

                                    Y_Start = this.Size.y - OriginPosition.y;
                                    Y_End = this.Size.y - OriginPosition.y - Geo.ScaledVectorY[i];

                                    // check if we are inside  of the draw area
                                    if ( ( Y_Start > 0 && Y_Start < this.Size.y ) || ( Y_End > 0 && Y_End < this.Size.y ) ) {

                                        // correct the Start Point if we are outside of the drawing area
                                        if ( OriginPosition.y > this.Size.y )
                                            Y_Start = 0;
                                        if ( OriginPosition.y < 0 )
                                            Y_Start = this.Size.y;

                                        // correct the End Point if we are outside of the drawing area
                                        if ( OriginPosition.y > this.Size.y - Geo.ScaledVectorY[i] )
                                            Y_End = 0;
                                        if ( Y_End > this.Size.y )
                                            Y_End = this.Size.y;

                                        // add the start of the line that start from x axes
                                        Geo_Sink.BeginFigure(new Vector2(X_point + OriginPosition.x, Y_Start), FigureBegin.Filled);

                                        // add the i'st point of the plot
                                        Geo_Sink.AddLine(new Vector2(X_point + OriginPosition.x, Y_End));

                                        // end the plot
                                        Geo_Sink.EndFigure( FigureEnd.Open );
                                    }
                                }

                                // increese the x position
                                switch ( Geo.StepType ) {
                                    case XStepType.ZeroToMax:
                                        X_point += X_Space;
                                        break;
                                    case XStepType.VectorDefine:
                                        X_point = Geo.ScaledVectorX[X_Index];
                                        X_Index++;
                                        break;
                                }

                                // check if we are out of the view area
                                if ( X_point + OriginPosition.x > this.Size.x )
                                    break;
                            }
                        }
                        break;

                    case PlotType.Bars: {
                            //////////////////////////////////////////////////////////////////////////////////////////// Plot Type Bars
                            // pass all the points
                            for ( int i = 0; i < Geo.ScaledVectorY.Size; i++ ) {

                                // check that we are insite of draw area
                                if ( X_point + OriginPosition.x > 0 ) {

                                    // add the start of the line that start from x axes
                                    Geo_Sink.BeginFigure(new Vector2(X_point + OriginPosition.x, this.Size.y - OriginPosition.y), FigureBegin.Filled);

                                    // add the i'st point of the plot
                                    Geo_Sink.AddLine(new Vector2(X_point + X_Space + OriginPosition.x, this.Size.y - OriginPosition.y));
                                    Geo_Sink.AddLine(new Vector2(X_point + X_Space + OriginPosition.x, this.Size.y - OriginPosition.y - Geo.ScaledVectorY[i]));
                                    Geo_Sink.AddLine(new Vector2(X_point + OriginPosition.x, this.Size.y - OriginPosition.y - Geo.ScaledVectorY[i]));

                                    // end the plot
                                    Geo_Sink.EndFigure( FigureEnd.Closed );

                                }

                                // increese the x position
                                switch ( Geo.StepType ) {
                                    case XStepType.ZeroToMax:
                                        X_point += X_Space;
                                        break;
                                    case XStepType.VectorDefine:
                                        X_point = Geo.ScaledVectorX[X_Index];
                                        X_Index++;
                                        break;
                                }

                                // check if we are out of the view area
                                if ( X_point + OriginPosition.x > this.Size.x )
                                    break;

                            }
                        }
                        break;
                }

                

                // close the mesh
                Geo_Sink.Close();
            }

            // set that we have the latest geometry
            IsGeomrtryDirty = false;
        }

        #endregion



        #region Mouse Events

        internal override void InternalMove(Vector.FxVector2f delta)
        {
            // change the position of the origine
            OriginPosition.x += delta.x;
            OriginPosition.y -= delta.y;

            // set that the geometry is dirty
            IsGeomrtryDirty = true;

            // force the element to redraw
            //Parent.ReDraw();
        }


        internal override void MouseClick(Vector.FxVector2f location)
        {
            // void
        }

        #endregion




        #region ToolStrip


        private System.Windows.Forms.ToolStripButton toolStrip_Fit;
        private System.Windows.Forms.ToolStripButton toolStrip_CenterFit;

        private void InitToolStrips()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Canvas));

            toolStrip_Fit = new System.Windows.Forms.ToolStripButton();
            toolStrip_Fit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStrip_Fit.Image = ((System.Drawing.Image)(resources.GetObject("zoom-fit-best.Image")));
            toolStrip_Fit.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStrip_Fit.Name = "Fit the Plot";
            toolStrip_Fit.Size = new System.Drawing.Size(28, 28);
            toolStrip_Fit.Text = "Fit the Plot";
            toolStrip_Fit.Click += toolStrip_Fit_Click;


            toolStrip_CenterFit = new System.Windows.Forms.ToolStripButton();
            toolStrip_CenterFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStrip_CenterFit.Image = ((System.Drawing.Image)(resources.GetObject("zoom-fit-height.Image")));
            toolStrip_CenterFit.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStrip_CenterFit.Name = "Fit the Plot";
            toolStrip_CenterFit.Size = new System.Drawing.Size(28, 28);
            toolStrip_CenterFit.Text = "Fit the Plot";
            toolStrip_CenterFit.Click += toolStrip_CenterFit_Click;
            

        }

        void toolStrip_Fit_Click(object sender, EventArgs e)
        {
            this.FitPlots();
            this.Parent.ReDraw();
        }


        void toolStrip_CenterFit_Click(object sender, EventArgs e)
        {
            this.CenterYOrigin();
            this.FitPlots();
            this.Parent.ReDraw();
        }

        public override void FillToolStrip(System.Windows.Forms.ToolStrip toolStrip)
        {
            toolStrip.Items.Add(toolStrip_Fit);
            toolStrip.Items.Add(toolStrip_CenterFit);
        }
        #endregion
    }
}
