using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tofft_preprod.Models
{
    public class BoardDTO
    {
        [Required(ErrorMessage="Название обязательно")]
        [DisplayName("Название проекта")]
        public string Title { get; set; }
        
        [DisplayName("Наименование ответственной компании")]
        public string Company { get; set; }

        [DisplayName("Описание проекта")]
        public string Description { get; set; }
    }
}
