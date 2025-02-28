using System;
using System.IO;
using System.Collections.Generic;
using Gtk;
using Newtonsoft.Json;
using AutoGestPro.DataStructures;

namespace AutoGestPro.GUI
{
    public class CargaMasivaWindow : Window
    {
        private Label labelMensaje;
        private ComboBox comboBoxCategorias;
        private ListaUsuarios listaUsuarios;
        private ListaVehiculos listaVehiculos;
        private ListaRepuestos listaRepuestos;
        private string categoriaSeleccionada;

        [Obsolete]
        public CargaMasivaWindow(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos, ListaRepuestos listaRepuestos) : base("Cargas Masivas")
        {
            this.categoriaSeleccionada = string.Empty;
            this.listaUsuarios = listaUsuarios;
            this.listaVehiculos = listaVehiculos;
            this.listaRepuestos = listaRepuestos;

            SetDefaultSize(400, 300);
            SetPosition(WindowPosition.Center);
            DeleteEvent += delegate { Application.Quit(); };

            VBox vbox = new VBox(false, 10);
            vbox.BorderWidth = 20;

            Label labelTitulo = new Label("<b><span size='14000'>Cargas Masivas</span></b>");
            labelTitulo.UseMarkup = true;
            vbox.PackStart(labelTitulo, false, false, 5);

            comboBoxCategorias = new ComboBox(new string[] { "Usuarios", "Vehículos", "Repuestos" });
            vbox.PackStart(comboBoxCategorias, false, false, 10);

            Button btnCargarArchivo = new Button("Seleccionar Archivo JSON");
            btnCargarArchivo.Clicked += OnSeleccionarArchivo;
            vbox.PackStart(btnCargarArchivo, false, false, 10);

            labelMensaje = new Label("");
            vbox.PackStart(labelMensaje, false, false, 5);

            Add(vbox);
            ShowAll();
        }

        private void OnSeleccionarArchivo(object? sender, EventArgs e)
        {
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
                    categoriaSeleccionada = (string)comboBoxCategorias.Model.GetValue(iter, 0);
                }
                else
                {
                    labelMensaje.Text = "❌ No se ha seleccionado ninguna categoría.";
                    return;
                }
                if (CargarDatosDesdeJSON(filePath, categoriaSeleccionada))
                {
                    labelMensaje.Text = "✅ Carga masiva completada con éxito.";
                }
                else
                {
                    labelMensaje.Text = "❌ Error al procesar el archivo.";
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
                        var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(jsonData);
                        if (usuarios != null)
                        {
                            listaUsuarios.AgregarUsuarios(usuarios);
                            Console.WriteLine($"📂 Cargados {usuarios.Count} usuarios.");
                            listaUsuarios.MostrarUsuarios();
                        }
                        else
                        {
                            Console.WriteLine("⚠️ No se encontraron usuarios en el JSON.");
                        }
                        break;

                    case "Vehículos":
                        var vehiculos = JsonConvert.DeserializeObject<List<Vehiculo>>(jsonData);
                        if (vehiculos != null)
                        {
                            foreach (var vehiculo in vehiculos)
                            {
                                listaVehiculos.AgregarVehiculo(vehiculo.ID, vehiculo.IDUsuario, vehiculo.Marca, vehiculo.Modelo, vehiculo.Placa);
                            }
                            Console.WriteLine($"📂 Cargados {vehiculos.Count} vehículos.");
                            listaVehiculos.MostrarVehiculos();
                        }
                        else
                        {
                            Console.WriteLine("⚠️ No se encontraron vehículos en el JSON.");
                        }
                        break;

                    case "Repuestos":
                        var repuestos = JsonConvert.DeserializeObject<List<Repuesto>>(jsonData);
                        if (repuestos != null)
                        {
                            foreach (var repuesto in repuestos)
                            {
                                listaRepuestos.AgregarRepuesto(repuesto.ID, repuesto.Nombre, repuesto.Detalles, repuesto.Costo);
                            }
                            Console.WriteLine($"📂 Cargados {repuestos.Count} repuestos.");
                            listaRepuestos.MostrarRepuestos();
                        }
                        else
                        {
                            Console.WriteLine("⚠️ No se encontraron repuestos en el JSON.");
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
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }
    }
}