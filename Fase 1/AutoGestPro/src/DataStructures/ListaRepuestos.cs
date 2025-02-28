using System;
using System.Collections.Generic;

namespace AutoGestPro.DataStructures
{
    public unsafe class ListaRepuestos
    {
        private static ListaRepuestos? instancia; // Instancia única de la clase
        private NodoRepuesto* cabeza; // Primer nodo de la lista

        private ListaRepuestos()
        {
            cabeza = null;
        }

        // Método para obtener la instancia única de la clase
        public static ListaRepuestos Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new ListaRepuestos();
                }
                return instancia;
            }
        }

        // Método para agregar un repuesto a la lista
        public void AgregarRepuesto(int id, string nombre, string detalles, float costo)
        {
            NodoRepuesto* nuevoRepuesto = (NodoRepuesto*)System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeof(NodoRepuesto));
            *nuevoRepuesto = new NodoRepuesto(id, nombre, detalles, costo);

            if (cabeza == null)
            {
                cabeza = nuevoRepuesto;
                cabeza->Siguiente = cabeza; // Apunta a sí mismo (circular)
            }
            else
            {
                NodoRepuesto* actual = cabeza;
                while (actual->Siguiente != cabeza)
                {
                    actual = actual->Siguiente;
                }
                actual->Siguiente = nuevoRepuesto;
                nuevoRepuesto->Siguiente = cabeza;
            }
        }

        // Método para agregar múltiples repuestos desde una lista
        public void AgregarRepuestos(List<Repuesto> repuestos)
        {
            foreach (var repuesto in repuestos)
            {
                AgregarRepuesto(repuesto.ID, repuesto.Nombre, repuesto.Detalles, repuesto.Costo);
            }
        }

        // Método para mostrar todos los repuestos
        public void MostrarRepuestos()
        {
            if (cabeza == null) return;

            NodoRepuesto* actual = cabeza;
            do
            {
                Console.WriteLine($"ID: {actual->ID}, Nombre: {GetString(actual->Nombre)}, Detalles: {GetString(actual->Detalles)}, Costo: {actual->Costo}");
                actual = actual->Siguiente;
            } while (actual != cabeza);
        }

        // Método para buscar un repuesto por ID
        public NodoRepuesto* BuscarRepuesto(int id)
        {
            if (cabeza == null) return null;

            NodoRepuesto* actual = cabeza;
            do
            {
                if (actual->ID == id)
                {
                    return actual;
                }
                actual = actual->Siguiente;
            } while (actual != cabeza);

            return null;
        }

        // Método para eliminar un repuesto por ID
        public void EliminarRepuesto(int id)
        {
            if (cabeza == null) return;

            NodoRepuesto* actual = cabeza;
            NodoRepuesto* previo = null;

            do
            {
                if (actual->ID == id)
                {
                    if (previo != null)
                    {
                        previo->Siguiente = actual->Siguiente;
                        if (actual == cabeza) // Si eliminamos el primer nodo
                            cabeza = actual->Siguiente;
                    }
                    else
                    {
                        // Si solo hay un nodo en la lista
                        if (cabeza->Siguiente == cabeza)
                        {
                            cabeza = null;
                        }
                        else
                        {
                            // Encontrar el último nodo para ajustar el puntero
                            NodoRepuesto* temp = cabeza;
                            while (temp->Siguiente != cabeza)
                            {
                                temp = temp->Siguiente;
                            }
                            temp->Siguiente = cabeza->Siguiente;
                            cabeza = cabeza->Siguiente;
                        }
                    }

                    System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)actual);
                    return;
                }

                previo = actual;
                actual = actual->Siguiente;
            } while (actual != cabeza);
        }

        // Función auxiliar para convertir char* a string
        private static string GetString(char* ptr)
        {
            return new string(ptr);
        }
    }

    public class Repuesto
    {
        public int ID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Detalles { get; set; } = string.Empty;
        public float Costo { get; set; }
    }
}