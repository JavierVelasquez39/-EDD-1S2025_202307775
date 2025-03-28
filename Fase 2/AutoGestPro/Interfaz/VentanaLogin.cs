using Gtk;
using System;
using AutoGestPro.Estructuras;

namespace AutoGestPro.Interfaz
{
    public class VentanaLogin : Window
    {
        private ListaSimpleUsuarios listaUsuarios;
        private ListaDobleVehiculos listaVehiculos;
        private ArbolAVLRepuestos arbolRepuestos;
        private ArbolBinarioServicios arbolServicios;
        private ArbolBFacturas arbolFacturas;

        [Obsolete]
        public VentanaLogin(ListaSimpleUsuarios listaUsuarios, ListaDobleVehiculos listaVehiculos, ArbolAVLRepuestos arbolRepuestos) : base("Inicio de Sesión")
        {
            this.listaUsuarios = listaUsuarios;
            this.listaVehiculos = listaVehiculos;
            this.arbolRepuestos = arbolRepuestos;

            // Inicializar estructuras adicionales
            this.arbolServicios = new ArbolBinarioServicios();
            this.arbolFacturas = new ArbolBFacturas();

            // Configuración de la ventana
            SetDefaultSize(400, 250);
            SetPosition(WindowPosition.Center);

            // Crear un contenedor vertical
            VBox vbox = new VBox(false, 10);

            // Etiqueta de bienvenida
            Label lblTitulo = new Label("Bienvenido a AutoGestPro");
            lblTitulo.SetAlignment(0.5f, 0.5f);

            // Campo de texto para correo
            Label lblCorreo = new Label("Correo:");
            Entry txtCorreo = new Entry();

            // Campo de texto para contraseña
            Label lblContrasenia = new Label("Contraseña:");
            Entry txtContrasenia = new Entry();
            txtContrasenia.Visibility = false; // Ocultar texto

            // Botón de inicio de sesión
            Button btnLogin = new Button("Iniciar Sesión");
            btnLogin.Clicked += (sender, e) =>
            {
                string correo = txtCorreo.Text;
                string contrasenia = txtContrasenia.Text;

                // Validar credenciales
                if (correo == "admin@usac.com" && contrasenia == "admin123")
                {
                    Console.WriteLine("Inicio de sesión exitoso como administrador.");
                    Destroy(); // Cerrar la ventana actual
                    VentanaAdmin ventanaAdmin = new VentanaAdmin(listaUsuarios, listaVehiculos, arbolRepuestos, arbolServicios, arbolFacturas);
                    ventanaAdmin.Show();
                }
                else
                {
                    MessageDialog dialog = new MessageDialog(
                        this,
                        DialogFlags.Modal,
                        MessageType.Error,
                        ButtonsType.Ok,
                        "Correo o contraseña incorrectos."
                    );
                    dialog.Run();
                    dialog.Destroy();
                }
            };

            // Agregar elementos al contenedor
            vbox.PackStart(lblTitulo, false, false, 10);
            vbox.PackStart(lblCorreo, false, false, 5);
            vbox.PackStart(txtCorreo, false, false, 5);
            vbox.PackStart(lblContrasenia, false, false, 5);
            vbox.PackStart(txtContrasenia, false, false, 5);
            vbox.PackStart(btnLogin, false, false, 10);

            // Agregar el contenedor a la ventana
            Add(vbox);

            // Mostrar todos los elementos
            ShowAll();
        }
    }
}