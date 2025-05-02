using System.Security.Cryptography;
using System.Text;

namespace AutoGestPro.Estructuras
{
    public class MerkleNode
    {
        public string Hash { get; private set; }
        public MerkleNode? Left { get; private set; }
        public MerkleNode? Right { get; private set; }
        public string? Data { get; private set; } // Datos de la factura (solo para hojas)

        // Constructor para nodos hoja
        public MerkleNode(string data)
        {
            Data = data;
            Hash = ComputeHash(data);
        }

        // Constructor para nodos internos
        public MerkleNode(MerkleNode left, MerkleNode right)
        {
            Left = left;
            Right = right;
            Hash = ComputeHash(left.Hash + right.Hash);
        }

        private string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
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