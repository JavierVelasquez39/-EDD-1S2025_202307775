using System;
using System.Collections.Generic;
using AutoGestPro.Modelos;

namespace AutoGestPro.Estructuras
{
    public class GrafoNoDirigido
    {
        private Dictionary<int, Nodo> nodos;

        public GrafoNoDirigido()
        {
            nodos = new Dictionary<int, Nodo>();
        }

        // Agregar un nodo al grafo
        public void AgregarNodo(int id, string tipo)
        {
            if (!nodos.ContainsKey(id))
            {
                nodos[id] = new Nodo(id, tipo);
            }
        }

        // Agregar una arista entre dos nodos
        public void AgregarArista(int id1, int id2)
        {
            if (nodos.ContainsKey(id1) && nodos.ContainsKey(id2))
            {
                Nodo nodo1 = nodos[id1];
                Nodo nodo2 = nodos[id2];

                if (!nodo1.Adyacentes.Contains(nodo2))
                {
                    nodo1.Adyacentes.Add(nodo2);
                }

                if (!nodo2.Adyacentes.Contains(nodo1))
                {
                    nodo2.Adyacentes.Add(nodo1);
                }
            }
            else
            {
                Console.WriteLine("❌ Uno o ambos nodos no existen en el grafo.");
            }
        }

        // Registrar un servicio y generar relaciones en el grafo
        public void RegistrarServicio(int idVehiculo, List<int> idsRepuestos, ListaDobleVehiculos listaVehiculos, ArbolAVLRepuestos arbolRepuestos)
        {
            // Validar que el vehículo exista
            Vehiculo? vehiculo = listaVehiculos.BuscarVehiculo(idVehiculo);
            if (vehiculo == null)
            {
                Console.WriteLine($"❌ El vehículo con ID {idVehiculo} no existe.");
                return;
            }

            // Agregar el nodo del vehículo al grafo
            AgregarNodo(idVehiculo, "Vehiculo");

            // Validar y agregar los repuestos
            foreach (int idRepuesto in idsRepuestos)
            {
                Repuesto? repuesto = arbolRepuestos.Buscar(idRepuesto);
                if (repuesto == null)
                {
                    Console.WriteLine($"❌ El repuesto con ID {idRepuesto} no existe.");
                    continue;
                }

                // Agregar el nodo del repuesto al grafo
                AgregarNodo(idRepuesto, "Repuesto");

                // Crear la relación (arista) entre el vehículo y el repuesto
                AgregarArista(idVehiculo, idRepuesto);
            }

            Console.WriteLine("✅ Servicio registrado y relaciones generadas en el grafo.");
        }

        // Imprimir el grafo
        public void ImprimirGrafo()
        {
            foreach (var nodo in nodos.Values)
            {
                Console.Write($"{nodo} -> ");
                foreach (var adyacente in nodo.Adyacentes)
                {
                    Console.Write($"{adyacente} ");
                }
                Console.WriteLine();
            }
        }
    }
}