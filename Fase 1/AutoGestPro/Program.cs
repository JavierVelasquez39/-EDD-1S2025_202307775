using System;
using Gtk;
using AutoGestPro.GUI;
using AutoGestPro.DataStructures;

class Program
{
    [Obsolete]
    static void Main()
    {
        Application.Init();
        
        try
        {
            // Crear instancias de las listas y la cola
            var listaUsuarios = ListaUsuarios.Instancia;
            var listaVehiculos = ListaVehiculos.Instancia;
            var listaRepuestos = ListaRepuestos.Instancia;
            var colaServicios = ColaServicios.Instancia;

            // Pasar las instancias al constructor de LoginWindow
            var loginWindow = new LoginWindow(listaUsuarios, listaVehiculos, listaRepuestos, colaServicios);
            loginWindow.Show();

            Application.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al iniciar la aplicación: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}