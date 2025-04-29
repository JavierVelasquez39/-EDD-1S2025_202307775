using System;
using Gtk;
using AutoGestPro.Estructuras;

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
            btnLogin.Clicked += (sender, e) =>
            {
                string correo = txtCorreo.Text?.Trim() ?? "";
                string contrasenia = txtContrasenia.Text ?? "";

                if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasenia))
                {
                    MostrarMensaje("Error", "Por favor ingrese correo y contraseña.");
                    return;
                }

                // Verificar si es el administrador
                if (correo == "admin@usac.com" && contrasenia == "admin123")
                {
                    Console.WriteLine("Inicio de sesión exitoso como administrador.");
                    
                    // Registrar entrada del administrador en el control de logueo
                    DatosCompartidos.ControlLog.RegistrarEntrada("Administrador");
                    
                    // Abrir la ventana de administrador
                    VentanaAdmin ventanaAdmin = new VentanaAdmin();
                    ventanaAdmin.Show();

                    // Cerrar la ventana de login
                    this.Destroy();
                }
                else
                {
                    // Intentar autenticar con el blockchain desde DatosCompartidos
                    Console.WriteLine($"Intentando autenticar usuario: {correo}");
                    Block? bloqueUsuario = DatosCompartidos.BlockchainUsuarios.AutenticarUsuario(correo, contrasenia);
                    
                    if (bloqueUsuario != null)
                    {
                        // Registro exitoso
                        string nombreCompleto = bloqueUsuario.Usuario;
                        Console.WriteLine($"Inicio de sesión exitoso para: {nombreCompleto}");
                        
                        // Guardar el bloque del usuario actual en los datos compartidos
                        DatosCompartidos.UsuarioActual = bloqueUsuario;
                        
                        // Registrar entrada del usuario en el control de logueo
                        DatosCompartidos.ControlLog.RegistrarEntrada(nombreCompleto);
                        
                        // Abrir la ventana de usuario normal
                        VentanaUsuario ventanaUsuario = new VentanaUsuario(bloqueUsuario);
                        ventanaUsuario.Show();

                        // Cerrar la ventana de login
                        this.Destroy();;
                    }
                    else
                    {
                        MostrarMensaje("Error", "Correo o contraseña incorrectos.");
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

        private void MostrarMensaje(string titulo, string mensaje)
        {
            using (MessageDialog dialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                titulo == "Error" ? MessageType.Error : MessageType.Info,
                ButtonsType.Ok,
                mensaje))
            {
                dialog.Run();
                dialog.Destroy();
            }
        }
    }
}