using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tofft_preprod.Models
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }
        public string Bio { get; set; }
        public string Speciality { get; set; }
        
        public List<UserToBoard> UserToBoards { get; set; }
    }
}
