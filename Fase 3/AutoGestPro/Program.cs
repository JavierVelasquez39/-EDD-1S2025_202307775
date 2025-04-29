using System;
using Gtk;
using AutoGestPro.Interfaz;

namespace AutoGestPro
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.Init();

            // Crear un usuario administrador de prueba - ejecutar solo la primera vez
            CrearUsuarioAdministrador();

            // Imprimir el estado del blockchain al iniciar
            Console.WriteLine($"Blockchain iniciado con {DatosCompartidos.BlockchainUsuarios.Chain.Count} bloques");
            MostrarUsuariosRegistrados(); // Esto mostrará todos los usuarios en el blockchain

            // Crear y mostrar la ventana de login
            VentanaLogin ventanaLogin = new VentanaLogin();
            ventanaLogin.Show();

            Application.Run();
        }

        private static void CrearUsuarioAdministrador()
        {
            // Buscar si ya existe un usuario con correo admin@usac.com
            bool adminExists = false;
            foreach (var block in DatosCompartidos.BlockchainUsuarios.Chain)
            {
                if (block.Index > 0 && 
                    block.Correo != null && 
                    block.Correo.Equals("admin@usac.com", StringComparison.OrdinalIgnoreCase))
                {
                    adminExists = true;
                    break;
                }
            }

            // Si no existe, crearlo
            if (!adminExists)
            {
                Console.WriteLine("Creando usuario administrador...");
                DatosCompartidos.BlockchainUsuarios.AddBlock("Admin USAC", "admin@usac.com", "admin123");
                Console.WriteLine("Usuario administrador creado con éxito");
            }
        }

        private static void MostrarUsuariosRegistrados()
        {
            Console.WriteLine("\n=== USUARIOS REGISTRADOS EN EL BLOCKCHAIN ===");
            foreach (var block in DatosCompartidos.BlockchainUsuarios.Chain)
            {
                if (block.Index > 0) // Ignorar bloque génesis
                {
                    Console.WriteLine($"- ID: {block.Index}, Correo: {block.Correo}, Nombre: {block.Usuario}");
                }
            }
            Console.WriteLine("==========================================\n");
        }
    }
}