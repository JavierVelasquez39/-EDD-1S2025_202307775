using System;
using System.Runtime.InteropServices;

namespace AutoGestPro.DataStructures
{
    public unsafe struct NodoRepuesto
    {
        public int ID;
        public char* Nombre;
        public char* Detalles;
        public float Costo;
        public NodoRepuesto* Siguiente;

        public NodoRepuesto(int id, string nombre, string detalles, float costo)
        {
            ID = id;
            Nombre = (char*)Marshal.AllocHGlobal((nombre?.Length ?? 0) * sizeof(char));
            Detalles = (char*)Marshal.AllocHGlobal((detalles?.Length ?? 0) * sizeof(char));
            Costo = costo;
            Siguiente = null;

            if (nombre != null)
            {
                CopyString(Nombre, nombre, nombre.Length);
            }

            if (detalles != null)
            {
                CopyString(Detalles, detalles, detalles.Length);
            }
        }

        public static void CopyString(char* destination, string source, int length)
        {
            for (int i = 0; i < length; i++)
            {
                destination[i] = source[i];
            }
        }
    }
}