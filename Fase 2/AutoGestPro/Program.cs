using Gtk;
using AutoGestPro;
using AutoGestPro.Interfaz;

class Program
{
    static void Main(string[] args)
    {
        Application.Init();

        // Crear y mostrar la ventana de inicio de sesión
        VentanaLogin ventanaLogin = new VentanaLogin();
        ventanaLogin.Show();

        Application.Run();
    }
}