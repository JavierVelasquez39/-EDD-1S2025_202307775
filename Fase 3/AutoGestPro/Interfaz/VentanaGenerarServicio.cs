using System;
using Gtk;
using AutoGestPro.Estructuras;
using AutoGestPro.Modelos;

namespace AutoGestPro.Interfaz
{
    public class VentanaGenerarServicio : Window
    {
        private Entry idServicioEntry;
        private Entry idRepuestoEntry;
        private Entry idVehiculoEntry;
        private Entry detallesEntry;
        private Entry costoEntry;
        private ComboBoxText metodoPagoComboBox;
        private Button guardarButton;

        private ArbolBinarioServicios arbolServicios;
        private MerkleTree arbolFacturas;
        private GrafoNoDirigido grafoRelaciones;
        private ListaDobleVehiculos listaVehiculos;
        private ArbolAVLRepuestos arbolRepuestos;

        public VentanaGenerarServicio(
            ArbolBinarioServicios arbolServicios,
            MerkleTree arbolFacturas,
            GrafoNoDirigido grafoRelaciones,
            ListaDobleVehiculos listaVehiculos,
            ArbolAVLRepuestos arbolRepuestos
        ) : base("Generar Servicio")
        {
            this.arbolServicios = arbolServicios;
            this.arbolFacturas = arbolFacturas;
            this.grafoRelaciones = grafoRelaciones;
            this.listaVehiculos = listaVehiculos;
            this.arbolRepuestos = arbolRepuestos;

            // Configuración de la ventana
            SetDefaultSize(400, 400);
            SetPosition(WindowPosition.Center);
            DeleteEvent += OnDeleteEvent;

            // Crear el contenedor principal
            VBox mainBox = new VBox(false, 10);
            mainBox.BorderWidth = 20;

            // Crear la tabla para el formulario
            Table formTable = new Table(6, 2, false);
            formTable.ColumnSpacing = 10;
            formTable.RowSpacing = 10;

            // ID del servicio
            Label idServicioLabel = new Label("ID del Servicio:");
            idServicioEntry = new Entry();
            formTable.Attach(idServicioLabel, 0, 1, 0, 1);
            formTable.Attach(idServicioEntry, 1, 2, 0, 1);

            // ID del repuesto
            Label idRepuestoLabel = new Label("ID del Repuesto:");
            idRepuestoEntry = new Entry();
            formTable.Attach(idRepuestoLabel, 0, 1, 1, 2);
            formTable.Attach(idRepuestoEntry, 1, 2, 1, 2);

            // ID del vehículo
            Label idVehiculoLabel = new Label("ID del Vehículo:");
            idVehiculoEntry = new Entry();
            formTable.Attach(idVehiculoLabel, 0, 1, 2, 3);
            formTable.Attach(idVehiculoEntry, 1, 2, 2, 3);

            // Detalles
            Label detallesLabel = new Label("Detalles:");
            detallesEntry = new Entry();
            formTable.Attach(detallesLabel, 0, 1, 3, 4);
            formTable.Attach(detallesEntry, 1, 2, 3, 4);

            // Costo
            Label costoLabel = new Label("Costo:");
            costoEntry = new Entry();
            formTable.Attach(costoLabel, 0, 1, 4, 5);
            formTable.Attach(costoEntry, 1, 2, 4, 5);

            // Método de pago
            Label metodoPagoLabel = new Label("Método de Pago:");
            metodoPagoComboBox = new ComboBoxText();
            metodoPagoComboBox.AppendText("Efectivo");
            metodoPagoComboBox.AppendText("Tarjeta");
            metodoPagoComboBox.AppendText("Transferencia");
            metodoPagoComboBox.Active = 0;
            formTable.Attach(metodoPagoLabel, 0, 1, 5, 6);
            formTable.Attach(metodoPagoComboBox, 1, 2, 5, 6);

            // Botón Guardar
            guardarButton = new Button("Guardar");
            guardarButton.Clicked += OnGuardarClicked;

            // Agregar componentes al contenedor principal
            mainBox.PackStart(formTable, false, false, 0);
            mainBox.PackStart(guardarButton, false, false, 0);

            // Agregar el contenedor principal a la ventana
            Add(mainBox);
            ShowAll();
        }

        private void OnGuardarClicked(object sender, EventArgs e)
        {
            // Validar los campos
            if (string.IsNullOrWhiteSpace(idServicioEntry.Text) ||
                string.IsNullOrWhiteSpace(idRepuestoEntry.Text) ||
                string.IsNullOrWhiteSpace(idVehiculoEntry.Text) ||
                string.IsNullOrWhiteSpace(detallesEntry.Text) ||
                string.IsNullOrWhiteSpace(costoEntry.Text))
            {
                MostrarMensaje("Por favor, complete todos los campos.", MessageType.Error);
                return;
            }

            try
            {
                // Recopilar datos del formulario
                int idServicio = int.Parse(idServicioEntry.Text);
                int idRepuesto = int.Parse(idRepuestoEntry.Text);
                int idVehiculo = int.Parse(idVehiculoEntry.Text);
                string detalles = detallesEntry.Text;
                decimal costo = decimal.Parse(costoEntry.Text);
                string metodoPago = metodoPagoComboBox.ActiveText;
                DateTime fecha = DateTime.Now; // Fecha actual

                // Validar que el ID del servicio sea único
                if (arbolServicios.Buscar(idServicio) != null)
                {
                    MostrarMensaje("El ID del servicio ya está registrado. Use un ID diferente.", MessageType.Error);
                    return;
                }

                // Validar que el ID del repuesto y el ID del vehículo existan
                if (arbolRepuestos.Buscar(idRepuesto) == null)
                {
                    MostrarMensaje("El ID del repuesto no existe.", MessageType.Error);
                    return;
                }

                if (listaVehiculos.BuscarVehiculo(idVehiculo) == null)
                {
                    MostrarMensaje("El ID del vehículo no existe.", MessageType.Error);
                    return;
                }

                // Crear el servicio y agregarlo al árbol binario
                Servicio nuevoServicio = new Servicio(idServicio, idRepuesto, idVehiculo, detalles, costo);
                arbolServicios.Insertar(nuevoServicio, arbolRepuestos, listaVehiculos, arbolFacturas);

                // Crear la factura y agregarla al árbol de Merkle
                Factura nuevaFactura = new Factura(idServicio, idServicio, idVehiculo, costo, fecha, metodoPago);
                arbolFacturas.AddFactura(nuevaFactura);

                // Actualizar el grafo no dirigido
                grafoRelaciones.RegistrarServicio(idVehiculo, new List<int> { idRepuesto }, listaVehiculos, arbolRepuestos);

                MostrarMensaje("Servicio registrado exitosamente.", MessageType.Info);

                // Limpiar los campos
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", MessageType.Error);
            }
        }

        private void MostrarMensaje(string mensaje, MessageType tipo)
        {
            MessageDialog dialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                tipo,
                ButtonsType.Ok,
                mensaje
            );
            dialog.Run();
            dialog.Destroy();
        }

        private void LimpiarCampos()
        {
            idServicioEntry.Text = string.Empty;
            idRepuestoEntry.Text = string.Empty;
            idVehiculoEntry.Text = string.Empty;
            detallesEntry.Text = string.Empty;
            costoEntry.Text = string.Empty;
            metodoPagoComboBox.Active = 0;
        }

        private void OnDeleteEvent(object sender, DeleteEventArgs a)
        {
            // Cerrar solo la ventana actual
            Destroy();
            a.RetVal = true;
        }
    }
}