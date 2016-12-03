using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            stpanel.Enabled = stbtn.Enabled = false;
            stopbtn.Enabled = true;
            dispbox.Clear();
            MainServer.Start((int)pno.Value, (double)plp.Value, (double)pcp.Value);
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
    }
}
