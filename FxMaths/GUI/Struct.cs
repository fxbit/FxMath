using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX;
using SharpDX.Direct2D1;
using Factory = SharpDX.Direct2D1.Factory;
using Ellipse = SharpDX.Direct2D1.Ellipse;

namespace FxMaths.GUI
{
    /// <summary>
    /// struct to pass all the needed arguments for Load/Render
    /// </summary>
    public struct CanvasRenderArguments
    {
        public Factory factory;
        public SharpDX.DirectWrite.Factory WriteFactory;
        public RenderTarget renderTarget;
        public WindowRenderTarget windowsTarget;
        public BitmapRenderTarget bitmapTarget;
    }

    /// <summary>
    /// Structure for text format 
    /// </summary>
    public struct TextElementFormat
    {
        public string familyName;
        public SharpDX.DirectWrite.FontCollection fontCollection;
        public SharpDX.DirectWrite.FontWeight weight;
        public SharpDX.DirectWrite.FontStyle fontStyle;
        public SharpDX.DirectWrite.FontStretch fontStretch;
        public float fontSize;
    }
}
