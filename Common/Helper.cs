using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    class Helper
    {
        public const short DGSZ = 512;
        public const short CDGSZ = 9;
        public const short HSZ = 16;
        public const short DSZ = DGSZ - DHSZ;
        public const short DHSZ = HSZ + 4;
        public static double PCP = 0.025;
        public static double PLP = 0.025;

        static Random ran = new Random();
        public static void CreateCDGram(ref byte[] dgram, SignalType sig, int seq)
        {
            if (dgram.Length < CDGSZ) throw new Exception("Datagram is too small.");
            I2B(seq, dgram, 4);
            byte k = (byte)sig;
            dgram[8] = k;
            long lcs = seq + k;
            int cs = seq + k;
            cs += ((lcs & 0x100000000) == 0 ? 0 : 1);
            I2B(cs, dgram, 0);
        }
        public static bool ParseCDG(byte[] dgram, out SignalType sig, out int seq)
        {
            int cs = B2I(dgram, 0);
            seq = B2I(dgram, 4);
            byte k = dgram[8];
            long lcs = seq + k;
            int ccs = seq + k;
            cs += ((lcs & 0x100000000) == 0 ? 0 : 1);
            try { sig = (SignalType)k; } catch { sig = SignalType.Unknown; return false; }
            if (MakeChoice(PCP)) return false; // Simulate corruption
            return cs == ccs;
        }
        public static void CreateDGram(ref byte[] dgram, byte[] data, int seq)
        {
            if (dgram.Length < data.Length + HSZ)
                throw new Exception("Datagram is too small.");
            I2B(seq, dgram, HSZ);
            data.CopyTo(dgram, HSZ + 4);
            for (int i = data.Length + HSZ + 4; i < dgram.Length; dgram[i++] = 0) ;
            var md = MD5.Create();
            var hash = md.ComputeHash(dgram, dgram.Length - DSZ, DSZ);
            hash.CopyTo(dgram, 0);
        }

        public static bool Extract(byte[] dgram, ref byte[] buffer, out int seq)
        {
            if (buffer == null || buffer.Length != DSZ) buffer = new byte[DSZ];
            byte[] hash = new byte[HSZ];
            Array.Copy(dgram, 0, hash, 0, HSZ);
            seq = B2I(dgram, HSZ);
            Array.Copy(dgram, HSZ + 4, buffer, 0, buffer.Length);
            var md = MD5.Create();
            md.ComputeHash(buffer);
            var chash = md.Hash;
            if (MakeChoice(PCP)) return false; // Simulate corruption
            return Compare(chash, hash);
        }

        internal static void CreateMD(ref byte[] buffer, RDTResponse resp, params int[] data)
        {
            byte[] d = new byte[data.Length * 4 + 1];
            d[0] = (byte)resp;
            for (int i = 0; i < data.Length; i++) I2B(data[i], d, i * 4 + 1);
            CreateDGram(ref buffer, d, 0);
        }

        private static bool Compare(byte[] chash, byte[] hash)
        {
            if (chash.Length != hash.Length) throw new Exception("Invalid hash comparison.");
            for (int i = 0; i < chash.Length; i++)
                if (chash[i] != hash[i]) return false;
            return true;
        }

        public static void I2B(int value, byte[] ba, int offset)
        {
            ba[offset + 3] = (byte)((value & 0xff000000) >> 24);
            ba[offset + 2] = (byte)((value & 0x00ff0000) >> 16);
            ba[offset + 1] = (byte)((value & 0x0000ff00) >> 8);
            ba[offset] = (byte)(value & 0x000000ff);
        }

        public static int B2I(byte[] dgram, int offset)
        {
            return dgram[offset] | dgram[offset + 1] << 8 | dgram[offset + 2] << 16 | dgram[offset + 3] << 24;
        }

        public static bool MakeChoice(double probability)
        {
            return ran.NextDouble() < probability;
        }
    }
}

public enum MType
{
    ExtraDetail, Important
}

public enum SignalType
{
    ACK = 1, NAK = 0, Unknown = -1
}

public enum RDTResponse
{
    Accepted = 0, Rejected = 1, FileNotFound = 2
}