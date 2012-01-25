using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX;
using SlimDX.Direct2D;
using Factory = SlimDX.Direct2D.Factory;
using Ellipse = SlimDX.Direct2D.Ellipse;

namespace FxMaths.GUI
{
    /// <summary>
    /// struct to pass all the needed arguments for Load/Render
    /// </summary>
    public struct CanvasRenderArguments
    {
        public Factory factory;
        public SlimDX.DirectWrite.Factory WriteFactory;
        public WindowRenderTarget renderTarget;
    }

    /// <summary>
    /// Structure for text format 
    /// </summary>
    public struct TextElementFormat
    {
        public string familyName;
        public SlimDX.DirectWrite.FontWeight weight;
        public SlimDX.DirectWrite.FontStyle fontStyle;
        public SlimDX.DirectWrite.FontStretch fontStretch;
        public float fontSize;
    }
}
