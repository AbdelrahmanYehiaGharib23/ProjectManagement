using TaskStatusEnum = Domain.Entities.Enum.TaskStatus;

namespace Domain.Entities
{
    public class Task : BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public TaskStatusEnum Status { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}