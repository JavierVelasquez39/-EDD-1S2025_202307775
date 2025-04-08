using AutoGestPro.Modelos;

namespace AutoGestPro.Estructuras
{
    public class NodoAVL
    {
        public Repuesto Repuesto { get; set; }
        public NodoAVL? Izquierdo { get; set; }
        public NodoAVL? Derecho { get; set; }
        public int Altura { get; set; }

        public NodoAVL(Repuesto repuesto)
        {
            Repuesto = repuesto;
            Izquierdo = null;
            Derecho = null;
            Altura = 1;
        }
    }
}