using System;
using Gtk;
using AutoGestPro.Estructuras;
using AutoGestPro.Modelos;

namespace AutoGestPro.Interfaz
{
    public class VentanaAdmin : Window
    {
        [Obsolete]
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
                "Carga Masiva",
                "Inserción de Usuarios",
                "Gestión de Entidades",
                "Visualización de Repuestos",
                "Generar Servicio",
                "Generar Reportes",
                "Generar Backup",
                "Cargar Backup",
                "Control Logueo"
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
                case "Carga Masiva":
                    Console.WriteLine("Abrir ventana de Carga Masiva");
                    // Pasar las instancias necesarias desde DatosCompartidos
                    new VentanaCargaMasiva(
                        DatosCompartidos.BlockchainUsuarios,
                        DatosCompartidos.ListaVehiculos,
                        DatosCompartidos.ArbolRepuestos
                    ).Show();
                    break;
                    
                case "Inserción de Usuarios":
                    // Pasar la instancia de blockchain
                    new VentanaInsercionUsuarios().Show();
                    break;
                    
                case "Gestión de Entidades":
                    Console.WriteLine("Abrir ventana de Gestión de Entidades");
                    // Pasar las instancias necesarias
                    new VentanaGestionEntidades(
                        DatosCompartidos.BlockchainUsuarios,
                        DatosCompartidos.ListaVehiculos
                    ).Show();
                    break;
                    
                case "Visualización de Repuestos":
                    Console.WriteLine("Abrir ventana de Visualización de Repuestos");
                    // Pasar la instancia del árbol de repuestos
                    new VentanaVisualizacionRepuestos(
                        DatosCompartidos.ArbolRepuestos
                    ).Show();
                    break;
                    
                case "Generar Servicio":
                    Console.WriteLine("Abrir ventana de Generar Servicio");
                    // Pasar todas las instancias necesarias
                    new VentanaGenerarServicio(
                        DatosCompartidos.ArbolServicios,
                        DatosCompartidos.ArbolFacturas,
                        DatosCompartidos.GrafoRelaciones,
                        DatosCompartidos.ListaVehiculos,
                        DatosCompartidos.ArbolRepuestos
                    ).Show();
                    break;
                    
                case "Generar Reportes":
                    Console.WriteLine("Abrir ventana de Generación de Gráficas");
                    // Pasar todas las instancias necesarias para generar gráficas
                    new VentanaGenerarGrafica(
                        DatosCompartidos.BlockchainUsuarios,
                        DatosCompartidos.ListaVehiculos,
                        DatosCompartidos.ArbolRepuestos,
                        DatosCompartidos.ArbolServicios,
                        DatosCompartidos.ArbolFacturas,
                        DatosCompartidos.GrafoRelaciones
                    ).Show();
                    break;
                    
                case "Generar Backup":
                    Console.WriteLine("Abrir ventana de Generar Backup");
                    // Implementar ventana de backup cuando esté disponible
                    break;
                    
                case "Cargar Backup":
                    Console.WriteLine("Abrir ventana de Cargar Backup");
                    // Implementar ventana de cargar backup cuando esté disponible
                    break;
                    
                case "Control Logueo":
                    Console.WriteLine("Abrir ventana de Control Logueo");
                    // Usar la instancia compartida del control de logueo
                    DatosCompartidos.ControlLog.Show();
                    break;
                    
                default:
                    Console.WriteLine("Opción no reconocida");
                    break;
            }
        }

        private void OnCerrarSesionClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Cerrando sesión...");
            
            // Registrar la salida del administrador
            DatosCompartidos.ControlLog.RegistrarSalida("Administrador");
            
            // Cerrar la ventana actual
            Destroy();
            
            // Mostrar la ventana de login SIN pasar instancias nuevas
            new VentanaLogin().ShowAll();
        }
    }
}