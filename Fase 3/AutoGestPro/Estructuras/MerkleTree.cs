using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AutoGestPro.Modelos;

namespace AutoGestPro.Estructuras
{
    public class MerkleTree
    {
        public MerkleNode Root { get; private set; }
        public List<MerkleNode> Leaves { get; set; }

        public MerkleTree()
        {
            Leaves = new List<MerkleNode>();
        }

        // Agregar una factura al árbol
        public void AddFactura(Factura factura)
        {
            // Estructurar los datos de la factura para incluir toda la información relevante
            string data = $"{factura.Id}|{factura.Id_Servicio}|{factura.Id_Vehiculo}|{factura.Total}|{factura.Fecha:yyyy-MM-dd}|{factura.MetodoPago}";
            Leaves.Add(new MerkleNode(data));
            BuildTree();
        }

        // Construir el árbol de Merkle
        private void BuildTree()
        {
            List<MerkleNode> currentLevel = Leaves;

            while (currentLevel.Count > 1)
            {
                List<MerkleNode> nextLevel = new List<MerkleNode>();

                for (int i = 0; i < currentLevel.Count; i += 2)
                {
                    if (i + 1 < currentLevel.Count)
                    {
                        // Combinar dos nodos
                        nextLevel.Add(new MerkleNode(currentLevel[i], currentLevel[i + 1]));
                    }
                    else
                    {
                        // Nodo impar, se pasa al siguiente nivel
                        nextLevel.Add(currentLevel[i]);
                    }
                }

                currentLevel = nextLevel;
            }

            Root = currentLevel.Count > 0 ? currentLevel[0] : null;
        }

        // Imprimir la raíz del árbol
        public void PrintTree()
        {
            Console.WriteLine("Raíz del Árbol de Merkle: " + Root?.Hash);
        }

        // Generar el archivo .dot y la imagen del árbol de Merkle
        public void GenerarDot(string filePath)
        {
            string dotFilePath = Path.Combine(filePath, "MerkleTree.dot");
            string imageFilePath = Path.Combine(filePath, "MerkleTree.png");

            using (StreamWriter writer = new StreamWriter(dotFilePath))
            {
                writer.WriteLine("digraph MerkleTree {");
                writer.WriteLine("node [shape=record];");

                if (Root != null)
                {
                    GenerarDotRecursivo(Root, writer);
                }

                writer.WriteLine("}");
            }

            // Generar la imagen utilizando Graphviz
            GenerarImagen(dotFilePath, imageFilePath);
        }

        private void GenerarDotRecursivo(MerkleNode nodo, StreamWriter writer)
        {
            // Verificar si el nodo es una hoja (contiene datos de una factura)
            if (nodo.Left == null && nodo.Right == null)
            {
                // Extraer los datos de la factura desde el nodo
                string[] datos = nodo.Data.Split('|');
                string id = datos.Length > 0 ? datos[0] : "N/A";
                string idServicio = datos.Length > 1 ? datos[1] : "N/A";
                string idVehiculo = datos.Length > 2 ? datos[2] : "N/A";
                string total = datos.Length > 3 ? datos[3] : "N/A";
                string fecha = datos.Length > 4 ? datos[4] : "N/A";
                string metodoPago = datos.Length > 5 ? datos[5] : "N/A";

                // Crear el nodo con los detalles de la factura
                writer.WriteLine($"\"{nodo.Hash}\" [label=\"{{ID: {id}|ID Servicio: {idServicio}|ID Vehículo: {idVehiculo}|Total: {total}|Fecha: {fecha}|Método de Pago: {metodoPago}}}\"];");
            }
            else
            {
                // Crear el nodo con solo el hash
                writer.WriteLine($"\"{nodo.Hash}\" [label=\"{nodo.Hash}\"];");
            }

            // Conexión con el hijo izquierdo
            if (nodo.Left != null)
            {
                writer.WriteLine($"\"{nodo.Hash}\" -> \"{nodo.Left.Hash}\";");
                GenerarDotRecursivo(nodo.Left, writer);
            }

            // Conexión con el hijo derecho
            if (nodo.Right != null)
            {
                writer.WriteLine($"\"{nodo.Hash}\" -> \"{nodo.Right.Hash}\";");
                GenerarDotRecursivo(nodo.Right, writer);
            }
        }

        private void GenerarImagen(string dotFilePath, string imageFilePath)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "dot";
                process.StartInfo.Arguments = $"-Tpng \"{dotFilePath}\" -o \"{imageFilePath}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Console.WriteLine($"✅ Imagen del Árbol de Merkle generada correctamente: {imageFilePath}");
                }
                else
                {
                    string error = process.StandardError.ReadToEnd();
                    Console.WriteLine($"❌ Error al generar la imagen del Árbol de Merkle: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al ejecutar Graphviz: {ex.Message}");
            }
        }

        // Buscar una factura por ID
        public Factura? BuscarFactura(int idFactura)
        {
            foreach (var leaf in Leaves)
            {
                if (leaf.Data != null)
                {
                    string[] datos = leaf.Data.Split('|');
                    if (int.TryParse(datos[0], out int id) && id == idFactura)
                    {
                        return new Factura(
                            id: id,
                            idServicio: int.Parse(datos[1]),
                            idVehiculo: int.Parse(datos[2]),
                            total: decimal.Parse(datos[3]),
                            fecha: DateTime.Parse(datos[4]),
                            metodoPago: datos[5]
                        );
                    }
                }
            }
            return null;
        }

        // Eliminar una factura del árbol
        public void EliminarFactura(int idFactura)
        {
            MerkleNode? nodoEliminar = Leaves.Find(nodo =>
            {
                if (nodo.Data != null)
                {
                    string[] datos = nodo.Data.Split('|');
                    return int.TryParse(datos[0], out int id) && id == idFactura;
                }
                return false;
            });

            if (nodoEliminar != null)
            {
                Leaves.Remove(nodoEliminar);
                BuildTree(); // Reconstruir el árbol
                Console.WriteLine($"✅ Factura con ID {idFactura} eliminada del Árbol de Merkle.");
            }
            else
            {
                Console.WriteLine($"❌ Factura con ID {idFactura} no encontrada en el Árbol de Merkle.");
            }
        }

    }
}