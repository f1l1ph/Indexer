namespace Indexer.Data
{
    public class MerkleNode(MerkleNode left, MerkleNode right, int key, int value)
    {
        public int Key { get; set; } = key;
        public int Value { get; set; } = value;
        public MerkleNode Left { get; set; } = left;
        public MerkleNode Right { get; set; } = right;
    }
}
