using System.Security.Cryptography;
using System.Text;

namespace AutoGestPro.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public int Edad { get; set; }
        public string Contrasenia { get; set; }

        public Usuario(int id, string nombres, string apellidos, string correo, int edad, string contrasenia)
        {
            Id = id;
            Nombres = nombres;
            Apellidos = apellidos;
            Correo = correo;
            Edad = edad;
            Contrasenia = EncriptarContrasenia(contrasenia);
        }

        private string EncriptarContrasenia(string contrasenia)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contrasenia));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public override string ToString()
        {
            return $"{Id}: {Nombres} {Apellidos} - {Correo} (Edad: {Edad})";
        }
    }
}