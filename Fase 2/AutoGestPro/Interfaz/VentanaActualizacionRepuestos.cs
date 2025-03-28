using Gtk;
using System;
using AutoGestPro.Estructuras;
using AutoGestPro.Modelos;

namespace AutoGestPro.Interfaz
{
    public class VentanaActualizacionRepuestos : Window
    {
        private ArbolAVLRepuestos arbolRepuestos;

        private Entry entryID;
        private Entry entryNombre;
        private Entry entryDetalles;
        private Entry entryCosto;
        private TextView textViewInfo;

        public VentanaActualizacionRepuestos(ArbolAVLRepuestos arbolRepuestos) : base("Actualización de Repuestos")
        {
            this.arbolRepuestos = arbolRepuestos;

            // Configuración básica de la ventana
            SetDefaultSize(500, 400);
            SetPosition(WindowPosition.Center);

            VBox mainVBox = new VBox(false, 10);
            mainVBox.BorderWidth = 20;

            // Etiqueta de título
            Label labelTitulo = new Label("<b><span size='16000'>Actualización de Repuestos</span></b>");
            labelTitulo.UseMarkup = true;
            mainVBox.PackStart(labelTitulo, false, false, 10);

            // Entrada para ID
            HBox hboxID = new HBox(false, 10);
            hboxID.PackStart(new Label("ID Repuesto:"), false, false, 0);
            entryID = new Entry();
            hboxID.PackStart(entryID, true, true, 0);
            Button btnBuscar = new Button("Buscar");
            btnBuscar.Clicked += OnBuscarRepuestoClicked;
            hboxID.PackStart(btnBuscar, false, false, 0);
            mainVBox.PackStart(hboxID, false, false, 10);

            // Área de texto para mostrar información
            textViewInfo = new TextView();
            textViewInfo.Editable = false;
            mainVBox.PackStart(textViewInfo, true, true, 10);

            // Formulario para actualizar repuesto
            Label labelActualizar = new Label("<b>Actualizar Repuesto</b>");
            labelActualizar.UseMarkup = true;
            mainVBox.PackStart(labelActualizar, false, false, 10);

            Table tableActualizar = new Table(3, 2, false);
            tableActualizar.RowSpacing = 10;
            tableActualizar.ColumnSpacing = 10;

            // Nombre
            Label labelNombre = new Label("Nombre:");
            labelNombre.Xalign = 1.0f;
            tableActualizar.Attach(labelNombre, 0, 1, 0, 1);
            entryNombre = new Entry();
            tableActualizar.Attach(entryNombre, 1, 2, 0, 1);

            // Detalles
            Label labelDetalles = new Label("Detalles:");
            labelDetalles.Xalign = 1.0f;
            tableActualizar.Attach(labelDetalles, 0, 1, 1, 2);
            entryDetalles = new Entry();
            tableActualizar.Attach(entryDetalles, 1, 2, 1, 2);

            // Costo
            Label labelCosto = new Label("Costo:");
            labelCosto.Xalign = 1.0f;
            tableActualizar.Attach(labelCosto, 0, 1, 2, 3);
            entryCosto = new Entry();
            tableActualizar.Attach(entryCosto, 1, 2, 2, 3);

            mainVBox.PackStart(tableActualizar, false, false, 10);

            // Botón para actualizar
            Button btnActualizar = new Button("Actualizar Repuesto");
            btnActualizar.Clicked += OnActualizarRepuestoClicked;
            mainVBox.PackStart(btnActualizar, false, false, 10);

            Add(mainVBox);
            ShowAll();
        }

        private void OnBuscarRepuestoClicked(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(entryID.Text, out id))
            {
                var repuesto = arbolRepuestos.Buscar(id);
                if (repuesto != null)
                {
                    textViewInfo.Buffer.Text = $"ID: {repuesto.Id}\nNombre: {repuesto.Nombre}\nDetalles: {repuesto.Detalles}\nCosto: {repuesto.Costo:C}";
                    entryNombre.Text = repuesto.Nombre;
                    entryDetalles.Text = repuesto.Detalles;
                    entryCosto.Text = repuesto.Costo.ToString();
                }
                else
                {
                    MostrarMensaje("Error", "Repuesto no encontrado.");
                }
            }
            else
            {
                MostrarMensaje("Error", "ID inválido.");
            }
        }

        private void OnActualizarRepuestoClicked(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(entryID.Text, out id))
            {
                var repuesto = arbolRepuestos.Buscar(id);
                if (repuesto != null)
                {
                    string nuevoNombre = entryNombre.Text;
                    string nuevosDetalles = entryDetalles.Text;
                    decimal nuevoCosto;

                    if (string.IsNullOrEmpty(nuevoNombre) || string.IsNullOrEmpty(nuevosDetalles) || !decimal.TryParse(entryCosto.Text, out nuevoCosto))
                    {
                        MostrarMensaje("Error", "Todos los campos son obligatorios y el costo debe ser un número válido.");
                        return;
                    }

                    // Actualizar los datos del repuesto
                    repuesto.Nombre = nuevoNombre;
                    repuesto.Detalles = nuevosDetalles;
                    repuesto.Costo = nuevoCosto;

                    MostrarMensaje("Éxito", "Repuesto actualizado correctamente.");
                    textViewInfo.Buffer.Text = $"ID: {repuesto.Id}\nNombre: {repuesto.Nombre}\nDetalles: {repuesto.Detalles}\nCosto: {repuesto.Costo:C}";
                }
                else
                {
                    MostrarMensaje("Error", "Repuesto no encontrado.");
                }
            }
            else
            {
                MostrarMensaje("Error", "ID inválido.");
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