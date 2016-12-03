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
        static Thread mainthread = new Thread(() => StartServing());
        static int port;
        static double plp, pcp;
        static Dictionary<string, RDTSender> AClients = new Dictionary<string, RDTSender>();
        static UdpClient udpsock;
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
                var rdts = new SAWSender(cl, Path.Combine(WD, fname), handle);
                AClients.Add(handle, rdts);
                rdts.Start();
            }
        }

        public static void Start(int _port, double _plp, double _pcp)
        {
            mainthread.Start();
            port = _port;
            plp = _plp;
            pcp = _pcp;
        }

        public static void Stop()
        {
            mainthread.Abort();
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