using AutoGestPro.Modelos;
using System.Collections.Generic;

namespace AutoGestPro.Estructuras
{
    public class NodoB
    {
        public List<Factura> Claves { get; set; } // Lista de facturas en el nodo
        public List<NodoB?> Hijos { get; set; }   // Lista de hijos del nodo
        public bool EsHoja { get; set; }         // Indica si el nodo es una hoja

        public NodoB(bool esHoja)
        {
            Claves = new List<Factura>();
            Hijos = new List<NodoB?>();
            EsHoja = esHoja;
        }
    }
}