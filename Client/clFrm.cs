using Common;
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
        RDTClient cl;
        private void rbtn_Click(object sender, EventArgs e)
        {
            if (protocol.SelectedIndex > -1)
            {
                rbtn.Enabled = stpanel.Enabled = false;
                stopbtn.Enabled = true;
                Helper.PLP = (double)plp.Value;
                Helper.PCP = (double)pcp.Value;
                cl = null;
                switch (protocol.SelectedIndex)
                {
                    case 0: cl = new SAWClient(ip.Text, (int)pno.Value); break;
                    case 1: cl = new GBNClient(ip.Text, (int)pno.Value); break;
                    case 2: cl = new SRClient(ip.Text, (int)pno.Value, (short)wsz.Value); break;
                }
                cl.Finished += fin;
                cl.Connect();
                cl.Retrieve(file.Text, filepath.Text);
            }
            else
                MessageBox.Show("Please select a protocol.", "Cannot Start");
        }

        private void fin()
        {
            Invoke(new Action(() => { stopbtn.Enabled = false; rbtn.Enabled = stpanel.Enabled = true; }));
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

        private void stopbtn_Click(object sender, EventArgs e)
        {
            cl.Stop();
            stopbtn.Enabled = false;
            rbtn.Enabled = stpanel.Enabled = true;
        }
    }
}
