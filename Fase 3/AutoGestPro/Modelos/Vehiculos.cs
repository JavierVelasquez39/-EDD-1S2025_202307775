using Newtonsoft.Json;

namespace AutoGestPro.Modelos
{
    public class Vehiculo
    {
        [JsonProperty("ID")]
        public int Id { get; set; }

        [JsonProperty("ID_Usuario")]
        public int IdUsuario { get; set; }

        [JsonProperty("Marca")]
        public string Marca { get; set; }

        [JsonProperty("Modelo")]
        public int Modelo { get; set; }

        [JsonProperty("Placa")]
        public string Placa { get; set; }

        public Vehiculo(int id, int idUsuario, string marca, int modelo, string placa)
        {
            Id = id;
            IdUsuario = idUsuario;
            Marca = marca;
            Modelo = modelo;
            Placa = placa;
        }

        public override string ToString()
        {
            return $"{Id}: {Marca} {Modelo} - Placa: {Placa} (Propietario: {IdUsuario})";
        }
    }
}