using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tofft_preprod.Models
{
    public class Board
    {
        public string Id { get; set; }
        public string AdminId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }

        public List<UserToBoard> UserToBoards { get; set; }
    }
}
