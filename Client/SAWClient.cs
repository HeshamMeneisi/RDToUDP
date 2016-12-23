using Common;

namespace Client
{
    internal class SAWClient : RDTClient
    {
        private int nseq;

        public SAWClient(string ip, int port) : base(ip, port)
        {
            nseq = 1;
        }

        protected override void OnDataReceived(bool valid, int seq)
        {
            if (valid)
            {
                if (seq == nseq)
                {
                    Deliver(seq);
                    nseq += Helper.DSZ;
                }
                SendACK(seq);
                if (nseq > filesize) OnFinished();
            }
            else // Corrupted
            {
                if (seq == nseq)
                    SendNAK(seq);
                else // If duplicate, acknowledge anyway
                    SendACK(seq);
            }
        }
    }
}