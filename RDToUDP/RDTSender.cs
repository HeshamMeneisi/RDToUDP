using Common;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace RDToUDP
{
    public abstract class RDTSender
    {
        public static Action<string, MType> Message;
        public static Action<string> Done;
        private IPEndPoint cl;
        private FileStream fstr;
        private string path, handle;
        private bool metasent;
        private UdpClient uc;
        protected byte[] dgbuffer = new byte[Helper.DGSZ];
        protected byte[] dbuffer = new byte[Helper.DSZ];
        protected bool started, done;
        protected bool cansend;
        protected int filesize;
        protected TimedTask rttask;

        public string Handle
        {
            get
            {
                return handle;
            }
        }

        public RDTSender(IPEndPoint cl, string path, string handle)
        {
            this.cl = cl;
            this.path = path;
            this.handle = handle;
            uc = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
            uc.Connect(cl);
        }

        public void Start()
        {
            ReceiveNext();
            Message?.Invoke(handle + " (Started)", MType.Important);
            // No need to resend after a timeout, the client is the benefiter and thus has to resend
            SendMeta();
        }

        private void ReceiveNext()
        {
            uc.ReceiveAsync().ContinueWith((u) => OnReceived(u.Result));
        }

        private void OnReceived(UdpReceiveResult result)
        {
            if (Helper.MakeChoice(Helper.PLP)) // Simulate packet loss
            {
                Message?.Invoke(handle + " Simulated packet loss. ", MType.Important);
                goto Skip;
            }
            if (!metasent) goto Skip; // The client doesn't know the new socket, but we have to be careful
            var dgram = result.Buffer;
            SignalType sig;
            int seq;
            bool valid;
            valid = Helper.ParseCDG(result.Buffer, out sig, out seq);
            Message?.Invoke(handle + " <= " + sig + " " + seq + (valid ? "" : "? Corrupt"), valid ? MType.ExtraDetail : MType.Important);
            if (valid)
            {
                if (!started && seq == 0)
                {
                    if (cansend && filesize > 0 && sig == SignalType.ACK)
                    {
                        started = true;
                        SendNext();
                    }
                    else if (sig == SignalType.NAK)
                        SendMeta();
                    else
                        OnDone();
                }
                else if (seq > 0)
                {
                    rttask.Stop();
                    OnResReceived(valid, seq, sig);
                }
            }
            else
            {
                if (started) OnResReceived(valid, seq, sig); // Let the implemented protocol handle it
                else SendMeta();
            }
            Skip:
            if (!done)
                ReceiveNext();
        }

        public void SendMeta()
        {
            if (started) return;
            if (!File.Exists(path))
                Helper.CreateMD(ref dgbuffer, RDTResponse.FileNotFound);
            else
            {
                fstr = File.OpenRead(path);
                filesize = (int)fstr.Length;
                Helper.CreateMD(ref dgbuffer, RDTResponse.Accepted, (int)fstr.Length);
                cansend = true;
            }
            uc.Send(dgbuffer, dgbuffer.Length);
            Message?.Invoke(handle + " => Metadata Sent", MType.Important);
            metasent = true;
        }

        protected void SendPacket(int seq)
        {
            fstr.Seek(seq - 1, SeekOrigin.Begin);
            int read = fstr.Read(dbuffer, 0, dbuffer.Length);
            Helper.CreateDGram(ref dgbuffer, dbuffer, seq);
            uc.Send(dgbuffer, dgbuffer.Length);
            Message?.Invoke(handle + " => Sent PKT: " + seq, MType.ExtraDetail);
        }

        protected void OnDone()
        {
            rttask.Stop();
            done = true;
            fstr.Close();
            uc.Close();
            Message?.Invoke(handle + " Done!", MType.Important);
            Done?.Invoke(handle);
        }

        protected abstract void SendNext();

        protected abstract void OnResReceived(bool valid, int seq, SignalType sig);
    }
}