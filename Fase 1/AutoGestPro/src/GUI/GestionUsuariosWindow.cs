using System;
using Gtk;
using AutoGestPro.DataStructures;

namespace AutoGestPro.GUI
{
    public unsafe class GestionUsuariosWindow : Window
    {
        private ListaUsuarios listaUsuarios;
        private ListaVehiculos listaVehiculos;

        private Entry entryUsuarioID;
        private Entry entryUsuarioNombres;
        private Entry entryUsuarioApellidos;
        private Entry entryUsuarioCorreo;
        private TextView textViewUsuarioInfo;

        [Obsolete]
        public GestionUsuariosWindow(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos) : base("Gestión de Usuarios")
        {
            this.listaUsuarios = listaUsuarios;
            this.listaVehiculos = listaVehiculos;

            SetDefaultSize(500, 500);
            SetPosition(WindowPosition.Center);
            DeleteEvent += delegate { this.Hide(); };

            VBox vbox = new VBox(false, 10);
            vbox.BorderWidth = 20;

            Label labelTitulo = new Label("<b><span size='16000'>Gestión de Usuarios</span></b>");
            labelTitulo.UseMarkup = true;
            vbox.PackStart(labelTitulo, false, false, 10);

            // Ver Usuario
            HBox hboxVer = new HBox(false, 10);
            hboxVer.PackStart(new Label("ID Usuario:"), false, false, 0);
            entryUsuarioID = new Entry();
            hboxVer.PackStart(entryUsuarioID, true, true, 0);
            Button btnVerUsuario = new Button("Ver Usuario");
            btnVerUsuario.Clicked += OnVerUsuarioClicked;
            hboxVer.PackStart(btnVerUsuario, false, false, 0);
            vbox.PackStart(hboxVer, false, false, 10);

            textViewUsuarioInfo = new TextView();
            textViewUsuarioInfo.Editable = false;
            vbox.PackStart(textViewUsuarioInfo, true, true, 10);

            // Editar Usuario
            Label labelEditar = new Label("<b>Editar Usuario</b>");
            labelEditar.UseMarkup = true;
            vbox.PackStart(labelEditar, false, false, 10);

            Table tableEditar = new Table(3, 2, false);
            tableEditar.RowSpacing = 10;
            tableEditar.ColumnSpacing = 10;

            Label labelNombres = new Label("Nombres:");
            labelNombres.Xalign = 1.0f;
            tableEditar.Attach(labelNombres, 0, 1, 0, 1);
            entryUsuarioNombres = new Entry();
            tableEditar.Attach(entryUsuarioNombres, 1, 2, 0, 1);

            Label labelApellidos = new Label("Apellidos:");
            labelApellidos.Xalign = 1.0f;
            tableEditar.Attach(labelApellidos, 0, 1, 1, 2);
            entryUsuarioApellidos = new Entry();
            tableEditar.Attach(entryUsuarioApellidos, 1, 2, 1, 2);

            Label labelCorreo = new Label("Correo:");
            labelCorreo.Xalign = 1.0f;
            tableEditar.Attach(labelCorreo, 0, 1, 2, 3);
            entryUsuarioCorreo = new Entry();
            tableEditar.Attach(entryUsuarioCorreo, 1, 2, 2, 3);

            vbox.PackStart(tableEditar, false, false, 10);

            Button btnEditarUsuario = new Button("Editar Usuario");
            btnEditarUsuario.Clicked += OnEditarUsuarioClicked;
            vbox.PackStart(btnEditarUsuario, false, false, 10);

            // Eliminar Usuario
            Label labelEliminar = new Label("<b>Eliminar Usuario</b>");
            labelEliminar.UseMarkup = true;
            vbox.PackStart(labelEliminar, false, false, 10);

            HBox hboxEliminar = new HBox(false, 10);
            hboxEliminar.PackStart(new Label("ID Usuario:"), false, false, 0);
            Entry entryEliminarUsuarioID = new Entry();
            hboxEliminar.PackStart(entryEliminarUsuarioID, true, true, 0);
            Button btnEliminarUsuario = new Button("Eliminar Usuario");
            btnEliminarUsuario.Clicked += (sender, e) => OnEliminarUsuarioClicked(entryEliminarUsuarioID.Text);
            hboxEliminar.PackStart(btnEliminarUsuario, false, false, 0);
            vbox.PackStart(hboxEliminar, false, false, 10);

            Add(vbox);
            ShowAll();
        }

        private void OnVerUsuarioClicked(object? sender, EventArgs e)
        {
            int id;
            if (int.TryParse(entryUsuarioID.Text, out id))
            {
                NodoUsuario* usuario = listaUsuarios.BuscarUsuario(id);
                if (usuario != null)
                {
                    string info = $"ID: {usuario->ID}\nNombres: {GetString(usuario->Nombres)}\nApellidos: {GetString(usuario->Apellidos)}\nCorreo: {GetString(usuario->Correo)}\n\nVehículos:\n";
                    var vehiculos = listaVehiculos.BuscarVehiculosPorUsuario(id);
                    foreach (var vehiculo in vehiculos)
                    {
                        info += $"- ID: {vehiculo.ID}, Marca: {vehiculo.Marca}, Modelo: {vehiculo.Modelo}, Placa: {vehiculo.Placa}\n";
                    }
                    textViewUsuarioInfo.Buffer.Text = info;
                }
                else
                {
                    MostrarMensaje("Error", "Usuario no encontrado");
                }
            }
            else
            {
                MostrarMensaje("Error", "ID inválido");
            }
        }

        private void OnEditarUsuarioClicked(object? sender, EventArgs e)
        {
            int id;
            if (int.TryParse(entryUsuarioID.Text, out id))
            {
                NodoUsuario* usuario = listaUsuarios.BuscarUsuario(id);
                if (usuario != null)
                {
                    string nombres = entryUsuarioNombres.Text;
                    string apellidos = entryUsuarioApellidos.Text;
                    string correo = entryUsuarioCorreo.Text;

                    if (string.IsNullOrEmpty(nombres) || string.IsNullOrEmpty(apellidos) || string.IsNullOrEmpty(correo))
                    {
                        MostrarMensaje("Error", "Todos los campos son obligatorios");
                        return;
                    }

                    listaUsuarios.EditarUsuario(id, nombres, apellidos, correo);
                    MostrarMensaje("Éxito", "Usuario editado correctamente");
                }
                else
                {
                    MostrarMensaje("Error", "Usuario no encontrado");
                }
            }
            else
            {
                MostrarMensaje("Error", "ID inválido");
            }
        }

        private void OnEliminarUsuarioClicked(string idText)
        {
            int id;
            if (int.TryParse(idText, out id))
            {
                NodoUsuario* usuario = listaUsuarios.BuscarUsuario(id);
                if (usuario != null)
                {
                    listaUsuarios.EliminarUsuario(id);
                    MostrarMensaje("Éxito", "Usuario eliminado correctamente");
                }
                else
                {
                    MostrarMensaje("Error", "Usuario no encontrado");
                }
            }
            else
            {
                MostrarMensaje("Error", "ID inválido");
            }
        }

        private void MostrarMensaje(string titulo, string mensaje)
        {
            MessageDialog dialog = new MessageDialog(this, 
                DialogFlags.Modal, 
                titulo == "Error" ? MessageType.Error : MessageType.Info, 
                ButtonsType.Ok, 
                mensaje);
            
            dialog.Run();
            dialog.Destroy();
        }

        private static string GetString(char* ptr)
        {
            return new string(ptr);
        }
    }
}