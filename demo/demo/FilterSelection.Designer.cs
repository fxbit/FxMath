namespace FXMaths_Demo
{
    partial class FilterSelection
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.high_radioButton = new System.Windows.Forms.RadioButton();
            this.low_radioButton = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.LPF_Q_label = new System.Windows.Forms.Label();
            this.LPF_CO_label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LPF_Q = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.LPF_CO = new System.Windows.Forms.TrackBar();
            this.LPF_SF_text = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBox_A = new System.Windows.Forms.RichTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.richTextBox_B = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LPF_Q)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LPF_CO)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(340, 258);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.LPF_Q_label);
            this.tabPage1.Controls.Add(this.LPF_CO_label);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.LPF_Q);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.LPF_CO);
            this.tabPage1.Controls.Add(this.LPF_SF_text);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(332, 232);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Low/HighPassFilter";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.high_radioButton);
            this.groupBox1.Controls.Add(this.low_radioButton);
            this.groupBox1.Location = new System.Drawing.Point(13, 154);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(110, 72);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type";
            // 
            // high_radioButton
            // 
            this.high_radioButton.AutoSize = true;
            this.high_radioButton.Location = new System.Drawing.Point(8, 42);
            this.high_radioButton.Name = "high_radioButton";
            this.high_radioButton.Size = new System.Drawing.Size(73, 17);
            this.high_radioButton.TabIndex = 1;
            this.high_radioButton.TabStop = true;
            this.high_radioButton.Text = "High Pass";
            this.high_radioButton.UseVisualStyleBackColor = true;
            this.high_radioButton.CheckedChanged += new System.EventHandler(this.low_radioButton_CheckedChanged);
            // 
            // low_radioButton
            // 
            this.low_radioButton.AutoSize = true;
            this.low_radioButton.Checked = true;
            this.low_radioButton.Location = new System.Drawing.Point(8, 19);
            this.low_radioButton.Name = "low_radioButton";
            this.low_radioButton.Size = new System.Drawing.Size(71, 17);
            this.low_radioButton.TabIndex = 0;
            this.low_radioButton.TabStop = true;
            this.low_radioButton.Text = "Low Pass";
            this.low_radioButton.UseVisualStyleBackColor = true;
            this.low_radioButton.CheckedChanged += new System.EventHandler(this.low_radioButton_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(219, 143);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Create";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LPF_Q_label
            // 
            this.LPF_Q_label.AutoSize = true;
            this.LPF_Q_label.Location = new System.Drawing.Point(167, 124);
            this.LPF_Q_label.Name = "LPF_Q_label";
            this.LPF_Q_label.Size = new System.Drawing.Size(22, 13);
            this.LPF_Q_label.TabIndex = 7;
            this.LPF_Q_label.Text = "0.3";
            // 
            // LPF_CO_label
            // 
            this.LPF_CO_label.AutoSize = true;
            this.LPF_CO_label.Location = new System.Drawing.Point(167, 73);
            this.LPF_CO_label.Name = "LPF_CO_label";
            this.LPF_CO_label.Size = new System.Drawing.Size(37, 13);
            this.LPF_CO_label.TabIndex = 6;
            this.LPF_CO_label.Text = "15000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Q :";
            // 
            // LPF_Q
            // 
            this.LPF_Q.LargeChange = 10;
            this.LPF_Q.Location = new System.Drawing.Point(104, 92);
            this.LPF_Q.Maximum = 300;
            this.LPF_Q.Minimum = 10;
            this.LPF_Q.Name = "LPF_Q";
            this.LPF_Q.Size = new System.Drawing.Size(190, 45);
            this.LPF_Q.TabIndex = 4;
            this.LPF_Q.Value = 30;
            this.LPF_Q.Scroll += new System.EventHandler(this.LPF_Q_Scroll);
            this.LPF_Q.ValueChanged += new System.EventHandler(this.LPF_Q_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "cutoff Freq :";
            // 
            // LPF_CO
            // 
            this.LPF_CO.LargeChange = 500;
            this.LPF_CO.Location = new System.Drawing.Point(104, 41);
            this.LPF_CO.Maximum = 21300;
            this.LPF_CO.Name = "LPF_CO";
            this.LPF_CO.Size = new System.Drawing.Size(190, 45);
            this.LPF_CO.SmallChange = 100;
            this.LPF_CO.TabIndex = 2;
            this.LPF_CO.Value = 15000;
            this.LPF_CO.Scroll += new System.EventHandler(this.LPF_CO_Scroll);
            this.LPF_CO.ValueChanged += new System.EventHandler(this.LPF_CO_ValueChanged);
            // 
            // LPF_SF_text
            // 
            this.LPF_SF_text.Location = new System.Drawing.Point(104, 15);
            this.LPF_SF_text.Name = "LPF_SF_text";
            this.LPF_SF_text.Size = new System.Drawing.Size(58, 20);
            this.LPF_SF_text.TabIndex = 1;
            this.LPF_SF_text.Text = "44100";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sampling Freq :";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBox1);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(332, 232);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "FIR Filter";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(6, 6);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(318, 189);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 201);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Update/Create";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.richTextBox_B);
            this.tabPage3.Controls.Add(this.richTextBox_A);
            this.tabPage3.Controls.Add(this.button3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(332, 232);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "IIR Filter";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBox_A
            // 
            this.richTextBox_A.Location = new System.Drawing.Point(9, 19);
            this.richTextBox_A.Name = "richTextBox_A";
            this.richTextBox_A.Size = new System.Drawing.Size(318, 75);
            this.richTextBox_A.TabIndex = 3;
            this.richTextBox_A.Text = "";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(9, 202);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(101, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Update/Create";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // richTextBox_B
            // 
            this.richTextBox_B.Location = new System.Drawing.Point(9, 112);
            this.richTextBox_B.Name = "richTextBox_B";
            this.richTextBox_B.Size = new System.Drawing.Size(318, 84);
            this.richTextBox_B.TabIndex = 4;
            this.richTextBox_B.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Feedback (A):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Feedforward (B):";
            // 
            // FilterSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 258);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FilterSelection";
            this.Text = "FilterSelection";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LPF_Q)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LPF_CO)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar LPF_CO;
        private System.Windows.Forms.TextBox LPF_SF_text;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LPF_Q_label;
        private System.Windows.Forms.Label LPF_CO_label;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar LPF_Q;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton high_radioButton;
        private System.Windows.Forms.RadioButton low_radioButton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox richTextBox_B;
        private System.Windows.Forms.RichTextBox richTextBox_A;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
    }
}