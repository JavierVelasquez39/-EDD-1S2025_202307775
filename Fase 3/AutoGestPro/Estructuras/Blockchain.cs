using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AutoGestPro.Modelos;

namespace AutoGestPro.Estructuras
{
    public class Blockchain
    {
        public List<Block> Chain { get; set; }
        public int Difficulty { get; set; }

        public Blockchain()
        {
            Chain = new List<Block>();
            Difficulty = 4; // Prefijo de 4 ceros
            AddGenesisBlock();
        }

        private void AddGenesisBlock()
        {
            // Crear el bloque génesis con los datos del administrador
            Block genesisBlock = new Block(0, "Admin USAC", "admin@usac.com", "admin123", "0000");
            genesisBlock.MineBlock(Difficulty);
            Chain.Add(genesisBlock);
            Console.WriteLine("✅ Bloque génesis creado con el usuario administrador.");
        }

        public void AddBlock(string usuario, string correo, string contrasenia)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasenia))
            {
                Console.WriteLine("❌ Error: Los datos del bloque no pueden estar vacíos.");
                return;
            }

            if (Chain.Exists(block => block.Correo != null && block.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"❌ El usuario con correo {correo} ya existe en el blockchain.");
                return;
            }

            Console.WriteLine($"Registrando usuario {usuario} con contraseña: {contrasenia}");

            Block previousBlock = Chain[Chain.Count - 1];
            Block newBlock = new Block(previousBlock.Index + 1, usuario, correo, contrasenia, previousBlock.Hash);
            newBlock.MineBlock(Difficulty);
            Chain.Add(newBlock);

            GuardarEnArchivo("blockchain_usuarios.json");
        }

        public void CargarUsuariosDesdeJson(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"❌ Error: El archivo {filePath} no existe.");
                    return;
                }

                string json = File.ReadAllText(filePath);
                var usuarios = JsonSerializer.Deserialize<List<Usuario>>(json);

                if (usuarios == null || usuarios.Count == 0)
                {
                    Console.WriteLine("❌ Error: No se encontraron usuarios en el archivo JSON.");
                    return;
                }

                Console.WriteLine($"Archivo leído correctamente: {filePath}");

                foreach (var usuario in usuarios)
                {
                    if (string.IsNullOrWhiteSpace(usuario.Nombres) || string.IsNullOrWhiteSpace(usuario.Apellidos) ||
                        string.IsNullOrWhiteSpace(usuario.Correo) || string.IsNullOrWhiteSpace(usuario.Contrasenia))
                    {
                        Console.WriteLine("❌ Error: Los datos del usuario no pueden estar vacíos.");
                        continue;
                    }

                    AddBlock($"{usuario.Nombres} {usuario.Apellidos}", usuario.Correo, usuario.Contrasenia);
                }

                Console.WriteLine($"✅ Usuarios cargados correctamente en el Blockchain.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al cargar usuarios desde el JSON: {ex.Message}");
            }
        }

        public Block? AutenticarUsuario(string correo, string contrasenia)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasenia))
            {
                Console.WriteLine("Error de autenticación: Correo o contraseña vacíos");
                return null;
            }

            Console.WriteLine($"Intentando autenticar usuario con correo: {correo}");

            foreach (var block in Chain)
            {
                if (block.Index == 0) // Saltar el bloque génesis
                    continue;

                if (block.Correo != null && block.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase) &&
                    block.ContraseniaTextoPlano == contrasenia)
                {
                    Console.WriteLine($"Autenticación exitosa para: {block.Usuario}");
                    return block;
                }
            }

            Console.WriteLine("Usuario no encontrado o contraseña incorrecta");
            return null;
        }

        public void GuardarEnArchivo(string filePath)
        {
            try
            {
                string json = JsonSerializer.Serialize(Chain, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
                Console.WriteLine($"✅ Blockchain guardado exitosamente en: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al guardar el blockchain: {ex.Message}");
            }
        }

        public static Blockchain CargarDesdeArchivo(string filePath)
        {
            Blockchain blockchain = new Blockchain();

            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    var chain = JsonSerializer.Deserialize<List<Block>>(json);

                    if (chain != null && chain.Count > 0)
                    {
                        blockchain.Chain = chain;
                        Console.WriteLine($"✅ Blockchain cargado exitosamente desde: {filePath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error al cargar el blockchain: {ex.Message}");
                }
            }

            return blockchain;
        }

        public Block? BuscarUsuario(int id)
        {
            foreach (var block in Chain)
            {
                if (block.Index == id)
                {
                    return block;
                }
            }
            return null;
        }

        public void GenerarDot(string outputDirectory)
        {
            string dotFilePath = Path.Combine(outputDirectory, "BlockchainUsuarios.dot");

            using (StreamWriter writer = new StreamWriter(dotFilePath))
            {
                writer.WriteLine("digraph Blockchain {");
                writer.WriteLine("rankdir=LR;");
                writer.WriteLine("node [shape=record];");

                for (int i = 0; i < Chain.Count; i++)
                {
                    Block block = Chain[i];
                    string blockLabel = $"\"Block {block.Index}\" [label=\"{{Index: {block.Index}|Usuario: {block.Usuario}|Correo: {block.Correo}|Hash: {block.Hash}|PreviousHash: {block.PreviousHash}}}\"];";
                    writer.WriteLine(blockLabel);

                    if (i > 0)
                    {
                        writer.WriteLine($"\"Block {Chain[i - 1].Index}\" -> \"Block {block.Index}\";");
                    }
                }

                writer.WriteLine("}");
            }

            Console.WriteLine($"✅ Archivo DOT generado en: {dotFilePath}");
        }

        public bool ValidarBlockchain()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block actual = Chain[i];
                Block anterior = Chain[i - 1];

                if (actual.PreviousHash != anterior.Hash)
                {
                    Console.WriteLine($"❌ Error: Blockchain corrupto en el bloque {actual.Index}. PreviousHash no coincide.");
                    return false;
                }

                if (actual.Hash != actual.GenerateHash())
                {
                    Console.WriteLine($"❌ Error: Blockchain corrupto en el bloque {actual.Index}. Hash no válido.");
                    return false;
                }
            }

            return true;
        }
    }
}