namespace FXMaths_Demo
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.Signal_Canva = new FxMaths.GUI.Canvas();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button14 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button11 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.canvas_audio = new FxMaths.GUI.Canvas();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.trackBar1 ) ).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add( this.tabPage1 );
            this.tabControl1.Controls.Add( this.tabPage2 );
            this.tabControl1.Controls.Add( this.tabPage3 );
            this.tabControl1.Controls.Add( this.tabPage4 );
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point( 0, 0 );
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size( 1078, 672 );
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add( this.richTextBox1 );
            this.tabPage1.Controls.Add( this.button2 );
            this.tabPage1.Controls.Add( this.button1 );
            this.tabPage1.Location = new System.Drawing.Point( 4, 22 );
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabPage1.Size = new System.Drawing.Size( 1070, 646 );
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Matrix";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point( 89, 20 );
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size( 973, 618 );
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point( 8, 35 );
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size( 75, 23 );
            this.button2.TabIndex = 1;
            this.button2.Text = "Multiply";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point( 8, 6 );
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size( 75, 23 );
            this.button1.TabIndex = 0;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add( this.button7 );
            this.tabPage2.Controls.Add( this.button6 );
            this.tabPage2.Controls.Add( this.button5 );
            this.tabPage2.Controls.Add( this.Signal_Canva );
            this.tabPage2.Controls.Add( this.button4 );
            this.tabPage2.Controls.Add( this.button3 );
            this.tabPage2.Location = new System.Drawing.Point( 4, 22 );
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabPage2.Size = new System.Drawing.Size( 1070, 646 );
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "FFT";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point( 6, 154 );
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size( 75, 23 );
            this.button7.TabIndex = 5;
            this.button7.Text = "Zero Delay";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point( 6, 93 );
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size( 75, 23 );
            this.button6.TabIndex = 4;
            this.button6.Text = "Conv";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler( this.button6_Click );
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point( 6, 64 );
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size( 75, 23 );
            this.button5.TabIndex = 3;
            this.button5.Text = "IFFT";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler( this.button5_Click );
            // 
            // Signal_Canva
            // 
            this.Signal_Canva.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.Signal_Canva.EditBorderColor = System.Drawing.Color.Brown;
            this.Signal_Canva.Location = new System.Drawing.Point( 87, 6 );
            this.Signal_Canva.Name = "Signal_Canva";
            this.Signal_Canva.SelectedBorderColor = System.Drawing.Color.Beige;
            this.Signal_Canva.Size = new System.Drawing.Size( 975, 632 );
            this.Signal_Canva.TabIndex = 2;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point( 6, 6 );
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size( 75, 23 );
            this.button4.TabIndex = 1;
            this.button4.Text = "Init COS";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler( this.button4_Click );
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point( 6, 35 );
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size( 75, 23 );
            this.button3.TabIndex = 0;
            this.button3.Text = "FFT";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler( this.button3_Click );
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point( 4, 22 );
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabPage3.Size = new System.Drawing.Size( 1070, 646 );
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "2D";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add( this.button14 );
            this.tabPage4.Controls.Add( this.button13 );
            this.tabPage4.Controls.Add( this.button12 );
            this.tabPage4.Controls.Add( this.trackBar1 );
            this.tabPage4.Controls.Add( this.checkBox3 );
            this.tabPage4.Controls.Add( this.checkBox2 );
            this.tabPage4.Controls.Add( this.button11 );
            this.tabPage4.Controls.Add( this.checkBox1 );
            this.tabPage4.Controls.Add( this.button10 );
            this.tabPage4.Controls.Add( this.button9 );
            this.tabPage4.Controls.Add( this.button8 );
            this.tabPage4.Controls.Add( this.canvas_audio );
            this.tabPage4.Location = new System.Drawing.Point( 4, 22 );
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding( 3 );
            this.tabPage4.Size = new System.Drawing.Size( 1070, 646 );
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Audio";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point( 8, 436 );
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size( 75, 39 );
            this.button14.TabIndex = 11;
            this.button14.Text = "Save WhiteNoise";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler( this.button14_Click );
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point( 8, 391 );
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size( 75, 39 );
            this.button13.TabIndex = 10;
            this.button13.Text = "Process WhiteNoise";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler( this.button13_Click );
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point( 8, 346 );
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size( 75, 39 );
            this.button12.TabIndex = 9;
            this.button12.Text = "Load WhiteNoise";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler( this.button12_Click );
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 10;
            this.trackBar1.Location = new System.Drawing.Point( 25, 212 );
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size( 45, 104 );
            this.trackBar1.TabIndex = 8;
            this.trackBar1.TickFrequency = 10;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar1.Value = 20;
            this.trackBar1.Scroll += new System.EventHandler( this.trackBar1_Scroll );
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point( 8, 168 );
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size( 136, 17 );
            this.checkBox3.TabIndex = 7;
            this.checkBox3.Text = "Show Output Spectrum";
            this.checkBox3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler( this.checkBox3_CheckedChanged );
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point( 8, 145 );
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size( 128, 17 );
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "Show Input Spectrum";
            this.checkBox2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler( this.checkBox2_CheckedChanged );
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point( 8, 116 );
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size( 75, 23 );
            this.button11.TabIndex = 5;
            this.button11.Text = "Filter";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler( this.button11_Click );
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point( 8, 93 );
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size( 51, 17 );
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Filtrer";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler( this.checkBox1_CheckedChanged );
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point( 8, 64 );
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size( 75, 23 );
            this.button10.TabIndex = 3;
            this.button10.Text = "Stop";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler( this.button10_Click );
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point( 8, 35 );
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size( 75, 23 );
            this.button9.TabIndex = 2;
            this.button9.Text = "Play";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler( this.button9_Click );
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point( 8, 6 );
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size( 75, 23 );
            this.button8.TabIndex = 1;
            this.button8.Text = "Open File";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler( this.button8_Click );
            // 
            // canvas_audio
            // 
            this.canvas_audio.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.canvas_audio.EditBorderColor = System.Drawing.Color.Brown;
            this.canvas_audio.Location = new System.Drawing.Point( 142, 6 );
            this.canvas_audio.Name = "canvas_audio";
            this.canvas_audio.SelectedBorderColor = System.Drawing.Color.Beige;
            this.canvas_audio.Size = new System.Drawing.Size( 920, 632 );
            this.canvas_audio.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 1078, 672 );
            this.Controls.Add( this.tabControl1 );
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.Form1_FormClosing );
            this.tabControl1.ResumeLayout( false );
            this.tabPage1.ResumeLayout( false );
            this.tabPage2.ResumeLayout( false );
            this.tabPage4.ResumeLayout( false );
            this.tabPage4.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.trackBar1 ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private FxMaths.GUI.Canvas canvas1;
        private FxMaths.GUI.Canvas Signal_Canva;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private FxMaths.GUI.Canvas canvas_audio;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

