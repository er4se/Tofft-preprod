using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Tofft_preprod.Models
{
    public class UserToBoard
    {
        public string UserId { get; set; }
        public string BoardId { get; set; }
        /// <summary>
        /// ������������ ��������� ������ �������<br/>
        /// ��������� �� ������ ����������<br/>
        /// ����� ��� ����������� �������������
        /// </summary>
        public string? UserLocalSpeciality { get; set; }
        /// <summary>
        /// Role - �������� �� ���� � �������<br/>
        /// 0 - ������������� (��� �����������)<br/>
        /// 1 - ��������� (����������� �� �������� �������)<br/>
        /// 3 - ��� (��������� ������ ����������� ��������)<br/>
        /// 4 - ��������� (��������� ���������� ��������� ������)
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
