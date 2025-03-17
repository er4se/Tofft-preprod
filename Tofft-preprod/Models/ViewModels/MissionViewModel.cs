namespace Tofft_preprod.Models
{
    public class MissionViewModel
    {
        //public Mission Mission { get; set; }
        public string BoardId { get; set; }
        public MissionStatus Status { get; set; }
        public List<Mission> Missions { get; set; }
    }
}
