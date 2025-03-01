using System;

namespace AutoGestPro.DataStructures
{
    public unsafe class PilaFacturas
    {
        private NodoFactura* cima; // Última factura agregada (top de la pila)

        public PilaFacturas()
        {
            cima = null;
        }

        // Método para agregar una factura a la pila (Push)
        public void ApilarFactura(int id, int idOrden, float total)
        {
            NodoFactura* nuevaFactura = (NodoFactura*)System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeof(NodoFactura));
            *nuevaFactura = new NodoFactura(id, idOrden, total);

            nuevaFactura->Siguiente = cima;
            cima = nuevaFactura;
        }

        // Método para procesar la última factura (Pop)
        public void DesapilarFactura()
        {
            if (cima == null)
            {
                Console.WriteLine("⚠️ No hay facturas en la pila.");
                return;
            }

            NodoFactura* temp = cima;
            cima = cima->Siguiente;

            Console.WriteLine($"Factura procesada - ID: {temp->ID}, Orden: {temp->ID_Orden}, Total: {temp->Total}");

            System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)temp);
        }

        // Método para mostrar todas las facturas pendientes
        public void MostrarFacturas()
        {
            if (cima == null)
            {
                Console.WriteLine("⚠️ No hay facturas en la pila.");
                return;
            }

            NodoFactura* actual = cima;
            while (actual != null)
            {
                Console.WriteLine($"ID: {actual->ID}, Orden: {actual->ID_Orden}, Total: {actual->Total}");
                actual = actual->Siguiente;
            }
        }
    }
}
