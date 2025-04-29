using Gtk;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using AutoGestPro.Estructuras;
using AutoGestPro.Modelos;

namespace AutoGestPro.Interfaz
{
    public class VentanaCargaMasiva : Window
    {
        private ComboBox comboBoxCategorias;
        private Button btnCargarArchivo;
        private Label labelMensaje;

        // Referencias a las estructuras de datos
        private Blockchain blockchainUsuarios;
        private ListaDobleVehiculos listaVehiculos;
        private ArbolAVLRepuestos arbolRepuestos;

        public VentanaCargaMasiva(Blockchain blockchainUsuarios, ListaDobleVehiculos listaVehiculos, ArbolAVLRepuestos arbolRepuestos) 
            : base("Carga Masiva")
        {
            this.blockchainUsuarios = blockchainUsuarios;
            this.listaVehiculos = listaVehiculos;
            this.arbolRepuestos = arbolRepuestos;

            // Configuración básica de la ventana
            SetDefaultSize(400, 300);
            SetPosition(WindowPosition.Center);

            // Crear un contenedor vertical
            VBox mainBox = new VBox(false, 10);
            mainBox.BorderWidth = 20;

            // Etiqueta de título
            Label labelTitulo = new Label("<b><span size='14000'>Cargas Masivas</span></b>");
            labelTitulo.UseMarkup = true;
            mainBox.PackStart(labelTitulo, false, false, 5);

            // Crear ComboBox para selección de categorías
            comboBoxCategorias = new ComboBox(new string[] { "Usuarios", "Vehículos", "Repuestos" });
            mainBox.PackStart(comboBoxCategorias, false, false, 10);

            // Crear botón para cargar archivo
            btnCargarArchivo = new Button("Seleccionar Archivo JSON");
            btnCargarArchivo.Clicked += OnSeleccionarArchivo;
            mainBox.PackStart(btnCargarArchivo, false, false, 10);

            // Etiqueta para mostrar mensajes
            labelMensaje = new Label("");
            mainBox.PackStart(labelMensaje, false, false, 5);

            // Agregar el contenedor a la ventana
            Add(mainBox);

            // Configurar eventos de cierre
            DeleteEvent += OnDeleteEvent;

            // Mostrar todos los elementos
            ShowAll();
        }

        private void OnSeleccionarArchivo(object sender, EventArgs e)
        {
            // Crear un diálogo para seleccionar archivos
            FileChooserDialog fileChooser = new FileChooserDialog(
                "Selecciona un archivo JSON",
                this,
                FileChooserAction.Open,
                "Cancelar", ResponseType.Cancel,
                "Abrir", ResponseType.Accept);

            if (fileChooser.Run() == (int)ResponseType.Accept)
            {
                string filePath = fileChooser.Filename;
                fileChooser.Destroy();

                TreeIter iter;
                if (comboBoxCategorias.GetActiveIter(out iter))
                {
                    string categoriaSeleccionada = (string)comboBoxCategorias.Model.GetValue(iter, 0);
                    Console.WriteLine($"Categoría seleccionada: {categoriaSeleccionada}");
                    Console.WriteLine($"Archivo seleccionado: {filePath}");

                    if (CargarDatosDesdeJSON(filePath, categoriaSeleccionada))
                    {
                        labelMensaje.Text = $"✅ Carga masiva completada para la categoría: {categoriaSeleccionada}";
                    }
                    else
                    {
                        labelMensaje.Text = "❌ Error al procesar el archivo.";
                    }
                }
                else
                {
                    labelMensaje.Text = "❌ No se ha seleccionado ninguna categoría.";
                }
            }
            else
            {
                fileChooser.Destroy();
            }
        }

        private bool CargarDatosDesdeJSON(string filePath, string categoriaSeleccionada)
        {
            try
            {
                string jsonData = File.ReadAllText(filePath);
                Console.WriteLine($"Archivo leído correctamente: {filePath}");

                switch (categoriaSeleccionada)
                {
                    case "Usuarios":
                        var usuarios = JsonConvert.DeserializeObject<List<Block>>(jsonData);
                        if (usuarios != null)
                        {
                            foreach (var usuario in usuarios)
                            {
                                // Agregar cada bloque al blockchain
                                blockchainUsuarios.AddBlock(usuario.Usuario, usuario.Correo, usuario.ContraseniaTextoPlano);
                            }
                            Console.WriteLine($"📂 Cargados {usuarios.Count} usuarios.");
                        }
                        break;

                    case "Vehículos":
                        var vehiculos = JsonConvert.DeserializeObject<List<Vehiculo>>(jsonData);
                        if (vehiculos != null)
                        {
                            foreach (var vehiculo in vehiculos)
                            {
                                listaVehiculos.AgregarVehiculo(vehiculo);
                            }
                            Console.WriteLine($"📂 Cargados {vehiculos.Count} vehículos.");
                        }
                        break;

                    case "Repuestos":
                        var repuestos = JsonConvert.DeserializeObject<List<Repuesto>>(jsonData);
                        if (repuestos != null)
                        {
                            foreach (var repuesto in repuestos)
                            {
                                arbolRepuestos.Insertar(repuesto);
                            }
                            Console.WriteLine($"📂 Cargados {repuestos.Count} repuestos.");
                        }
                        break;

                    default:
                        Console.WriteLine($"⚠️ Categoría desconocida: {categoriaSeleccionada}");
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al cargar el JSON: {ex.Message}");
                return false;
            }
        }

        private void OnDeleteEvent(object sender, DeleteEventArgs a)
        {
            // Cerrar solo la ventana actual
            Destroy();
            a.RetVal = true;
        }
    }
}