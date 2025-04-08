using System;
using System.Collections.Generic;
using AutoGestPro.Modelos;

namespace AutoGestPro.Estructuras
{
    public class Blockchain
    {
        public List<Block> Chain { get; private set; }
        public int Difficulty { get; set; }

        public Blockchain()
        {
            Chain = new List<Block>();
            Difficulty = 4; // Prefijo de 4 ceros
            AddGenesisBlock();
        }

        private void AddGenesisBlock()
        {
            Usuario genesisUser = new Usuario(0, "Genesis", "Block", "genesis@blockchain.com", 0, "0000");
            Block genesisBlock = new Block(0, genesisUser, "0000");
            genesisBlock.MineBlock(Difficulty);
            Chain.Add(genesisBlock);
        }

        public void AddBlock(Usuario data)
        {
            Block previousBlock = Chain[Chain.Count - 1];
            Block newBlock = new Block(previousBlock.Index + 1, data, previousBlock.Hash);
            newBlock.MineBlock(Difficulty);
            Chain.Add(newBlock);
        }

        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (currentBlock.Hash != currentBlock.GenerateHash())
                    return false;

                if (currentBlock.PreviousHash != previousBlock.Hash)
                    return false;
            }
            return true;
        }
    }
}