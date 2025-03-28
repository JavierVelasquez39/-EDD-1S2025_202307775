using AutoGestPro.Modelos;

namespace AutoGestPro.Estructuras
{
    public class ArbolBinarioServicios
    {
        private NodoServicio? raiz;

        public ArbolBinarioServicios()
        {
            raiz = null;
        }

        // Método para insertar un servicio
        public void Insertar(Servicio servicio, ArbolAVLRepuestos arbolRepuestos, ListaDobleVehiculos listaVehiculos)
        {
            // Validar que el ID del repuesto y el ID del vehículo existan
            if (arbolRepuestos.Buscar(servicio.Id_Repuesto) == null)
            {
                throw new Exception($"❌ Error: El repuesto con ID {servicio.Id_Repuesto} no existe.");
            }

            if (listaVehiculos.BuscarVehiculo(servicio.Id_Vehiculo) == null)
            {
                throw new Exception($"❌ Error: El vehículo con ID {servicio.Id_Vehiculo} no existe.");
            }

            raiz = InsertarNodo(raiz, servicio);
        }

        private NodoServicio InsertarNodo(NodoServicio? nodo, Servicio servicio)
        {
            if (nodo == null)
                return new NodoServicio(servicio);

            if (servicio.Id < nodo.Servicio.Id)
                nodo.Izquierdo = InsertarNodo(nodo.Izquierdo, servicio);
            else if (servicio.Id > nodo.Servicio.Id)
                nodo.Derecho = InsertarNodo(nodo.Derecho, servicio);
            else
                throw new Exception("❌ Error: El ID del servicio ya existe en el sistema.");

            return nodo;
        }

        // Método para buscar un servicio por ID
        public Servicio? Buscar(int id)
        {
            return BuscarNodo(raiz, id)?.Servicio;
        }

        private NodoServicio? BuscarNodo(NodoServicio? nodo, int id)
        {
            if (nodo == null || nodo.Servicio.Id == id)
                return nodo;

            if (id < nodo.Servicio.Id)
                return BuscarNodo(nodo.Izquierdo, id);

            return BuscarNodo(nodo.Derecho, id);
        }

        // Método para mostrar los servicios en orden
        public void MostrarServicios()
        {
            MostrarEnOrden(raiz);
        }

        private void MostrarEnOrden(NodoServicio? nodo)
        {
            if (nodo != null)
            {
                MostrarEnOrden(nodo.Izquierdo);
                Console.WriteLine(nodo.Servicio);
                MostrarEnOrden(nodo.Derecho);
            }
        }

        // Método para generar el archivo .dot
        public void GenerarDot(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("digraph ArbolBinarioServicios {");
                writer.WriteLine("node [shape=record];");

                if (raiz != null)
                {
                    GenerarDotRecursivo(raiz, writer);
                }

                writer.WriteLine("}");
            }

            Console.WriteLine($"✅ Archivo .dot generado en: {filePath}");
        }

        private void GenerarDotRecursivo(NodoServicio nodo, StreamWriter writer)
        {
            // Crear el nodo actual
            writer.WriteLine($"\"{nodo.Servicio.Id}\" [label=\"ID: {nodo.Servicio.Id}\\nDetalles: {nodo.Servicio.Detalles}\\nCosto: {nodo.Servicio.Costo:C}\"];");

            // Conexión con el hijo izquierdo
            if (nodo.Izquierdo != null)
            {
                writer.WriteLine($"\"{nodo.Servicio.Id}\" -> \"{nodo.Izquierdo.Servicio.Id}\";");
                GenerarDotRecursivo(nodo.Izquierdo, writer);
            }

            // Conexión con el hijo derecho
            if (nodo.Derecho != null)
            {
                writer.WriteLine($"\"{nodo.Servicio.Id}\" -> \"{nodo.Derecho.Servicio.Id}\";");
                GenerarDotRecursivo(nodo.Derecho, writer);
            }
        }
    }
}