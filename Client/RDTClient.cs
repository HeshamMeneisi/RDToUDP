using Common;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public abstract class RDTClient
    {
        const short DGSZ = 512;
        const string WD = @"D:\WorkDir\Client\";

        public static Action<string, MType> Message;
        public Action Finished;
        private byte[] sb = new byte[Helper.CDGSZ];
        private string ip, fname;
        private int port;
        private UdpClient uc, fudp;
        private bool retrieving, connected, metareceived;
        protected bool finished, cancelled;
        private int cport;
        protected int filesize;
        private TimedTask rttask;
        protected byte[] buffer = new byte[Helper.DSZ];
        protected FileStream fstr;

        protected UdpClient InSock
        {
            get
            {
                return fudp;
            }
        }

        protected bool Connected
        {
            get
            {
                return connected;
            }
        }

        public RDTClient(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            uc = new UdpClient();
        }

        public void Connect()
        {
            uc.Connect(ip, port);
            connected = true;
        }

        public void Retrieve(string fname, string path)
        {
            if (retrieving) throw new Exception("An operation is already in processing.");
            retrieving = true;
            this.fname = fname;
            fstr = File.Create(path);
            StartListening();
            rttask = new TimedTask(500, () => SendFileReq());
            SendFileReq();
            rttask.Start();
        }

        public void Stop()
        {
            cancelled = true;
        }

        private void SendFileReq()
        {
            byte[] dgram = new byte[DGSZ];
            byte[] fndata = Encoding.ASCII.GetBytes(fname);
            Helper.CreateDGram(ref dgram, fndata, fndata.Length);
            uc.Send(dgram, dgram.Length);
        }

        private void StartListening()
        {
            var rep = ((IPEndPoint)uc.Client.LocalEndPoint);
            fudp = new UdpClient(rep);
            finished = false;
            filesize = 0;
            ReceiveNext();
        }

        protected void ReceiveNext()
        {
            fudp.ReceiveAsync().ContinueWith((u) => OnReceived(u.Result));
        }

        protected virtual void OnMetaReceived()
        {
            // Parse metadata               
            metareceived = true;
            RDTResponse res = (RDTResponse)buffer[0];
            filesize = Helper.B2I(buffer, 1);
            SendACK(0);
            Message("Metadata received. " + (res == RDTResponse.Accepted ? "FileSize=" + filesize : res.ToString()), MType.Important);
            if (res != RDTResponse.Accepted || filesize == 0) OnFinished();
        }

        protected void OnFinished()
        {
            rttask.Stop();
            finished = true;
            fstr.Close();
            uc.Close();
            fudp.Close();
            Message?.Invoke("Finished request!", MType.Important);
            Finished?.Invoke();
        }

        void OnReceived(UdpReceiveResult res)
        {
            if (cancelled) return;
            if (Helper.MakeChoice(Helper.PLP))  // Simulate packet loss
            {
                Message?.Invoke("Simulated packet loss. ", MType.Important); goto Skip;
            }
            int seq;
            bool valid = Helper.Extract(res.Buffer, ref buffer, out seq);
            Message?.Invoke("Received PKT: " + seq + (valid ? "" : "? (Corrupt)"), valid ? MType.ExtraDetail : MType.Important);
            if (valid)
            {
                if (seq == 0)
                {
                    if (metareceived) SendACK(0);
                    else
                    {
                        // Establish connection using new socket
                        fudp.Connect(res.RemoteEndPoint);
                        rttask.Set(() => SendACK(0));
                        OnMetaReceived();
                    }
                }
                else
                {
                    if (rttask.Running) rttask.Stop();
                    OnDataReceived(valid, seq);
                }
            }
            else
            {
                if (metareceived)
                    OnDataReceived(valid, seq);
            }
            Skip:
            if (!finished)
                ReceiveNext();
        }
        protected void SendACK(int seq)
        {
            Helper.CreateCDGram(ref sb, SignalType.ACK, seq);
            fudp.Send(sb, sb.Length);
            Message?.Invoke("ACK PKT: " + seq, MType.ExtraDetail);
        }
        protected void SendNAK(int seq)
        {
            Helper.CreateCDGram(ref sb, SignalType.NAK, seq);
            fudp.Send(sb, sb.Length);
            Message?.Invoke("NAK PKT: " + seq, MType.ExtraDetail);
        }

        internal void Deliver(int seq)
        {
            int dsz = Math.Min(Helper.DSZ, filesize - seq + 1);
            fstr.Seek(seq - 1, SeekOrigin.Begin);
            fstr.Write(buffer, 0, dsz);
            Message?.Invoke("Disk Write PKT: " + seq, MType.ExtraDetail);
        }
        protected abstract void OnDataReceived(bool valid, int seq);
    }
}