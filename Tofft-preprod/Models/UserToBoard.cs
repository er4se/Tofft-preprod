using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Tofft_preprod.Models
{
    public class UserToBoard
    {
        public string UserId { get; set; }
        public string BoardId { get; set; }
        /// <summary>
        /// Определенная должность внутри проекта<br/>
        /// Назависит от других параметров<br/>
        /// Нужна для визуального представления
        /// </summary>
        public string? UserLocalSpeciality { get; set; }
        /// <summary>
        /// Role - отвечает за роли в проекте<br/>
        /// 0 - Администратор (нет ограничений)<br/>
        /// 1 - Модератор (ограничение на удаление проекта)<br/>
        /// 3 - Лид (ограничен полным управлением задачами)<br/>
        /// 4 - Сотрудник (ограничен изменением состояний задачи)
        /// </summary>
        public BoardRole Role { get; set; }
        public User User { get; set; }
        public Board Board { get; set; }
    }

    public enum BoardRole
    {
        Admin = 0,
        Moderator = 1,
        Lead = 2,
        Memeber = 3
    }
}
