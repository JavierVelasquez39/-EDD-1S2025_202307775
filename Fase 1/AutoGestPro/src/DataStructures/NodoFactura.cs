namespace AutoGestPro.DataStructures
{
    public unsafe struct NodoFactura
    {
        public int ID;
        public int ID_Orden;
        public float Total;
        public NodoFactura* Siguiente; // Puntero al nodo anterior en la pila

        public NodoFactura(int id, int idOrden, float total)
        {
            ID = id;
            ID_Orden = idOrden;
            Total = total;
            Siguiente = null;
        }
    }
}
