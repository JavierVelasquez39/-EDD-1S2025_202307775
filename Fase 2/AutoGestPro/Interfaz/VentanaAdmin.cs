using System;
using Gtk;
using AutoGestPro.Estructuras;
using AutoGestPro.Interfaz;

namespace AutoGestPro.Interfaz
{
    public class VentanaAdmin : Window
    {
        // Referencias a las estructuras de datos
        private ListaSimpleUsuarios listaUsuarios;
        private ListaDobleVehiculos listaVehiculos;
        private ArbolAVLRepuestos arbolRepuestos;
        private ArbolBinarioServicios arbolServicios;
        private ArbolBFacturas arbolFacturas;

        public VentanaAdmin(ListaSimpleUsuarios listaUsuarios, ListaDobleVehiculos listaVehiculos, ArbolAVLRepuestos arbolRepuestos, ArbolBinarioServicios arbolServicios, ArbolBFacturas arbolFacturas) 
            : base("Menú Administrador")
        {
            this.listaUsuarios = listaUsuarios;
            this.listaVehiculos = listaVehiculos;
            this.arbolRepuestos = arbolRepuestos;
            this.arbolServicios = arbolServicios;
            this.arbolFacturas = arbolFacturas;

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
            // Aquí puedes agregar la lógica específica para cada botón
            switch (boton.Label)
            {
                case "Cargas Masivas":
                    Console.WriteLine("Abrir ventana de Cargas Masivas");
                    VentanaCargaMasiva ventanaCargaMasiva = new VentanaCargaMasiva(listaUsuarios, listaVehiculos, arbolRepuestos);
                    ventanaCargaMasiva.Show();
                    break;
                case "Gestión de Entidades":
                    Console.WriteLine("Abrir ventana de Gestión de Entidades");
                    VentanaGestionEntidades ventanaGestionEntidades = new VentanaGestionEntidades(listaUsuarios, listaVehiculos);
                    ventanaGestionEntidades.Show();
                    break;
                case "Actualización de Repuestos":
                    Console.WriteLine("Abrir ventana de Actualización de Repuestos");
                    VentanaActualizacionRepuestos ventanaActualizacionRepuestos = new VentanaActualizacionRepuestos(arbolRepuestos);
                    ventanaActualizacionRepuestos.Show();
                    break;
                case "Visualización de Repuestos":
                    Console.WriteLine("Abrir ventana de Visualización de Repuestos");
                    VentanaVisualizacionRepuestos ventanaVisualizacionRepuestos = new VentanaVisualizacionRepuestos(arbolRepuestos);
                    ventanaVisualizacionRepuestos.Show();
                    break;
                case "Generar Servicios":
                    Console.WriteLine("Abrir ventana de Generar Servicios");
                    VentanaGenerarServicio ventanaGenerarServicio = new VentanaGenerarServicio(arbolRepuestos, listaVehiculos, arbolServicios, arbolFacturas);
                    ventanaGenerarServicio.Show();
                    break;
                case "Control de Logeo":
                    Console.WriteLine("Abrir ventana de Control de Logeo");
                    VentanaControlLogueo ventanaControlLogueo = new VentanaControlLogueo();
                    ventanaControlLogueo.Show();
                    break;
                case "Generar Reportes":
                    Console.WriteLine("Abrir ventana de Generar Reportes");
                    VentanaGenerarGrafica ventanaGenerarGrafica = new VentanaGenerarGrafica(listaUsuarios, listaVehiculos, arbolRepuestos, arbolServicios, arbolFacturas);
                    ventanaGenerarGrafica.Show();
                    break;
                default:
                    Console.WriteLine("Opción no reconocida");
                    break;
            }
        }
    }
}