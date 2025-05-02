using AutoGestPro.Modelos;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutoGestPro.Estructuras
{
    [Serializable]
    public class ArbolBinarioServicios
    {
        private NodoServicio? raiz;

        public ArbolBinarioServicios()
        {
            raiz = null;
        }

        public NodoServicio? Raiz
        {
            get { return raiz; }
        }

        // Método público para insertar un servicio
        public void Insertar(Servicio servicio, ArbolAVLRepuestos arbolRepuestos, ListaDobleVehiculos listaVehiculos, MerkleTree arbolFacturas)
        {
            // Validaciones
            if (arbolRepuestos.Buscar(servicio.Id_Repuesto) == null)
            {
                throw new Exception($"❌ Error: El repuesto con ID {servicio.Id_Repuesto} no existe.");
            }

            if (listaVehiculos.BuscarVehiculo(servicio.Id_Vehiculo) == null)
            {
                throw new Exception($"❌ Error: El vehículo con ID {servicio.Id_Vehiculo} no existe.");
            }

            // Insertar el servicio en el árbol
            raiz = InsertarNodo(raiz, servicio);
            Console.WriteLine($"✅ Servicio con ID {servicio.Id} insertado correctamente.");

            // Generar una factura para el servicio
            Factura nuevaFactura = new Factura(
                id: servicio.Id, // Usar el mismo ID del servicio
                idServicio: servicio.Id,
                idVehiculo: servicio.Id_Vehiculo,
                total: servicio.Costo,
                fecha: DateTime.Now,
                metodoPago: "Pendiente"
            );

            arbolFacturas.AddFactura(nuevaFactura);
            Console.WriteLine($"✅ Factura generada para el servicio con ID {servicio.Id}.");
        }

        // Método privado recursivo para insertar un nodo
        private NodoServicio InsertarNodo(NodoServicio? nodo, Servicio servicio)
        {
            // Caso base: si el nodo es nulo, creamos un nuevo nodo
            if (nodo == null)
                return new NodoServicio(servicio);

            // Comparamos el ID del servicio con el del nodo actual
            if (servicio.Id < nodo.Servicio.Id)
            {
                // Si es menor, insertamos en el subárbol izquierdo
                nodo.Izquierdo = InsertarNodo(nodo.Izquierdo, servicio);
            }
            else if (servicio.Id > nodo.Servicio.Id)
            {
                // Si es mayor, insertamos en el subárbol derecho
                nodo.Derecho = InsertarNodo(nodo.Derecho, servicio);
            }
            else
            {
                // Si es igual, no permitimos duplicados
                throw new Exception($"❌ Error: El servicio con ID {servicio.Id} ya existe en el sistema.");
            }

            // Retornamos el nodo actual actualizado
            return nodo;
        }

        // Método para buscar un servicio por ID
        public Servicio? Buscar(int id)
        {
            return BuscarNodo(raiz, id)?.Servicio;
        }

        // Método privado recursivo para buscar un nodo
        private NodoServicio? BuscarNodo(NodoServicio? nodo, int id)
        {
            // Si el nodo es nulo o encontramos el ID, retornamos el nodo
            if (nodo == null || nodo.Servicio.Id == id)
                return nodo;

            // Si el ID es menor, buscamos en el subárbol izquierdo
            if (id < nodo.Servicio.Id)
                return BuscarNodo(nodo.Izquierdo, id);
            
            // Si el ID es mayor, buscamos en el subárbol derecho
            return BuscarNodo(nodo.Derecho, id);
        }

        // Método para mostrar los servicios en orden (inorden)
        public void MostrarServicios()
        {
            Console.WriteLine("=== Servicios en orden ===");
            MostrarEnOrden(raiz);
            Console.WriteLine("=========================");
        }

        // Método privado recursivo para mostrar los nodos en orden
        private void MostrarEnOrden(NodoServicio? nodo)
        {
            if (nodo != null)
            {
                MostrarEnOrden(nodo.Izquierdo);
                Console.WriteLine($"ID: {nodo.Servicio.Id}, Detalles: {nodo.Servicio.Detalles}, Costo: {nodo.Servicio.Costo:C}");
                MostrarEnOrden(nodo.Derecho);
            }
        }

        // Método para generar el archivo DOT para visualizar el árbol
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

            Console.WriteLine($"✅ Archivo DOT generado en: {filePath}");
        }

        // Método recursivo para generar la representación DOT del árbol
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

        // Método para verificar que el árbol está correctamente construido
        public bool VerificarArbolBinario()
        {
            return VerificarArbolBinario(raiz, int.MinValue, int.MaxValue);
        }

        private bool VerificarArbolBinario(NodoServicio? nodo, int min, int max)
        {
            // Un árbol vacío es un árbol binario de búsqueda válido
            if (nodo == null)
                return true;

            // Si el valor del nodo está fuera del rango permitido, el árbol no es válido
            if (nodo.Servicio.Id <= min || nodo.Servicio.Id >= max)
                return false;

            // Verificar recursivamente que los subárboles izquierdo y derecho también sean árboles binarios válidos
            return VerificarArbolBinario(nodo.Izquierdo, min, nodo.Servicio.Id) &&
                   VerificarArbolBinario(nodo.Derecho, nodo.Servicio.Id, max);
        }

        // Método para obtener servicios por vehículo
        public List<Servicio> ObtenerServiciosPorVehiculo(int idVehiculo)
        {
            List<Servicio> servicios = new List<Servicio>();
            ObtenerServiciosPorVehiculoRecursivo(raiz, idVehiculo, servicios);
            return servicios;
        }

        private void ObtenerServiciosPorVehiculoRecursivo(NodoServicio? nodo, int idVehiculo, List<Servicio> servicios)
        {
            if (nodo != null)
            {
                // Verificar si el servicio pertenece al vehículo
                if (nodo.Servicio.Id_Vehiculo == idVehiculo)
                {
                    servicios.Add(nodo.Servicio);
                }

                // Recorrer los subárboles izquierdo y derecho
                ObtenerServiciosPorVehiculoRecursivo(nodo.Izquierdo, idVehiculo, servicios);
                ObtenerServiciosPorVehiculoRecursivo(nodo.Derecho, idVehiculo, servicios);
            }
        }
    }
}