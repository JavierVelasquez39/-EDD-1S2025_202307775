using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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
            Block genesisBlock = new Block(0, "Genesis", "genesis@blockchain.com", "0000", "0000");
            genesisBlock.MineBlock(Difficulty);
            Chain.Add(genesisBlock);
        }

        public void AddBlock(string usuario, string correo, string contrasenia)
        {
            // Validar que los datos no sean nulos o vacíos
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasenia))
            {
                Console.WriteLine("❌ Error: Los datos del bloque no pueden estar vacíos.");
                return;
            }

            // Validar si el usuario ya existe
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

            // Guardar automáticamente el blockchain después de agregar un nuevo bloque
            GuardarEnArchivo("blockchain_usuarios.json");
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
                    block.ContraseniaTextoPlano == contrasenia) // Comparar con la contraseña en texto plano
                {
                    Console.WriteLine($"Autenticación exitosa para: {block.Usuario}");
                    return block; // Devolver el bloque completo
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
                        // Validar los bloques cargados
                        foreach (var block in chain)
                        {
                            if (block.Index > 0 && (string.IsNullOrWhiteSpace(block.Usuario) || string.IsNullOrWhiteSpace(block.Correo)))
                            {
                                Console.WriteLine($"⚠️ Bloque inválido encontrado: ID={block.Index}. Datos incompletos.");
                                continue; // Saltar bloques inválidos
                            }
                            blockchain.Chain.Add(block);
                        }

                        Console.WriteLine($"✅ Blockchain cargado exitosamente desde: {filePath} con {blockchain.Chain.Count} bloques válidos");
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
    }
}