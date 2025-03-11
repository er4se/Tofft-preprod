using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tofft_preprod.Models
{
    public class Mission
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [ForeignKey("Board")]
        public string BoardId { get; set; }

        [ForeignKey("User")]
        public string AdminId { get; set; }

        [ForeignKey("User")]
        public string ExecutorId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Group {  get; set; }

        public DateTime Created { get; set; }
        public DateTime Deadline { get; set; }
    }
}
