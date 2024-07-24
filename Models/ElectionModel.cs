namespace IVS_VotingAPI.Models
{
    public class ElectionModel
    {
        public long ElectionId { get; set; }
        public string StateName { get; set; }
        public string StageName { get; set; }
        public DateOnly ElectionDate { get; set; }
        public string DB {  get; set; }
    }
}
