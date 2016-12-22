using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RDToUDP
{
    public class MainServer
    {
        const string WD = @"D:\WorkDir\Server\";

        public static Action<string, MType> Message;
        static Thread mainthread;
        static TimedTask cleaner = new TimedTask(5000, () => Cleanup());
        static int port;
        static Dictionary<string, RDTSender> AClients = new Dictionary<string, RDTSender>();
        static UdpClient udpsock;
        static Method method;
        static short wsz;
        private static void StartServing()
        {
            udpsock = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            RDTSender.Done += onsenderdone;
            byte[] data = new byte[Helper.DSZ];
            int len = 0;
            while (true)
            {
                // Handshaking with client     
                IPEndPoint cl = new IPEndPoint(IPAddress.Any, 0);
                var dgram = udpsock.Receive(ref cl);
                var valid = Helper.Extract(dgram, ref data, out len);
                if (valid && len > 0)
                {
                    string fname = Encoding.ASCII.GetString(data, 0, len);
                    Serve(cl, fname);
                }
                else
                {
                    Message?.Invoke(cl + " <=> Corrupt request.", MType.Important);
                }
            }
        }

        private static void Serve(IPEndPoint cl, string fname)
        {
            string handle = cl + " <= " + fname;
            if (AClients.ContainsKey(handle))
            {
                AClients[handle].SendMeta();
            }
            else
            {
                RDTSender rdts = null;
                switch (method)
                {
                    case Method.SAW:
                        rdts = new SAWSender(cl, Path.Combine(WD, fname), handle);
                        break;
                    case Method.GBN:
                        rdts = new GBNSender(cl, Path.Combine(WD, fname), handle, wsz);
                        break;
                }
                if (rdts != null)
                {
                    AClients.Add(handle, rdts);
                    rdts.Start();
                }
                else
                    throw new Exception("Unidentified method: " + method);
            }
        }

        public static void Start(int _port, Method m, params object[] extra)
        {
            method = m;
            switch (m)
            {
                case Method.GBN:
                    if (!short.TryParse(extra[0].ToString(), out wsz))
                        wsz = 5;
                    break;
            }
            mainthread = new Thread(() => StartServing());
            mainthread.Start();
            port = _port;
            cleaner.Start();
        }

        public static void Stop()
        {
            mainthread.Abort();
            udpsock.Close();
            foreach (RDTSender s in AClients.Values)
                s.Stop();
            AClients.Clear();
            cleaner.Stop();
        }

        private static void Cleanup()
        {
            foreach (RDTSender s in AClients.Values)
                if (s.IsDone) { s.Stop(); AClients.Remove(s.Handle); }
        }

        private static void onsenderdone(string obj)
        {
            try
            {
                AClients.Remove(obj);
            }
            catch { }
        }
    }
}