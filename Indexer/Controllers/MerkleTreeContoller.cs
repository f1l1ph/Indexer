using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.IO;
using Indexer.Services;

namespace Indexer.Controllers;

[ApiController]
[Route("[controller]")]
public class MerkleTreeContoller(MerkleTreeService service) : ControllerBase
{
    [HttpPost("Create")]
    public Task<OkObjectResult> CreateMerkleTree(int depth,byte[][] bottomLayer)
    { 
        return Task.FromResult(Ok(service.CreateMerkleTree(depth, bottomLayer)));
    }

    [HttpGet("Proof")]
    public Task<OkObjectResult> GetMerkleProof(byte[][] bottomLayer, int proofIndex, int depth)
    {
        var proof = service.GetMerkleProof(service.CreateMerkleTree(depth, bottomLayer), proofIndex);
        return Task.FromResult(Ok(proof));
    }
}
