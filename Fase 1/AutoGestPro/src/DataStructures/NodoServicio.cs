namespace AutoGestPro.DataStructures
{
    public unsafe struct NodoServicio
    {
        public int ID;
        public int ID_Repuesto;
        public int ID_Vehiculo;
        public fixed char Detalles[100];
        public float Costo;
        public NodoServicio* Siguiente; // Puntero al siguiente nodo en la cola

        public NodoServicio(int id, int idRepuesto, int idVehiculo, string detalles, float costo)
        {
            ID = id;
            ID_Repuesto = idRepuesto;
            ID_Vehiculo = idVehiculo;
            Costo = costo;
            Siguiente = null;

            // Copiar strings a los arreglos fijos
            fixed (char* ptr = Detalles) { CopyString(ptr, detalles, 100); }
        }

        // Función auxiliar para copiar strings a arreglos fijos
        private static void CopyString(char* destination, string source, int length)
        {
            for (int i = 0; i < length && i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }
    }
}
