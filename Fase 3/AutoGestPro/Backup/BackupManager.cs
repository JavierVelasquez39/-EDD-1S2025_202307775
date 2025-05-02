using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using AutoGestPro.Estructuras;
using AutoGestPro.Modelos;

namespace AutoGestPro.Backup
{
    public static class BackupManager
    {
        public static bool CargarBackup(
            string blockchainPath,
            string vehiculosBackupPath,
            string repuestosBackupPath,
            Blockchain blockchain,
            ListaDobleVehiculos listaVehiculos,
            ArbolAVLRepuestos arbolRepuestos)
        {
            try
            {
                // 1. Cargar Blockchain de Usuarios
                Console.WriteLine("Cargando Blockchain de Usuarios...");
                Blockchain blockchainCargado = Blockchain.CargarDesdeArchivo(blockchainPath);

                if (!ValidarBlockchain(blockchainCargado))
                {
                    Console.WriteLine("❌ Error: Blockchain corrupto. No se puede proceder con la carga.");
                    return false;
                }

                blockchain.Chain = blockchainCargado.Chain;
                Console.WriteLine("✅ Blockchain cargado correctamente.");

                // 2. Descomprimir y cargar Vehículos
                Console.WriteLine("Cargando Vehículos desde el backup...");
                int vehiculosCargados = CargarVehiculosDesdeBackup(vehiculosBackupPath, listaVehiculos);
                Console.WriteLine($"✅ Vehículos cargados: {vehiculosCargados}");

                // 3. Descomprimir y cargar Repuestos
                Console.WriteLine("Cargando Repuestos desde el backup...");
                int repuestosCargados = CargarRepuestosDesdeBackup(repuestosBackupPath, arbolRepuestos);
                Console.WriteLine($"✅ Repuestos cargados: {repuestosCargados}");

                // 4. Validar consistencia
                if (!ValidarConsistencia(vehiculosCargados, repuestosCargados, listaVehiculos, arbolRepuestos))
                {
                    Console.WriteLine("❌ Error: Inconsistencias detectadas en los datos. No se puede proceder con la carga.");
                    return false;
                }

                Console.WriteLine("✅ Backup cargado correctamente y validado.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al cargar el backup: {ex.Message}");
                return false;
            }
        }

        public static void GenerarBackup(
            string vehiculosBackupPath,
            string repuestosBackupPath,
            ListaDobleVehiculos listaVehiculos,
            ArbolAVLRepuestos arbolRepuestos)
        {
            try
            {
                // 1. Crear el archivo de backup para vehículos
                Console.WriteLine("Generando backup de vehículos...");
                string vehiculosJson = JsonSerializer.Serialize(listaVehiculos.ObtenerTodosLosVehiculos(), new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(vehiculosBackupPath, vehiculosJson);
                Console.WriteLine($"✅ Backup de vehículos generado en: {vehiculosBackupPath}");

                // 2. Crear el archivo de backup para repuestos
                Console.WriteLine("Generando backup de repuestos...");
                string repuestosJson = JsonSerializer.Serialize(arbolRepuestos.ObtenerTodosLosRepuestos(), new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(repuestosBackupPath, repuestosJson);
                Console.WriteLine($"✅ Backup de repuestos generado en: {repuestosBackupPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al generar el backup: {ex.Message}");
            }
        }

        private static int CargarVehiculosDesdeBackup(string backupPath, ListaDobleVehiculos listaVehiculos)
        {
            if (!File.Exists(backupPath))
            {
                throw new FileNotFoundException($"El archivo de backup de vehículos no existe: {backupPath}");
            }

            string json = File.ReadAllText(backupPath);
            var vehiculos = JsonSerializer.Deserialize<List<Vehiculo>>(json);

            if (vehiculos == null)
            {
                throw new Exception("No se pudieron deserializar los datos de vehículos.");
            }

            foreach (var vehiculo in vehiculos)
            {
                listaVehiculos.AgregarVehiculo(vehiculo);
            }

            return vehiculos.Count;
        }

        private static int CargarRepuestosDesdeBackup(string backupPath, ArbolAVLRepuestos arbolRepuestos)
        {
            if (!File.Exists(backupPath))
            {
                throw new FileNotFoundException($"El archivo de backup de repuestos no existe: {backupPath}");
            }

            string json = File.ReadAllText(backupPath);
            var repuestos = JsonSerializer.Deserialize<List<Repuesto>>(json);

            if (repuestos == null)
            {
                throw new Exception("No se pudieron deserializar los datos de repuestos.");
            }

            foreach (var repuesto in repuestos)
            {
                arbolRepuestos.Insertar(repuesto);
            }

            return repuestos.Count;
        }

        private static bool ValidarBlockchain(Blockchain blockchain)
        {
            for (int i = 1; i < blockchain.Chain.Count; i++)
            {
                Block actual = blockchain.Chain[i];
                Block anterior = blockchain.Chain[i - 1];

                // Validar el PreviousHash
                if (actual.PreviousHash != anterior.Hash)
                {
                    Console.WriteLine($"❌ Error: Blockchain corrupto en el bloque {actual.Index}. PreviousHash no coincide.");
                    return false;
                }

                // Validar el Hash actual
                if (actual.Hash != actual.GenerateHash())
                {
                    Console.WriteLine($"❌ Error: Blockchain corrupto en el bloque {actual.Index}. Hash no válido.");
                    return false;
                }
            }

            return true;
        }

        private static bool ValidarConsistencia(
            int vehiculosCargados,
            int repuestosCargados,
            ListaDobleVehiculos listaVehiculos,
            ArbolAVLRepuestos arbolRepuestos)
        {
            // Validar la cantidad de vehículos
            int vehiculosEnLista = 0;
            NodoVehiculo? actual = listaVehiculos.ObtenerCabeza();
            while (actual != null)
            {
                vehiculosEnLista++;
                actual = actual.Siguiente;
            }

            if (vehiculosCargados != vehiculosEnLista)
            {
                Console.WriteLine($"❌ Error: Inconsistencia en la cantidad de vehículos. Cargados: {vehiculosCargados}, En lista: {vehiculosEnLista}");
                return false;
            }

            // Validar la cantidad de repuestos
            int repuestosEnArbol = arbolRepuestos.ContarNodos();
            if (repuestosCargados != repuestosEnArbol)
            {
                Console.WriteLine($"❌ Error: Inconsistencia en la cantidad de repuestos. Cargados: {repuestosCargados}, En árbol: {repuestosEnArbol}");
                return false;
            }

            return true;
        }
    }
}