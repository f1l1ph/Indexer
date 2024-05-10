using Indexer.Data;
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
    public async Task<OkObjectResult> CreateMerkleTree(MerkleTree tree)
    { 
        await service.AddMerklerTree(tree);

        return Ok(tree);
    }
    [HttpPost("Update")]
    public async Task<OkObjectResult> UpdateMerkleTree(MerkleTree tree)
    {
        await service.UpdateMerklerTree(tree);

        return Ok(tree);
    }

    [HttpGet("GetMerkleTree")]
    public async Task<string?> GetMerkleTree()
    {
        return await service.GetMerklerTree();
    }
}
