﻿namespace RDToUDP
{
    partial class srFrm
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
            this.stbtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.stopbtn = new System.Windows.Forms.Button();
            this.stpanel = new System.Windows.Forms.Panel();
            this.pcp = new System.Windows.Forms.NumericUpDown();
            this.plp = new System.Windows.Forms.NumericUpDown();
            this.pno = new System.Windows.Forms.NumericUpDown();
            this.dispbox = new System.Windows.Forms.RichTextBox();
            this.fd = new System.Windows.Forms.CheckBox();
            this.stpanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pno)).BeginInit();
            this.SuspendLayout();
            // 
            // stbtn
            // 
            this.stbtn.Location = new System.Drawing.Point(207, 24);
            this.stbtn.Name = "stbtn";
            this.stbtn.Size = new System.Drawing.Size(75, 23);
            this.stbtn.TabIndex = 1;
            this.stbtn.Text = "Start";
            this.stbtn.UseVisualStyleBackColor = true;
            this.stbtn.Click += new System.EventHandler(this.stbtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "PLP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "PCP";
            // 
            // stopbtn
            // 
            this.stopbtn.Enabled = false;
            this.stopbtn.Location = new System.Drawing.Point(207, 48);
            this.stopbtn.Name = "stopbtn";
            this.stopbtn.Size = new System.Drawing.Size(75, 23);
            this.stopbtn.TabIndex = 1;
            this.stopbtn.Text = "Stop";
            this.stopbtn.UseVisualStyleBackColor = true;
            this.stopbtn.Click += new System.EventHandler(this.stopbtn_Click);
            // 
            // stpanel
            // 
            this.stpanel.Controls.Add(this.pcp);
            this.stpanel.Controls.Add(this.plp);
            this.stpanel.Controls.Add(this.pno);
            this.stpanel.Controls.Add(this.label1);
            this.stpanel.Controls.Add(this.label2);
            this.stpanel.Controls.Add(this.label3);
            this.stpanel.Location = new System.Drawing.Point(1, 2);
            this.stpanel.Name = "stpanel";
            this.stpanel.Size = new System.Drawing.Size(157, 81);
            this.stpanel.TabIndex = 4;
            // 
            // pcp
            // 
            this.pcp.DecimalPlaces = 4;
            this.pcp.Location = new System.Drawing.Point(37, 52);
            this.pcp.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pcp.Name = "pcp";
            this.pcp.Size = new System.Drawing.Size(117, 20);
            this.pcp.TabIndex = 4;
            this.pcp.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // plp
            // 
            this.plp.DecimalPlaces = 4;
            this.plp.Location = new System.Drawing.Point(37, 30);
            this.plp.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.plp.Name = "plp";
            this.plp.Size = new System.Drawing.Size(117, 20);
            this.plp.TabIndex = 4;
            this.plp.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // pno
            // 
            this.pno.Location = new System.Drawing.Point(37, 7);
            this.pno.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.pno.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pno.Name = "pno";
            this.pno.Size = new System.Drawing.Size(117, 20);
            this.pno.TabIndex = 4;
            this.pno.Value = new decimal(new int[] {
            8888,
            0,
            0,
            0});
            // 
            // dispbox
            // 
            this.dispbox.BackColor = System.Drawing.Color.Black;
            this.dispbox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dispbox.ForeColor = System.Drawing.Color.Green;
            this.dispbox.Location = new System.Drawing.Point(0, 80);
            this.dispbox.Name = "dispbox";
            this.dispbox.ReadOnly = true;
            this.dispbox.Size = new System.Drawing.Size(323, 320);
            this.dispbox.TabIndex = 5;
            this.dispbox.Text = "";
            // 
            // fd
            // 
            this.fd.AutoSize = true;
            this.fd.Location = new System.Drawing.Point(207, 6);
            this.fd.Name = "fd";
            this.fd.Size = new System.Drawing.Size(77, 17);
            this.fd.TabIndex = 6;
            this.fd.Text = "Full Debug";
            this.fd.UseVisualStyleBackColor = true;
            // 
            // srFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 400);
            this.Controls.Add(this.fd);
            this.Controls.Add(this.dispbox);
            this.Controls.Add(this.stpanel);
            this.Controls.Add(this.stopbtn);
            this.Controls.Add(this.stbtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(100, 100);
            this.Name = "srFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Server";
            this.Load += new System.EventHandler(this.srFrm_Load);
            this.stpanel.ResumeLayout(false);
            this.stpanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pno)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button stbtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button stopbtn;
        private System.Windows.Forms.Panel stpanel;
        private System.Windows.Forms.RichTextBox dispbox;
        private System.Windows.Forms.NumericUpDown pcp;
        private System.Windows.Forms.NumericUpDown plp;
        private System.Windows.Forms.NumericUpDown pno;
        private System.Windows.Forms.CheckBox fd;
    }
}

