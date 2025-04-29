using System;
using Gtk;
using AutoGestPro.Estructuras;

namespace AutoGestPro.Interfaz
{
    public class VentanaInsercionUsuarios : Window
    {
        private Entry idEntry;
        private Entry nombresEntry;
        private Entry apellidosEntry;
        private Entry correoEntry;
        private Entry edadEntry;
        private Entry contraseniaEntry;
        private Button guardarButton;

        public VentanaInsercionUsuarios() : base("Registrar Usuario")
        {
            // Configuración de la ventana
            SetDefaultSize(400, 300);
            SetPosition(WindowPosition.Center);
            DeleteEvent += OnDeleteEvent;

            // Crear el contenedor principal
            VBox mainBox = new VBox(false, 10);
            mainBox.BorderWidth = 20;

            // Crear la tabla para el formulario
            Table formTable = new Table(6, 2, false);
            formTable.ColumnSpacing = 10;
            formTable.RowSpacing = 10;

            // ID del usuario
            Label idLabel = new Label("ID del Usuario:");
            idEntry = new Entry();
            formTable.Attach(idLabel, 0, 1, 0, 1);
            formTable.Attach(idEntry, 1, 2, 0, 1);

            // Nombres
            Label nombresLabel = new Label("Nombres:");
            nombresEntry = new Entry();
            formTable.Attach(nombresLabel, 0, 1, 1, 2);
            formTable.Attach(nombresEntry, 1, 2, 1, 2);

            // Apellidos
            Label apellidosLabel = new Label("Apellidos:");
            apellidosEntry = new Entry();
            formTable.Attach(apellidosLabel, 0, 1, 2, 3);
            formTable.Attach(apellidosEntry, 1, 2, 2, 3);

            // Correo
            Label correoLabel = new Label("Correo:");
            correoEntry = new Entry();
            formTable.Attach(correoLabel, 0, 1, 3, 4);
            formTable.Attach(correoEntry, 1, 2, 3, 4);

            // Edad
            Label edadLabel = new Label("Edad:");
            edadEntry = new Entry();
            formTable.Attach(edadLabel, 0, 1, 4, 5);
            formTable.Attach(edadEntry, 1, 2, 4, 5);

            // Contraseña
            Label contraseniaLabel = new Label("Contraseña:");
            contraseniaEntry = new Entry();
            contraseniaEntry.Visibility = false; // Ocultar texto para contraseñas
            formTable.Attach(contraseniaLabel, 0, 1, 5, 6);
            formTable.Attach(contraseniaEntry, 1, 2, 5, 6);

            // Botón Guardar
            guardarButton = new Button("Guardar");
            guardarButton.Clicked += OnGuardarClicked;

            // Agregar componentes al contenedor principal
            mainBox.PackStart(formTable, false, false, 0);
            mainBox.PackStart(guardarButton, false, false, 0);

            // Agregar el contenedor principal a la ventana
            Add(mainBox);
            ShowAll();
        }

        private void OnGuardarClicked(object sender, EventArgs e)
        {
            // Validar los campos
            if (string.IsNullOrWhiteSpace(idEntry.Text) ||
                string.IsNullOrWhiteSpace(nombresEntry.Text) ||
                string.IsNullOrWhiteSpace(apellidosEntry.Text) ||
                string.IsNullOrWhiteSpace(correoEntry.Text) ||
                string.IsNullOrWhiteSpace(edadEntry.Text) ||
                string.IsNullOrWhiteSpace(contraseniaEntry.Text))
            {
                MostrarMensaje("Por favor, complete todos los campos.", MessageType.Error);
                return;
            }

            try
            {
                // Crear el nuevo bloque
                int id = int.Parse(idEntry.Text);
                string nombres = nombresEntry.Text;
                string apellidos = apellidosEntry.Text;
                string correo = correoEntry.Text.Trim();
                int edad = int.Parse(edadEntry.Text);
                string contrasenia = contraseniaEntry.Text;

                // Verificar si el ID del usuario ya está registrado
                if (DatosCompartidos.BlockchainUsuarios.BuscarUsuario(id) != null)
                {
                    MostrarMensaje("El ID del usuario ya está registrado. Use un ID diferente.", MessageType.Error);
                    return;
                }

                // Verificar si el correo ya está en uso
                foreach (var block in DatosCompartidos.BlockchainUsuarios.Chain)
                {
                    if (block.Index > 0 && // Saltar bloque génesis
                        block.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase))
                    {
                        MostrarMensaje("El correo electrónico ya está registrado. Use un correo diferente.", MessageType.Error);
                        return;
                    }
                }

                // Agregar el usuario al blockchain
                Console.WriteLine($"Registrando nuevo usuario: ID={id}, Correo={correo}");
                DatosCompartidos.BlockchainUsuarios.AddBlock($"{nombres} {apellidos}", correo, contrasenia);

                Console.WriteLine($"✅ Usuario registrado exitosamente: {nombres} {apellidos} ({correo})");
                MostrarMensaje($"Usuario {nombres} {apellidos} registrado exitosamente.", MessageType.Info);

                // Limpiar los campos
                LimpiarCampos();
            }
            catch (FormatException)
            {
                MostrarMensaje("Error: El ID y la edad deben ser números enteros.", MessageType.Error);
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", MessageType.Error);
            }
        }

        private void MostrarMensaje(string mensaje, MessageType tipo)
        {
            MessageDialog dialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                tipo,
                ButtonsType.Ok,
                mensaje
            );
            dialog.Run();
            dialog.Destroy();
        }

        private void LimpiarCampos()
        {
            idEntry.Text = string.Empty;
            nombresEntry.Text = string.Empty;
            apellidosEntry.Text = string.Empty;
            correoEntry.Text = string.Empty;
            edadEntry.Text = string.Empty;
            contraseniaEntry.Text = string.Empty;
        }

        private void OnDeleteEvent(object sender, DeleteEventArgs a)
        {
            Destroy();
            a.RetVal = true;
        }
    }
}