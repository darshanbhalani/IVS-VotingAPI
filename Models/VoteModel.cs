namespace IVS_VotingAPI.Models
{
    public class VoteModel
    {
        public string VoterId { get; set; }
        public long PhoneNumber { get; set; }
        public int DistrictId { get; set; }
        public string Token { get; set; }
        public long CandidateId { get; set; }
        //public string DB { get; set;}
    }
}
