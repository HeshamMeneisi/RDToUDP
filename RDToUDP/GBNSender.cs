using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RDToUDP
{
    class GBNSender : RDTSender
    {
        const int MaxResend = 20;
        int baseseq = 1;
        int rsc = 0;
        short wsz;

        public GBNSender(IPEndPoint cl, string path, string handle, short wsz) : base(cl, path, handle)
        {
            this.wsz = wsz;
            rttask = new TimedTask(50, () => SendNext());
        }

        protected void ResendWindow()
        {
            rsc++;
            for (int i = 0; i < wsz; i++)
            {
                int seq = baseseq + i * Helper.DSZ;
                if (seq < filesize)
                    SendPacket(seq);
            }
            if (rsc >= MaxResend)
            {
                rttask.Stop();
                Message(Handle + " Gaveup on client.", MType.Important);
            }
        }

        protected override void OnResReceived(bool valid, int seq, SignalType sig)
        {
            if (valid && sig == SignalType.ACK)
            {
                if (seq >= baseseq && seq < baseseq + wsz * Helper.DSZ)
                {
                    rsc = 0;
                    baseseq = seq + Helper.DSZ;
                }
            }
            if (baseseq > filesize) // Check if file was completely transmitted     
                OnDone();
            else
                rttask.Start();
        }

        protected override void SendNext()
        {
            ResendWindow();
        }
    }
}
