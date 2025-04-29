using AutoGestPro.Modelos;

namespace AutoGestPro.Estructuras
{
    [Serializable]
    public class ArbolAVLRepuestos
    {
        private NodoAVL? raiz;

        public ArbolAVLRepuestos()
        {
            raiz = null;
        }

        // Método para obtener la altura de un nodo
        private int ObtenerAltura(NodoAVL? nodo)
        {
            return nodo?.Altura ?? 0;
        }

        // Método para calcular el factor de balance
        private int ObtenerFactorBalance(NodoAVL? nodo)
        {
            return nodo == null ? 0 : ObtenerAltura(nodo.Izquierdo) - ObtenerAltura(nodo.Derecho);
        }

        // Rotación simple a la derecha
        private NodoAVL RotacionDerecha(NodoAVL y)
        {
            NodoAVL x = y.Izquierdo!;
            NodoAVL T2 = x.Derecho!;

            x.Derecho = y;
            y.Izquierdo = T2;

            y.Altura = Math.Max(ObtenerAltura(y.Izquierdo), ObtenerAltura(y.Derecho)) + 1;
            x.Altura = Math.Max(ObtenerAltura(x.Izquierdo), ObtenerAltura(x.Derecho)) + 1;

            return x;
        }

        // Rotación simple a la izquierda
        private NodoAVL RotacionIzquierda(NodoAVL x)
        {
            NodoAVL y = x.Derecho!;
            NodoAVL T2 = y.Izquierdo!;

            y.Izquierdo = x;
            x.Derecho = T2;

            x.Altura = Math.Max(ObtenerAltura(x.Izquierdo), ObtenerAltura(x.Derecho)) + 1;
            y.Altura = Math.Max(ObtenerAltura(y.Izquierdo), ObtenerAltura(y.Derecho)) + 1;

            return y;
        }

        // Método para insertar un repuesto
        public void Insertar(Repuesto repuesto)
        {
            raiz = InsertarNodo(raiz, repuesto);
        }

        private NodoAVL InsertarNodo(NodoAVL? nodo, Repuesto repuesto)
        {
            if (nodo == null)
                return new NodoAVL(repuesto);

            if (repuesto.Id < nodo.Repuesto.Id)
                nodo.Izquierdo = InsertarNodo(nodo.Izquierdo, repuesto);
            else if (repuesto.Id > nodo.Repuesto.Id)
                nodo.Derecho = InsertarNodo(nodo.Derecho, repuesto);
            else
                throw new Exception("❌ Error: El ID del repuesto ya existe en el sistema.");

            nodo.Altura = 1 + Math.Max(ObtenerAltura(nodo.Izquierdo), ObtenerAltura(nodo.Derecho));

            int balance = ObtenerFactorBalance(nodo);

            // Rotaciones para balancear el árbol
            if (balance > 1 && repuesto.Id < nodo.Izquierdo!.Repuesto.Id)
                return RotacionDerecha(nodo);

            if (balance < -1 && repuesto.Id > nodo.Derecho!.Repuesto.Id)
                return RotacionIzquierda(nodo);

            if (balance > 1 && repuesto.Id > nodo.Izquierdo!.Repuesto.Id)
            {
                nodo.Izquierdo = RotacionIzquierda(nodo.Izquierdo);
                return RotacionDerecha(nodo);
            }

            if (balance < -1 && repuesto.Id < nodo.Derecho!.Repuesto.Id)
            {
                nodo.Derecho = RotacionDerecha(nodo.Derecho);
                return RotacionIzquierda(nodo);
            }

            return nodo;
        }

        // Método para buscar un repuesto por ID
        public Repuesto? Buscar(int id)
        {
            return BuscarNodo(raiz, id)?.Repuesto;
        }

        private NodoAVL? BuscarNodo(NodoAVL? nodo, int id)
        {
            if (nodo == null || nodo.Repuesto.Id == id)
                return nodo;

            if (id < nodo.Repuesto.Id)
                return BuscarNodo(nodo.Izquierdo, id);

            return BuscarNodo(nodo.Derecho, id);
        }

        // Métodos de recorrido

        // PRE-ORDEN
        public void RecorrerPreOrden(Action<Repuesto> accion)
        {
            RecorrerPreOrden(raiz, accion);
        }

        private void RecorrerPreOrden(NodoAVL? nodo, Action<Repuesto> accion)
        {
            if (nodo == null) return;

            accion(nodo.Repuesto);
            RecorrerPreOrden(nodo.Izquierdo, accion);
            RecorrerPreOrden(nodo.Derecho, accion);
        }

        // IN-ORDEN
        public void RecorrerInOrden(Action<Repuesto> accion)
        {
            RecorrerInOrden(raiz, accion);
        }

        private void RecorrerInOrden(NodoAVL? nodo, Action<Repuesto> accion)
        {
            if (nodo == null) return;

            RecorrerInOrden(nodo.Izquierdo, accion);
            accion(nodo.Repuesto);
            RecorrerInOrden(nodo.Derecho, accion);
        }

        // POST-ORDEN
        public void RecorrerPostOrden(Action<Repuesto> accion)
        {
            RecorrerPostOrden(raiz, accion);
        }

        private void RecorrerPostOrden(NodoAVL? nodo, Action<Repuesto> accion)
        {
            if (nodo == null) return;

            RecorrerPostOrden(nodo.Izquierdo, accion);
            RecorrerPostOrden(nodo.Derecho, accion);
            accion(nodo.Repuesto);
        }

        // Método para mostrar los repuestos en orden
        public void MostrarRepuestos()
        {
            MostrarEnOrden(raiz);
        }

        private void MostrarEnOrden(NodoAVL? nodo)
        {
            if (nodo != null)
            {
                MostrarEnOrden(nodo.Izquierdo);
                Console.WriteLine(nodo.Repuesto);
                MostrarEnOrden(nodo.Derecho);
            }
        }

        // Método para generar el archivo .dot
        public void GenerarDot(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("digraph ArbolAVLRepuestos {");
                writer.WriteLine("node [shape=record];"); // Cambiar a 'record' para mostrar múltiples datos en el nodo

                if (raiz != null)
                {
                    GenerarDotRecursivo(raiz, writer);
                }

                writer.WriteLine("}");
            }

            Console.WriteLine($"✅ Archivo .dot generado en: {filePath}");
        }

        private void GenerarDotRecursivo(NodoAVL nodo, StreamWriter writer)
        {
            // Crear el nodo actual con los nuevos detalles
            writer.WriteLine($"\"{nodo.Repuesto.Id}\" [label=\"{{ID: {nodo.Repuesto.Id}|Nombre: {nodo.Repuesto.Nombre}|Detalles: {nodo.Repuesto.Detalles}|Costo: {nodo.Repuesto.Costo:C}}}\"];");

            // Conexión con el hijo izquierdo
            if (nodo.Izquierdo != null)
            {
                writer.WriteLine($"\"{nodo.Repuesto.Id}\" -> \"{nodo.Izquierdo.Repuesto.Id}\";");
                GenerarDotRecursivo(nodo.Izquierdo, writer);
            }

            // Conexión con el hijo derecho
            if (nodo.Derecho != null)
            {
                writer.WriteLine($"\"{nodo.Repuesto.Id}\" -> \"{nodo.Derecho.Repuesto.Id}\";");
                GenerarDotRecursivo(nodo.Derecho, writer);
            }
        }
    }
}