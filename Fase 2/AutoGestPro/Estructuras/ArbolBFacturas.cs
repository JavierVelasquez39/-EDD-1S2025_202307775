using AutoGestPro.Modelos;
using System;

namespace AutoGestPro.Estructuras
{
    [Serializable]
    public class ArbolBFacturas
    {
        private NodoB? raiz;
        private const int Orden = 5; // Orden del Árbol B

        public ArbolBFacturas()
        {
            raiz = null;
        }

        // Propiedad pública para acceder al nodo raíz
        public NodoB? Raiz
        {
            get { return raiz; }
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

        public void Eliminar(int id)
        {
            if (raiz == null)
            {
                Console.WriteLine("El árbol está vacío.");
                return;
            }

            EliminarEnNodo(raiz, id);

            // Si la raíz quedó vacía y no es hoja, ajustamos la raíz
            if (raiz.Claves.Count == 0 && !raiz.EsHoja)
            {
                raiz = raiz.Hijos[0];
            }
            else if (raiz.Claves.Count == 0)
            {
                raiz = null;
            }
        }

        private void EliminarEnNodo(NodoB nodo, int id)
        {
            int idx = 0;

            // Encontrar la posición de la clave a eliminar
            while (idx < nodo.Claves.Count && nodo.Claves[idx].Id < id)
            {
                idx++;
            }

            // Caso 1: La clave está en el nodo actual
            if (idx < nodo.Claves.Count && nodo.Claves[idx].Id == id)
            {
                if (nodo.EsHoja)
                {
                    // Caso 1a: El nodo es hoja
                    nodo.Claves.RemoveAt(idx);
                }
                else
                {
                    // Caso 1b: El nodo no es hoja
                    EliminarClaveNoHoja(nodo, idx);
                }
            }
            else
            {
                // Caso 2: La clave no está en el nodo actual
                if (nodo.EsHoja)
                {
                    Console.WriteLine("La clave no está en el árbol.");
                    return;
                }

                // Determinar si el hijo donde buscar tiene menos claves de las necesarias
                bool ultimoHijo = (idx == nodo.Claves.Count);
                if (nodo.Hijos[idx].Claves.Count < (Orden - 1) / 2)
                {
                    LlenarNodo(nodo, idx);
                }

                // Recursivamente eliminar en el hijo adecuado
                if (ultimoHijo && idx > nodo.Claves.Count)
                {
                    EliminarEnNodo(nodo.Hijos[idx - 1], id);
                }
                else
                {
                    EliminarEnNodo(nodo.Hijos[idx], id);
                }
            }
        }

        private void EliminarClaveNoHoja(NodoB nodo, int idx)
        {
            Factura clave = nodo.Claves[idx];

            // Caso 2a: El predecesor tiene suficientes claves
            if (nodo.Hijos[idx].Claves.Count >= (Orden - 1) / 2)
            {
                Factura predecesor = ObtenerPredecesor(nodo, idx);
                nodo.Claves[idx] = predecesor;
                EliminarEnNodo(nodo.Hijos[idx], predecesor.Id);
            }
            else if (nodo.Hijos[idx + 1].Claves.Count >= (Orden - 1) / 2)
            {
                // Caso 2b: El sucesor tiene suficientes claves
                Factura sucesor = ObtenerSucesor(nodo, idx);
                nodo.Claves[idx] = sucesor;
                EliminarEnNodo(nodo.Hijos[idx + 1], sucesor.Id);
            }
            else
            {
                // Caso 2c: Fusionar los hijos
                FusionarNodos(nodo, idx);
                EliminarEnNodo(nodo.Hijos[idx], clave.Id);
            }
        }

        private Factura ObtenerPredecesor(NodoB nodo, int idx)
        {
            NodoB actual = nodo.Hijos[idx];
            while (!actual.EsHoja)
            {
                actual = actual.Hijos[actual.Claves.Count];
            }
            return actual.Claves[actual.Claves.Count - 1];
        }

        private Factura ObtenerSucesor(NodoB nodo, int idx)
        {
            NodoB actual = nodo.Hijos[idx + 1];
            while (!actual.EsHoja)
            {
                actual = actual.Hijos[0];
            }
            return actual.Claves[0];
        }

        private void FusionarNodos(NodoB nodo, int idx)
        {
            NodoB hijoIzquierdo = nodo.Hijos[idx];
            NodoB hijoDerecho = nodo.Hijos[idx + 1];

            // Mover la clave del nodo actual al hijo izquierdo
            hijoIzquierdo.Claves.Add(nodo.Claves[idx]);

            // Mover las claves del hijo derecho al hijo izquierdo
            hijoIzquierdo.Claves.AddRange(hijoDerecho.Claves);

            // Mover los hijos del hijo derecho al hijo izquierdo
            if (!hijoIzquierdo.EsHoja)
            {
                hijoIzquierdo.Hijos.AddRange(hijoDerecho.Hijos);
            }

            // Eliminar la clave y el hijo derecho del nodo actual
            nodo.Claves.RemoveAt(idx);
            nodo.Hijos.RemoveAt(idx + 1);
        }

        private void LlenarNodo(NodoB nodo, int idx)
        {
            if (idx > 0 && nodo.Hijos[idx - 1].Claves.Count >= (Orden - 1) / 2)
            {
                TomarPrestadoIzquierda(nodo, idx);
            }
            else if (idx < nodo.Hijos.Count - 1 && nodo.Hijos[idx + 1].Claves.Count >= (Orden - 1) / 2)
            {
                TomarPrestadoDerecha(nodo, idx);
            }
            else
            {
                if (idx < nodo.Hijos.Count - 1)
                {
                    FusionarNodos(nodo, idx);
                }
                else
                {
                    FusionarNodos(nodo, idx - 1);
                }
            }
        }

        private void TomarPrestadoIzquierda(NodoB nodo, int idx)
        {
            NodoB hijo = nodo.Hijos[idx];
            NodoB hermano = nodo.Hijos[idx - 1];

            hijo.Claves.Insert(0, nodo.Claves[idx - 1]);
            nodo.Claves[idx - 1] = hermano.Claves[hermano.Claves.Count - 1];
            hermano.Claves.RemoveAt(hermano.Claves.Count - 1);

            if (!hijo.EsHoja)
            {
                hijo.Hijos.Insert(0, hermano.Hijos[hermano.Hijos.Count - 1]);
                hermano.Hijos.RemoveAt(hermano.Hijos.Count - 1);
            }
        }

        private void TomarPrestadoDerecha(NodoB nodo, int idx)
        {
            NodoB hijo = nodo.Hijos[idx];
            NodoB hermano = nodo.Hijos[idx + 1];

            hijo.Claves.Add(nodo.Claves[idx]);
            nodo.Claves[idx] = hermano.Claves[0];
            hermano.Claves.RemoveAt(0);

            if (!hijo.EsHoja)
            {
                hijo.Hijos.Add(hermano.Hijos[0]);
                hermano.Hijos.RemoveAt(0);
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
