namespace Tofft_preprod.Models
{
    public class InviteLink
    {
        public string Id { get; set; }
        public string BoardId { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }

        public Board Board { get; set; }

        public InviteLink()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
