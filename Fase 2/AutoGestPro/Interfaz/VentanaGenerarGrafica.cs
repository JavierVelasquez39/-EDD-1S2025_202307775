using Gtk;
using System;
using System.Diagnostics;
using System.IO;
using AutoGestPro.Estructuras;

namespace AutoGestPro.Interfaz
{
    public class VentanaGenerarGrafica : Window
    {
        private ComboBoxText comboBoxEstructuras;
        private Image imagenGrafica;
        private string reportesPath = "Reportes";

        // Instancias de las estructuras
        private ListaSimpleUsuarios listaUsuarios;
        private ListaDobleVehiculos listaVehiculos;
        private ArbolAVLRepuestos arbolRepuestos;
        private ArbolBinarioServicios arbolServicios;
        private ArbolBFacturas arbolFacturas;

        public VentanaGenerarGrafica(
            ListaSimpleUsuarios listaUsuarios,
            ListaDobleVehiculos listaVehiculos,
            ArbolAVLRepuestos arbolRepuestos,
            ArbolBinarioServicios arbolServicios,
            ArbolBFacturas arbolFacturas
        ) : base("Generación de Gráficas")
        {
            // Asignar las instancias de las estructuras
            this.listaUsuarios = listaUsuarios;
            this.listaVehiculos = listaVehiculos;
            this.arbolRepuestos = arbolRepuestos;
            this.arbolServicios = arbolServicios;
            this.arbolFacturas = arbolFacturas;

            // Configuración básica de la ventana
            SetDefaultSize(600, 400);
            SetPosition(WindowPosition.Center);

            VBox mainVBox = new VBox(false, 10);
            mainVBox.BorderWidth = 20;

            // Etiqueta de título
            Label labelTitulo = new Label("<b><span size='16000'>Generación de Gráficas</span></b>");
            labelTitulo.UseMarkup = true;
            mainVBox.PackStart(labelTitulo, false, false, 10);

            // ComboBox para seleccionar la estructura
            Label labelComboBox = new Label("Seleccione la estructura:");
            mainVBox.PackStart(labelComboBox, false, false, 5);

            comboBoxEstructuras = new ComboBoxText();
            comboBoxEstructuras.AppendText("Lista Simple Usuarios");
            comboBoxEstructuras.AppendText("Lista Doble Vehículos");
            comboBoxEstructuras.AppendText("Árbol AVL Repuestos");
            comboBoxEstructuras.AppendText("Árbol Binario Servicios");
            comboBoxEstructuras.AppendText("Árbol B Facturas");
            comboBoxEstructuras.Active = 0; // Seleccionar el primer elemento por defecto
            mainVBox.PackStart(comboBoxEstructuras, false, false, 5);

            // Botón para generar la gráfica
            Button btnGenerar = new Button("Generar Gráfica");
            btnGenerar.Clicked += OnGenerarGraficaClicked;
            mainVBox.PackStart(btnGenerar, false, false, 10);

            // Área para mostrar la imagen generada
            imagenGrafica = new Image();
            ScrolledWindow scrolledWindow = new ScrolledWindow();
            scrolledWindow.Add(imagenGrafica);
            mainVBox.PackStart(scrolledWindow, true, true, 10);

            Add(mainVBox);
            ShowAll();
        }

        private void OnGenerarGraficaClicked(object sender, EventArgs e)
        {
            string estructuraSeleccionada = comboBoxEstructuras.ActiveText;

            if (!Directory.Exists(reportesPath))
            {
                Directory.CreateDirectory(reportesPath);
            }

            string dotFilePath = $"{reportesPath}/{estructuraSeleccionada.Replace(" ", "")}.dot";
            string imageFilePath = $"{reportesPath}/{estructuraSeleccionada.Replace(" ", "")}.png";

            try
            {
                // Generar el archivo .dot y la imagen según la estructura seleccionada
                switch (estructuraSeleccionada)
                {
                    case "Lista Simple Usuarios":
                        listaUsuarios.GenerarDot(dotFilePath);
                        break;
                    case "Lista Doble Vehículos":
                        listaVehiculos.GenerarDot(dotFilePath);
                        break;
                    case "Árbol AVL Repuestos":
                        arbolRepuestos.GenerarDot(dotFilePath);
                        break;
                    case "Árbol Binario Servicios":
                        arbolServicios.GenerarDot(dotFilePath);
                        break;
                    case "Árbol B Facturas":
                        arbolFacturas.GenerarDot(dotFilePath);
                        break;
                    default:
                        throw new Exception("Estructura no reconocida.");
                }

                // Ejecutar Graphviz para generar la imagen
                GenerarImagen(dotFilePath, imageFilePath);

                // Mostrar la imagen generada
                if (File.Exists(imageFilePath))
                {
                    imagenGrafica.Pixbuf = new Gdk.Pixbuf(imageFilePath);
                }
                else
                {
                    MostrarMensaje("Error", "No se pudo generar la imagen.");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error", $"Ocurrió un error: {ex.Message}");
            }
        }

        private void GenerarImagen(string dotFilePath, string outputImagePath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("dot")
            {
                Arguments = $"-Tpng \"{dotFilePath}\" -o \"{outputImagePath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();
            }
        }

        private void MostrarMensaje(string titulo, string mensaje)
        {
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, mensaje);
            dialog.Title = titulo;
            dialog.Run();
            dialog.Destroy();
        }
    }
}