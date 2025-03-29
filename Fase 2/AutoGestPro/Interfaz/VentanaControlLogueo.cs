using Gtk;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AutoGestPro.Interfaz
{
    public class VentanaControlLogueo : Window
    {
        private List<LogEntry> logEntries;
        private TextView textViewLog;

        public VentanaControlLogueo() : base("Control de Logueo")
        {
            logEntries = CargarLogDesdeArchivo("LogEntradasSalidas.json");

            // Configuración básica de la ventana
            SetDefaultSize(500, 300);
            SetPosition(WindowPosition.Center);

            VBox mainVBox = new VBox(false, 10);
            mainVBox.BorderWidth = 20;

            // Etiqueta de título
            Label labelTitulo = new Label("<b><span size='16000'>Control de Logueo</span></b>");
            labelTitulo.UseMarkup = true;
            mainVBox.PackStart(labelTitulo, false, false, 10);

            // Botón para cargar un archivo JSON
            Button btnCargarArchivo = new Button("Cargar Archivo JSON");
            btnCargarArchivo.Clicked += OnCargarArchivoClicked;
            mainVBox.PackStart(btnCargarArchivo, false, false, 10);

            // Botón para exportar el log a JSON
            Button btnExportarLog = new Button("Exportar Log a JSON");
            btnExportarLog.Clicked += OnExportarLogClicked;
            mainVBox.PackStart(btnExportarLog, false, false, 10);

            // Área de texto para mostrar el log
            textViewLog = new TextView();
            textViewLog.Editable = false;
            ScrolledWindow scrolledWindow = new ScrolledWindow();
            scrolledWindow.Add(textViewLog);
            mainVBox.PackStart(scrolledWindow, true, true, 10);

            // Mostrar el log en el área de texto
            MostrarLog();

            Add(mainVBox);
            ShowAll();
        }

        private List<LogEntry> CargarLogDesdeArchivo(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<LogEntry>>(json) ?? new List<LogEntry>();
            }
            return new List<LogEntry>();
        }

        private void MostrarLog()
        {
            string logText = "";
            foreach (var entry in logEntries)
            {
                logText += $"Usuario: {entry.Usuario}\nEntrada: {entry.Entrada}\nSalida: {entry.Salida}\n\n";
            }
            textViewLog.Buffer.Text = logText;
        }

        private void OnCargarArchivoClicked(object sender, EventArgs e)
        {
            FileChooserDialog fileChooser = new FileChooserDialog(
                "Seleccione un archivo JSON",
                this,
                FileChooserAction.Open,
                "Cancelar", ResponseType.Cancel,
                "Abrir", ResponseType.Accept
            );

            if (fileChooser.Run() == (int)ResponseType.Accept)
            {
                string filePath = fileChooser.Filename;
                try
                {
                    logEntries = CargarLogDesdeArchivo(filePath);
                    MostrarLog();
                    MostrarMensaje("Éxito", $"Archivo cargado correctamente: {filePath}");
                }
                catch (Exception ex)
                {
                    MostrarMensaje("Error", $"Ocurrió un error al cargar el archivo: {ex.Message}");
                }
            }

            fileChooser.Destroy();
        }

        private void OnExportarLogClicked(object sender, EventArgs e)
        {
            FileChooserDialog fileChooser = new FileChooserDialog(
                "Guardar Log como JSON",
                this,
                FileChooserAction.Save,
                "Cancelar", ResponseType.Cancel,
                "Guardar", ResponseType.Accept
            );

            if (fileChooser.Run() == (int)ResponseType.Accept)
            {
                string filePath = fileChooser.Filename;
                try
                {
                    string json = JsonConvert.SerializeObject(logEntries, Formatting.Indented);
                    File.WriteAllText(filePath, json);
                    MostrarMensaje("Éxito", $"Log exportado correctamente: {filePath}");
                }
                catch (Exception ex)
                {
                    MostrarMensaje("Error", $"Ocurrió un error al exportar el archivo: {ex.Message}");
                }
            }

            fileChooser.Destroy();
        }

        private void MostrarMensaje(string titulo, string mensaje)
        {
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, titulo == "Error" ? MessageType.Error : MessageType.Info, ButtonsType.Ok, mensaje);
            dialog.Run();
            dialog.Destroy();
        }

        public void RegistrarEntrada(string usuario)
        {
            // Verificar si ya existe un registro de entrada sin salida para este usuario
            var logExistente = logEntries.Find(entry => entry.Usuario == usuario && string.IsNullOrEmpty(entry.Salida));
            if (logExistente != null)
            {
                Console.WriteLine($"El usuario {usuario} ya tiene un registro de entrada sin salida. No se creará un nuevo registro.");
                return;
            }

            // Crear un nuevo registro de entrada
            logEntries.Add(new LogEntry
            {
                Usuario = usuario,
                Entrada = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                Salida = ""
            });

            // Guardar los cambios en el archivo JSON
            GuardarLogEnArchivo("LogEntradasSalidas.json");
            MostrarLog();
        }

        public void RegistrarSalida(string usuario)
        {
            var log = logEntries.Find(entry => entry.Usuario == usuario && string.IsNullOrEmpty(entry.Salida));
            if (log != null)
            {
                log.Salida = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                GuardarLogEnArchivo("LogEntradasSalidas.json");
                MostrarLog();
            }
        }

        private void GuardarLogEnArchivo(string filePath)
        {
            try
            {
                string json = JsonConvert.SerializeObject(logEntries, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error", $"No se pudo guardar el log en el archivo: {ex.Message}");
            }
        }
    }

    public class LogEntry
    {
        public string Usuario { get; set; }
        public string Entrada { get; set; }
        public string Salida { get; set; }
    }
}