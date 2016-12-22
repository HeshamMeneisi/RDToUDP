using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RDToUDP
{
    class SRSender : RDTSender
    {
        const int MaxResend = 20;
        int baseseq = 1;
        int rsc = 0;
        short wsz;
        bool[] acklist;

        public SRSender(IPEndPoint cl, string path, string handle, short wsz) : base(cl, path, handle)
        {
            this.wsz = wsz;
            acklist = new bool[wsz];
            rttask = new TimedTask(50, () => SendNext());
        }

        protected void ResendWindow()
        {
            lock (this)
            {
                rsc++;
                for (int i = 0; i < wsz; i++)
                {
                    if (acklist[i]) continue;
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
        }

        protected override void OnResReceived(bool valid, int seq, SignalType sig)
        {
            lock (this)
            {
                if (valid && sig == SignalType.ACK)
                {
                    int n = (seq - baseseq) / Helper.DSZ;
                    if (n >= 0 && n < wsz)
                    {
                        rsc = 0;
                        acklist[n] = true;

                        int i, j = 0;
                        for (i = 0; i < wsz && acklist[i]; i++) ;

                        baseseq += i * Helper.DSZ;

                        for (; j < wsz && i < wsz; j++)
                            acklist[j] = acklist[i++];

                        for (; j < wsz; j++)
                            acklist[j] = false;

                        if (baseseq > filesize) OnDone();
                    }
                }
                if (baseseq > filesize) // Check if file was completely transmitted     
                    OnDone();
                else
                    rttask.Start();
            }
        }

        protected override void SendNext()
        {
            ResendWindow();
        }
    }
}
