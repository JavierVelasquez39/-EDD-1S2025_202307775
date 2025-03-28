using Gtk;
using System;
using AutoGestPro;
using AutoGestPro.Estructuras;
using AutoGestPro.Modelos;

namespace AutoGestPro.Interfaz
{
    public class CancelarFacturas : Window
    {
        private ArbolBFacturas arbolFacturas;
        private Entry txtIdFactura;
        private Label lblDetallesFactura;

        public CancelarFacturas(ArbolBFacturas arbolFacturas) : base("Cancelar Facturas")
        {
            this.arbolFacturas = arbolFacturas;

            // Configuración básica de la ventana
            SetDefaultSize(400, 300);
            SetPosition(WindowPosition.Center);
            BorderWidth = 10;

            // Crear un contenedor vertical
            VBox contenedor = new VBox(false, 10);

            // Campo para ingresar el ID de la factura
            Label lblIdFactura = new Label("Ingrese el ID de la factura:");
            txtIdFactura = new Entry();
            Button btnBuscar = new Button("Buscar");
            btnBuscar.Clicked += OnBuscarFactura;

            // Etiqueta para mostrar los detalles de la factura
            lblDetallesFactura = new Label("Detalles de la factura aparecerán aquí.");

            // Botón para pagar la factura
            Button btnPagar = new Button("Pagar");
            btnPagar.Clicked += OnPagarFactura;

            // Botón para cerrar la ventana
            Button btnCerrar = new Button("Cerrar");
            btnCerrar.Clicked += (sender, e) => Destroy();

            // Agregar los elementos al contenedor
            contenedor.PackStart(lblIdFactura, false, false, 5);
            contenedor.PackStart(txtIdFactura, false, false, 5);
            contenedor.PackStart(btnBuscar, false, false, 5);
            contenedor.PackStart(lblDetallesFactura, false, false, 10);
            contenedor.PackStart(btnPagar, false, false, 5);
            contenedor.PackStart(btnCerrar, false, false, 5);

            // Agregar el contenedor a la ventana
            Add(contenedor);
            ShowAll();
        }

        private void OnBuscarFactura(object sender, EventArgs e)
        {
            // Obtener el ID ingresado por el usuario
            if (int.TryParse(txtIdFactura.Text, out int idFactura))
            {
                // Buscar la factura en el árbol
                Factura? factura = arbolFacturas.Buscar(idFactura);

                if (factura != null)
                {
                    // Mostrar los detalles de la factura
                    lblDetallesFactura.Text = $"Factura encontrada:\nID: {factura.Id}\nOrden: {factura.Id_Servicio}\nTotal: {factura.Total:C}";
                }
                else
                {
                    lblDetallesFactura.Text = "Factura no encontrada.";
                }
            }
            else
            {
                lblDetallesFactura.Text = "Por favor, ingrese un ID válido.";
            }
        }

        private void OnPagarFactura(object sender, EventArgs e)
        {
            // Obtener el ID ingresado por el usuario
            if (int.TryParse(txtIdFactura.Text, out int idFactura))
            {
                // Buscar la factura en el árbol
                Factura? factura = arbolFacturas.Buscar(idFactura);

                if (factura != null)
                {
                    // Eliminar la factura del árbol
                    arbolFacturas.Eliminar(idFactura);
                    lblDetallesFactura.Text = "Factura pagada y eliminada del sistema.";
                }
                else
                {
                    lblDetallesFactura.Text = "Factura no encontrada.";
                }
            }
            else
            {
                lblDetallesFactura.Text = "Por favor, ingrese un ID válido.";
            }
        }
    }
}