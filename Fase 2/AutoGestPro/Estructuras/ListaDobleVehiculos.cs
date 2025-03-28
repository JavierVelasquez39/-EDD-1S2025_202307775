using System;
using AutoGestPro.Modelos;

namespace AutoGestPro.Estructuras
{
    public class ListaDobleVehiculos
    {
        private NodoVehiculo? cabeza;
        private NodoVehiculo? cola;

        public ListaDobleVehiculos()
        {
            cabeza = null;
            cola = null;
        }

        // Método para agregar un vehículo
        public void AgregarVehiculo(Vehiculo nuevoVehiculo)
        {
            if (ExisteVehiculo(nuevoVehiculo.Id))
            {
                Console.WriteLine("❌ Error: El ID del vehículo ya existe en el sistema.");
                return;
            }

            NodoVehiculo nuevoNodo = new NodoVehiculo(nuevoVehiculo);

            if (cabeza == null)
            {
                cabeza = nuevoNodo;
                cola = nuevoNodo;
            }
            else
            {
                cola!.Siguiente = nuevoNodo;
                nuevoNodo.Anterior = cola;
                cola = nuevoNodo;
            }

            Console.WriteLine("✅ Vehículo agregado con éxito.");
        }

        // Método para eliminar un vehículo
        public void EliminarVehiculo(int id)
        {
            if (cabeza == null)
            {
                Console.WriteLine("⚠️ La lista está vacía.");
                return;
            }

            NodoVehiculo? actual = cabeza;

            while (actual != null && actual.Vehiculo.Id != id)
            {
                actual = actual.Siguiente;
            }

            if (actual == null)
            {
                Console.WriteLine("❌ Vehículo no encontrado.");
                return;
            }

            if (actual == cabeza)
            {
                cabeza = cabeza.Siguiente;
                if (cabeza != null)
                    cabeza.Anterior = null;
            }
            else if (actual == cola)
            {
                cola = cola.Anterior;
                if (cola != null)
                    cola.Siguiente = null;
            }
            else
            {
                actual.Anterior!.Siguiente = actual.Siguiente;
                actual.Siguiente!.Anterior = actual.Anterior;
            }

            Console.WriteLine("✅ Vehículo eliminado.");
        }

        // Método para buscar un vehículo
        public Vehiculo? BuscarVehiculo(int id)
        {
            NodoVehiculo? actual = cabeza;
            while (actual != null)
            {
                if (actual.Vehiculo.Id == id)
                {
                    return actual.Vehiculo;
                }
                actual = actual.Siguiente;
            }
            return null;
        }

        // Método para verificar si un vehículo existe
        private bool ExisteVehiculo(int id)
        {
            NodoVehiculo? actual = cabeza;
            while (actual != null)
            {
                if (actual.Vehiculo.Id == id)
                {
                    return true;
                }
                actual = actual.Siguiente;
            }
            return false;
        }

        // Método para mostrar vehículos en orden normal
        public void MostrarVehiculos()
        {
            if (cabeza == null)
            {
                Console.WriteLine("⚠️ No hay vehículos registrados.");
                return;
            }

            NodoVehiculo? actual = cabeza;
            Console.WriteLine("📋 Lista de Vehículos (Orden Normal):");
            while (actual != null)
            {
                Console.WriteLine(actual.Vehiculo);
                actual = actual.Siguiente;
            }
        }

        // Método para mostrar vehículos en orden inverso
        public void MostrarVehiculosInverso()
        {
            if (cola == null)
            {
                Console.WriteLine("⚠️ No hay vehículos registrados.");
                return;
            }

            NodoVehiculo? actual = cola;
            Console.WriteLine("📋 Lista de Vehículos (Orden Inverso):");
            while (actual != null)
            {
                Console.WriteLine(actual.Vehiculo);
                actual = actual.Anterior;
            }
        }

        // Método para generar el archivo .dot
        public void GenerarDot(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("digraph ListaDobleVehiculos {");
                writer.WriteLine("rankdir=LR;"); // Dirección de izquierda a derecha
                writer.WriteLine("node [shape=record];");

                NodoVehiculo? actual = cabeza;
                int contador = 0;

                while (actual != null)
                {
                    // Crear un nodo para cada vehículo
                    writer.WriteLine($"node{contador} [label=\"ID: {actual.Vehiculo.Id}\\nModelo: {actual.Vehiculo.Modelo}\"];");

                    // Crear la conexión hacia adelante
                    if (actual.Siguiente != null)
                    {
                        writer.WriteLine($"node{contador} -> node{contador + 1};");
                        writer.WriteLine($"node{contador + 1} -> node{contador};"); // Conexión inversa
                    }

                    actual = actual.Siguiente;
                    contador++;
                }

                writer.WriteLine("}");
            }

            Console.WriteLine($"✅ Archivo .dot generado en: {filePath}");
        }
    }
}
