using System;
using Gtk;
using AutoGestPro.DataStructures;

namespace AutoGestPro.GUI
{
    public class MainWindow : Window
    {
        private ListaUsuarios listaUsuarios;
        private ListaVehiculos listaVehiculos;
        private ListaRepuestos listaRepuestos;
        private ColaServicios colaServicios;

        [Obsolete]
        public MainWindow(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos, ListaRepuestos listaRepuestos, ColaServicios colaServicios) : base("AutoGest Pro - Menú Principal")
        {
            // Inicializar las listas y la cola
            this.listaUsuarios = listaUsuarios;
            this.listaVehiculos = listaVehiculos;
            this.listaRepuestos = listaRepuestos;
            this.colaServicios = colaServicios;

            SetDefaultSize(400, 400);
            SetPosition(WindowPosition.Center);
            DeleteEvent += delegate { Application.Quit(); };

            // Estilo de fondo
            var cssProvider = new CssProvider();
            cssProvider.LoadFromData("window { background-color: #c8c8c8; }");
            StyleContext.AddProviderForScreen(Gdk.Screen.Default, cssProvider, uint.MaxValue);

            VBox vbox = new VBox(false, 15);
            vbox.BorderWidth = 20;

            // Título "Menú"
            Label labelTitulo = new Label("<b><span size='14000'>Menú</span></b>");
            labelTitulo.UseMarkup = true;
            vbox.PackStart(labelTitulo, false, false, 5);

            // Botones del menú
            string[] opciones = { "Cargas Masivas", "Ingreso Manual", "Gestión de Usuarios", "Generar Servicio", "Cancelar Factura" };
            
            foreach (string texto in opciones)
            {
                Button boton = new Button(texto);
                // Estilo de los botones usando CSS
                var buttonCssProvider = new CssProvider();
                buttonCssProvider.LoadFromData("button { background-color: #00c800; color: #000000; }");
                boton.StyleContext.AddProvider(buttonCssProvider, uint.MaxValue);
                boton.SetSizeRequest(200, 40);
                boton.Clicked += OnBotonClicked;
                vbox.PackStart(boton, false, false, 5);
            }

            Add(vbox);
            ShowAll();
        }

        [Obsolete]
        private void OnBotonClicked(object? sender, EventArgs e)
        {
            if (sender is Button boton)
            {
                Console.WriteLine($"🟢 Opción seleccionada: {boton.Label}");
                if (boton.Label == "Cargas Masivas")
                {
                    var cargasMasivasWindow = new CargaMasivaWindow(listaUsuarios, listaVehiculos, listaRepuestos);
                    cargasMasivasWindow.Show();
                }
                else if (boton.Label == "Ingreso Manual")
                {
                    var ingresoManualWindow = new IngresoManualWindow(listaUsuarios, listaVehiculos, listaRepuestos, colaServicios);
                    ingresoManualWindow.Show();
                }
                else if (boton.Label == "Gestión de Usuarios")
                {
                    var gestionUsuariosWindow = new GestionUsuariosWindow(listaUsuarios, listaVehiculos);
                    gestionUsuariosWindow.Show();
                }
            }
        }
    }
}