namespace Indexer.Data
{
    public class MerkleNode(MerkleNode parent ,MerkleNode left, MerkleNode right, int key, string value)
    {
        public int Key { get; set; } = key;
        public string Value { get; set; } = value;
        public MerkleNode? Parent { get; set; } = parent;
        public MerkleNode? Left { get; set; } = left;
        public MerkleNode? Right { get; set; } = right;
    }
}
