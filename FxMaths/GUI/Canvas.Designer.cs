namespace FxMaths.GUI
{
    partial class Canvas
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

            MyDispose( disposing );

            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Canvas));
            this.RenderArea = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ToolButton_GetPosition = new System.Windows.Forms.ToolStripButton();
            this.ToolButtonSetPosition = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RenderArea
            // 
            this.RenderArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderArea.Location = new System.Drawing.Point(0, 25);
            this.RenderArea.Name = "RenderArea";
            this.RenderArea.Size = new System.Drawing.Size(341, 233);
            this.RenderArea.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolButton_GetPosition,
            this.ToolButtonSetPosition});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(341, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ToolButton_GetPosition
            // 
            this.ToolButton_GetPosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolButton_GetPosition.Image = ((System.Drawing.Image)(resources.GetObject("ToolButton_GetPosition.Image")));
            this.ToolButton_GetPosition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolButton_GetPosition.Name = "ToolButton_GetPosition";
            this.ToolButton_GetPosition.Size = new System.Drawing.Size(23, 22);
            this.ToolButton_GetPosition.Text = "GetThePosition";
            this.ToolButton_GetPosition.Click += new System.EventHandler(this.ToolButton_GetPosition_Click);
            // 
            // ToolButtonSetPosition
            // 
            this.ToolButtonSetPosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolButtonSetPosition.Image = ((System.Drawing.Image)(resources.GetObject("ToolButtonSetPosition.Image")));
            this.ToolButtonSetPosition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolButtonSetPosition.Name = "ToolButtonSetPosition";
            this.ToolButtonSetPosition.Size = new System.Drawing.Size(23, 22);
            this.ToolButtonSetPosition.Text = "SetPosition";
            this.ToolButtonSetPosition.Click += new System.EventHandler(this.ToolButtonSetPosition_Click);
            // 
            // Canvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RenderArea);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Canvas";
            this.Size = new System.Drawing.Size(341, 258);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel RenderArea;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ToolButton_GetPosition;
        private System.Windows.Forms.ToolStripButton ToolButtonSetPosition;
    }
}
