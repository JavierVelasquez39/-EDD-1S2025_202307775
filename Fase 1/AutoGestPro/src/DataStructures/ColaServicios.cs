using System;

namespace AutoGestPro.DataStructures
{
    public unsafe class ColaServicios
    {
        private static ColaServicios? instancia;
        private NodoServicio* frente;  // Primer nodo en la cola
        private NodoServicio* final;   // Último nodo en la cola

        public ColaServicios()
        {
            frente = null;
            final = null;
        }

        // Método para obtener la instancia de la única de clase
        public static ColaServicios Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new ColaServicios();
                }
                return instancia;
            }
        }

        // Método para agregar un servicio a la cola (Encolar)
        public void EncolarServicio(int id, int idRepuesto, int idVehiculo, string detalles, float costo)
        {
            NodoServicio* nuevoServicio = (NodoServicio*)System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeof(NodoServicio));
            *nuevoServicio = new NodoServicio(id, idRepuesto, idVehiculo, detalles, costo);

            if (frente == null)
            {
                frente = nuevoServicio;
                final = nuevoServicio;
            }
            else
            {
                final->Siguiente = nuevoServicio;
                final = nuevoServicio;
            }
        }

        // Método para atender (desencolar) un servicio
        public void DesencolarServicio()
        {
            if (frente == null)
            {
                Console.WriteLine("⚠️ La cola de servicios está vacía.");
                return;
            }

            NodoServicio* temp = frente;
            frente = frente->Siguiente;

            if (frente == null) // Si eliminamos el último nodo
                final = null;

            System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)temp);
        }

        // Método para mostrar todos los servicios en espera
        public void MostrarServicios()
        {
            if (frente == null)
            {
                Console.WriteLine("⚠️ No hay servicios en la cola.");
                return;
            }

            NodoServicio* actual = frente;
            while (actual != null)
            {
                Console.WriteLine($"ID: {actual->ID}, Repuesto: {actual->ID_Repuesto}, Vehículo: {actual->ID_Vehiculo}, Detalles: {GetString(actual->Detalles)}, Costo: {actual->Costo}");
                actual = actual->Siguiente;
            }
        }

        // Función auxiliar para convertir char* a string
        private static string GetString(char* ptr)
        {
            return new string(ptr);
        }
    }

    public class Servicio
    {
        public int ID { get; set; }
        public int IDRepuesto { get; set; }
        public int IDVehiculo { get; set; }
        public required string Detalles { get; set; }
        public float Costo { get; set; }
    }
}
