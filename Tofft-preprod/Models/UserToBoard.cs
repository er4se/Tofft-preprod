using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Tofft_preprod.Models
{
    public class UserToBoard
    {
        public string UserId { get; set; }
        public string BoardId { get; set; }
        public string UserLocalSpeciality { get; set; }
        public User User { get; set; }
        public Board Board { get; set; }
    }
}
