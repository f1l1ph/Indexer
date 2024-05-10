using System;
using System.Collections.Generic;

// Sample pre-hashed data (replace with your actual data)
byte[] data1 = { /* Your hash for data 1 */ };
byte[] data2 = { /* Your hash for data 2 */ };
byte[] data3 = { /* Your hash for data 3 */ };

// Create the Merkle Tree
var merkleTree = new MerkleTree();

// Add data to the Merkle Tree
merkleTree.Add(data1);
merkleTree.Add(data2);
merkleTree.Add(data3);

Console.WriteLine("Merkle Root Hash:");
Console.WriteLine(Convert.ToHexString(merkleTree._root.Hash)); // Display root hash

// Find data2 (modify as needed)
byte[][] proof;
if (merkleTree.Find(data2, out proof))
{
    Console.WriteLine("\nData2 found with proof:");
    foreach (var hash in proof)
    {
        Console.WriteLine(Convert.ToHexString(hash));
    }
}
else
{
    Console.WriteLine("\nData2 not found.");
}

// Edit data1 (replace with your new pre-hashed value)
byte[] newData1 = { /* Your new hash for data 1 */ };
if (merkleTree.Edit(0, newData1)) // Edit at index 0
{
    Console.WriteLine("\nData1 edited successfully.");
    Console.WriteLine("New Merkle Root Hash:");
    Console.WriteLine(Convert.ToHexString(merkleTree._root.Hash)); // Display updated root hash
}
else
{
    Console.WriteLine("\nData1 edit failed.");
}

public class MerkleTree
{
    private MerkleTreeNode2 _root;

    public void Add(byte[] hashedData)
    {
        if (_root == null)
        {
            _root = new MerkleTreeNode2(hashedData);
            return;
        }

        var newLeaf = new MerkleTreeNode2(hashedData);
        var queue = new Queue<MerkleTreeNode2>();
        queue.Enqueue(_root);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (node.LeftChild == null)
            {
                node.LeftChild = newLeaf;
                RecalculateHash(node);
                break;
            }
            else if (node.RightChild == null)
            {
                node.RightChild = newLeaf;
                RecalculateHash(node);
                break;
            }
            queue.Enqueue(node.LeftChild);
            queue.Enqueue(node.RightChild);
        }
    }

    public bool Find(byte[] hashedData, out byte[][] proof)
    {
        if (_root == null)
        {
            proof = null;
            return false;
        }

        var proofList = new List<byte[]>();
        var current = _root;

        while (current != null)
        {
            if (current.Hash.SequenceEqual(hashedData))
            {
                proof = proofList.ToArray();
                return true;
            }

            if (current.LeftChild != null && current.LeftChild.Hash.SequenceEqual(hashedData))
            {
                proofList.Add(current.RightChild?.Hash);
                current = current.LeftChild;
            }
            else if (current.RightChild != null && current.RightChild.Hash.SequenceEqual(hashedData))
            {
                proofList.Add(current.LeftChild?.Hash);
                current = current.RightChild;
            }
            else
            {
                proof = null;
                return false;
            }
        }

        proof = null;
        return false;
    }

    public bool Edit(int index, byte[] newHashedData)
    {
        if (_root == null || index < 0)
        {
            return false;
        }

        var queue = new Queue<MerkleTreeNode2>();
        queue.Enqueue(_root);
        int currentIndex = 0;

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (currentIndex == index)
            {
                node.Hash = newHashedData;
                RecalculateHashUpwards(node);
                return true;
            }

            currentIndex++;

            if (node.LeftChild != null)
            {
                queue.Enqueue(node.LeftChild);
            }

            if (node.RightChild != null)
            {
                queue.Enqueue(node.RightChild);
            }
        }

        return false;
    }

    private void RecalculateHash(MerkleTreeNode2 node)
    {
        if (node.LeftChild == null || node.RightChild == null)
        {
            return;
        }

        node.Hash = CombineHashes(node.LeftChild.Hash, node.RightChild.Hash);
    }

    private void RecalculateHashUpwards(MerkleTreeNode2 node)
    {
        var parent = node.Parent;
        while (parent != null)
        {
            RecalculateHash(parent);
            parent = parent.Parent;
        }
    }

    private byte[] CombineHashes(byte[] hash1, byte[] hash2)
    {
        var combined = new byte[hash1.Length + hash2.Length];
        Array.Copy(hash1, 0, combined, 0, hash1.Length);
        Array.Copy(hash2, 0, combined, hash1.Length, hash2.Length);
        return combined; // Concatenate the hashes
    }
}

public class MerkleTreeNode2
{
    public byte[] Hash { get; set; }
    public MerkleTreeNode2 LeftChild { get; set; }
    public MerkleTreeNode2 RightChild { get; set; }
    public MerkleTreeNode2 Parent { get; set; }

    public MerkleTreeNode2(byte[] hash)
    {
        Hash = hash;
    }
}