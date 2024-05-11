namespace Indexer.Data
{
    public class GetProofIndexDTO
    {
        public byte[][] proof { get; set; }
        public byte[] owner { get; set; }
        public int index { get; set; }
    }
}
