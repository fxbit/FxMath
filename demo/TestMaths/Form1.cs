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
using FxMaths.Vector;
using FxMaths.GUI;

namespace TestMaths
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            FxVectorF vec1 = new FxVectorF(10, 6f);
            FxVectorF vec2 = new FxVectorF(10, 4f);

            vec1 /= vec2;
            var c = vec1/vec2;
            
            Console.WriteLine(c.Size);
            Console.WriteLine(c[0]);
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
                canvas1.AddElement(imE);


                

            }
        }
    }
}
