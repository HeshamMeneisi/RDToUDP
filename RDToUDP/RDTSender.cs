using Common;
using System;
using System.Collections.Generic;
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
        private Dictionary<int, DateTime> rtable = new Dictionary<int, DateTime>();
        private double rtt = 5, dev = 0, timeout;
        const double alpha = 0.125, beta = 0.25;
        const double mintimeout = 5;
        public string Handle
        {
            get
            {
                return handle;
            }
        }

        public bool IsDone { get { return done; } }

        public double EstTimeout { get { return timeout; } }

        public RDTSender(IPEndPoint cl, string path, string handle)
        {
            this.cl = cl;
            this.path = path;
            this.handle = handle;
            timeout = Math.Max(rtt + 4 * dev, mintimeout);
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

        public void Stop()
        {
            lock (this)
            {
                rttask.Stop();
                uc.Close();
                fstr.Close();
            }
        }

        private void ReceiveNext()
        {
            uc.ReceiveAsync().ContinueWith((u) => OnReceived(u.Result));
        }

        private void OnReceived(UdpReceiveResult result)
        {
            DateTime rec = DateTime.Now;
            if (!done)
                ReceiveNext();
            lock (this)
            {
                if (Helper.MakeChoice(Helper.PLP)) // Simulate packet loss
                {
                    Message?.Invoke(handle + " Simulated packet loss. ", MType.Important);
                    return;
                }
                if (!metasent) return; // The client doesn't know the new socket, but we have to be careful
                var dgram = result.Buffer;
                SignalType sig;
                int seq;
                bool valid;
                valid = Helper.ParseCDG(result.Buffer, out sig, out seq);
                Message?.Invoke(handle + " <= " + sig + " " + seq + (valid ? "" : "? Corrupt"), valid ? MType.ExtraDetail : MType.Important);
                if (valid)
                {
                    MonitorCongestion(seq, rec);
                    rttask.SetTime(EstTimeout);
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
            }
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
            MonitorCongestion(seq);
            fstr.Seek(seq - 1, SeekOrigin.Begin);
            int read = fstr.Read(dbuffer, 0, dbuffer.Length);
            Helper.CreateDGram(ref dgbuffer, dbuffer, seq);
            uc.Send(dgbuffer, dgbuffer.Length);
            Message?.Invoke(handle + " => Sent PKT: " + seq, MType.ExtraDetail);
        }

        private void MonitorCongestion(int seq, DateTime? rec = null)
        {
            lock (this)
            {
                if (rtable.ContainsKey(seq) && rec.HasValue)
                {
                    double time = ((DateTime)rec).Subtract(rtable[seq]).TotalMilliseconds;
                    // EstimatedRTT = (1-alpha)*EstimatedRTT + alpha*SampleRTT
                    rtt = (1 - alpha) * rtt + alpha * time;
                    // DevRTT = (1-beta)*DevRTT + beta*| SampleRTT - EstimatedRTT |
                    dev = (1 - beta) * dev + beta * Math.Abs(time - rtt);
                    var lt = timeout;
                    timeout = Math.Max(rtt + 4 * dev, mintimeout);
                    if (timeout != lt)
                        Message?.Invoke(handle + " <=> Timeout = " + EstTimeout.ToString(".##"), MType.ExtraDetail);
                    rtable.Remove(seq);
                }
                else
                {
                    rtable[seq] = DateTime.Now;
                }
            }
        }

        protected void OnDone()
        {
            if (done) return;
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