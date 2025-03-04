using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tofft_preprod.Models
{
    public class Report
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [ForeignKey("User")]
        public string AuthorId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
    }
}
