using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct2D1;
using System.Drawing;

namespace FxMaths.GUI
{
    public abstract class CanvasElements
    {

        /// <summary>
        /// The upper position of the element
        /// </summary>
        public FxMaths.Vector.FxVector2f Position;

        /// <summary>
        /// The size of the element
        /// Width : x , Height y.
        /// </summary>
        public FxMaths.Vector.FxVector2f Size;

        /// <summary>
        /// The element is selected by the user and have the focus.
        /// </summary>
        public Boolean isSelected;

        /// <summary>
        /// The parent canvas of this element
        /// </summary>
        public Canvas Parent;

        /// <summary>
        /// Render the element in the canva.
        /// </summary>
        /// <param name="renderTarget"></param>
        /// <param name="Zoom">The zoom factor for the internal state</param>
        public abstract void Render( CanvasRenderArguments renderTarget, SizeF Zoom );

        /// <summary>
        /// Load the element to the canva
        /// </summary>
        /// <param name="renderTarget"></param>
        public abstract void Load( CanvasRenderArguments renderTarget );

        /// <summary>
        /// Check if the specific points lie in the element
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public bool IsHit( FxMaths.Vector.FxVector2f  point )
        {
            if ( point.x > Position.x && point.y > Position.y ) {
                if ( point.x < Position.x + Size.x && point.y < Position.y + Size.y ) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///  Move the element 
        /// </summary>
        /// <param name="delta"></param>
        internal void Move( Vector.FxVector2f delta )
        {
            Position += delta;
        }

        /// <summary>
        /// Send the ivent of the moving internal to the element
        /// </summary>
        /// <param name="delta"></param>
        internal abstract void InternalMove( Vector.FxVector2f delta );

        /// <summary>
        /// Clean the memory
        /// </summary>
        public abstract void Dispose();


        /// <summary>
        /// Mouse click event.
        /// </summary>
        /// <param name="newLocation"></param>
        internal abstract void MouseClick(Vector.FxVector2f location);


        public event MouseClickEventHandler MouseClickEvent;
        public delegate void MouseClickEventHandler(CanvasElements m, Vector.FxVector2f location);

        //The event-invoking method that derived classes can override. 
        protected void OnMouseClickEvent(CanvasElements m, Vector.FxVector2f location)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            MouseClickEventHandler handler = MouseClickEvent;
            if(handler != null) {
                handler(m, location);
            }
        }
    }
}
