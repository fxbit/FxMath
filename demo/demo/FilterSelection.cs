using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FxMaths.DSP;

namespace FXMaths_Demo
{
    public partial class FilterSelection : Form
    {

        public UpdateFilterCb cb;
        public FilterSelection()
        {
            InitializeComponent();
        }

        #region Low/High Filter

        private void LPF_CO_ValueChanged(object sender, EventArgs e)
        {
            TrackBar tr = sender as TrackBar;
            LPF_CO_label.Text = tr.Value.ToString();
        }

        private void LPF_Q_ValueChanged(object sender, EventArgs e)
        {
            TrackBar tr = sender as TrackBar;
            LPF_Q_label.Text = (tr.Value/(float)tr.Maximum).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateFilter();
        }

        private void UpdateFilter()
        {
            if ( low_radioButton.Checked )
                cb( BiQuadFilter.LowPassFilter( float.Parse( LPF_SF_text.Text ), (float)LPF_CO.Value, (float)LPF_Q.Value / LPF_Q.Maximum ));
            else
                cb( BiQuadFilter.HighPassFilter( float.Parse( LPF_SF_text.Text ), (float)LPF_CO.Value, (float)LPF_Q.Value / LPF_Q.Maximum ));
        }

        private void LPF_CO_Scroll( object sender, EventArgs e )
        {
            LPF_CO_label.Text = LPF_CO.Value.ToString();
            UpdateFilter();
        }

        private void LPF_Q_Scroll( object sender, EventArgs e )
        {
            LPF_Q_label.Text = LPF_Q.Value.ToString();
            UpdateFilter();
        }

        private void low_radioButton_CheckedChanged( object sender, EventArgs e )
        {
            UpdateFilter();
        }

        #endregion

        private FxMaths.Vector.FxVectorF ParseVector(String str)
        {
            FxMaths.Vector.FxVectorF filData = null;

            // replace all the commas
            str = str.Replace('.', ',');

            // split the lines base on space and enters
            char[] par = { ' ', '\n', '\r', '\t' };
            String[] values = str.Split(par, StringSplitOptions.RemoveEmptyEntries);

            // check if we have datas
            if (values.Length > 0)
            {

                // allocate the vector for the datas
                filData = new FxMaths.Vector.FxVectorF(values.Length);

                Boolean success = true;
                // pass all the input data
                for (int i = 0; i < values.Length; i++)
                {
                    float val;

                    if ( !float.TryParse( values[i], out val ) )
                    {
                        MessageBox.Show("You have insert not valid data");
                        success = false;
                        break;
                    }

                    // set the new value to vector
                    filData[i] = val;
                }

                if (success)
                {
                    return filData;
                }
            }

            return null;
        }

        private void button2_Click( object sender, EventArgs e )
        {
            FxMaths.Vector.FxVectorF filData;

            // try to parse the input data
            string str = richTextBox1.Text;

            // get the vector 
            filData = ParseVector(str);

            if (filData!=null)
            {
                // create the generic filter
                // and call the callback for the updated filter
                cb(new FIR_GenericFilter(filData));
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FxMaths.Vector.FxVectorF A,B;

            // get the vectors
            A = ParseVector(richTextBox_A.Text);
            B = ParseVector(richTextBox_B.Text);

            // check that we have success
            if (A != null && B != null)
            {
                // create the generic filter
                // and call the callback for the updated filter
                cb(new IIR_GenericFilter(A,B));
            }
        }

    }

    public delegate void UpdateFilterCb(IFilter Fil);
}
