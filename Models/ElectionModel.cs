namespace IVS_VotingAPI.Models
{
    public class ElectionModel
    {
        public long ElectionId { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string EletctionType {  get; set; }
        public int TotalAssemblies { get; set; }
        public DateOnly Electiondate { get; set; }
    }
}
