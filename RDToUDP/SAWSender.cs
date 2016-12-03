using Common;
using System;
using System.Net;

namespace RDToUDP
{
    internal class SAWSender : RDTSender
    {
        const int MaxResend = 20;
        int cseq = 1;
        int rsc = 0;

        public SAWSender(IPEndPoint cl, string path, string handle) : base(cl, path, handle)
        {
            rttask = new TimedTask(50, () => Resend());
        }

        private void Resend()
        {
            rsc++;
            SendNext();
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
                if (seq == cseq)
                {
                    rsc = 0;
                    cseq += Helper.DSZ;
                }
                else goto Discard; // Else, it's a redundant ACK                
            }
            // If corrupted or NAK, we will resend last packet (no change to nseq)
            if (cseq > filesize) // Check if file was completely transmitted     
                OnDone();
            else
                SendNext();
            Discard: rttask.Start();
        }

        protected override void SendNext()
        {
            SendPacket(cseq);
        }
    }
}