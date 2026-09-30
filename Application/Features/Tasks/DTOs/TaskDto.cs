using TaskStatusEnum = Domain.Entities.Enum.TaskStatus;

namespace Application.Features.Tasks.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public TaskStatusEnum Status { get; set; }

        public int ProjectId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}