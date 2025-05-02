using System;
using Gtk;
using AutoGestPro.Interfaz;
using AutoGestPro.Estructuras;

namespace AutoGestPro
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.Init();

            // Inicializar estructuras compartidas
            DatosCompartidos.BlockchainUsuarios = new Blockchain();
            DatosCompartidos.ListaVehiculos = new ListaDobleVehiculos();
            DatosCompartidos.ArbolRepuestos = new ArbolAVLRepuestos();

            // Imprimir el estado del blockchain al iniciar
            Console.WriteLine($"Blockchain iniciado con {DatosCompartidos.BlockchainUsuarios.Chain.Count} bloques");
            MostrarUsuariosRegistrados(); // Esto mostrará todos los usuarios en el blockchain

            // Crear y mostrar la ventana de login
            VentanaLogin ventanaLogin = new VentanaLogin();
            ventanaLogin.Show();

            Application.Run();
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