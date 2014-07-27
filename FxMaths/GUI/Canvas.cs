using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SharpDX;
using SharpDX.Direct2D1;
using Factory = SharpDX.Direct2D1.Factory;
using Ellipse = SharpDX.Direct2D1.Ellipse;
using Color = SharpDX.Color;
using RectangleF = SharpDX.RectangleF;
using FxMaths.Vector;
using System.Reflection;

namespace FxMaths.GUI
{
    public partial class Canvas : UserControl, IDisposable
    {

        // direct 2d specific code
        internal CanvasRenderArguments RenderVariables;

        // the brush for the origin sign
        SolidColorBrush originBrush;

        // variable that set the changed of control
        Boolean isDirty;

        // the selected figure for move
        public CanvasElements SelectedElement = null;

        // set that the selected element can be internal edit
        private Boolean SelectedElementInEditMode = false;

        // the moving figure for move
        CanvasElements MovingElement = null;

        // offset of the screen
        Vector2 _ScreenOffset;
        SizeF _Zoom;
        Boolean MovingScreen;

        public Vector2 ScreenOffset
        {
            get { return _ScreenOffset; }
            set { _ScreenOffset = value; ReDraw(); }
        }
        public SizeF Zoom
        {
            get { return _Zoom; }
            set { _Zoom = value; ReDraw(); }
        }

        // list with all elements that the user have insert
        List<CanvasElements> ElementsList;

        // event from canvas when a user click in canvas
        public delegate void CanvasMouseClickHandler(object sender, CanvasMouseClickEventArgs e);
        public event CanvasMouseClickHandler OnCanvasMouseClick;


        #region Selected/Edit Border Brush/Color

        SolidColorBrush BrushSelectedBorder;
        SolidColorBrush BrushEditBorder;

        private Color _SelectedBorderColor;
        private Color _EditBorderColor;

        /// <summary>
        /// The Border color of the selected element
        /// </summary>
        public Color SelectedBorderColor
        {
            get { return _SelectedBorderColor; }
            set
            {
                _SelectedBorderColor = value;

                if (BrushSelectedBorder != null)
                    BrushSelectedBorder.Dispose();
                BrushSelectedBorder = new SolidColorBrush( RenderVariables.renderTarget, _SelectedBorderColor );
            }
        }

        /// <summary>
        /// The Border color of the editable element
        /// </summary>
        public Color EditBorderColor
        {
            get { return _EditBorderColor; }
            set
            {
                _EditBorderColor = value;
                if (BrushEditBorder != null)
                    BrushEditBorder.Dispose();
                BrushEditBorder = new SolidColorBrush( RenderVariables.renderTarget, _EditBorderColor );
            }
        }
        #endregion




        #region Constructor

        public Canvas()
        {
            InitializeComponent();

            // start the factory
            RenderVariables.factory = new Factory(FactoryType.MultiThreaded);


            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);

            // criate the properties of the controler
            HwndRenderTargetProperties windowProperties = new HwndRenderTargetProperties();

            // fill the properties of the controller
            windowProperties.Hwnd = RenderArea.Handle;
            windowProperties.PixelSize = new Size2(RenderArea.Size.Width, RenderArea.Size.Height);
            windowProperties.PresentOptions = PresentOptions.None;

            // create the render target 
            RenderVariables.renderTarget = new WindowRenderTarget(RenderVariables.factory,
                new RenderTargetProperties(new PixelFormat(SharpDX.DXGI.Format.Unknown, AlphaMode.Premultiplied)),
                windowProperties);

            // create the write factory
            RenderVariables.WriteFactory = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared);

            // create write target
            // set the antialias mode
            RenderVariables.renderTarget.AntialiasMode = AntialiasMode.PerPrimitive;

            // set that the control must pe paint one time
            isDirty = false;

            // set the initial tradformation
            RenderVariables.renderTarget.Transform = Matrix3x2.Identity;

            // init the offset
            _ScreenOffset = new Vector2(10, 10);

            // init zoom
            _Zoom = new SizeF(1, 1);

            // set the origineBrush
            if (originBrush != null)
                originBrush.Dispose();
            originBrush = new SolidColorBrush(RenderVariables.renderTarget, new Color4(Color.Bisque.R, Color.Bisque.G, Color.Bisque.B, 1.0f));

            // init elements list
            ElementsList = new List<CanvasElements>();

            // init the color for the borders
            SelectedBorderColor = Color.Beige;
            EditBorderColor = Color.Brown;
            SelectedElementInEditMode = false;

            // draw the canvas
            ReDraw();

            // get mouse events from RenderArea
            RenderArea.MouseDown += new MouseEventHandler(RenderArea_MouseDown);
            RenderArea.MouseMove += new MouseEventHandler(RenderArea_MouseMove);
            RenderArea.MouseUp += new MouseEventHandler(RenderArea_MouseUp);
            RenderArea.MouseClick += new MouseEventHandler(RenderArea_MouseClick);
            RenderArea.MouseDoubleClick += new MouseEventHandler(RenderArea_MouseDoubleClick);


            // link propertie grid
            propertyGrid1.SelectedObject = this;
        }


        private void Canvas_Load(object sender, EventArgs e)
        {
            this.BeginInvoke((Action)(() =>
            {
                ComboBox_elements.Items.Clear();
                ComboBox_elements.Items.AddRange(ElementsList.ToArray());
            }));
        }



        private void ComboBox_elements_SelectedIndexChanged(object sender, EventArgs e)
        {
            CanvasElements element = (CanvasElements)ComboBox_elements.SelectedItem;
            ChangeSelectedElement(element);
        }



        #endregion




        #region Draw stuff

        public void ReDraw()
        {
            if (RenderVariables.renderTarget != null)
            {
                // set that the control must pe paint one time
                isDirty = true;

                // redraw
                Render();
            }
        }


        private void Render()
        {
            lock ( RenderVariables.renderTarget ) {
                // check if we must render
                if ( isDirty && RenderVariables.renderTarget != null && !RenderVariables.renderTarget.IsDisposed ) {
                    isDirty = false;

                    Matrix3x2 newMAtrix = Matrix3x2.Identity;
                    newMAtrix.M11 = _Zoom.Width;
                    newMAtrix.M22 = _Zoom.Height;
                    newMAtrix.M31 = _ScreenOffset.X;
                    newMAtrix.M32 = _ScreenOffset.Y;
                    RenderVariables.renderTarget.Transform = newMAtrix;

                    // start drawing
                    RenderVariables.renderTarget.BeginDraw();

                    // clean the screen
                    RenderVariables.renderTarget.Clear( new Color4( 0.3f, 0.3f, 0.3f , 1.0f) );

                    // protect the element list
                    lock (ElementsList)
                    {
                        // render all the elements
                        foreach (CanvasElements element in ElementsList)
                        {

                            newMAtrix.M31 = _ScreenOffset.X + element.Position.x * _Zoom.Width;
                            newMAtrix.M32 = _ScreenOffset.Y + element.Position.y * _Zoom.Height;
                            RenderVariables.renderTarget.Transform = newMAtrix;

                            element.Render(RenderVariables, _Zoom);

                            newMAtrix.M31 = _ScreenOffset.X;
                            newMAtrix.M32 = _ScreenOffset.Y;
                            RenderVariables.renderTarget.Transform = newMAtrix;

                            // draw selection rectacngle 
                            if (element.isSelected)
                            {
                                if (SelectedElementInEditMode)
                                {
                                    RenderVariables.renderTarget.DrawRectangle(new RectangleF(element.Position.x, element.Position.y, element.Size.x, element.Size.y), BrushEditBorder);
                                }
                                else
                                {
                                    RenderVariables.renderTarget.DrawRectangle(new RectangleF(element.Position.x, element.Position.y, element.Size.x, element.Size.y), BrushEditBorder);
                                }
                            }
                        }
                    }

                    // draw the origine
                    RenderVariables.renderTarget.DrawLine(new Vector2(-10, 0), new Vector2(10, 0), originBrush);
                    RenderVariables.renderTarget.DrawLine(new Vector2(0, -10), new Vector2(0, 10), originBrush);

                    // end drawing
                    RenderVariables.renderTarget.EndDraw();
                }
          
            }
        }

        #endregion




        #region point tranlsation in space
        private void TranslatePoint( PointF point , out FxVector2f tpoint)
        {
            tpoint.x = (point.X - _ScreenOffset.X) / _Zoom.Width;
            tpoint.y = (point.Y - _ScreenOffset.Y) / _Zoom.Height;
        }

        private FxVector2f TranslatePoint(PointF point)
        {
            FxVector2f tpoint;
            tpoint.x = (point.X - _ScreenOffset.X) / _Zoom.Width;
            tpoint.y = (point.Y - _ScreenOffset.Y) / _Zoom.Height;
            return tpoint;
        }

        private float TranslatePointX( float point )
        {
            return ( point - _ScreenOffset.X ) / _Zoom.Width;
        }

        private float TranslatePointY( float point )
        {
            return ( point - _ScreenOffset.Y ) / _Zoom.Height;
        }
        #endregion




        #region Elements Handling


        #region Add/Remove Elements
        public void AddElement(CanvasElements element, Boolean Redraw = true, Boolean addToFront = true)
        {
            // set the parent of the new element
            element.Parent = this;

            // load the element before add
            element.Load(RenderVariables);

            // protect the element list
            lock (ElementsList)
            {
                // add the element to the internal list
                if (addToFront)
                    ElementsList.Add(element);
                else
                    ElementsList.Insert(0, element);
            }

            // update element list in form
            if (this.IsHandleCreated)
                this.BeginInvoke((Action)(() =>
                {
                    ComboBox_elements.Items.Clear();
                    ComboBox_elements.Items.AddRange(ElementsList.ToArray());
                }));


            // refresh the image
            if (Redraw)
                ReDraw();
        }

        public void RemoveElement(CanvasElements element, Boolean Redraw = true)
        {
            // protect the element list
            lock (ElementsList)
            {
                // remove the element from the internal list
                ElementsList.Remove(element);
            }

            // update element list in form
            if (this.IsHandleCreated)
                this.BeginInvoke((Action)(() =>
                {
                    ComboBox_elements.Items.Clear();
                    ComboBox_elements.Items.AddRange(ElementsList.ToArray());
                }));

            // refresh the image
            if (Redraw)
                ReDraw();
        }
        #endregion



        #region Internal Handling

        private void ChangeSelectedElement(CanvasElements element)
        {
#if true    
            // clean the prev selection
            if (SelectedElement != null)
                SelectedElement.isSelected = false;

#else
            // protect the element list
            lock (ElementsList)
            {
                // clean all the prev selection
                foreach (CanvasElements element in ElementsList)
                    element.isSelected = false;
            }
#endif

            // update the selected element
            SelectedElement = element;

            if (element != null)
            {
                // set the flag of the seleceted element 
                SelectedElement.isSelected = true;

                // update the link to property grid
                propertyGrid1.SelectedObject = element;

                // update the toolstrip
                
                // remove the old items
                toolStrip_SelectedItem.Items.Clear();

                // add the toolstrip items of the selectes object
                SelectedElement.FillToolStrip(toolStrip_SelectedItem);
                toolStrip_SelectedItem.Visible = true;
            }
            else
            {
                // update the link to property grid
                propertyGrid1.SelectedObject = this;

                // remove the old items
                toolStrip_SelectedItem.Visible = false;
                toolStrip_SelectedItem.Items.Clear();
            }

            // update the combobox 
            ComboBox_elements.SelectedItem = element;
        }

        private CanvasElements HitElement( Vector.FxVector2f point )
        {
            // protect the element list
            lock (ElementsList)
            {
                // search all the elements
                foreach (CanvasElements element in ElementsList)
                {
                    if (element.IsHit(point))
                        return element;
                }
            }

            // if we don't find it we return null
            return null;
        }

        #endregion


        #endregion




        #region Mouse events

        void RenderArea_MouseDoubleClick( object sender, MouseEventArgs e )
        {
            if ( e.Button == System.Windows.Forms.MouseButtons.Left ) {
                this.Focus();
                
                // translate the location to the screen and then
                // find if we have hit
                CanvasElements hitElement = HitElement(TranslatePoint(e.Location));

                // update the selected element
                ChangeSelectedElement(hitElement);

                if ( SelectedElement != null ) {
                    // because is double click then go to edit state
                    SelectedElementInEditMode = true;

                } else {
                    // because is no hit then leave to edit state
                    SelectedElementInEditMode = false;
                }
            }

            ReDraw();

            // get the translated points
            Vector.FxVector2f newLocation;
            TranslatePoint(e.Location, out newLocation);

            // send the event to the base
            base.OnMouseDoubleClick(new MouseEventArgs(e.Button, e.Clicks, (int)newLocation.X, (int)newLocation.Y, e.Delta));

        }

        Vector2 privMousePosition;
        void RenderArea_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Left) {
                this.Focus();

                // get the translated points
                Vector.FxVector2f pos;
                TranslatePoint(e.Location, out pos);

                // check that we have selected element and check that we are click inside it
                if(SelectedElement != null && SelectedElement.IsHit(pos)) {

                    // remove the position of the selected element
                    pos.Subtract(ref SelectedElement._Position);

                    // send the event to selected element
                    SelectedElement.MouseClick(pos);
                }

                

                if(this.OnCanvasMouseClick!= null)
                {
                    CanvasElements elem =  HitElement(pos);

                    // find the internal hit point
                    Vector.FxVector2f inpos = pos;
                    if (elem!=null)
                        inpos.Subtract(ref elem._Position);

                    CanvasMouseClickEventArgs args = new CanvasMouseClickEventArgs(e, elem, pos, inpos);
                    this.OnCanvasMouseClick(this, args);
                }
            }

            ReDraw();
        }

        void RenderArea_MouseUp( object sender, MouseEventArgs e )
        {
            base.OnMouseUp( e );

            // unselect the figure
            MovingElement = null;
            MovingScreen = false;
        }

        void RenderArea_MouseMove( object sender, MouseEventArgs e )
        {
            if ( MovingElement != null ) {

                // check if the element is in edit mode
                if ( SelectedElementInEditMode ) {
                    MovingElement.InternalMove( new FxMaths.Vector.FxVector2f( ( e.Location.X - privMousePosition.X ) / _Zoom.Width, ( e.Location.Y - privMousePosition.Y ) / _Zoom.Height ) );
                } else {
                    MovingElement.Move( new FxMaths.Vector.FxVector2f( ( e.Location.X - privMousePosition.X ) / _Zoom.Width, ( e.Location.Y - privMousePosition.Y ) / _Zoom.Height ) );
                }

                // set the priv position
                privMousePosition = new Vector2(e.Location.X,e.Location.Y);

                // redraw the points
                ReDraw();

            } else if ( MovingScreen ) {

                // set the new offset of the screen
                _ScreenOffset = new Vector2( _ScreenOffset.X + e.Location.X - privMousePosition.X, _ScreenOffset.Y + e.Location.Y - privMousePosition.Y );

                // set the priv position
                privMousePosition = new Vector2(e.Location.X, e.Location.Y);

                // redraw the points
                ReDraw();

            }
        }

        void RenderArea_MouseDown( object sender, MouseEventArgs e )
        {
            if ( e.Button == System.Windows.Forms.MouseButtons.Left ) {

                // translate the location to the screen and the find the moving element
                MovingElement = HitElement( TranslatePoint( e.Location ) );

                // avoid move lock element
                if (MovingElement != null && MovingElement.lockMoving == true)
                    MovingElement = null;

                // set the current position of the mouse to be able to find the delta
                privMousePosition = new Vector2(e.Location.X, e.Location.Y);

                if ( MovingElement == null ) {
                    // set that we start mouving
                    MovingScreen = true;
                }

            }
        }

        protected override void OnMouseWheel( MouseEventArgs e )
        {
            // increse the zoom
            _Zoom.Height += e.Delta * 0.0005f;
            _Zoom.Width += e.Delta * 0.0005f;

            // be sure that we are not too far
            if ( _Zoom.Height < 0.05 ) {
                _Zoom.Height = 0.05f;
                _Zoom.Width = 0.05f;
            }

            // fix the offset
            _ScreenOffset.X -= e.Delta * 0.0005f * RenderArea.Width;
            _ScreenOffset.Y -= e.Delta * 0.0005f * RenderArea.Height;

            // redraw
            ReDraw();
        }

        #endregion




        #region Name to String

        public override string ToString()
        {
            return "Test Canvas";
        }

        #endregion




        #region Dispose

        protected virtual void MyDispose( bool disposing )
        {
            if ( disposing ) {
                // get rid of managed resources
                originBrush.Dispose();
                if (BrushSelectedBorder != null)
                    BrushSelectedBorder.Dispose();
                if (BrushEditBorder != null)
                    BrushEditBorder.Dispose();
                RenderVariables.factory.Dispose();
                RenderVariables.renderTarget.Dispose();
                RenderVariables.WriteFactory.Dispose();

                // protect the element list
                lock (ElementsList)
                {
                    // dispose all the elements
                    foreach (CanvasElements elem in ElementsList)
                    {
                        elem.Dispose();
                    }

                    ElementsList.Clear();
                }
            }
            // get rid of unmanaged resources
        }

        ~Canvas()
        {
            Dispose( false );
        }


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr GetOpenClipboardWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int GetWindowText(int hwnd, StringBuilder text, int count);

        private static string getOpenClipboardWindowText()
        {
            IntPtr hwnd = GetOpenClipboardWindow();
            StringBuilder sb = new StringBuilder(501);
            GetWindowText(hwnd.ToInt32(), sb, 500);
            return sb.ToString();
            // example:
            // skype_plugin_core_proxy_window: 02490E80
        }

        private void ToolButton_GetPosition_Click(object sender, EventArgs e)
        {
            String pos = (new FxMaths.Vector.FxVector2f(_ScreenOffset.X, _ScreenOffset.Y)).ToString() + "|" +
                (new FxMaths.Vector.FxVector2f(_Zoom.Width, _Zoom.Height)).ToString();
            MessageBox.Show(pos, "Position/Zoom");

            try
            {
                Clipboard.Clear();
                Clipboard.SetText(pos);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                msg += Environment.NewLine;
                msg += Environment.NewLine;
                msg += "The problem:";
                msg += Environment.NewLine;
                msg += getOpenClipboardWindowText();
                MessageBox.Show(msg);
            }
            
        }

        private void ToolButtonSetPosition_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                String str = Clipboard.GetText();
                char []sep = {'|'};

                String[] pos_zoom = str.Split(sep, StringSplitOptions.RemoveEmptyEntries);

                if (pos_zoom.Length == 2)
                {
                    try
                    {
                        FxMaths.Vector.FxVector2f offset = FxMaths.Vector.FxVector2f.Parse(pos_zoom[0]);
                        FxMaths.Vector.FxVector2f zoom = FxMaths.Vector.FxVector2f.Parse(pos_zoom[1]);

                        _ScreenOffset.X = offset.X;
                        _ScreenOffset.Y = offset.Y;

                        _Zoom.Width = zoom.X;
                        _Zoom.Height = zoom.Y;

                        ReDraw();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Wrong Format");
                }
            }
        }



        #endregion




        #region Misc Utils

        /// <summary>
        /// Zoom out.
        /// </summary>
        public void FitView()
        {
            if (ElementsList.Count == 0)
                return;

            // find boundary of the internal elements.
            FxVector2f min = new FxVector2f(float.MaxValue);
            FxVector2f max = new FxVector2f(float.MinValue);
            foreach (CanvasElements c in ElementsList)
            {
                if (c.Position.x < min.x)
                    min.x = c.Position.x;
                if (c.Position.y < min.y)
                    min.y = c.Position.y;
                var m = c.Position.x + c.Size.x;
                if (m > max.x)
                    max.x = m;
                m = c.Position.y + c.Size.y;
                if (m > max.y)
                    max.y = m;
            }

            // find the correct zoom factor
            var delta = max - min;
            var z = RenderArea.Width / delta.x;
            z = (z > RenderArea.Height / delta.y) ? RenderArea.Height / delta.y : z;
            this._Zoom.Width = z;
            this._Zoom.Height = z;
            this._ScreenOffset.X = 0;
            this._ScreenOffset.Y = 0;
        }


        private void Button_ViewFit_Click(object sender, EventArgs e)
        {
            this.FitView();
            this.ReDraw();
        }

        #endregion





        #region Properties grid show/hide button
        private void toolStripButton_propertieGrid_Click(object sender, EventArgs e)
        {
            if (toolStripButton_propertieGrid.Checked)
            {
                splitContainer1.Panel1Collapsed = true;
                toolStripButton_propertieGrid.Checked = false;
            }
            else
            {
                splitContainer1.Panel1Collapsed = false;
                toolStripButton_propertieGrid.Checked = true;
            }
        }
        #endregion





        #region RenderArea events

        private void RenderArea_Paint(object sender, PaintEventArgs e)
        {
            ReDraw();
        }

        private void RenderArea_Resize(object sender, EventArgs e)
        {
            if (RenderVariables.renderTarget != null)
            {
                lock (RenderVariables.renderTarget)
                {
                    // resize  the buffers
                    RenderVariables.renderTarget.Resize(new Size2(RenderArea.Size.Width, RenderArea.Size.Height));

                    // set that the control must pe paint one time
                    isDirty = true;

                    // redraw
                    Refresh();
                }
            }
        } 

        #endregion





    }


    #region Event Arguments

    public class CanvasMouseClickEventArgs : EventArgs
    {
        public MouseEventArgs mouseClick;
        public CanvasElements hitElement;
        public FxVector2f hitPoint;
        public FxVector2f insidePoint;

        public CanvasMouseClickEventArgs(MouseEventArgs mouseClick, CanvasElements hitElement, FxVector2f hitPoint, FxVector2f insidePoint)
        {
            this.mouseClick = mouseClick;
            this.hitElement = hitElement;
            this.hitPoint = hitPoint;
            this.insidePoint = insidePoint;
        }
    }

    #endregion

}
