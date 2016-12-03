namespace Client
{
    partial class clFrm
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
            this.dispbox = new System.Windows.Forms.RichTextBox();
            this.stpanel = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.filepath = new System.Windows.Forms.TextBox();
            this.file = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ip = new System.Windows.Forms.MaskedTextBox();
            this.pno = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rbtn = new System.Windows.Forms.Button();
            this.fd = new System.Windows.Forms.CheckBox();
            this.stpanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pno)).BeginInit();
            this.SuspendLayout();
            // 
            // dispbox
            // 
            this.dispbox.BackColor = System.Drawing.Color.Black;
            this.dispbox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dispbox.Location = new System.Drawing.Point(0, 116);
            this.dispbox.Name = "dispbox";
            this.dispbox.ReadOnly = true;
            this.dispbox.Size = new System.Drawing.Size(284, 306);
            this.dispbox.TabIndex = 9;
            this.dispbox.Text = "";
            // 
            // stpanel
            // 
            this.stpanel.Controls.Add(this.button1);
            this.stpanel.Controls.Add(this.filepath);
            this.stpanel.Controls.Add(this.file);
            this.stpanel.Controls.Add(this.label4);
            this.stpanel.Controls.Add(this.label3);
            this.stpanel.Controls.Add(this.ip);
            this.stpanel.Controls.Add(this.pno);
            this.stpanel.Controls.Add(this.label2);
            this.stpanel.Controls.Add(this.label1);
            this.stpanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.stpanel.Location = new System.Drawing.Point(0, 0);
            this.stpanel.Name = "stpanel";
            this.stpanel.Size = new System.Drawing.Size(284, 81);
            this.stpanel.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(250, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(31, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // filepath
            // 
            this.filepath.Location = new System.Drawing.Point(37, 59);
            this.filepath.Name = "filepath";
            this.filepath.Size = new System.Drawing.Size(210, 20);
            this.filepath.TabIndex = 8;
            this.filepath.Text = "D:\\WorkDir\\Client\\test.jpg";
            // 
            // file
            // 
            this.file.Location = new System.Drawing.Point(37, 33);
            this.file.Name = "file";
            this.file.Size = new System.Drawing.Size(235, 20);
            this.file.TabIndex = 8;
            this.file.Text = "myimage.jpg";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Path";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "File";
            // 
            // ip
            // 
            this.ip.Location = new System.Drawing.Point(37, 4);
            this.ip.Name = "ip";
            this.ip.Size = new System.Drawing.Size(117, 20);
            this.ip.TabIndex = 6;
            this.ip.Text = "127.0.0.1";
            // 
            // pno
            // 
            this.pno.Location = new System.Drawing.Point(216, 4);
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
            this.pno.Size = new System.Drawing.Size(65, 20);
            this.pno.TabIndex = 4;
            this.pno.Value = new decimal(new int[] {
            8888,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "IP";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(184, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port";
            // 
            // rbtn
            // 
            this.rbtn.Location = new System.Drawing.Point(101, 87);
            this.rbtn.Name = "rbtn";
            this.rbtn.Size = new System.Drawing.Size(75, 23);
            this.rbtn.TabIndex = 10;
            this.rbtn.Text = "Request";
            this.rbtn.UseVisualStyleBackColor = true;
            this.rbtn.Click += new System.EventHandler(this.rbtn_Click);
            // 
            // fd
            // 
            this.fd.AutoSize = true;
            this.fd.Location = new System.Drawing.Point(6, 91);
            this.fd.Name = "fd";
            this.fd.Size = new System.Drawing.Size(77, 17);
            this.fd.TabIndex = 11;
            this.fd.Text = "Full Debug";
            this.fd.UseVisualStyleBackColor = true;
            // 
            // clFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 422);
            this.Controls.Add(this.fd);
            this.Controls.Add(this.rbtn);
            this.Controls.Add(this.dispbox);
            this.Controls.Add(this.stpanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(500, 100);
            this.Name = "clFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Client";
            this.Load += new System.EventHandler(this.clFrm_Load);
            this.stpanel.ResumeLayout(false);
            this.stpanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pno)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox dispbox;
        private System.Windows.Forms.Panel stpanel;
        private System.Windows.Forms.NumericUpDown pno;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox filepath;
        private System.Windows.Forms.TextBox file;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox ip;
        private System.Windows.Forms.Button rbtn;
        private System.Windows.Forms.CheckBox fd;
    }
}

