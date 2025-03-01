using System;

namespace AutoGestPro.DataStructures
{
    public unsafe class ListaVehiculos
    {
        private static ListaVehiculos? instancia;
        private NodoVehiculo* cabeza; // Primer nodo de la lista
        private NodoVehiculo* cola;   // Último nodo de la lista

        public ListaVehiculos()
        {
            cabeza = null;
            cola = null;
        }

        public static ListaVehiculos Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new ListaVehiculos();
                }
                return instancia;
            }
        }


        // Método para agregar un vehículo al final de la lista
        public void AgregarVehiculo(int id, int idUsuario, string marca, string modelo, string placa)
        {
            NodoVehiculo* nuevoVehiculo = (NodoVehiculo*)System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeof(NodoVehiculo));
            *nuevoVehiculo = new NodoVehiculo(id, idUsuario, marca, modelo, placa);

            if (cabeza == null)
            {
                cabeza = nuevoVehiculo;
                cola = nuevoVehiculo;
            }
            else
            {
                nuevoVehiculo->Anterior = cola;
                cola->Siguiente = nuevoVehiculo;
                cola = nuevoVehiculo;
            }
        }

        // Método para mostrar todos los vehículos
        public void MostrarVehiculos()
        {
            NodoVehiculo* actual = cabeza;
            while (actual != null)
            {
                Console.WriteLine($"ID: {actual->ID}, Marca: {GetString(actual->Marca)}, Modelo: {GetString(actual->Modelo)}, Placa: {GetString(actual->Placa)}");
                actual = actual->Siguiente;
            }
        }

        // Método para buscar un vehículo por ID
        public NodoVehiculo* BuscarVehiculo(int id)
        {
            NodoVehiculo* actual = cabeza;
            while (actual != null)
            {
                if (actual->ID == id)
                {
                    return actual;
                }
                actual = actual->Siguiente;
            }
            return null;
        }

        // Método para buscar vehículos por ID de usuario
        public List<Vehiculo> BuscarVehiculosPorUsuario(int idUsuario)
        {
            List<Vehiculo> vehiculos = new List<Vehiculo>();
            NodoVehiculo* actual = cabeza;
            while (actual != null)
            {
                if (actual->ID_Usuario == idUsuario)
                {
                    vehiculos.Add(new Vehiculo
                    {
                        ID = actual->ID,
                        IDUsuario = actual->ID_Usuario,
                        Marca = GetString(actual->Marca),
                        Modelo = GetString(actual->Modelo),
                        Placa = GetString(actual->Placa)
                    });
                }
                actual = actual->Siguiente;
            }
            return vehiculos;
        }

        // Método para eliminar un vehículo por ID
        public void EliminarVehiculo(int id)
        {
            if (cabeza == null) return;

            NodoVehiculo* actual = cabeza;

            while (actual != null)
            {
                if (actual->ID == id)
                {
                    if (actual->Anterior != null)
                        actual->Anterior->Siguiente = actual->Siguiente;
                    else
                        cabeza = actual->Siguiente; // Eliminar el primer nodo

                    if (actual->Siguiente != null)
                        actual->Siguiente->Anterior = actual->Anterior;
                    else
                        cola = actual->Anterior; // Eliminar el último nodo

                    System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)actual);
                    return;
                }
                actual = actual->Siguiente;
            }
        }

        // Función auxiliar para convertir char* a string
        private static string GetString(char* ptr)
        {
            return new string(ptr);
        }
    }

    public class Vehiculo {
        public int ID { get; set; }
        public int IDUsuario { get; set; }
        public required string Marca { get; set; }
        public required string Modelo { get; set; }
        public required string Placa { get; set; }

    }

}
