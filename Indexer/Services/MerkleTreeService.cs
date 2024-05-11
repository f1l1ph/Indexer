using Epoche;
using System.Text;
using System.Text.Json;
using Indexer.Data;



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
                    for (int i = 19; i < 31; i++)
                    {
                        data[i] = 0;
                    }
                    tree[layer + 1][leaf / 2] = data.Take(32).ToArray();
                    
                    Console.WriteLine();
                }
            }
            return tree;
        }

        public GetProofDTO GetMerkleProof(byte[][][] merkleTree, int proofIndex)
        {
            byte[][] proof = new byte[merkleTree.Length - 1][];

            for(var layerIndex = 0; layerIndex < merkleTree.Length-1; layerIndex++) 
            {
                var position = proofIndex & 0b1; //0 - left, 1 - right
                var layer = merkleTree[layerIndex];

                proof[layerIndex] = position == 0 ? layer[proofIndex + 1] : layer[proofIndex - 1];
                
                proofIndex = proofIndex >> 1;
            }

            var dto = new GetProofDTO
            {
                proof = proof,
                owner = merkleTree[0][proofIndex]
            };
            return dto;
        }

        public GetProofIndexDTO? GetProofFromAddress(byte[] address)
        {
            var leaves = LoadLeaves();
            var index = GetIndex(address);
            if (index == -1)
            {
                return null;
            }

            var tree = CreateMerkleTree((int)Math.Ceiling(Math.Sqrt(leaves.Length)), leaves);

            var proof = GetMerkleProof(tree, index);

            var result = new GetProofIndexDTO
            {
                proof = proof.proof,
                owner = proof.owner,
                index = index
            };
            return result;
        }

        public int GetIndex(byte[] address)
        {
            var leaves = LoadLeaves();
            for (var i = 0; i < leaves.Length; i++)
            {
                if (leaves[i].SequenceEqual(address))
                {
                    return i;
                }
            }
            return -1;
        }

        public void SaveLeaves(byte[][] leaves)
        {
            //save leaves to json file
            var json = JsonSerializer.Serialize<byte[][]>(leaves);
            File.WriteAllText("leaves.json", json);

        }

        public byte[][]? LoadLeaves()
        {
            //load leaves from json file
            var json = File.ReadAllText("leaves.json");
            return JsonSerializer.Deserialize<byte[][]>(json);
        }
    }
}