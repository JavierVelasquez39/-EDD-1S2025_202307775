using System.Security.Cryptography;
using System.Text;

namespace AutoGestPro.Estructuras
{
    public class MerkleNode
    {
        public string Hash { get; set; }
        public MerkleNode Left { get; set; }
        public MerkleNode Right { get; set; }

        public MerkleNode(string data)
        {
            Hash = ComputeHash(data);
        }

        public MerkleNode(MerkleNode left, MerkleNode right)
        {
            Left = left;
            Right = right;
            Hash = ComputeHash(left.Hash + right.Hash);
        }

        private string ComputeHash(string data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}