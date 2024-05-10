﻿using Epoche;
using System.Text;

namespace Indexer.Services
{
    public class MerkleTreeService
    {
        private const int HashWidth = 20;
        public byte[][][] CreateMerkleTree(int depth, byte[][] leaves)
        {
            var tree = new byte[depth + 1][][];
            tree[0] = leaves;

            //Initialize arrays
            for (var i = 1; i < depth + 1; i++)
            {
                tree[i] = new byte[tree[i - 1].Length / 2][];
            }

            //Hash leafs
            for (var layer = 0; layer < depth; layer++)
            {
                for (var leaf = 0; leaf < tree[layer].Length; leaf += 2)
                {
                    List<byte> list = [];
                    list.AddRange(tree[layer][leaf]);
                    list.AddRange(tree[layer][leaf + 1]);

                    //Apply hash function & save to the layer above
                    var data = Keccak256.ComputeHash(Encoding.Default.GetString(list.ToArray()));
                    tree[layer + 1][leaf / 2] = data.Take(HashWidth).ToArray();
                    Console.WriteLine();
                }
            }
            return tree;
        }

        public byte[] GetMerkleProof(byte[][] merkleTree, int proofIndex)
        {

            byte[] proof = new byte[merkleTree.Length - 1];

            for(var layerIndex = 0; layerIndex < merkleTree.Length-1; layerIndex++) 
            {
                var position = proofIndex & 0b1; //0 - left, 1 - right
                var layer = merkleTree[layerIndex];

                proof[layerIndex] = position == 0 ? layer[layerIndex + 1] : layer[layerIndex - 1];

                proofIndex = proofIndex >> 1;
            }

            return proof;
        }
    }
}