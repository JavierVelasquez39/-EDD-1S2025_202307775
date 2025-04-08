using Gtk;

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

            // Crear un contenedor vertical
            VBox vbox = new VBox(false, 10);

            // Título
            Label lblTitulo = new Label("Bienvenido a AutoGestPro");
            lblTitulo.SetAlignment(0.5f, 0.5f);

            // Campo de correo
            Label lblCorreo = new Label("Correo:");
            Entry txtCorreo = new Entry();

            // Campo de contraseña
            Label lblContrasenia = new Label("Contraseña:");
            Entry txtContrasenia = new Entry
            {
                Visibility = false // Ocultar texto para contraseñas
            };

            // Botón de inicio de sesión
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
                }
                else
                {
                    // Mostrar mensaje de error
                    using (MessageDialog dialog = new MessageDialog(
                        this,
                        DialogFlags.Modal,
                        MessageType.Error,
                        ButtonsType.Ok,
                        "Correo o contraseña incorrectos."
                    ))
                    {
                        dialog.Run();
                    }
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
            ShowAll();
        }
    }
}