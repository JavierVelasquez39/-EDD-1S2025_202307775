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
            Contrasenia = contrasenia;
        }

        public override string ToString()
        {
            return $"{Id}: {Nombres} {Apellidos} - {Correo} (Edad: {Edad})";
        }
    }
}
