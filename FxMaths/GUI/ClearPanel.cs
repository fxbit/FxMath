using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FxMaths.GUI
{
    class ClearPanel : Panel
    {
        public ClearPanel()
        {
            this.SetStyle(  ControlStyles.AllPaintingInWmPaint |
                            ControlStyles.UserPaint |
                            ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = false;
        }

#if false
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000020;
                return createParams;
            }
        }
#endif

        protected override void OnPaintBackground(PaintEventArgs e) { }

       // protected override void OnPaint(PaintEventArgs e) { }

    }
}
