using Gtk;
using AutoGestPro.Estructuras;
using AutoGestPro.Interfaz;

class Program
{
    static void Main(string[] args)
    {
        Application.Init();

        // Inicializar estructuras de datos
        ListaSimpleUsuarios listaUsuarios = new ListaSimpleUsuarios();
        ListaDobleVehiculos listaVehiculos = new ListaDobleVehiculos();
        ArbolAVLRepuestos arbolRepuestos = new ArbolAVLRepuestos();

        // Crear y mostrar la ventana de inicio de sesión
        VentanaLogin ventanaLogin = new VentanaLogin(listaUsuarios, listaVehiculos, arbolRepuestos);
        ventanaLogin.Show();

        Application.Run();
    }
}