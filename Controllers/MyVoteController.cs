using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IVS_VotingAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MyVoteController : ControllerBase
    {
        [HttpGet("GetAllElections")]
        public IActionResult GetAllElections()
        {
            return Ok();
        }


        [HttpGet("GetAllElections")]
        public IActionResult Vote(long electionId) {
            return Ok();
        }
    }
}
