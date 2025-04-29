using Gtk;
using System;
using AutoGestPro.Estructuras;

namespace AutoGestPro.Interfaz
{
    public class VentanaGestionEntidades : Window
    {
        private Blockchain blockchainUsuarios;
        private ListaDobleVehiculos listaVehiculos;

        private Entry entryID;
        private TextView textViewInfo;

        public VentanaGestionEntidades(Blockchain blockchainUsuarios, ListaDobleVehiculos listaVehiculos) : base("Gestión de Entidades")
        {
            this.blockchainUsuarios = blockchainUsuarios;
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

            // Entrada para ID
            HBox hboxID = new HBox(false, 10);
            hboxID.PackStart(new Label("ID:"), false, false, 0);
            entryID = new Entry();
            hboxID.PackStart(entryID, true, true, 0);
            mainVBox.PackStart(hboxID, false, false, 10);

            // Botones para acciones
            Button btnVerUsuario = new Button("Ver Usuario");
            btnVerUsuario.Clicked += OnVerUsuarioClicked;
            mainVBox.PackStart(btnVerUsuario, false, false, 5);

            Button btnVerVehiculo = new Button("Ver Vehículo");
            btnVerVehiculo.Clicked += OnVerVehiculoClicked;
            mainVBox.PackStart(btnVerVehiculo, false, false, 5);

            // Área de texto para mostrar información
            textViewInfo = new TextView();
            textViewInfo.Editable = false;
            mainVBox.PackStart(textViewInfo, true, true, 10);

            Add(mainVBox);
            ShowAll();
        }

        private void OnVerUsuarioClicked(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(entryID.Text, out id))
            {
                var usuario = blockchainUsuarios.BuscarUsuario(id);
                if (usuario != null)
                {
                    textViewInfo.Buffer.Text = $"ID: {usuario.Index}\nUsuario: {usuario.Usuario}\nCorreo: {usuario.Correo}\nHash: {usuario.Hash}\nPrevious Hash: {usuario.PreviousHash}";
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

        private void MostrarMensaje(string titulo, string mensaje)
        {
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, titulo == "Error" ? MessageType.Error : MessageType.Info, ButtonsType.Ok, mensaje);
            dialog.Run();
            dialog.Destroy();
        }
    }
}