using System;
using System.Security.Cryptography;
using System.Text;
using AutoGestPro.Modelos;

namespace AutoGestPro.Estructuras
{
    public class Block
    {
        public int Index { get; set; }
        public string Timestamp { get; set; }
        public Usuario Data { get; set; }
        public int Nonce { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }

        public Block(int index, Usuario data, string previousHash)
        {
            Index = index;
            Timestamp = DateTime.Now.ToString("dd-MM-yy::HH:mm:ss");
            Data = data;
            Nonce = 0;
            PreviousHash = previousHash;
            Hash = GenerateHash();
        }

        public string GenerateHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = $"{Index}{Timestamp}{Data}{Nonce}{PreviousHash}";
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public void MineBlock(int difficulty)
        {
            string prefix = new string('0', difficulty);
            while (!Hash.StartsWith(prefix))
            {
                Nonce++;
                Hash = GenerateHash();
            }
        }

        public override string ToString()
        {
            return $"Index: {Index}, Timestamp: {Timestamp}, Hash: {Hash}, PreviousHash: {PreviousHash}, Nonce: {Nonce}";
        }
    }
}