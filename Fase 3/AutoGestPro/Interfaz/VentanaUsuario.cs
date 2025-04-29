using Gtk;
using System;
using AutoGestPro;
using AutoGestPro.Estructuras;

namespace AutoGestPro.Interfaz
{
    public class VentanaUsuario : Window
    {
        private Block usuarioActual; // Bloque del usuario actual

        private Notebook notebook;
        private TreeView treeViewVehiculos;
        private TreeView treeViewServicios;
        private TreeView treeViewFacturas;
        private ComboBoxText comboRecorrido;

        [Obsolete]
        public VentanaUsuario(Block usuarioActual) : base("Panel de Usuario")
        {
            this.usuarioActual = usuarioActual;

            // Configuración básica de la ventana
            SetDefaultSize(800, 600);
            SetPosition(WindowPosition.Center);

            // Crear contenedor principal
            VBox mainBox = new VBox(false, 10);
            mainBox.BorderWidth = 20;

            // Banner de bienvenida
            Label lblBienvenida = new Label($"<b><span size='16000'>Bienvenido/a, {usuarioActual.Usuario}</span></b>");
            lblBienvenida.UseMarkup = true;
            mainBox.PackStart(lblBienvenida, false, false, 10);

            // Notebook (pestañas) para las distintas secciones
            notebook = new Notebook();

            // Pestaña 1: Mis Vehículos
            VBox vehiculosBox = CrearSeccionVehiculos();
            notebook.AppendPage(vehiculosBox, new Label("Mis Vehículos"));

            // Pestaña 2: Servicios Realizados
            VBox serviciosBox = CrearSeccionServicios();
            notebook.AppendPage(serviciosBox, new Label("Servicios Realizados"));

            // Pestaña 3: Facturas Pendientes
            VBox facturasBox = CrearSeccionFacturas();
            notebook.AppendPage(facturasBox, new Label("Facturas Pendientes"));

            mainBox.PackStart(notebook, true, true, 0);

            // Botones de acciones generales
            HBox botonesBox = new HBox(true, 10);

            Button btnRefrescar = new Button("Refrescar Datos");
            btnRefrescar.Clicked += OnRefrescarDatos;
            botonesBox.PackStart(btnRefrescar, true, true, 0);

            Button btnCerrarSesion = new Button("Cerrar Sesión");
            btnCerrarSesion.Clicked += OnCerrarSesion;
            botonesBox.PackStart(btnCerrarSesion, true, true, 0);

            mainBox.PackStart(botonesBox, false, false, 10);

            // Configurar evento de cierre para registrar la salida
            DeleteEvent += (sender, e) =>
            {
                DatosCompartidos.ControlLog.RegistrarSalida(usuarioActual.Usuario);
                Application.Quit();
            };

            Add(mainBox);
            ShowAll();
        }

        [Obsolete]
        private VBox CrearSeccionVehiculos()
        {
            VBox box = new VBox(false, 10);

            Label lblTitulo = new Label("<b>Mis Vehículos</b>");
            lblTitulo.UseMarkup = true;
            lblTitulo.SetAlignment(0, 0.5f);
            box.PackStart(lblTitulo, false, false, 5);

            // TreeView para mostrar los vehículos
            ScrolledWindow scrollWin = new ScrolledWindow();
            treeViewVehiculos = new TreeView();

            // Configurar columnas
            treeViewVehiculos.AppendColumn("ID", new CellRendererText(), "text", 0);
            treeViewVehiculos.AppendColumn("ID Usuario", new CellRendererText(), "text", 1);
            treeViewVehiculos.AppendColumn("Marca", new CellRendererText(), "text", 2);
            treeViewVehiculos.AppendColumn("Modelo", new CellRendererText(), "text", 3);
            treeViewVehiculos.AppendColumn("Placa", new CellRendererText(), "text", 4);

            // Crear modelo de datos
            ListStore listStore = new ListStore(typeof(int), typeof(int), typeof(string), typeof(int), typeof(string));
            treeViewVehiculos.Model = listStore;

            // Obtener vehículos del usuario actual
            var vehiculos = DatosCompartidos.ListaVehiculos.ObtenerVehiculosPorUsuario(usuarioActual.Index);
            foreach (var vehiculo in vehiculos)
            {
                listStore.AppendValues(vehiculo.Id, vehiculo.IdUsuario, vehiculo.Marca, vehiculo.Modelo, vehiculo.Placa);
            }

            scrollWin.Add(treeViewVehiculos);
            box.PackStart(scrollWin, true, true, 0);

            return box;
        }

        [Obsolete]
        private VBox CrearSeccionServicios()
        {
            VBox box = new VBox(false, 10);

            Label lblTitulo = new Label("<b>Servicios Realizados</b>");
            lblTitulo.UseMarkup = true;
            lblTitulo.SetAlignment(0, 0.5f);
            box.PackStart(lblTitulo, false, false, 5);

            // TreeView para mostrar los servicios
            ScrolledWindow scrollWin = new ScrolledWindow();
            treeViewServicios = new TreeView();

            // Configurar columnas
            treeViewServicios.AppendColumn("ID Servicio", new CellRendererText(), "text", 0);
            treeViewServicios.AppendColumn("Detalles", new CellRendererText(), "text", 1);
            treeViewServicios.AppendColumn("Costo", new CellRendererText(), "text", 2);
            treeViewServicios.AppendColumn("ID Vehículo", new CellRendererText(), "text", 3);

            // Crear modelo de datos
            ListStore listStore = new ListStore(typeof(int), typeof(string), typeof(double), typeof(int));
            treeViewServicios.Model = listStore;

            // Obtener los vehículos del usuario actual
            var vehiculos = DatosCompartidos.ListaVehiculos.ObtenerVehiculosPorUsuario(usuarioActual.Index);

            // Obtener los servicios asociados a cada vehículo
            foreach (var vehiculo in vehiculos)
            {
                var servicios = DatosCompartidos.ArbolServicios.ObtenerServiciosPorVehiculo(vehiculo.Id);
                foreach (var servicio in servicios)
                {
                    listStore.AppendValues(servicio.Id, servicio.Detalles, (double)servicio.Costo, servicio.Id_Vehiculo);
                }
            }

            scrollWin.Add(treeViewServicios);
            box.PackStart(scrollWin, true, true, 0);

            return box;
        }

        [Obsolete]
        private VBox CrearSeccionFacturas()
        {
            VBox box = new VBox(false, 10);

            Label lblTitulo = new Label("<b>Facturas Pendientes</b>");
            lblTitulo.UseMarkup = true;
            lblTitulo.SetAlignment(0, 0.5f);
            box.PackStart(lblTitulo, false, false, 5);

            // TreeView para mostrar las facturas
            ScrolledWindow scrollWin = new ScrolledWindow();
            treeViewFacturas = new TreeView();

            // Configurar columnas
            treeViewFacturas.AppendColumn("ID", new CellRendererText(), "text", 0);
            treeViewFacturas.AppendColumn("Fecha", new CellRendererText(), "text", 1);
            treeViewFacturas.AppendColumn("Total", new CellRendererText(), "text", 2);
            treeViewFacturas.AppendColumn("Estado", new CellRendererText(), "text", 3);

            scrollWin.Add(treeViewFacturas);
            box.PackStart(scrollWin, true, true, 0);

            // Botón para pagar facturas seleccionadas
            Button btnPagar = new Button("Pagar Seleccionadas");
            btnPagar.Clicked += OnPagarFacturas;
            box.PackStart(btnPagar, false, false, 5);

            return box;
        }

        private void OnRefrescarDatos(object sender, EventArgs e)
        {
            MostrarMensaje("Información", "Actualizando datos...");
        }

        private void OnCerrarSesion(object sender, EventArgs e)
        {
            DatosCompartidos.ControlLog.RegistrarSalida(usuarioActual.Usuario);
            DatosCompartidos.UsuarioActual = null;

            VentanaLogin login = new VentanaLogin();
            login.Show();

            this.Destroy();
        }

        private void OnAplicarFiltro(object sender, EventArgs e)
        {
            string tipoRecorrido = comboRecorrido.ActiveText;
            MostrarMensaje("Información", $"Filtrando servicios por recorrido: {tipoRecorrido}");
        }

        private void OnPagarFacturas(object sender, EventArgs e)
        {
            MostrarMensaje("Información", "Procesando pago de facturas seleccionadas...");
        }

        private void MostrarMensaje(string titulo, string mensaje)
        {
            MessageDialog dialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                titulo == "Error" ? MessageType.Error : MessageType.Info,
                ButtonsType.Ok,
                mensaje
            );
            dialog.Run();
            dialog.Destroy();
        }
    }
}