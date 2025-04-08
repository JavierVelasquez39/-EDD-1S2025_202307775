using System;
using System.Collections.Generic;
using AutoGestPro.Modelos;

namespace AutoGestPro.Estructuras
{
    public class MerkleTree
    {
        public MerkleNode Root { get; private set; }
        private List<MerkleNode> Leaves { get; set; }

        public MerkleTree()
        {
            Leaves = new List<MerkleNode>();
        }

        public void AddFactura(Factura factura)
        {
            string data = $"{factura.Id}{factura.Id_Servicio}{factura.Total}{factura.Fecha}{factura.MetodoPago}";
            Leaves.Add(new MerkleNode(data));
            BuildTree();
        }

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

            Root = currentLevel[0];
        }

        public void PrintTree()
        {
            Console.WriteLine("Raíz del Árbol de Merkle: " + Root?.Hash);
        }
    }
}