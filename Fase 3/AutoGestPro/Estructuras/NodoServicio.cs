using AutoGestPro.Modelos;

namespace AutoGestPro.Estructuras
{
    public class NodoServicio
    {
        public Servicio Servicio { get; set; }
        public NodoServicio? Izquierdo { get; set; }
        public NodoServicio? Derecho { get; set; }

        public NodoServicio(Servicio servicio)
        {
            Servicio = servicio;
            Izquierdo = null;
            Derecho = null;
        }
    }
}