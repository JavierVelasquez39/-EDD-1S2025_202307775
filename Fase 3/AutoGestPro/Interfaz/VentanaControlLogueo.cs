using Gtk;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using AutoGestPro.Estructuras;

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
            SetDefaultSize(500, 400);
            SetPosition(WindowPosition.Center);

            VBox mainVBox = new VBox(false, 10);
            mainVBox.BorderWidth = 20;

            // Etiqueta de título
            Label labelTitulo = new Label("<b><span size='16000'>Control de Logueo</span></b>");
            labelTitulo.UseMarkup = true;
            mainVBox.PackStart(labelTitulo, false, false, 10);

            // Crear sección de autenticación
            Frame frameLogin = new Frame("Autenticación");
            VBox loginVBox = new VBox(false, 5);
            loginVBox.BorderWidth = 10;

            // Campo de correo
            HBox correoBox = new HBox(false, 5);
            Label labelCorreo = new Label("Correo:");
            Entry entryCorreo = new Entry();
            correoBox.PackStart(labelCorreo, false, false, 5);
            correoBox.PackStart(entryCorreo, true, true, 5);
            loginVBox.PackStart(correoBox, false, false, 5);

            // Campo de contraseña
            HBox contraseniaBox = new HBox(false, 5);
            Label labelContrasenia = new Label("Contraseña:");
            Entry entryContrasenia = new Entry();
            entryContrasenia.Visibility = false; // Ocultar el texto para la contraseña
            contraseniaBox.PackStart(labelContrasenia, false, false, 5);
            contraseniaBox.PackStart(entryContrasenia, true, true, 5);
            loginVBox.PackStart(contraseniaBox, false, false, 5);

            // Botones de entrada y salida
            HBox botonesBox = new HBox(true, 10);
            Button btnRegistrarEntrada = new Button("Registrar Entrada");
            Button btnRegistrarSalida = new Button("Registrar Salida");

            btnRegistrarEntrada.Clicked += (sender, e) =>
            {
                string correo = entryCorreo.Text;
                string contrasenia = entryContrasenia.Text;

                // Usar DatosCompartidos.BlockchainUsuarios para autenticar al usuario
                var usuario = DatosCompartidos.BlockchainUsuarios.AutenticarUsuario(correo, contrasenia);
                if (usuario != null)
                {
                    string nombreCompleto = usuario.Usuario; // Usar la propiedad Usuario
                    RegistrarEntrada(nombreCompleto);
                    MostrarMensaje("Éxito", $"Entrada registrada para: {nombreCompleto}");
                    entryContrasenia.Text = ""; // Limpiar por seguridad
                }
                else
                {
                    MostrarMensaje("Error", "Credenciales incorrectas");
                }
            };

            btnRegistrarSalida.Clicked += (sender, e) =>
            {
                string correo = entryCorreo.Text;
                string contrasenia = entryContrasenia.Text;

                // Usar DatosCompartidos.BlockchainUsuarios para autenticar al usuario
                var usuario = DatosCompartidos.BlockchainUsuarios.AutenticarUsuario(correo, contrasenia);
                if (usuario != null)
                {
                    string nombreCompleto = usuario.Usuario; // Usar la propiedad Usuario
                    RegistrarSalida(nombreCompleto);
                    MostrarMensaje("Éxito", $"Salida registrada para: {nombreCompleto}");
                    entryContrasenia.Text = ""; // Limpiar por seguridad
                }
                else
                {
                    MostrarMensaje("Error", "Credenciales incorrectas");
                }
            };

            botonesBox.PackStart(btnRegistrarEntrada, true, true, 5);
            botonesBox.PackStart(btnRegistrarSalida, true, true, 5);
            loginVBox.PackStart(botonesBox, false, false, 10);

            frameLogin.Add(loginVBox);
            mainVBox.PackStart(frameLogin, false, false, 10);

            // Botones para gestionar archivos JSON
            HBox archivosBtnsBox = new HBox(true, 10);
            Button btnCargarArchivo = new Button("Cargar Archivo JSON");
            btnCargarArchivo.Clicked += OnCargarArchivoClicked;
            Button btnExportarLog = new Button("Exportar Log a JSON");
            btnExportarLog.Clicked += OnExportarLogClicked;
            archivosBtnsBox.PackStart(btnCargarArchivo, true, true, 5);
            archivosBtnsBox.PackStart(btnExportarLog, true, true, 5);
            mainVBox.PackStart(archivosBtnsBox, false, false, 5);

            // Área de texto para mostrar el log
            Label labelLog = new Label("<b>Registro de entradas y salidas:</b>");
            labelLog.UseMarkup = true;
            labelLog.SetAlignment(0, 0);
            mainVBox.PackStart(labelLog, false, false, 5);

            textViewLog = new TextView();
            textViewLog.Editable = false;
            ScrolledWindow scrolledWindow = new ScrolledWindow();
            scrolledWindow.Add(textViewLog);
            mainVBox.PackStart(scrolledWindow, true, true, 5);

            // Mostrar el log en el área de texto
            MostrarLog();

            Add(mainVBox);
            ShowAll();

            // Configurar para cerrar la ventana sin cerrar la aplicación
            DeleteEvent += (o, args) =>
            {
                args.RetVal = true;
                Hide();
            };
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
                string salidaInfo = string.IsNullOrEmpty(entry.Salida) ? "Aún en sesión" : entry.Salida;
                logText += $"Usuario: {entry.Usuario}\nEntrada: {entry.Entrada}\nSalida: {salidaInfo}\n\n";
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
            var logExistente = logEntries.Find(entry => entry.Usuario == usuario && string.IsNullOrEmpty(entry.Salida));
            if (logExistente != null)
            {
                MostrarMensaje("Información", $"El usuario {usuario} ya tiene una sesión activa.");
                return;
            }

            logEntries.Add(new LogEntry
            {
                Usuario = usuario,
                Entrada = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Salida = ""
            });

            GuardarLogEnArchivo("LogEntradasSalidas.json");
            MostrarLog();
        }

        public void RegistrarSalida(string usuario)
        {
            var log = logEntries.Find(entry => entry.Usuario == usuario && string.IsNullOrEmpty(entry.Salida));
            if (log != null)
            {
                log.Salida = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                GuardarLogEnArchivo("LogEntradasSalidas.json");
                MostrarLog();
            }
            else
            {
                MostrarMensaje("Información", $"No se encontró una sesión activa para el usuario {usuario}.");
            }
        }

        private void GuardarLogEnArchivo(string filePath)
        {
            try
            {
                string json = JsonConvert.SerializeObject(logEntries, Formatting.Indented);
                File.WriteAllText(filePath, json);
                Console.WriteLine($"✅ Log guardado en: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al guardar el log: {ex.Message}");
                MostrarMensaje("Error", $"No se pudo guardar el log en el archivo: {ex.Message}");
            }
        }
    }

    [Serializable]
    public class LogEntry
    {
        public string Usuario { get; set; } = "";
        public string Entrada { get; set; } = "";
        public string Salida { get; set; } = "";
    }
}