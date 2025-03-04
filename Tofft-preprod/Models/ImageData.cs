using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tofft_preprod.Models
{
    public class ImageData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string SourceId { get; set; }
        public string Reference {  get; set; }
        public int TypeOfImage { get; set; }
    }
}
