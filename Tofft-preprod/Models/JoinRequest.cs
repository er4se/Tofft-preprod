namespace Tofft_preprod.Models
{
    public class JoinRequest
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string BoardId { get; set; }
        public JoinStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public Board Board { get; set; }

        public JoinRequest()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public enum JoinStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}
