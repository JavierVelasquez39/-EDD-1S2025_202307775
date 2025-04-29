using Gtk;
using System;
using AutoGestPro.Estructuras;
using AutoGestPro.Modelos;

namespace AutoGestPro.Interfaz
{
    public class VentanaVisualizacionRepuestos : Window
    {
        private ArbolAVLRepuestos arbolRepuestos;
        private ComboBoxText comboBoxOrden;
        private TreeView treeViewRepuestos;
        private ListStore listStore;

        public VentanaVisualizacionRepuestos(ArbolAVLRepuestos arbolRepuestos) : base("Visualización de Repuestos")
        {
            this.arbolRepuestos = arbolRepuestos;

            // Configuración básica de la ventana
            SetDefaultSize(600, 400);
            SetPosition(WindowPosition.Center);

            VBox mainVBox = new VBox(false, 10);
            mainVBox.BorderWidth = 20;

            // Etiqueta de título
            Label labelTitulo = new Label("<b><span size='16000'>Visualización de Repuestos</span></b>");
            labelTitulo.UseMarkup = true;
            mainVBox.PackStart(labelTitulo, false, false, 10);

            // Selector de orden
            HBox hboxOrden = new HBox(false, 10);
            hboxOrden.PackStart(new Label("Seleccione el orden de visualización:"), false, false, 0);

            comboBoxOrden = new ComboBoxText();
            comboBoxOrden.AppendText("PRE-ORDEN");
            comboBoxOrden.AppendText("IN-ORDEN");
            comboBoxOrden.AppendText("POST-ORDEN");
            comboBoxOrden.Active = 0;
            hboxOrden.PackStart(comboBoxOrden, true, true, 0);

            Button btnVisualizar = new Button("Visualizar");
            btnVisualizar.Clicked += OnVisualizarClicked;
            hboxOrden.PackStart(btnVisualizar, false, false, 0);

            mainVBox.PackStart(hboxOrden, false, false, 10);

            // Tabla para mostrar los repuestos
            treeViewRepuestos = new TreeView();
            listStore = new ListStore(typeof(int), typeof(string), typeof(string), typeof(decimal));

            // Configurar columnas de la tabla
            treeViewRepuestos.Model = listStore;
            treeViewRepuestos.AppendColumn("ID", new CellRendererText(), "text", 0);
            treeViewRepuestos.AppendColumn("Nombre", new CellRendererText(), "text", 1);
            treeViewRepuestos.AppendColumn("Detalles", new CellRendererText(), "text", 2);
            treeViewRepuestos.AppendColumn("Costo", new CellRendererText(), "text", 3);

            ScrolledWindow scrolledWindow = new ScrolledWindow();
            scrolledWindow.Add(treeViewRepuestos);
            mainVBox.PackStart(scrolledWindow, true, true, 10);

            Add(mainVBox);
            ShowAll();
        }

        private void OnVisualizarClicked(object sender, EventArgs e)
        {
            string ordenSeleccionado = comboBoxOrden.ActiveText;

            // Limpiar la tabla
            listStore.Clear();

            // Obtener los repuestos en el orden seleccionado
            switch (ordenSeleccionado)
            {
                case "PRE-ORDEN":
                    arbolRepuestos.RecorrerPreOrden(repuesto => AgregarFila(repuesto));
                    break;

                case "IN-ORDEN":
                    arbolRepuestos.RecorrerInOrden(repuesto => AgregarFila(repuesto));
                    break;

                case "POST-ORDEN":
                    arbolRepuestos.RecorrerPostOrden(repuesto => AgregarFila(repuesto));
                    break;

                default:
                    MostrarMensaje("Error", "Seleccione un orden válido.");
                    break;
            }
        }

        private void AgregarFila(Repuesto repuesto)
        {
            listStore.AppendValues(repuesto.Id, repuesto.Nombre, repuesto.Detalles, repuesto.Costo);
        }

        private void MostrarMensaje(string titulo, string mensaje)
        {
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, titulo == "Error" ? MessageType.Error : MessageType.Info, ButtonsType.Ok, mensaje);
            dialog.Run();
            dialog.Destroy();
        }
    }
}