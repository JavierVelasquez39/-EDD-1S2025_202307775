using Gtk;
using System;
using AutoGestPro.Estructuras;

namespace AutoGestPro.Interfaz
{
    public class VentanaGestionEntidades : Window
    {
        private ListaSimpleUsuarios listaUsuarios;
        private ListaDobleVehiculos listaVehiculos;

        private ComboBoxText comboBoxCategorias;
        private VBox formContainer;
        private TextView textViewInfo;

        private Entry entryID;

        public VentanaGestionEntidades(ListaSimpleUsuarios listaUsuarios, ListaDobleVehiculos listaVehiculos) : base("Gestión de Entidades")
        {
            this.listaUsuarios = listaUsuarios;
            this.listaVehiculos = listaVehiculos;

            // Configuración básica de la ventana
            SetDefaultSize(500, 400);
            SetPosition(WindowPosition.Center);

            VBox mainVBox = new VBox(false, 10);
            mainVBox.BorderWidth = 20;

            // Etiqueta de título
            Label labelTitulo = new Label("<b><span size='16000'>Gestión de Entidades</span></b>");
            labelTitulo.UseMarkup = true;
            mainVBox.PackStart(labelTitulo, false, false, 10);

            // Selector de categoría
            HBox categoryBox = new HBox(false, 10);
            categoryBox.PackStart(new Label("Seleccione tipo de entidad:"), false, false, 0);

            comboBoxCategorias = new ComboBoxText();
            comboBoxCategorias.AppendText("Usuarios");
            comboBoxCategorias.AppendText("Vehículos");
            comboBoxCategorias.Active = 0;
            comboBoxCategorias.Changed += OnCategoriaChanged;
            categoryBox.PackStart(comboBoxCategorias, true, true, 0);

            mainVBox.PackStart(categoryBox, false, false, 10);

            // Contenedor para formularios
            formContainer = new VBox(false, 10);
            mainVBox.PackStart(formContainer, true, true, 10);

            // Crear formularios
            CreateUsuarioForm();
            CreateVehiculoForm();

            // Mostrar formulario inicial
            ShowForm("Usuarios");

            Add(mainVBox);
            ShowAll();
        }

        private void OnCategoriaChanged(object sender, EventArgs e)
        {
            ShowForm(comboBoxCategorias.ActiveText);
        }

        private void ShowForm(string categoria)
        {
            // Limpiar el contenedor de formulario
            foreach (Widget child in formContainer.Children)
            {
                formContainer.Remove(child);
            }

            // Mostrar el formulario correspondiente
            switch (categoria)
            {
                case "Usuarios":
                    formContainer.PackStart(CreateUsuarioForm(), true, true, 0);
                    break;
                case "Vehículos":
                    formContainer.PackStart(CreateVehiculoForm(), true, true, 0);
                    break;
            }

            formContainer.ShowAll();
        }

        private VBox CreateUsuarioForm()
        {
            VBox usuarioForm = new VBox(false, 10);

            // Entrada para ID
            HBox hboxID = new HBox(false, 10);
            hboxID.PackStart(new Label("ID Usuario:"), false, false, 0);
            entryID = new Entry();
            hboxID.PackStart(entryID, true, true, 0);
            usuarioForm.PackStart(hboxID, false, false, 10);

            // Botones para acciones
            Button btnVerUsuario = new Button("Ver Usuario");
            btnVerUsuario.Clicked += OnVerUsuarioClicked;
            usuarioForm.PackStart(btnVerUsuario, false, false, 5);

            Button btnEliminarUsuario = new Button("Eliminar Usuario");
            btnEliminarUsuario.Clicked += OnEliminarUsuarioClicked;
            usuarioForm.PackStart(btnEliminarUsuario, false, false, 5);

            // Área de texto para mostrar información
            textViewInfo = new TextView();
            textViewInfo.Editable = false;
            usuarioForm.PackStart(textViewInfo, true, true, 10);

            return usuarioForm;
        }

        private VBox CreateVehiculoForm()
        {
            VBox vehiculoForm = new VBox(false, 10);

            // Entrada para ID
            HBox hboxID = new HBox(false, 10);
            hboxID.PackStart(new Label("ID Vehículo:"), false, false, 0);
            entryID = new Entry();
            hboxID.PackStart(entryID, true, true, 0);
            vehiculoForm.PackStart(hboxID, false, false, 10);

            // Botones para acciones
            Button btnVerVehiculo = new Button("Ver Vehículo");
            btnVerVehiculo.Clicked += OnVerVehiculoClicked;
            vehiculoForm.PackStart(btnVerVehiculo, false, false, 5);

            Button btnEliminarVehiculo = new Button("Eliminar Vehículo");
            btnEliminarVehiculo.Clicked += OnEliminarVehiculoClicked;
            vehiculoForm.PackStart(btnEliminarVehiculo, false, false, 5);

            // Área de texto para mostrar información
            textViewInfo = new TextView();
            textViewInfo.Editable = false;
            vehiculoForm.PackStart(textViewInfo, true, true, 10);

            return vehiculoForm;
        }

        private void OnVerUsuarioClicked(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(entryID.Text, out id))
            {
                var usuario = listaUsuarios.BuscarUsuario(id);
                if (usuario != null)
                {
                    textViewInfo.Buffer.Text = $"ID: {usuario.Id}\nNombres: {usuario.Nombres}\nApellidos: {usuario.Apellidos}\nCorreo: {usuario.Correo}\nEdad: {usuario.Edad}";
                }
                else
                {
                    MostrarMensaje("Error", "Usuario no encontrado.");
                }
            }
            else
            {
                MostrarMensaje("Error", "ID inválido.");
            }
        }

        private void OnEliminarUsuarioClicked(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(entryID.Text, out id))
            {
                var usuario = listaUsuarios.BuscarUsuario(id);
                if (usuario != null)
                {
                    listaUsuarios.EliminarUsuario(id);
                    MostrarMensaje("Éxito", "Usuario eliminado correctamente.");
                }
                else
                {
                    MostrarMensaje("Error", "Usuario no encontrado.");
                }
            }
            else
            {
                MostrarMensaje("Error", "ID inválido.");
            }
        }

        private void OnVerVehiculoClicked(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(entryID.Text, out id))
            {
                var vehiculo = listaVehiculos.BuscarVehiculo(id);
                if (vehiculo != null)
                {
                    textViewInfo.Buffer.Text = $"ID: {vehiculo.Id}\nID Usuario: {vehiculo.IdUsuario}\nMarca: {vehiculo.Marca}\nModelo: {vehiculo.Modelo}\nPlaca: {vehiculo.Placa}";
                }
                else
                {
                    MostrarMensaje("Error", "Vehículo no encontrado.");
                }
            }
            else
            {
                MostrarMensaje("Error", "ID inválido.");
            }
        }

        private void OnEliminarVehiculoClicked(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(entryID.Text, out id))
            {
                var vehiculo = listaVehiculos.BuscarVehiculo(id);
                if (vehiculo != null)
                {
                    listaVehiculos.EliminarVehiculo(id);
                    MostrarMensaje("Éxito", "Vehículo eliminado correctamente.");
                }
                else
                {
                    MostrarMensaje("Error", "Vehículo no encontrado.");
                }
            }
            else
            {
                MostrarMensaje("Error", "ID inválido.");
            }
        }

        private void MostrarMensaje(string titulo, string mensaje)
        {
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, titulo == "Error" ? MessageType.Error : MessageType.Info, ButtonsType.Ok, mensaje);
            dialog.Run();
            dialog.Destroy();
        }
    }
}