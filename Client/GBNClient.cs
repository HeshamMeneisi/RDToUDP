using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class GBNClient : RDTClient
    {
        private int expseq;

        public GBNClient(string ip, int port) : base(ip, port)
        {
            expseq = 1;
        }

        protected override void OnDataReceived(bool valid, int seq)
        {
            if (valid)
            {
                if (seq == expseq)
                {
                    Deliver(seq);
                    expseq += Helper.DSZ;
                    SendACK(seq);
                }
                if (expseq > filesize) OnFinished();
            }
        }
    }
}
