namespace IVS_VotingAPI.Models
{
    public class VotingModel
    {
        public long? ElectionId { get; set; }
        public string VoterId { get; set; }
        public long VoterPhoneNumber { get; set; }
        public int? DistrictId { get; set; }
        public int? AssemblyId { get; set; }
        public long? CandidateId { get; set; }
        public string? Otp { get; set; }
        public string? Token { get; set; }
    }
}
