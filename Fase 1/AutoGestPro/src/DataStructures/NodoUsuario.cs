namespace AutoGestPro.DataStructures
{
    public unsafe struct NodoUsuario
    {
        public int ID;
        public fixed char Nombres[50];
        public fixed char Apellidos[50];
        public fixed char Correo[50];
        public fixed char Contrasenia[50];
        public NodoUsuario* Siguiente; // Puntero al siguiente nodo

        public NodoUsuario(int id, string nombres, string apellidos, string correo, string contrasenia)
        {
            ID = id;
            Siguiente = null;

            // Copiar strings a los arreglos fijos (para evitar referencias a objetos en heap)
            fixed (char* ptr = Nombres) { CopyString(ptr, nombres, 50); }
            fixed (char* ptr = Apellidos) { CopyString(ptr, apellidos, 50); }
            fixed (char* ptr = Correo) { CopyString(ptr, correo, 50); }
            fixed (char* ptr = Contrasenia) { CopyString(ptr, contrasenia, 50); }
        }

        // Función auxiliar para copiar strings a arreglos fijos
        public static void CopyString(char* destination, string source, int length)
        {
            for (int i = 0; i < length && i < source.Length; i++)
            {
                destination[i] = source[i];
            }
            for (int i = source.Length; i < length; i++)
            {
                destination[i] = '\0'; // Rellenar con caracteres nulos
            }
        }
    }
}

