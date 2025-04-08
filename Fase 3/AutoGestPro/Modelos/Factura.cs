using System;

namespace AutoGestPro.Modelos
{
    public class Factura
    {
        public int Id { get; set; }
        public int Id_Servicio { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public string MetodoPago { get; set; }

        public Factura(int id, int idServicio, decimal total, DateTime fecha, string metodoPago)
        {
            Id = id;
            Id_Servicio = idServicio;
            Total = total;
            Fecha = fecha;
            MetodoPago = metodoPago;
        }

        public override string ToString()
        {
            return $"{Id}: Servicio {Id_Servicio} - Total: {Total:C} - Fecha: {Fecha} - Método de Pago: {MetodoPago}";
        }
    }
}