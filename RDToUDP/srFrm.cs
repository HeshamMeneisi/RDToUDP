using Common;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RDToUDP
{
    public partial class srFrm : Form
    {
        public srFrm()
        {
            InitializeComponent();
        }

        private void stbtn_Click(object sender, EventArgs e)
        {
            if (protocol.SelectedIndex > -1)
            {
                stpanel.Enabled = stbtn.Enabled = false;
                stopbtn.Enabled = true;
                dispbox.Clear();
                Helper.PLP = (double)plp.Value;
                Helper.PCP = (double)pcp.Value;
                MainServer.Start((int)pno.Value, (Method)protocol.SelectedIndex, wsz.Value);
            }
            else
                MessageBox.Show("Please select a protocol.", "Cannot Start");
        }

        private void stopbtn_Click(object sender, EventArgs e)
        {
            stopbtn.Enabled = false;
            stpanel.Enabled = stbtn.Enabled = true;
            MainServer.Stop();
        }

        private void srFrm_Load(object sender, EventArgs e)
        {
            MainServer.Message += (m, mt) => Invoke(new Action(() => smsg(m, mt)));
            RDTSender.Message += (m, mt) => Invoke(new Action(() => smsg(m, mt)));
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

        private void protocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (protocol.SelectedIndex)
            {
                case 0: wsz.Enabled = wszlabel.Enabled = false; break;
                default: wsz.Enabled = wszlabel.Enabled = true; break;
            }
        }
    }
}
