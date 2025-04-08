namespace AutoGestPro.Modelos
{
    public class Servicio
    {
        public int Id { get; set; }
        public int Id_Repuesto { get; set; }
        public int Id_Vehiculo { get; set; }
        public string Detalles { get; set; }
        public decimal Costo { get; set; }

        public Servicio(int id, int idRepuesto, int idVehiculo, string detalles, decimal costo)
        {
            Id = id;
            Id_Repuesto = idRepuesto;
            Id_Vehiculo = idVehiculo;
            Detalles = detalles;
            Costo = costo;
        }

        public override string ToString()
        {
            return $"{Id}: Repuesto {Id_Repuesto}, Vehículo {Id_Vehiculo} - {Detalles} (Costo: {Costo:C})";
        }
    }
}