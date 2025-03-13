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
        public DateTime Deadline { get; set; }

        [DisplayName("Описание задачи")]
        public string Description { get; set; }

        [DisplayName("Группа ответственности задачи")]
        public string Group { get; set; }
    }
}
