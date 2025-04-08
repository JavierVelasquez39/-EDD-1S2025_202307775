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

            // Crear y mostrar la ventana de login
            VentanaLogin ventanaLogin = new VentanaLogin();
            ventanaLogin.Show();

            Application.Run();
        }
    }
}