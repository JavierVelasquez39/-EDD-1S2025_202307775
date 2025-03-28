using Gtk;
using System;
using AutoGestPro.Estructuras;
using AutoGestPro.Modelos;

namespace AutoGestPro.Interfaz
{
    public class VisualizacionServicios : Window
    {
        private ArbolBinarioServicios arbolServicios;
        private ComboBoxText comboBoxOrden;
        private TreeView treeViewServicios;
        private ListStore listStore;

        public VisualizacionServicios(ArbolBinarioServicios arbolServicios) : base("Visualización de Servicios")
        {
            this.arbolServicios = arbolServicios;

            // Configuración básica de la ventana
            SetDefaultSize(600, 400);
            SetPosition(WindowPosition.Center);

            VBox mainVBox = new VBox(false, 10);
            mainVBox.BorderWidth = 20;

            // Etiqueta de título
            Label labelTitulo = new Label("<b><span size='16000'>Visualización de Servicios</span></b>");
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

            // Tabla para mostrar los servicios
            treeViewServicios = new TreeView();
            listStore = new ListStore(typeof(int), typeof(int), typeof(int), typeof(string), typeof(decimal));

            // Configurar columnas de la tabla
            treeViewServicios.Model = listStore;
            treeViewServicios.AppendColumn("ID", new CellRendererText(), "text", 0);
            treeViewServicios.AppendColumn("ID Repuesto", new CellRendererText(), "text", 1);
            treeViewServicios.AppendColumn("ID Vehículo", new CellRendererText(), "text", 2);
            treeViewServicios.AppendColumn("Detalles", new CellRendererText(), "text", 3);
            treeViewServicios.AppendColumn("Costo", new CellRendererText(), "text", 4);

            ScrolledWindow scrolledWindow = new ScrolledWindow();
            scrolledWindow.Add(treeViewServicios);
            mainVBox.PackStart(scrolledWindow, true, true, 10);

            Add(mainVBox);
            ShowAll();
        }

        private void OnVisualizarClicked(object sender, EventArgs e)
        {
            string ordenSeleccionado = comboBoxOrden.ActiveText;

            // Limpiar la tabla
            listStore.Clear();

            // Obtener los servicios en el orden seleccionado
            switch (ordenSeleccionado)
            {
                case "PRE-ORDEN":
                    RecorrerPreOrden(arbolServicios, servicio => AgregarFila(servicio));
                    break;

                case "IN-ORDEN":
                    RecorrerInOrden(arbolServicios, servicio => AgregarFila(servicio));
                    break;

                case "POST-ORDEN":
                    RecorrerPostOrden(arbolServicios, servicio => AgregarFila(servicio));
                    break;

                default:
                    MostrarMensaje("Error", "Seleccione un orden válido.");
                    break;
            }
        }

        private void AgregarFila(Servicio servicio)
        {
            listStore.AppendValues(servicio.Id, servicio.Id_Repuesto, servicio.Id_Vehiculo, servicio.Detalles, servicio.Costo);
        }

        private void MostrarMensaje(string titulo, string mensaje)
        {
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, titulo == "Error" ? MessageType.Error : MessageType.Info, ButtonsType.Ok, mensaje);
            dialog.Run();
            dialog.Destroy();
        }

        // Métodos para recorrer el árbol binario
        private void RecorrerPreOrden(ArbolBinarioServicios arbol, Action<Servicio> accion)
        {
            void PreOrden(NodoServicio? nodo)
            {
                if (nodo == null) return;
                accion(nodo.Servicio);
                PreOrden(nodo.Izquierdo);
                PreOrden(nodo.Derecho);
            }
            PreOrden(arbol.Raiz);
        }

        private void RecorrerInOrden(ArbolBinarioServicios arbol, Action<Servicio> accion)
        {
            void InOrden(NodoServicio? nodo)
            {
                if (nodo == null) return;
                InOrden(nodo.Izquierdo);
                accion(nodo.Servicio);
                InOrden(nodo.Derecho);
            }
            InOrden(arbol.Raiz);
        }

        private void RecorrerPostOrden(ArbolBinarioServicios arbol, Action<Servicio> accion)
        {
            void PostOrden(NodoServicio? nodo)
            {
                if (nodo == null) return;
                PostOrden(nodo.Izquierdo);
                PostOrden(nodo.Derecho);
                accion(nodo.Servicio);
            }
            PostOrden(arbol.Raiz);
        }
    }
}