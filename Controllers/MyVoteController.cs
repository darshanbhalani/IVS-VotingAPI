using IVS_API.Models;
using IVS_API.Repo.Class;
using IVS_VotingAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Npgsql;
using System.Data.Common;
using System.Reflection;

namespace IVS_VotingAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MyVoteController : ControllerBase
    {
        private readonly NpgsqlConnection _connection;
        private readonly IConfiguration _configuration;
        public MyVoteController(NpgsqlConnection connection, IConfiguration configuration)
        {
            _connection = connection;
            _configuration = configuration;
        }
        

        [HttpGet("GetAllElections")]
        public IActionResult GetAllElections()
        {
            List<ElectionModel> elections = new List<ElectionModel>();
            DateTime timeStamp = TimeZoneIST.now();
            try
            {
                _connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM IVS_VOTING_GETALLELECTIONS()", _connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            elections.Add(new ElectionModel
                            {
                                ElectionId = reader.GetInt64(reader.GetOrdinal("stateelectionId")),
                                StateName = reader.GetString(reader.GetOrdinal("statename")),
                                StageName = reader.GetString(reader.GetOrdinal("electionstagesname")),
                                ElectionDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("stateelectiondate"))),
                                DB = reader.GetString(reader.GetOrdinal("db"))
                            });
                        }
                    }
                }
                return Ok(new { success = true, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { data = elections } });
            }
            catch (NpgsqlException pex)
            {
                return Ok(new { success = false, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { error = "Something went wrong." } });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { error = "Something went wrong." } });
            }
        }


        [HttpGet("ValidateVoter")]
        public IActionResult CheckEligiblity(string voterId,long phoneNumber,string db) {
            DateTime timeStamp = TimeZoneIST.now();
            try
            {
                var connectionString = $"Host={_configuration["DBConfiguration:Host"]};Port={_configuration["DBConfiguration:Port"]};Username={_configuration["DBConfiguration:Username"]};Password={_configuration["DBConfiguration:Password"]};Database={db}";
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM IVS_DB_CHECKVOTER(@in_voterId,@in_phoneNumber)", conn))
                    {
                        cmd.Parameters.AddWithValue("in_voterId", voterId);
                        cmd.Parameters.AddWithValue("in_phoneNumber", phoneNumber);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.GetBoolean(reader.GetOrdinal("success")))
                                {
                                    return Ok(new { success = true, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { message = "OTP successfully sended to your phone number." } });
                                }
                            }
                            
                        }
                    }
                }
                return Ok(new { success = false, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { error = "Invalid Voter-Id or Phone-Number." } });
            }
            catch (NpgsqlException pex)
            {
                return Ok(new { success = false, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { error = "Something went wrong." } });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { error = "Something went wrong." } });
            }
        }


        [HttpGet("VerifyVoter")]
        public IActionResult VerifyVoter(string voterId, long phoneNumber, string db,string otp)
        {
            DateTime timeStamp = TimeZoneIST.now();
            try
            {
                var connectionString = $"Host={_configuration["DBConfiguration:Host"]};Port={_configuration["DBConfiguration:Port"]};Username={_configuration["DBConfiguration:Username"]};Password={_configuration["DBConfiguration:Password"]};Database={db}";
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM IVS_DB_VERIFYVOTER(@in_voterId,@in_phoneNumber,@in_otp)", conn))
                    {
                        cmd.Parameters.AddWithValue("in_voterId", voterId);
                        cmd.Parameters.AddWithValue("in_phoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("in_otp", otp);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.GetBoolean(reader.GetOrdinal("success")))
                                {
                                    return Ok(new { success = true, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { message = reader.GetString(reader.GetOrdinal("message")), districtId = reader.GetInt32(reader.GetOrdinal("districtId")), assemblyId = reader.GetInt32(reader.GetOrdinal("assemblyId")), token = reader.GetString(reader.GetOrdinal("token")) } });
                                }
                            }
                        }
                    }
                }
                return Ok(new { success = false, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { error = "Invalid Voter-Id or Phone-Number." } });
            }
            catch (NpgsqlException pex)
            {
                return Ok(new { success = false, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { error = "Something went wrong." } });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { error = "Something went wrong." } });
            }
        }

        [HttpGet("GetCandidates")]
        public IActionResult GetCandidates(int assemblyId, string db)
        {
            DateTime timeStamp = TimeZoneIST.now();
            try
            {
                var connectionString = $"Host={_configuration["DBConfiguration:Host"]};Port={_configuration["DBConfiguration:Port"]};Username={_configuration["DBConfiguration:Username"]};Password={_configuration["DBConfiguration:Password"]};Database={db}";
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM IVS_DB_GETALLCANDIDATESBYASSEMBLY(@in_assemblyId)", conn))
                    {
                        cmd.Parameters.AddWithValue("in_assemblyId", assemblyId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return Ok(new { success = true, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { message = reader.GetString(reader.GetOrdinal("message")), districtId = reader.GetInt32(reader.GetOrdinal("districtId")), assemblyId = reader.GetInt32(reader.GetOrdinal("assemblyId")), token = reader.GetString(reader.GetOrdinal("token")) } });
                            }
                        }
                    }
                }
                return Ok(new { success = false, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { error = "Something went wrong." } });
            }
            catch (NpgsqlException pex)
            {
                return Ok(new { success = false, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { error = "Something went wrong." } });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { error = "Something went wrong." } });
            }
        }

        [HttpGet("Vote")]
        public IActionResult Vote(VoteModel data)
        {
            DateTime timeStamp = TimeZoneIST.now();
            try
            {
                var connectionString = $"Host={_configuration["DBConfiguration:Host"]};Port={_configuration["DBConfiguration:Port"]};Username={_configuration["DBConfiguration:Username"]};Password={_configuration["DBConfiguration:Password"]};Database={data.DB}";
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM IVS_DB_RECORDVOTE(@in_voterId,@in_phoneNumber,@in_districtId,@in_token,@in_candidateId)", conn))
                    {
                        cmd.Parameters.AddWithValue("in_voterId", data.VoterId);
                        cmd.Parameters.AddWithValue("in_phoneNumber", data.PhoneNumber);
                        cmd.Parameters.AddWithValue("in_districtId", data.DistrictId);
                        cmd.Parameters.AddWithValue("in_token", data.Token);
                        cmd.Parameters.AddWithValue("in_candidateId", data.CandidateId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.GetBoolean(reader.GetOrdinal("success")))
                            {
                                return Ok(new { success = true, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { message = reader.GetString(reader.GetOrdinal("message"))} });
                            }
                        }
                    }
                }
                return Ok(new { success = false, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { error = "Something went wrong." } });
            }
            catch (NpgsqlException pex)
            {
                return Ok(new { success = false, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { error = "Something went wrong." } });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, header = new { requestTime = timeStamp, responsTime = TimeZoneIST.now() }, body = new { error = "Something went wrong." } });
            }
        }
    }
}
