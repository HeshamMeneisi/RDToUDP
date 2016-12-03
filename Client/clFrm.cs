using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class clFrm : Form
    {
        public clFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                filepath.Text = dlg.FileName;
            }
        }

        private void rbtn_Click(object sender, EventArgs e)
        {
            rbtn.Enabled = stpanel.Enabled = false;
            RDTClient cl = new SAWClient(ip.Text, (int)pno.Value);
            cl.Finished += fin;
            cl.Connect();
            cl.Retrieve(file.Text, filepath.Text);
        }

        private void fin()
        {
            Invoke(new Action(() => rbtn.Enabled = stpanel.Enabled = true));
        }

        private void clFrm_Load(object sender, EventArgs e)
        {
            RDTClient.Message += (m, mt) => Invoke(new Action(() => smsg(m, mt)));
        }

        private void smsg(string m, MType mt)
        {
            lock (dispbox) // Avoid multiple threads intertwining text
            {
                if (mt == MType.ExtraDetail && !fd.Checked) return;
                dispbox.AppendText(m);
                dispbox.ScrollToCaret();
                int fc = dispbox.GetFirstCharIndexOfCurrentLine();
                dispbox.Select(fc, dispbox.Text.Length - fc);
                if (mt == MType.Important)
                {
                    dispbox.SelectionColor = Color.Red;
                }
                else
                    dispbox.SelectionColor = Color.Green;
                dispbox.AppendText("\r\n");
            }
        }
    }
}
