using Gtk;
using AutoGestPro;
using AutoGestPro.Estructuras;
using AutoGestPro.Modelos;

namespace AutoGestPro.Interfaz
{
    public class VentanaLogin : Window
    {
        [Obsolete]
        public VentanaLogin() : base("Inicio de Sesión")
        {
            // Configuración de la ventana
            SetDefaultSize(400, 250);
            SetPosition(WindowPosition.Center);

            VBox vbox = new VBox(false, 10);

            Label lblTitulo = new Label("Bienvenido a AutoGestPro");
            lblTitulo.SetAlignment(0.5f, 0.5f);

            Label lblCorreo = new Label("Correo:");
            Entry txtCorreo = new Entry();

            Label lblContrasenia = new Label("Contraseña:");
            Entry txtContrasenia = new Entry();
            txtContrasenia.Visibility = false;

            Button btnLogin = new Button("Iniciar Sesión");

            // Evento del botón de inicio de sesión
            btnLogin.Clicked += (sender, e) =>
            {
                string correo = txtCorreo.Text;
                string contrasenia = txtContrasenia.Text;

                // Verificar si es el administrador
                if (correo == "admin@usac.com" && contrasenia == "admint123")
                {
                    Console.WriteLine("Inicio de sesión exitoso como administrador.");
                    
                    // Registrar entrada del administrador
                    DatosCompartidos.ControlLog.RegistrarEntrada(correo);

                    // Asignar el administrador como usuario actual
                    DatosCompartidos.UsuarioActual = new Usuario(0, correo, "Administrador", "admin@usac.com", 1, "AdminRole");

                    Destroy();
                    VentanaAdmin ventanaAdmin = new VentanaAdmin();
                    ventanaAdmin.Show();
                }
                else
                {
                    // Buscar al usuario en la lista de usuarios
                    Usuario? usuario = DatosCompartidos.ListaUsuarios.BuscarUsuarioPorCredenciales(correo, contrasenia);

                    if (usuario != null)
                    {
                        Console.WriteLine($"Inicio de sesión exitoso como usuario: {usuario.Correo}");
                        
                        // Registrar entrada del usuario
                        DatosCompartidos.ControlLog.RegistrarEntrada(correo);

                        // Asignar el usuario como usuario actual
                        DatosCompartidos.UsuarioActual = usuario;

                        Destroy();
                        VentanaUsuarios ventanaUsuarios = new VentanaUsuarios();
                        ventanaUsuarios.Show();
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
                }
            };

            vbox.PackStart(lblTitulo, false, false, 10);
            vbox.PackStart(lblCorreo, false, false, 5);
            vbox.PackStart(txtCorreo, false, false, 5);
            vbox.PackStart(lblContrasenia, false, false, 5);
            vbox.PackStart(txtContrasenia, false, false, 5);
            vbox.PackStart(btnLogin, false, false, 10);

            Add(vbox);
            ShowAll();
        }
    }
}