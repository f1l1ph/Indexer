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
}
