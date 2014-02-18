using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using FxMaths;

using SharpDX.Direct2D1;
using SharpDX;

using RectangleF = SharpDX.RectangleF;

namespace FxMaths.GUI
{
    public class TextElement : CanvasElements
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

        /// <summary>
        /// The string that we want to draw
        /// </summary>
        private String Internal_String;
        private SharpDX.DirectWrite.TextFormat DW_textFormat;
        private RectangleF textRectangle;

        /// <summary>
        /// The drawing text
        /// </summary>
        public String Text
        {
            set { Internal_String = value; }
            get { return Internal_String; }
        }

        /// <summary>
        /// The text format
        /// </summary>
        public TextElementFormat _TextFormat;

        /// <summary>
        /// The text format
        /// </summary>
        public TextElementFormat TextFormat
        {
            get { return _TextFormat; }
            set { TextFormat = value; IsGeomrtryDirty = true; }
        }

        private Color4 _FontColor;

        /// <summary>
        /// Set/Get the color of the fonts
        /// </summary>
        public Color4 FontColor
        {
            get { return _FontColor; }
            set { _FontColor = value; IsGeomrtryDirty = true; }
        }

        public TextElement( String  str )
        {
            // set the text 
            Internal_String = str;

            // init the lists
            listPlotsGeometry = new List<PlotGeometry>();

            // set the position and the size of the element
            this.Position = new Vector.FxVector2f( 0 );
            this.Size = new Vector.FxVector2f( 500, 250 );

            // init format 
            _TextFormat = new TextElementFormat();
            _TextFormat.familyName = "Calibri";
            _TextFormat.weight = SharpDX.DirectWrite.FontWeight.Black;
            _TextFormat.fontStyle = SharpDX.DirectWrite.FontStyle.Normal;
            _TextFormat.fontStretch = SharpDX.DirectWrite.FontStretch.Normal;
            _TextFormat.fontSize = 8.0f;

            // init text rectangle
            textRectangle = new SharpDX.RectangleF( 0, 0, Size.x, Size.y );

            // init the color of the fonts
            _FontColor = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
        }




        #region Canvas Specific Functions

        public override void Render( CanvasRenderArguments args, System.Drawing.SizeF Zoom )
        {
            if ( IsGeomrtryDirty ) {
                Load( args );

                IsGeomrtryDirty = false;
            }

            args.renderTarget.DrawText( Internal_String, DW_textFormat, textRectangle, lineBrush );
        }

        public override void Load( CanvasRenderArguments args )
        {
            if ( lineBrush != null && !lineBrush.IsDisposed )
                lineBrush.Dispose();

            if (DW_textFormat != null && !DW_textFormat.IsDisposed)
                DW_textFormat.Dispose();

            // init the lines brushs
            lineBrush = new SolidColorBrush( args.renderTarget, _FontColor );

            _TextFormat.fontCollection = args.WriteFactory.GetSystemFontCollection(false);
            // init the text format
            DW_textFormat = new SharpDX.DirectWrite.TextFormat( args.WriteFactory,
                                                            _TextFormat.familyName,
                                                            _TextFormat.fontCollection,
                                                            _TextFormat.weight,
                                                            _TextFormat.fontStyle,
                                                            _TextFormat.fontStretch,
                                                            _TextFormat.fontSize, 
                                                            "en-us" );

            // get the size of the string
            SharpDX.DirectWrite.TextLayout textL= new SharpDX.DirectWrite.TextLayout( args.WriteFactory, Internal_String, DW_textFormat, 1500, 1500 );
            Size = new Vector.FxVector2f(textL.GetFontSize(0) * Internal_String.Length,
                                         textL.GetFontSize(0));
            textL.Dispose();

            // init text rectangle
            textRectangle = new RectangleF( 0, 0, Size.x, Size.y );

        }

        internal override void InternalMove( Vector.FxVector2f delta )
        {
            // Do nothing
        }


        public override void Dispose()
        {
            if (lineBrush != null && !lineBrush.IsDisposed)
                lineBrush.Dispose();

            DW_textFormat.Dispose();
        }


        #endregion





        #region Mouse Events

        internal override void MouseClick(Vector.FxVector2f location)
        {
            // void
        } 

        #endregion
    }
}
