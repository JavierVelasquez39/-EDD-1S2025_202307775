using System;
using Gtk;
using AutoGestPro.DataStructures;

namespace AutoGestPro.GUI
{
    public class IngresoManualWindow : Window
    {
        private ListaUsuarios listaUsuarios;
        private ListaVehiculos listaVehiculos;
        private ListaRepuestos listaRepuestos;
        private ColaServicios colaServicios;

        // Widgets principales
        private ComboBoxText comboBoxCategorias;
        private VBox mainVBox;
        private VBox formContainer;
        private Label titleLabel;

        // Contenedores para cada tipo de formulario
        [Obsolete]
        private VBox usuarioForm = new VBox();
        [Obsolete]
        private VBox vehiculoForm = new VBox();
        [Obsolete]
        private VBox repuestoForm = new VBox();
        [Obsolete]
        private VBox servicioForm = new VBox();

        // Controles para Usuario
        private Entry entryUsuarioID = new Entry();
        private Entry entryUsuarioNombres = new Entry();
        private Entry entryUsuarioApellidos = new Entry();
        private Entry entryUsuarioCorreo = new Entry();
        private Entry entryUsuarioContrasenia = new Entry();

        // Controles para Vehículo
        private Entry entryVehiculoID = new Entry();
        private Entry entryVehiculoIDUsuario = new Entry();
        private Entry entryVehiculoMarca = new Entry();
        private Entry entryVehiculoModelo = new Entry();
        private Entry entryVehiculoPlaca = new Entry();

        // Controles para Repuesto
        private Entry entryRepuestoID = new Entry();
        private Entry entryRepuestoNombre = new Entry();
        private Entry entryRepuestoDetalles = new Entry();
        private Entry entryRepuestoCosto = new Entry();

        // Controles para Servicio
        private Entry entryServicioID = new Entry();
        private Entry entryServicioIDRepuesto = new Entry();
        private Entry entryServicioIDVehiculo = new Entry();
        private Entry entryServicioDetalles = new Entry();
        private Entry entryServicioCosto = new Entry();

        [Obsolete]
        public IngresoManualWindow(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos, ListaRepuestos listaRepuestos, ColaServicios colaServicios) : base("Ingreso Manual")
        {
            this.listaUsuarios = listaUsuarios;
            this.listaVehiculos = listaVehiculos;
            this.listaRepuestos = listaRepuestos;
            this.colaServicios = colaServicios;

            SetDefaultSize(500, 500);
            SetPosition(WindowPosition.Center);
            DeleteEvent += delegate { this.Hide(); };

            // Crear estructura principal
            mainVBox = new VBox(false, 10);
            mainVBox.BorderWidth = 20;

            // Título
            titleLabel = new Label("<b><span size='16000'>Ingreso Manual</span></b>");
            titleLabel.UseMarkup = true;
            mainVBox.PackStart(titleLabel, false, false, 10);

            // Descripción
            Label descriptionLabel = new Label("El usuario root podrá realizar el ingreso de forma manual de las siguientes entidades \"Usuarios\", \"Vehículos\", \"Repuestos\" y \"Servicios\".");
            descriptionLabel.LineWrap = true;
            mainVBox.PackStart(descriptionLabel, false, false, 10);

            // Selector de categoría
            HBox categoryBox = new HBox(false, 10);
            categoryBox.PackStart(new Label("Seleccione tipo de entidad:"), false, false, 0);
            
            comboBoxCategorias = new ComboBoxText();
            comboBoxCategorias.AppendText("Usuarios");
            comboBoxCategorias.AppendText("Vehículos");
            comboBoxCategorias.AppendText("Repuestos");
            comboBoxCategorias.AppendText("Servicios");
            comboBoxCategorias.Active = 0;
            comboBoxCategorias.Changed += OnCategoriaChanged;
            categoryBox.PackStart(comboBoxCategorias, true, true, 0);
            
            mainVBox.PackStart(categoryBox, false, false, 10);

            // Contenedor para formularios (cambiará según la categoría seleccionada)
            Frame formFrame = new Frame();
            formFrame.ShadowType = ShadowType.EtchedIn;
            
            formContainer = new VBox(false, 0);
            formFrame.Add(formContainer);
            
            mainVBox.PackStart(formFrame, true, true, 0);

            // Crear formularios
            CreateUsuarioForm();
            CreateVehiculoForm();
            CreateRepuestoForm();
            CreateServicioForm();

            // Mostrar formulario inicial
            ShowForm("Usuarios");

            Add(mainVBox);
            ShowAll();
        }

        [Obsolete]
        private void OnCategoriaChanged(object? sender, EventArgs e)
        {
            ShowForm(comboBoxCategorias.ActiveText);
        }

        [Obsolete]
        private void ShowForm(string categoria)
        {
            // Limpiar el contenedor de formulario
            foreach (Widget child in formContainer.Children)
            {
                formContainer.Remove(child);
            }

            // Actualizar título del formulario
            titleLabel.Markup = $"<b><span size='16000'>Ingreso Manual - {categoria}</span></b>";

            // Mostrar el formulario correspondiente
            switch (categoria)
            {
                case "Usuarios":
                    formContainer.PackStart(usuarioForm, true, true, 0);
                    break;
                case "Vehículos":
                    formContainer.PackStart(vehiculoForm, true, true, 0);
                    break;
                case "Repuestos":
                    formContainer.PackStart(repuestoForm, true, true, 0);
                    break;
                case "Servicios":
                    formContainer.PackStart(servicioForm, true, true, 0);
                    break;
            }

            formContainer.ShowAll();
        }

        [Obsolete]
        private void CreateUsuarioForm()
        {
            usuarioForm = new VBox(false, 10);
            usuarioForm.BorderWidth = 15;

            Label formTitle = new Label("<b>Ingreso de Usuario</b>");
            formTitle.UseMarkup = true;
            formTitle.Xalign = 0.5f;
            usuarioForm.PackStart(formTitle, false, false, 10);

            // Crear tabla para organizar los campos
            Table table = new Table(5, 2, false);
            table.RowSpacing = 10;
            table.ColumnSpacing = 10;

            // ID
            Label labelID = new Label("ID:");
            labelID.Xalign = 1.0f;
            table.Attach(labelID, 0, 1, 0, 1);
            
            entryUsuarioID = new Entry();
            table.Attach(entryUsuarioID, 1, 2, 0, 1);

            // Nombres
            Label labelNombres = new Label("Nombres:");
            labelNombres.Xalign = 1.0f;
            table.Attach(labelNombres, 0, 1, 1, 2);
            
            entryUsuarioNombres = new Entry();
            table.Attach(entryUsuarioNombres, 1, 2, 1, 2);

            // Apellidos
            Label labelApellidos = new Label("Apellidos:");
            labelApellidos.Xalign = 1.0f;
            table.Attach(labelApellidos, 0, 1, 2, 3);
            
            entryUsuarioApellidos = new Entry();
            table.Attach(entryUsuarioApellidos, 1, 2, 2, 3);

            // Correo
            Label labelCorreo = new Label("Correo:");
            labelCorreo.Xalign = 1.0f;
            table.Attach(labelCorreo, 0, 1, 3, 4);
            
            entryUsuarioCorreo = new Entry();
            table.Attach(entryUsuarioCorreo, 1, 2, 3, 4);

            // Contraseña
            Label labelContrasenia = new Label("Contraseña:");
            labelContrasenia.Xalign = 1.0f;
            table.Attach(labelContrasenia, 0, 1, 4, 5);
            
            entryUsuarioContrasenia = new Entry();
            entryUsuarioContrasenia.Visibility = false;
            table.Attach(entryUsuarioContrasenia, 1, 2, 4, 5);

            usuarioForm.PackStart(table, true, true, 0);

            // Botón Guardar
            Button btnGuardar = new Button("Guardar");
            btnGuardar.HeightRequest = 40;
            btnGuardar.WidthRequest = 120;

            // Estilo verde para el botón
            CssProvider cssProvider = new CssProvider();
            cssProvider.LoadFromData("button { background: #1ED760; color: white; border-radius: 5px; }");
            btnGuardar.StyleContext.AddProvider(cssProvider, uint.MaxValue);

            btnGuardar.Clicked += OnGuardarUsuarioClicked;

            HBox buttonBox = new HBox(false, 0);
            buttonBox.PackStart(new Label(""), true, true, 0);
            buttonBox.PackStart(btnGuardar, false, false, 0);
            buttonBox.PackStart(new Label(""), true, true, 0);

            usuarioForm.PackStart(buttonBox, false, false, 10);
        }

        [Obsolete]
        private void CreateVehiculoForm()
        {
            vehiculoForm = new VBox(false, 10);
            vehiculoForm.BorderWidth = 15;

            Label formTitle = new Label("<b>Ingreso de Vehículo</b>");
            formTitle.UseMarkup = true;
            formTitle.Xalign = 0.5f;
            vehiculoForm.PackStart(formTitle, false, false, 10);

            // Crear tabla para organizar los campos
            Table table = new Table(5, 2, false);
            table.RowSpacing = 10;
            table.ColumnSpacing = 10;

            // ID
            Label labelID = new Label("ID:");
            labelID.Xalign = 1.0f;
            table.Attach(labelID, 0, 1, 0, 1);
            
            entryVehiculoID = new Entry();
            table.Attach(entryVehiculoID, 1, 2, 0, 1);

            // ID Usuario
            Label labelIDUsuario = new Label("ID Usuario:");
            labelIDUsuario.Xalign = 1.0f;
            table.Attach(labelIDUsuario, 0, 1, 1, 2);
            
            entryVehiculoIDUsuario = new Entry();
            table.Attach(entryVehiculoIDUsuario, 1, 2, 1, 2);

            // Marca
            Label labelMarca = new Label("Marca:");
            labelMarca.Xalign = 1.0f;
            table.Attach(labelMarca, 0, 1, 2, 3);
            
            entryVehiculoMarca = new Entry();
            table.Attach(entryVehiculoMarca, 1, 2, 2, 3);

            // Modelo
            Label labelModelo = new Label("Modelo:");
            labelModelo.Xalign = 1.0f;
            table.Attach(labelModelo, 0, 1, 3, 4);
            
            entryVehiculoModelo = new Entry();
            table.Attach(entryVehiculoModelo, 1, 2, 3, 4);

            // Placa
            Label labelPlaca = new Label("Placa:");
            labelPlaca.Xalign = 1.0f;
            table.Attach(labelPlaca, 0, 1, 4, 5);
            
            entryVehiculoPlaca = new Entry();
            table.Attach(entryVehiculoPlaca, 1, 2, 4, 5);

            vehiculoForm.PackStart(table, true, true, 0);

            // Botón Guardar
            Button btnGuardar = new Button("Guardar");
            btnGuardar.HeightRequest = 40;
            btnGuardar.WidthRequest = 120;

            // Estilo verde para el botón
            CssProvider cssProvider = new CssProvider();
            cssProvider.LoadFromData("button { background: #1ED760; color: white; border-radius: 5px; }");
            btnGuardar.StyleContext.AddProvider(cssProvider, uint.MaxValue);

            btnGuardar.Clicked += OnGuardarVehiculoClicked;

            HBox buttonBox = new HBox(false, 0);
            buttonBox.PackStart(new Label(""), true, true, 0);
            buttonBox.PackStart(btnGuardar, false, false, 0);
            buttonBox.PackStart(new Label(""), true, true, 0);

            vehiculoForm.PackStart(buttonBox, false, false, 10);
        }

        [Obsolete]
        private void CreateRepuestoForm()
        {
            repuestoForm = new VBox(false, 10);
            repuestoForm.BorderWidth = 15;

            Label formTitle = new Label("<b>Ingreso de Repuesto</b>");
            formTitle.UseMarkup = true;
            formTitle.Xalign = 0.5f;
            repuestoForm.PackStart(formTitle, false, false, 10);

            // Crear tabla para organizar los campos
            Table table = new Table(4, 2, false);
            table.RowSpacing = 10;
            table.ColumnSpacing = 10;

            // ID
            Label labelID = new Label("ID:");
            labelID.Xalign = 1.0f;
            table.Attach(labelID, 0, 1, 0, 1);
            
            entryRepuestoID = new Entry();
            table.Attach(entryRepuestoID, 1, 2, 0, 1);

            // Nombre
            Label labelNombre = new Label("Nombre:");
            labelNombre.Xalign = 1.0f;
            table.Attach(labelNombre, 0, 1, 1, 2);
            
            entryRepuestoNombre = new Entry();
            table.Attach(entryRepuestoNombre, 1, 2, 1, 2);

            // Detalles
            Label labelDetalles = new Label("Detalles:");
            labelDetalles.Xalign = 1.0f;
            table.Attach(labelDetalles, 0, 1, 2, 3);
            
            entryRepuestoDetalles = new Entry();
            table.Attach(entryRepuestoDetalles, 1, 2, 2, 3);

            // Costo
            Label labelCosto = new Label("Costo:");
            labelCosto.Xalign = 1.0f;
            table.Attach(labelCosto, 0, 1, 3, 4);
            
            entryRepuestoCosto = new Entry();
            table.Attach(entryRepuestoCosto, 1, 2, 3, 4);

            repuestoForm.PackStart(table, true, true, 0);

            // Botón Guardar
            Button btnGuardar = new Button("Guardar");
            btnGuardar.HeightRequest = 40;
            btnGuardar.WidthRequest = 120;

            // Estilo verde para el botón
            CssProvider cssProvider = new CssProvider();
            cssProvider.LoadFromData("button { background: #1ED760; color: white; border-radius: 5px; }");
            btnGuardar.StyleContext.AddProvider(cssProvider, uint.MaxValue);

            btnGuardar.Clicked += OnGuardarRepuestoClicked;

            HBox buttonBox = new HBox(false, 0);
            buttonBox.PackStart(new Label(""), true, true, 0);
            buttonBox.PackStart(btnGuardar, false, false, 0);
            buttonBox.PackStart(new Label(""), true, true, 0);

            repuestoForm.PackStart(buttonBox, false, false, 10);
        }

        [Obsolete]
        private void CreateServicioForm()
        {
            servicioForm = new VBox(false, 10);
            servicioForm.BorderWidth = 15;

            Label formTitle = new Label("<b>Ingreso de Servicio</b>");
            formTitle.UseMarkup = true;
            formTitle.Xalign = 0.5f;
            servicioForm.PackStart(formTitle, false, false, 10);

            // Crear tabla para organizar los campos
            Table table = new Table(5, 2, false);
            table.RowSpacing = 10;
            table.ColumnSpacing = 10;

            // ID
            Label labelID = new Label("ID:");
            labelID.Xalign = 1.0f;
            table.Attach(labelID, 0, 1, 0, 1);
            
            entryServicioID = new Entry();
            table.Attach(entryServicioID, 1, 2, 0, 1);

            // ID Repuesto
            Label labelIDRepuesto = new Label("ID Repuesto:");
            labelIDRepuesto.Xalign = 1.0f;
            table.Attach(labelIDRepuesto, 0, 1, 1, 2);
            
            entryServicioIDRepuesto = new Entry();
            table.Attach(entryServicioIDRepuesto, 1, 2, 1, 2);

            // ID Vehículo
            Label labelIDVehiculo = new Label("ID Vehículo:");
            labelIDVehiculo.Xalign = 1.0f;
            table.Attach(labelIDVehiculo, 0, 1, 2, 3);
            
            entryServicioIDVehiculo = new Entry();
            table.Attach(entryServicioIDVehiculo, 1, 2, 2, 3);

            // Detalles
            Label labelDetalles = new Label("Detalles:");
            labelDetalles.Xalign = 1.0f;
            table.Attach(labelDetalles, 0, 1, 3, 4);
            
            entryServicioDetalles = new Entry();
            table.Attach(entryServicioDetalles, 1, 2, 3, 4);

            // Costo
            Label labelCosto = new Label("Costo:");
            labelCosto.Xalign = 1.0f;
            table.Attach(labelCosto, 0, 1, 4, 5);
            
            entryServicioCosto = new Entry();
            table.Attach(entryServicioCosto, 1, 2, 4, 5);

            servicioForm.PackStart(table, true, true, 0);

            // Botón Guardar
            Button btnGuardar = new Button("Guardar");
            btnGuardar.HeightRequest = 40;
            btnGuardar.WidthRequest = 120;

            // Estilo verde para el botón
            CssProvider cssProvider = new CssProvider();
            cssProvider.LoadFromData("button { background: #1ED760; color: white; border-radius: 5px; }");
            btnGuardar.StyleContext.AddProvider(cssProvider, uint.MaxValue);

            btnGuardar.Clicked += OnGuardarServicioClicked;

            HBox buttonBox = new HBox(false, 0);
            buttonBox.PackStart(new Label(""), true, true, 0);
            buttonBox.PackStart(btnGuardar, false, false, 0);
            buttonBox.PackStart(new Label(""), true, true, 0);

            servicioForm.PackStart(buttonBox, false, false, 10);
        }

        private unsafe void OnGuardarUsuarioClicked(object? sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(entryUsuarioID.Text);
                string nombres = entryUsuarioNombres.Text;
                string apellidos = entryUsuarioApellidos.Text;
                string correo = entryUsuarioCorreo.Text;
                string contrasenia = entryUsuarioContrasenia.Text;

                if (string.IsNullOrEmpty(nombres) || string.IsNullOrEmpty(apellidos) || 
                    string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasenia))
                {
                    MostrarMensaje("Error", "Todos los campos son obligatorios");
                    return;
                }

                // Validar si el ID ya existe
                if (listaUsuarios.BuscarUsuario(id) != null)
                {
                    MostrarMensaje("Error", "El ID de usuario ya existe");
                    return;
                }

                listaUsuarios.AgregarUsuario(id, nombres, apellidos, correo, contrasenia);
                MostrarMensaje("Éxito", "Usuario agregado correctamente");
                LimpiarFormularioUsuario();

                listaUsuarios.MostrarUsuarios();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error", "Datos inválidos: " + ex.Message);
            }
        }

        private unsafe void OnGuardarVehiculoClicked(object? sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(entryVehiculoID.Text);
                int idUsuario = int.Parse(entryVehiculoIDUsuario.Text);
                string marca = entryVehiculoMarca.Text;
                string modelo = entryVehiculoModelo.Text;
                string placa = entryVehiculoPlaca.Text;

                if (string.IsNullOrEmpty(marca) || string.IsNullOrEmpty(modelo) || string.IsNullOrEmpty(placa))
                {
                    MostrarMensaje("Error", "Todos los campos son obligatorios");
                    return;
                }

                // Validar si el ID del vehículo ya existe
                if (listaVehiculos.BuscarVehiculo(id) != null)
                {
                    MostrarMensaje("Error", "El ID del vehículo ya existe");
                    return;
                }

                // Validar si el ID del usuario existe
                if (listaUsuarios.BuscarUsuario(idUsuario) == null)
                {
                    MostrarMensaje("Error", "El ID de usuario no existe");
                    return;
                }

                listaVehiculos.AgregarVehiculo(id, idUsuario, marca, modelo, placa);
                MostrarMensaje("Éxito", "Vehículo agregado correctamente");
                LimpiarFormularioVehiculo();

                listaVehiculos.MostrarVehiculos();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error", "Datos inválidos: " + ex.Message);
            }
        }

        private void OnGuardarRepuestoClicked(object? sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(entryRepuestoID.Text);
                string nombre = entryRepuestoNombre.Text;
                string detalles = entryRepuestoDetalles.Text;
                float costo = float.Parse(entryRepuestoCosto.Text);

                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(detalles))
                {
                    MostrarMensaje("Error", "Todos los campos son obligatorios");
                    return;
                }

                listaRepuestos.AgregarRepuesto(id, nombre, detalles, costo);
                MostrarMensaje("Éxito", "Repuesto agregado correctamente");
                LimpiarFormularioRepuesto();

                listaRepuestos.MostrarRepuestos();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error", "Datos inválidos: " + ex.Message);
            }
        }

        private void OnGuardarServicioClicked(object? sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(entryServicioID.Text);
                int idRepuesto = int.Parse(entryServicioIDRepuesto.Text);
                int idVehiculo = int.Parse(entryServicioIDVehiculo.Text);
                string detalles = entryServicioDetalles.Text;
                float costo = float.Parse(entryServicioCosto.Text);

                if (string.IsNullOrEmpty(detalles))
                {
                    MostrarMensaje("Error", "Todos los campos son obligatorios");
                    return;
                }

                colaServicios.EncolarServicio(id, idRepuesto, idVehiculo, detalles, costo);
                MostrarMensaje("Éxito", "Servicio agregado correctamente");
                LimpiarFormularioServicio();

                colaServicios.MostrarServicios();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error", "Datos inválidos: " + ex.Message);
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

        private void LimpiarFormularioUsuario()
        {
            entryUsuarioID.Text = "";
            entryUsuarioNombres.Text = "";
            entryUsuarioApellidos.Text = "";
            entryUsuarioCorreo.Text = "";
            entryUsuarioContrasenia.Text = "";
        }

        private void LimpiarFormularioVehiculo()
        {
            entryVehiculoID.Text = "";
            entryVehiculoIDUsuario.Text = "";
            entryVehiculoMarca.Text = "";
            entryVehiculoModelo.Text = "";
            entryVehiculoPlaca.Text = "";
        }

        private void LimpiarFormularioRepuesto()
        {
            entryRepuestoID.Text = "";
            entryRepuestoNombre.Text = "";
            entryRepuestoDetalles.Text = "";
            entryRepuestoCosto.Text = "";
        }

        private void LimpiarFormularioServicio()
        {
            entryServicioID.Text = "";
            entryServicioIDRepuesto.Text = "";
            entryServicioIDVehiculo.Text = "";
            entryServicioDetalles.Text = "";
            entryServicioCosto.Text = "";
        }
    }
}