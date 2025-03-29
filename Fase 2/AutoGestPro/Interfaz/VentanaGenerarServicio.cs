using Gtk;
using System;
using AutoGestPro.Estructuras;
using AutoGestPro.Modelos;

namespace AutoGestPro.Interfaz
{
    public class VentanaGenerarServicio : Window
    {
        private ArbolAVLRepuestos arbolRepuestos;
        private ListaDobleVehiculos listaVehiculos;
        private ArbolBinarioServicios arbolServicios;
        private ArbolBFacturas arbolFacturas;

        private Entry entryIdServicio;
        private Entry entryIdRepuesto;
        private Entry entryIdVehiculo;
        private Entry entryDetalles;
        private Entry entryCosto;

        public VentanaGenerarServicio(ArbolAVLRepuestos arbolRepuestos, ListaDobleVehiculos listaVehiculos, ArbolBinarioServicios arbolServicios, ArbolBFacturas arbolFacturas) 
            : base("Generar Servicio")
        {
            this.arbolRepuestos = arbolRepuestos;
            this.listaVehiculos = listaVehiculos;
            this.arbolServicios = arbolServicios;
            this.arbolFacturas = arbolFacturas;

            // Configuración básica de la ventana
            SetDefaultSize(500, 400);
            SetPosition(WindowPosition.Center);

            VBox mainVBox = new VBox(false, 10);
            mainVBox.BorderWidth = 20;

            // Etiqueta de título
            Label labelTitulo = new Label("<b><span size='16000'>Generar Servicio</span></b>");
            labelTitulo.UseMarkup = true;
            mainVBox.PackStart(labelTitulo, false, false, 10);

            // Formulario para ingresar datos del servicio
            Table tableFormulario = new Table(5, 2, false);
            tableFormulario.RowSpacing = 10;
            tableFormulario.ColumnSpacing = 10;

            // ID Servicio
            Label labelIdServicio = new Label("ID Servicio:");
            labelIdServicio.Xalign = 1.0f;
            tableFormulario.Attach(labelIdServicio, 0, 1, 0, 1);
            entryIdServicio = new Entry();
            tableFormulario.Attach(entryIdServicio, 1, 2, 0, 1);

            // ID Repuesto
            Label labelIdRepuesto = new Label("ID Repuesto:");
            labelIdRepuesto.Xalign = 1.0f;
            tableFormulario.Attach(labelIdRepuesto, 0, 1, 1, 2);
            entryIdRepuesto = new Entry();
            tableFormulario.Attach(entryIdRepuesto, 1, 2, 1, 2);

            // ID Vehículo
            Label labelIdVehiculo = new Label("ID Vehículo:");
            labelIdVehiculo.Xalign = 1.0f;
            tableFormulario.Attach(labelIdVehiculo, 0, 1, 2, 3);
            entryIdVehiculo = new Entry();
            tableFormulario.Attach(entryIdVehiculo, 1, 2, 2, 3);

            // Detalles
            Label labelDetalles = new Label("Detalles:");
            labelDetalles.Xalign = 1.0f;
            tableFormulario.Attach(labelDetalles, 0, 1, 3, 4);
            entryDetalles = new Entry();
            tableFormulario.Attach(entryDetalles, 1, 2, 3, 4);

            // Costo
            Label labelCosto = new Label("Costo:");
            labelCosto.Xalign = 1.0f;
            tableFormulario.Attach(labelCosto, 0, 1, 4, 5);
            entryCosto = new Entry();
            tableFormulario.Attach(entryCosto, 1, 2, 4, 5);

            mainVBox.PackStart(tableFormulario, false, false, 10);

            // Botón para generar servicio
            Button btnGenerarServicio = new Button("Generar Servicio");
            btnGenerarServicio.Clicked += OnGenerarServicioClicked;
            mainVBox.PackStart(btnGenerarServicio, false, false, 10);

            Add(mainVBox);
            ShowAll();
        }

        private void OnGenerarServicioClicked(object sender, EventArgs e)
        {
            try
            {
                // Obtener datos del formulario
                int idServicio = int.Parse(entryIdServicio.Text);
                int idRepuesto = int.Parse(entryIdRepuesto.Text);
                int idVehiculo = int.Parse(entryIdVehiculo.Text);
                string detalles = entryDetalles.Text;
                decimal costoServicio = decimal.Parse(entryCosto.Text);

                // Validar existencia del repuesto y vehículo
                Repuesto? repuesto = arbolRepuestos.Buscar(idRepuesto);
                if (repuesto == null)
                {
                    MostrarMensaje("Error", $"El repuesto con ID {idRepuesto} no existe.");
                    return;
                }

                if (listaVehiculos.BuscarVehiculo(idVehiculo) == null)
                {
                    MostrarMensaje("Error", $"El vehículo con ID {idVehiculo} no existe.");
                    return;
                }

                // Sumar el costo del servicio con el costo del repuesto
                decimal costoTotal = costoServicio + repuesto.Costo;

                // Crear y agregar el servicio
                Servicio servicio = new Servicio(idServicio, idRepuesto, idVehiculo, detalles, costoTotal);
                arbolServicios.Insertar(servicio, arbolRepuestos, listaVehiculos);

                // Generar factura automáticamente
                Factura factura = new Factura(idServicio, idServicio, costoTotal);
                arbolFacturas.Insertar(factura);

                MostrarMensaje("Éxito", $"Servicio y factura generados correctamente.\nFactura ID: {factura.Id}\nTotal: {factura.Total:C}");
            }
            catch (FormatException)
            {
                MostrarMensaje("Error", "Por favor, ingrese datos válidos en todos los campos.");
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error", ex.Message);
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