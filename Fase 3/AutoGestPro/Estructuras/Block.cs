using System;
using System.Security.Cryptography;
using System.Text;

namespace AutoGestPro.Estructuras
{
    [Serializable]
    public class Block
    {
        public int Index { get; set; }
        public string Timestamp { get; set; }
        public string Usuario { get; set; } // Nombre del usuario
        public string Correo { get; set; } // Correo del usuario
        public string ContraseniaTextoPlano { get; set; } // Contraseña en texto plano
        public string ContraseniaHash { get; set; } // Contraseña hasheada
        public int Nonce { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }

        public Block() { }

        public Block(int index, string usuario, string correo, string contrasenia, string previousHash)
        {
            Index = index;
            Timestamp = DateTime.Now.ToString("dd-MM-yy::HH:mm:ss");
            Usuario = usuario;
            Correo = correo;
            ContraseniaTextoPlano = contrasenia;
            ContraseniaHash = CalcularHash(contrasenia); // Generar el hash de la contraseña
            Nonce = 0;
            PreviousHash = previousHash;
            Hash = GenerateHash();
        }

        public string GenerateHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = $"{Index}{Timestamp}{Usuario}{Correo}{ContraseniaHash}{Nonce}{PreviousHash}";
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
            Console.WriteLine($"Bloque #{Index} minado con nonce {Nonce} y hash {Hash}");
        }

        private string CalcularHash(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(texto);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToHexString(hashBytes).ToLower();
            }
        }
    }
}