using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tofft_preprod.Models
{
    public class MissionDTO
    {
        [Required(ErrorMessage = "Название обязательно")]
        [DisplayName("Название задачи")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Срок выполнения обязателен")]
        [DisplayName("Срок выполнения задачи")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }

        [DisplayName("Описание задачи")]
        public string Description { get; set; }

        [DisplayName("Группа ответственности задачи")]
        public string Group { get; set; }
    }
}
