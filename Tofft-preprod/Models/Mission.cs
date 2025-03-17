using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tofft_preprod.Models;

namespace Tofft_preprod.Models
{
    public class Mission
    {
        public string Id { get; set; }
        public string BoardId { get; set; }
        public string? CreatorId { get; set; }
        public string? ExecutorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public MissionStatus Status { get; set; }
        public string Group {  get; set; }

        public DateTime Created { get; set; }
        public DateTime Deadline { get; set; }

        public Board Board { get; set; }

        public Mission()
        {
            Id = Guid.NewGuid().ToString();
            Created = (DateTime.Now).Date;
            Status = MissionStatus.Unknown;
        }

        public Mission(MissionDTO dto) : this()
        {
            this.Title = dto.Title;
            this.Description = dto.Description;
            this.Group = dto.Group;
            this.Deadline = dto.Deadline;

            this.Status = MissionStatus.Available;
        }
    }

    public enum MissionStatus
    {
        Unknown = 0,
        Available = 1,
        Processing = 2,
        Done = 3
    }
}
