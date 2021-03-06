﻿namespace FxMaths.GUI
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_propertieGrid = new System.Windows.Forms.ToolStripButton();
            this.ToolButton_GetPosition = new System.Windows.Forms.ToolStripButton();
            this.ToolButtonSetPosition = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ComboBox_elements = new System.Windows.Forms.ToolStripComboBox();
            this.Button_ViewFit = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.RenderArea = new FxMaths.GUI.ClearPanel();
            this.toolStrip_SelectedItem = new System.Windows.Forms.ToolStrip();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_propertieGrid,
            this.ToolButton_GetPosition,
            this.ToolButtonSetPosition,
            this.toolStripSeparator1,
            this.ComboBox_elements,
            this.Button_ViewFit});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(248, 31);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_propertieGrid
            // 
            this.toolStripButton_propertieGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_propertieGrid.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_propertieGrid.Image")));
            this.toolStripButton_propertieGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_propertieGrid.Name = "toolStripButton_propertieGrid";
            this.toolStripButton_propertieGrid.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton_propertieGrid.Text = "Show Properties";
            this.toolStripButton_propertieGrid.Click += new System.EventHandler(this.toolStripButton_propertieGrid_Click);
            // 
            // ToolButton_GetPosition
            // 
            this.ToolButton_GetPosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolButton_GetPosition.Image = ((System.Drawing.Image)(resources.GetObject("ToolButton_GetPosition.Image")));
            this.ToolButton_GetPosition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolButton_GetPosition.Name = "ToolButton_GetPosition";
            this.ToolButton_GetPosition.Size = new System.Drawing.Size(28, 28);
            this.ToolButton_GetPosition.Text = "GetThePosition";
            this.ToolButton_GetPosition.Click += new System.EventHandler(this.ToolButton_GetPosition_Click);
            // 
            // ToolButtonSetPosition
            // 
            this.ToolButtonSetPosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolButtonSetPosition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolButtonSetPosition.Name = "ToolButtonSetPosition";
            this.ToolButtonSetPosition.Size = new System.Drawing.Size(23, 28);
            this.ToolButtonSetPosition.Text = "SetPosition";
            this.ToolButtonSetPosition.Click += new System.EventHandler(this.ToolButtonSetPosition_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // ComboBox_elements
            // 
            this.ComboBox_elements.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_elements.DropDownWidth = 250;
            this.ComboBox_elements.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComboBox_elements.MaxDropDownItems = 12;
            this.ComboBox_elements.Name = "ComboBox_elements";
            this.ComboBox_elements.Size = new System.Drawing.Size(121, 31);
            this.ComboBox_elements.SelectedIndexChanged += new System.EventHandler(this.ComboBox_elements_SelectedIndexChanged);
            // 
            // Button_ViewFit
            // 
            this.Button_ViewFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_ViewFit.Image = global::FxMaths.Properties.Resources.view_fullscreen_6;
            this.Button_ViewFit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_ViewFit.Name = "Button_ViewFit";
            this.Button_ViewFit.Size = new System.Drawing.Size(28, 28);
            this.Button_ViewFit.Text = "Fit";
            this.Button_ViewFit.Click += new System.EventHandler(this.Button_ViewFit_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.propertyGrid1);
            this.splitContainer1.Panel1Collapsed = true;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.RenderArea);
            this.splitContainer1.Size = new System.Drawing.Size(709, 351);
            this.splitContainer1.SplitterDistance = 236;
            this.splitContainer1.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(236, 100);
            this.propertyGrid1.TabIndex = 0;
            // 
            // RenderArea
            // 
            this.RenderArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderArea.Location = new System.Drawing.Point(0, 0);
            this.RenderArea.Name = "RenderArea";
            this.RenderArea.Size = new System.Drawing.Size(709, 351);
            this.RenderArea.TabIndex = 0;
            this.RenderArea.Paint += new System.Windows.Forms.PaintEventHandler(this.RenderArea_Paint);
            this.RenderArea.Resize += new System.EventHandler(this.RenderArea_Resize);
            // 
            // toolStrip_SelectedItem
            // 
            this.toolStrip_SelectedItem.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip_SelectedItem.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip_SelectedItem.Location = new System.Drawing.Point(366, 0);
            this.toolStrip_SelectedItem.Name = "toolStrip_SelectedItem";
            this.toolStrip_SelectedItem.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip_SelectedItem.Size = new System.Drawing.Size(43, 25);
            this.toolStrip_SelectedItem.TabIndex = 2;
            this.toolStrip_SelectedItem.Text = "toolStrip2";
            this.toolStrip_SelectedItem.Visible = false;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(709, 351);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(709, 382);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip_SelectedItem);
            // 
            // Canvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "Canvas";
            this.Size = new System.Drawing.Size(709, 382);
            this.Load += new System.EventHandler(this.Canvas_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ClearPanel RenderArea;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ToolButton_GetPosition;
        private System.Windows.Forms.ToolStripButton ToolButtonSetPosition;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStripButton toolStripButton_propertieGrid;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox ComboBox_elements;
        private System.Windows.Forms.ToolStripButton Button_ViewFit;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip_SelectedItem;
    }
}
