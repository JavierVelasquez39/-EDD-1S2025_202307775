using AutoGestPro.Modelos;
using System;

namespace AutoGestPro.Estructuras
{
    public class ArbolBFacturas
    {
        private NodoB? raiz;
        private const int Orden = 5; // Orden del Árbol B

        public ArbolBFacturas()
        {
            raiz = null;
        }

        // Método para buscar una factura por ID
        public Factura? Buscar(int id)
        {
            return BuscarEnNodo(raiz, id);
        }

        private Factura? BuscarEnNodo(NodoB? nodo, int id)
        {
            if (nodo == null) return null;

            int i = 0;
            while (i < nodo.Claves.Count && id > nodo.Claves[i].Id)
            {
                i++;
            }

            if (i < nodo.Claves.Count && nodo.Claves[i].Id == id)
            {
                return nodo.Claves[i];
            }

            if (nodo.EsHoja)
            {
                return null;
            }

            return BuscarEnNodo(nodo.Hijos[i], id);
        }

        // Método para insertar una factura
        public void Insertar(Factura factura)
        {
            if (raiz == null)
            {
                raiz = new NodoB(true);
                raiz.Claves.Add(factura);
            }
            else
            {
                if (raiz.Claves.Count == Orden - 1)
                {
                    NodoB nuevaRaiz = new NodoB(false);
                    nuevaRaiz.Hijos.Add(raiz);
                    DividirNodo(nuevaRaiz, 0, raiz);
                    raiz = nuevaRaiz;
                }

                InsertarEnNodo(raiz, factura);
            }
        }

        private void DividirNodo(NodoB padre, int indice, NodoB nodo)
        {
            NodoB nuevoNodo = new NodoB(nodo.EsHoja);
            int mitad = (Orden - 1) / 2;

            // Mover las claves de la mitad superior al nuevo nodo
            for (int j = 0; j < mitad; j++)
            {
                if (mitad + 1 + j < nodo.Claves.Count)
                {
                    nuevoNodo.Claves.Add(nodo.Claves[mitad + 1 + j]);
                }
            }

            // Mover los hijos de la mitad superior al nuevo nodo (si no es hoja)
            if (!nodo.EsHoja)
            {
                for (int j = 0; j <= mitad; j++)
                {
                    if (mitad + 1 + j < nodo.Hijos.Count)
                    {
                        nuevoNodo.Hijos.Add(nodo.Hijos[mitad + 1 + j]);
                    }
                }
            }

            // Eliminar las claves y los hijos movidos del nodo original
            nodo.Claves.RemoveRange(mitad + 1, nodo.Claves.Count - (mitad + 1));
            if (!nodo.EsHoja && nodo.Hijos.Count > mitad + 1)
            {
                nodo.Hijos.RemoveRange(mitad + 1, nodo.Hijos.Count - (mitad + 1));
            }

            // Insertar el nuevo nodo y la clave mediana en el padre
            padre.Hijos.Insert(indice + 1, nuevoNodo);
            padre.Claves.Insert(indice, nodo.Claves[mitad]);
            nodo.Claves.RemoveAt(mitad);
        }

        private void InsertarEnNodo(NodoB nodo, Factura factura)
        {
            int i = nodo.Claves.Count - 1;

            if (nodo.EsHoja)
            {
                // Insertar la clave en el nodo hoja
                while (i >= 0 && factura.Id < nodo.Claves[i].Id)
                {
                    i--;
                }

                nodo.Claves.Insert(i + 1, factura);
            }
            else
            {
                // Encontrar el hijo correcto para insertar
                while (i >= 0 && factura.Id < nodo.Claves[i].Id)
                {
                    i--;
                }

                i++;
                if (i < nodo.Hijos.Count && nodo.Hijos[i]!.Claves.Count == Orden - 1)
                {
                    DividirNodo(nodo, i, nodo.Hijos[i]!);

                    if (factura.Id > nodo.Claves[i].Id)
                    {
                        i++;
                    }
                }

                if (i < nodo.Hijos.Count)
                {
                    InsertarEnNodo(nodo.Hijos[i]!, factura);
                }
            }
        }

        // Método para mostrar las facturas en orden
        public void MostrarFacturas()
        {
            MostrarEnOrden(raiz);
        }

        private void MostrarEnOrden(NodoB? nodo)
        {
            if (nodo == null) return;

            for (int i = 0; i < nodo.Claves.Count; i++)
            {
                // Mostrar el subárbol izquierdo antes de la clave
                if (!nodo.EsHoja && i < nodo.Hijos.Count)
                {
                    MostrarEnOrden(nodo.Hijos[i]);
                }

                // Mostrar la clave actual
                Console.WriteLine(nodo.Claves[i]);
            }

            // Mostrar el subárbol derecho después de la última clave
            if (!nodo.EsHoja && nodo.Hijos.Count > nodo.Claves.Count)
            {
                MostrarEnOrden(nodo.Hijos[nodo.Claves.Count]);
            }
        }    

        // Método para generar el archivo .dot
        public void GenerarDot(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("digraph ArbolBFacturas {");
                writer.WriteLine("node [shape=record];");

                if (raiz != null)
                {
                    GenerarDotRecursivo(raiz, writer);
                }

                writer.WriteLine("}");
            }

            Console.WriteLine($"✅ Archivo .dot generado en: {filePath}");
        }

        private void GenerarDotRecursivo(NodoB nodo, StreamWriter writer)
        {
            // Crear el nodo actual
            string nodoId = $"nodo{nodo.GetHashCode()}";
            string etiquetas = string.Join("|", nodo.Claves.ConvertAll(f => $"<f{f.Id}> ID: {f.Id}\\nTotal: {f.Total:C}"));
            writer.WriteLine($"{nodoId} [label=\"{etiquetas}\"];");

            // Conexión con los hijos
            for (int i = 0; i < nodo.Hijos.Count; i++)
            {
                if (nodo.Hijos[i] != null)
                {
                    string hijoId = $"nodo{nodo.Hijos[i]!.GetHashCode()}";
                    writer.WriteLine($"{nodoId}:f{i} -> {hijoId};");
                    GenerarDotRecursivo(nodo.Hijos[i]!, writer);
                }
            }
        }
        
    }
}
