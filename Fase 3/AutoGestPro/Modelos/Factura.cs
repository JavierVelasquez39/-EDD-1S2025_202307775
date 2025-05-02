namespace AutoGestPro.Modelos
{
    public class Factura
    {
        public int Id { get; set; }
        public int Id_Servicio { get; set; }
        public int Id_Vehiculo { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public string MetodoPago { get; set; }

        public Factura(int id, int idServicio, int idVehiculo, decimal total, DateTime fecha, string metodoPago)
        {
            Id = id;
            Id_Servicio = idServicio;
            Id_Vehiculo = idVehiculo;
            Total = total;
            Fecha = fecha;
            MetodoPago = metodoPago;
        }

        public override string ToString()
        {
            return $"{Id}|{Id_Servicio}|{Id_Vehiculo}|{Total}|{Fecha:yyyy-MM-dd}|{MetodoPago}";
        }
    }
}