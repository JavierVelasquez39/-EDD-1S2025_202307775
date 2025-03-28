using System;
using Gtk;
using AutoGestPro;

namespace AutoGestPro.Interfaz
{
    public class VentanaAdmin : Window
    {
        public VentanaAdmin() : base("Menú Administrador")
        {
            // Configuración básica de la ventana
            SetDefaultSize(300, 400);
            SetPosition(WindowPosition.Center);
            BorderWidth = 10;

            // Crear un contenedor vertical
            VBox contenedor = new VBox(true, 10);

            // Crear botones con las opciones del menú
            string[] opciones = {
                "Cargas Masivas",
                "Gestión de Entidades",
                "Actualización de Repuestos",
                "Visualización de Repuestos",
                "Generar Servicios",
                "Control de Logeo",
                "Generar Reportes"
            };

            // Agregar botones al contenedor
            foreach (string opcion in opciones)
            {
                Button boton = new Button(opcion);
                boton.Clicked += OnBotonClicked;
                contenedor.PackStart(boton, false, false, 0);
            }

            // Botón "Cerrar Sesión"
            Button cerrarSesionButton = new Button("Cerrar Sesión");
            cerrarSesionButton.Clicked += OnCerrarSesionClicked;
            contenedor.PackStart(cerrarSesionButton, false, false, 0);

            // Agregar el contenedor a la ventana
            Add(contenedor);

            // Manejar el cierre de la ventana
            DeleteEvent += delegate { Application.Quit(); };

            ShowAll();
        }

        private void OnBotonClicked(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            Console.WriteLine($"Botón presionado: {boton.Label}");
            switch (boton.Label)
            {
                case "Cargas Masivas":
                    Console.WriteLine("Abrir ventana de Cargas Masivas");
                    VentanaCargaMasiva ventanaCargaMasiva = new VentanaCargaMasiva(
                        DatosCompartidos.ListaUsuarios,
                        DatosCompartidos.ListaVehiculos,
                        DatosCompartidos.ArbolRepuestos
                    );
                    ventanaCargaMasiva.Show();
                    break;
                case "Gestión de Entidades":
                    Console.WriteLine("Abrir ventana de Gestión de Entidades");
                    VentanaGestionEntidades ventanaGestionEntidades = new VentanaGestionEntidades(
                        DatosCompartidos.ListaUsuarios,
                        DatosCompartidos.ListaVehiculos
                    );
                    ventanaGestionEntidades.Show();
                    break;
                case "Actualización de Repuestos":
                    Console.WriteLine("Abrir ventana de Actualización de Repuestos");
                    VentanaActualizacionRepuestos ventanaActualizacionRepuestos = new VentanaActualizacionRepuestos(
                        DatosCompartidos.ArbolRepuestos
                    );
                    ventanaActualizacionRepuestos.Show();
                    break;
                case "Visualización de Repuestos":
                    Console.WriteLine("Abrir ventana de Visualización de Repuestos");
                    VentanaVisualizacionRepuestos ventanaVisualizacionRepuestos = new VentanaVisualizacionRepuestos(
                        DatosCompartidos.ArbolRepuestos
                    );
                    ventanaVisualizacionRepuestos.Show();
                    break;
                case "Generar Servicios":
                    Console.WriteLine("Abrir ventana de Generar Servicios");
                    VentanaGenerarServicio ventanaGenerarServicio = new VentanaGenerarServicio(
                        DatosCompartidos.ArbolRepuestos,
                        DatosCompartidos.ListaVehiculos,
                        DatosCompartidos.ArbolServicios,
                        DatosCompartidos.ArbolFacturas
                    );
                    ventanaGenerarServicio.Show();
                    break;
                case "Control de Logeo":
                    Console.WriteLine("Abrir ventana de Control de Logeo");
                    VentanaControlLogueo ventanaControlLogueo = new VentanaControlLogueo();
                    ventanaControlLogueo.Show();
                    break;
                case "Generar Reportes":
                    Console.WriteLine("Abrir ventana de Generar Reportes");
                    VentanaGenerarGrafica ventanaGenerarGrafica = new VentanaGenerarGrafica(
                        DatosCompartidos.ListaUsuarios,
                        DatosCompartidos.ListaVehiculos,
                        DatosCompartidos.ArbolRepuestos,
                        DatosCompartidos.ArbolServicios,
                        DatosCompartidos.ArbolFacturas
                    );
                    ventanaGenerarGrafica.Show();
                    break;
                default:
                    Console.WriteLine("Opción no reconocida");
                    break;
            }
        }

        private void OnCerrarSesionClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Cerrando sesión...");
            Destroy(); // Cierra la ventana actual
            new VentanaLogin().ShowAll(); // Muestra la ventana de inicio de sesión
        }
    }
}