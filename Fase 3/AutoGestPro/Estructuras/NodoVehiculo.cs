using AutoGestPro.Modelos;

namespace AutoGestPro.Estructuras
{
    public class NodoVehiculo
    {
        public Vehiculo Vehiculo { get; set; }
        public NodoVehiculo? Anterior { get; set; }
        public NodoVehiculo? Siguiente { get; set; }

        public NodoVehiculo(Vehiculo vehiculo)
        {
            Vehiculo = vehiculo;
            Anterior = null;
            Siguiente = null;
        }
    }
}
