namespace AutoGestPro.DataStructures
{
    public unsafe struct NodoVehiculo
    {
        public int ID;
        public int ID_Usuario;
        public fixed char Marca[30];
        public fixed char Modelo[30];
        public fixed char Placa[10];

        public NodoVehiculo* Anterior; // Puntero al nodo anterior
        public NodoVehiculo* Siguiente; // Puntero al nodo siguiente

        public NodoVehiculo(int id, int idUsuario, string marca, string modelo, string placa)
        {
            ID = id;
            ID_Usuario = idUsuario;
            Anterior = null;
            Siguiente = null;

            // Copiar strings a los arreglos fijos
            fixed (char* ptr = Marca) { CopyString(ptr, marca, 30); }
            fixed (char* ptr = Modelo) { CopyString(ptr, modelo, 30); }
            fixed (char* ptr = Placa) { CopyString(ptr, placa, 10); }
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
