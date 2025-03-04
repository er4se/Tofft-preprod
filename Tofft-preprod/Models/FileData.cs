using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tofft_preprod.Models
{
    public class FileData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [ForeignKey("Task")]
        public string SourceId { get; set; }

        public string Reference { get; set; }
    }
}
