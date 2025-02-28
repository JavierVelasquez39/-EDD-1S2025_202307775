namespace AutoGestPro.DataStructures
{
    public unsafe struct NodoBitacora
    {
        public int ID_Vehiculo;
        public int ID_Repuesto;
        public int Cantidad;

        public NodoBitacora* Derecha; // Puntero al siguiente nodo en la fila
        public NodoBitacora* Abajo;   // Puntero al siguiente nodo en la columna

        public NodoBitacora(int idVehiculo, int idRepuesto, int cantidad)
        {
            ID_Vehiculo = idVehiculo;
            ID_Repuesto = idRepuesto;
            Cantidad = cantidad;
            Derecha = null;
            Abajo = null;
        }
    }
}
