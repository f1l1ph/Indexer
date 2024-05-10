using System.Text.Json;
using Indexer.Data;

namespace Indexer.Services
{
    public class MerkleTreeService
    {
        private const string File = "MerkleTree.json";

        public async Task AddMerklerTree(MerkleTree tree)
        {
            var jsonString = JsonSerializer.Serialize(tree);

            await System.IO.File.WriteAllTextAsync(File, jsonString);
        }

        public async Task UpdateMerklerTree(MerkleTree tree)
        {
            var jsonString = JsonSerializer.Serialize(tree);

            await System.IO.File.WriteAllTextAsync(File, jsonString);
        }

        public async Task<String?> GetMerklerTree()
        {
            var jsonString = await System.IO.File.ReadAllTextAsync(File);

            return jsonString;
        }
    }
}



