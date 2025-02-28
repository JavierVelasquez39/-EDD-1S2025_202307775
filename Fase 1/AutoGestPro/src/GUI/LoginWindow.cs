using System;
using Gtk;
using AutoGestPro.DataStructures;

namespace AutoGestPro.GUI
{
    public class LoginWindow : Window
    {
        private Entry entryUsuario;
        private Entry entryContrasenia;
        private ListaUsuarios listaUsuarios;
        private ListaVehiculos listaVehiculos;
        private ListaRepuestos listaRepuestos;
        private ColaServicios colaServicios;

        [Obsolete]
        public LoginWindow(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos, ListaRepuestos listaRepuestos, ColaServicios colaServicios) : base("AutoGest Pro - Login")
        {
            // Inicializar las listas y la cola
            this.listaUsuarios = listaUsuarios;
            this.listaVehiculos = listaVehiculos;
            this.listaRepuestos = listaRepuestos;
            this.colaServicios = colaServicios;

            SetDefaultSize(300, 200);
            SetPosition(WindowPosition.Center);
            DeleteEvent += delegate { Application.Quit(); };

            VBox vbox = new VBox(false, 10);
            vbox.BorderWidth = 20;

            Label labelUsuario = new Label("Usuario:");
            vbox.PackStart(labelUsuario, false, false, 5);

            entryUsuario = new Entry();
            vbox.PackStart(entryUsuario, false, false, 5);

            Label labelContrasenia = new Label("Contraseña:");
            vbox.PackStart(labelContrasenia, false, false, 5);

            entryContrasenia = new Entry();
            entryContrasenia.Visibility = false;
            vbox.PackStart(entryContrasenia, false, false, 5);

            Button btnLogin = new Button("Login");
            btnLogin.Clicked += OnLoginClicked;
            vbox.PackStart(btnLogin, false, false, 5);

            Add(vbox);
            ShowAll();
        }

        [Obsolete]
        private void OnLoginClicked(object? sender, EventArgs e)
        {
            string usuario = entryUsuario.Text;
            string contrasenia = entryContrasenia.Text;

            // Aquí puedes agregar la lógica para validar el usuario y la contraseña

            // Si la validación es exitosa, abrir la ventana principal
            var mainWindow = new MainWindow(listaUsuarios, listaVehiculos, listaRepuestos, colaServicios);
            mainWindow.Show();
            this.Hide();
        }
    }
}