using System;

namespace AutoGestPro.DataStructures
{
    public unsafe class MatrizBitacora
    {
        private NodoBitacora* cabeza; // Nodo inicial de la matriz

        public MatrizBitacora()
        {
            cabeza = null;
        }

        // Método para agregar o actualizar un nodo en la matriz
        public void AgregarRegistro(int idVehiculo, int idRepuesto, int cantidad)
        {
            NodoBitacora* actual = cabeza;
            NodoBitacora* previoFila = null;
            NodoBitacora* previoColumna = null;

            // Buscar si ya existe la relación en la matriz
            while (actual != null)
            {
                if (actual->ID_Vehiculo == idVehiculo && actual->ID_Repuesto == idRepuesto)
                {
                    actual->Cantidad += cantidad; // Si existe, solo actualizamos la cantidad
                    return;
                }

                if (actual->ID_Vehiculo < idVehiculo)
                {
                    previoFila = actual;
                    actual = actual->Abajo;
                }
                else if (actual->ID_Repuesto < idRepuesto)
                {
                    previoColumna = actual;
                    actual = actual->Derecha;
                }
                else
                {
                    break;
                }
            }

            // Crear un nuevo nodo
            NodoBitacora* nuevoNodo = (NodoBitacora*)System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeof(NodoBitacora));
            *nuevoNodo = new NodoBitacora(idVehiculo, idRepuesto, cantidad);

            // Insertar en la matriz
            if (previoFila == null && previoColumna == null)
            {
                cabeza = nuevoNodo;
            }
            else if (previoFila != null)
            {
                nuevoNodo->Abajo = previoFila->Abajo;
                previoFila->Abajo = nuevoNodo;
            }
            else if (previoColumna != null)
            {
                nuevoNodo->Derecha = previoColumna->Derecha;
                previoColumna->Derecha = nuevoNodo;
            }
        }

        // Método para mostrar la bitácora en formato de matriz
        public void MostrarBitacora()
        {
            if (cabeza == null)
            {
                Console.WriteLine("⚠️ La bitácora está vacía.");
                return;
            }

            NodoBitacora* actualFila = cabeza;
            while (actualFila != null)
            {
                NodoBitacora* actualColumna = actualFila;
                while (actualColumna != null)
                {
                    Console.Write($"[{actualColumna->ID_Vehiculo}, {actualColumna->ID_Repuesto}] = {actualColumna->Cantidad}   ");
                    actualColumna = actualColumna->Derecha;
                }
                Console.WriteLine();
                actualFila = actualFila->Abajo;
            }
        }
    }
}
