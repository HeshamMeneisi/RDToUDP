using Common;

namespace Client
{
    class SRClient : RDTClient
    {
        private int expseq;
        private bool[] acklist;
        private short wsz;
        public SRClient(string ip, int port, short wsz) : base(ip, port)
        {
            expseq = 1;
            acklist = new bool[wsz];
            this.wsz = wsz;
        }
        protected override void OnDataReceived(bool valid, int seq)
        {
            if (valid)
            {
                int n = (seq - expseq) / Helper.DSZ; // seq number relative to base
                if (n >= 0 && n < wsz)
                {
                    SendACK(seq);
                    Deliver(seq); // Deliver works out-of-order, no need to buffer

                    acklist[n] = true;
                    int i, j = 0;
                    for (i = 0; i < wsz && acklist[i]; i++) ;

                    expseq += i * Helper.DSZ;

                    for (; j < wsz && i < wsz; j++)
                        acklist[j] = acklist[i++];

                    for (; j < wsz; j++)
                        acklist[j] = false;

                    if (expseq > filesize) OnFinished();
                }
                else if (n < 0 && n >= -wsz)
                {
                    SendACK(seq);
                }
            }
        }
    }
}
