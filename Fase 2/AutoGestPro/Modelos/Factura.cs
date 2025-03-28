namespace AutoGestPro.Modelos
{
    public class Factura
    {
        public int Id { get; set; }
        public int Id_Servicio { get; set; }
        public decimal Total { get; set; }

        public Factura(int id, int idServicio, decimal total)
        {
            Id = id;
            Id_Servicio = idServicio;
            Total = total;
        }

        public override string ToString()
        {
            return $"{Id}: Servicio {Id_Servicio} - Total: {Total:C}";
        }
    }
}