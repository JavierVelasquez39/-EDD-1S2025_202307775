using System;

namespace AutoGestPro.Modelos
{
    public class Usuario
    {
        public int ID { get; set; } // ID único del usuario
        public string Nombres { get; set; } // Nombre del usuario
        public string Apellidos { get; set; } // Apellidos del usuario
        public string Correo { get; set; } // Correo electrónico del usuario
        public int Edad { get; set; } // Edad del usuario
        public string Contrasenia { get; set; } // Contraseña del usuario
    }
}