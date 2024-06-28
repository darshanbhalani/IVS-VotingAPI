using IVS_VotingAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IVS_VotingAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MyVoteController : ControllerBase
    {
        public VotingModel voting = new VotingModel();


        [HttpGet("GetAllElections")]
        public IActionResult GetAllElections()
        {
            return Ok();
        }


        [HttpGet("ValidateVoter")]
        public IActionResult CheckEligiblity(VotingModel voting) {
            return Ok();
        }


        [HttpGet("VerifyVoter")]
        public IActionResult VerifyVoter(VotingModel voting)
        {
            return Ok();
        }


        [HttpGet("Vote")]
        public IActionResult Vote(VotingModel voting)
        {
            return Ok();
        }
    }
}
