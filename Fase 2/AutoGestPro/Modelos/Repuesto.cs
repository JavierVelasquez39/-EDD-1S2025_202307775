using Newtonsoft.Json;

namespace AutoGestPro.Modelos
{
    public class Repuesto
    {
        public int Id { get; set; }

        [JsonProperty("Repuesto")]
        public string Nombre { get; set; }

        public string Detalles { get; set; }
        public decimal Costo { get; set; }

        public Repuesto(int id, string nombre, string detalles, decimal costo)
        {
            Id = id;
            Nombre = nombre;
            Detalles = detalles;
            Costo = costo;
        }

        public override string ToString()
        {
            return $"{Id}: {Nombre} - {Detalles} (Costo: {Costo:C})";
        }
    }
}