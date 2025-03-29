using Gtk;
using System;
using AutoGestPro;

namespace AutoGestPro.Interfaz
{
    public class VentanaUsuarios : Window
    {
        public VentanaUsuarios() : base("Menú de Usuario")
        {
            // Configuración básica de la ventana
            SetDefaultSize(300, 400);
            SetPosition(WindowPosition.Center);
            BorderWidth = 10;

            // Crear un contenedor vertical
            VBox contenedor = new VBox(true, 10);

            // Crear botones con las opciones del menú
            string[] opciones = {
                "Insertar Vehículo",
                "Visualización de Servicios",
                "Visualización de Facturas",
                "Cancelar Facturas"
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

            // Lógica específica para cada botón
            switch (boton.Label)
            {
                case "Insertar Vehículo":
                    Console.WriteLine("Abrir ventana para insertar vehículo");
                    InsertarVehiculo ventanaInsertar = new InsertarVehiculo(
                        DatosCompartidos.ListaVehiculos,
                        DatosCompartidos.ListaUsuarios
                    );
                    ventanaInsertar.ShowAll();
                    break;

                case "Visualización de Servicios":
                    Console.WriteLine("Abrir ventana para visualizar servicios");
                    VisualizacionServicios ventanaServicios = new VisualizacionServicios(
                        DatosCompartidos.ArbolServicios
                    );
                    ventanaServicios.ShowAll();
                    break;

                case "Visualización de Facturas":
                    Console.WriteLine("Abrir ventana para visualizar facturas");
                    VisualizacionFacturas ventanaFacturas = new VisualizacionFacturas(DatosCompartidos.ArbolFacturas);
                    ventanaFacturas.ShowAll();
                    break;

                case "Cancelar Facturas":
                    Console.WriteLine("Abrir ventana para cancelar facturas");
                    CancelarFacturas ventanaCancelar = new CancelarFacturas(DatosCompartidos.ArbolFacturas);
                    ventanaCancelar.ShowAll();
                    break;

                default:
                    Console.WriteLine("Opción no reconocida");
                    break;
            }
        }

        private void OnCerrarSesionClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Cerrando sesión...");

            // Registrar la salida del usuario
            DatosCompartidos.ControlLog.RegistrarSalida(DatosCompartidos.UsuarioActual.Correo);

            Destroy(); // Cierra la ventana actual
            new VentanaLogin().ShowAll(); // Muestra la ventana de inicio de sesión
        }
    }
}