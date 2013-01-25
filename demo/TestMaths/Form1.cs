using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FxMaths.Images;
using FxMaths.GUI;

namespace TestMaths
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // load the image that the user have select
                FxImages im = FxTools.FxImages_safe_constructors(new Bitmap(openFileDialog1.FileName));

                // create a new image element 
                ImageElement imE = new ImageElement(im);

                // add the element on canva
                canvas1.AddElements(imE);
            }
        }
    }
}
