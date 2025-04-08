using System.Collections.Generic;

namespace AutoGestPro.Estructuras
{
    public class Nodo
    {
        public int Id { get; set; }
        public string Tipo { get; set; } // "Vehiculo" o "Repuesto"
        public List<Nodo> Adyacentes { get; set; }

        public Nodo(int id, string tipo)
        {
            Id = id;
            Tipo = tipo;
            Adyacentes = new List<Nodo>();
        }

        public override string ToString()
        {
            return $"{Tipo} (ID: {Id})";
        }
    }
}