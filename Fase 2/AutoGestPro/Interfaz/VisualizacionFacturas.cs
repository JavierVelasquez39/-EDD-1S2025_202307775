using Gtk;
using System;
using AutoGestPro;
using AutoGestPro.Estructuras;
using AutoGestPro.Modelos;

namespace AutoGestPro.Interfaz
{
    public class VisualizacionFacturas : Window
    {
        private ArbolBFacturas arbolFacturas;

        public VisualizacionFacturas(ArbolBFacturas arbolFacturas) : base("Visualización de Facturas")
        {
            this.arbolFacturas = arbolFacturas;

            // Configuración básica de la ventana
            SetDefaultSize(600, 400);
            SetPosition(WindowPosition.Center);
            BorderWidth = 10;

            // Crear un contenedor vertical
            VBox contenedor = new VBox(false, 10);

            // Crear el TreeView para mostrar las facturas
            TreeView tablaFacturas = CrearTablaFacturas();

            // Llenar la tabla con las facturas pendientes
            LlenarTablaFacturas(tablaFacturas);

            // Agregar la tabla al contenedor
            ScrolledWindow scrolledWindow = new ScrolledWindow();
            scrolledWindow.Add(tablaFacturas);
            contenedor.PackStart(scrolledWindow, true, true, 0);

            // Botón para cerrar la ventana
            Button btnCerrar = new Button("Cerrar");
            btnCerrar.Clicked += (sender, e) => Destroy();
            contenedor.PackStart(btnCerrar, false, false, 10);

            // Agregar el contenedor a la ventana
            Add(contenedor);
            ShowAll();
        }

        private TreeView CrearTablaFacturas()
        {
            // Crear el TreeView
            TreeView treeView = new TreeView();

            // Crear las columnas
            TreeViewColumn colId = new TreeViewColumn { Title = "ID" };
            TreeViewColumn colOrden = new TreeViewColumn { Title = "Orden (Servicio)" };
            TreeViewColumn colTotal = new TreeViewColumn { Title = "Total" };

            // Crear las celdas para las columnas
            CellRendererText cellId = new CellRendererText();
            CellRendererText cellOrden = new CellRendererText();
            CellRendererText cellTotal = new CellRendererText();

            // Agregar las celdas a las columnas
            colId.PackStart(cellId, true);
            colOrden.PackStart(cellOrden, true);
            colTotal.PackStart(cellTotal, true);

            // Configurar el mapeo de datos
            colId.AddAttribute(cellId, "text", 0);
            colOrden.AddAttribute(cellOrden, "text", 1);
            colTotal.AddAttribute(cellTotal, "text", 2);

            // Agregar las columnas al TreeView
            treeView.AppendColumn(colId);
            treeView.AppendColumn(colOrden);
            treeView.AppendColumn(colTotal);

            // Crear el modelo de datos
            ListStore listStore = new ListStore(typeof(string), typeof(string), typeof(string));
            treeView.Model = listStore;

            return treeView;
        }

        private void LlenarTablaFacturas(TreeView treeView)
        {
            // Obtener el modelo de datos del TreeView
            ListStore listStore = (ListStore)treeView.Model;

            // Recorrer las facturas en el árbol B y agregarlas a la tabla
            AgregarFacturasDesdeNodo(arbolFacturas.Raiz, listStore);
        }

        private void AgregarFacturasDesdeNodo(NodoB? nodo, ListStore listStore)
        {
            if (nodo == null) return;

            // Recorrer las claves del nodo actual
            foreach (Factura factura in nodo.Claves)
            {
                listStore.AppendValues(
                    factura.Id.ToString(),
                    factura.Id_Servicio.ToString(), // Orden (Servicio)
                    factura.Total.ToString("C") // Total
                );
            }

            // Recorrer los hijos del nodo
            foreach (NodoB? hijo in nodo.Hijos)
            {
                AgregarFacturasDesdeNodo(hijo, listStore);
            }
        }
    }
}