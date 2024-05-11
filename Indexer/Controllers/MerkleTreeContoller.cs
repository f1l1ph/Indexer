using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.IO;
using Indexer.Services;

namespace Indexer.Controllers;

[ApiController]
[Route("[controller]")]
public class MerkleTreeContoller(MerkleTreeService service) : ControllerBase
{
    [HttpPost("Add")]
    public OkObjectResult Add(byte[][] adress)
    {
        var adresses = service.LoadLeaves().ToList();
        for (int i = 0; i < adress.Length; i++)
        {
            adresses.Add(adress[i]);
        }
        
        service.SaveLeaves(adresses.ToArray());
        return Ok("ok");
    }

    [HttpGet("GetAllTokens")]
    public byte[][]? GetAll()
    {
        var adresses = service.LoadLeaves();
        return adresses;
    }

    [HttpGet("GetProoFromAdress")]
    public Task<OkObjectResult> GetProofFromAdress(byte[] adress)
    {
        var result = service.GetProofFromAddress(adress);
        return Task.FromResult(Ok(result));
    }

    [HttpGet("GetProof")]
    public Task<OkObjectResult> GetMerkleProof(byte[][] bottomLayer, int proofIndex, int depth)
    {
        var proof = service.GetMerkleProof(service.CreateMerkleTree(depth, bottomLayer), proofIndex);
        return Task.FromResult(Ok(proof));
    }
}
