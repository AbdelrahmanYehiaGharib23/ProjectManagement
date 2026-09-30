using Application.Common.Interfaces;
using Application.Features.Tasks.DTOs;
using MediatR;

using TaskStatusEnum = Domain.Entities.Enum.TaskStatus;

namespace Application.Features.Tasks.Commands
{
    public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, TaskDto>
    {
        private readonly IGenericRepository<Domain.Entities.Task> _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Domain.Entities.Project> _projectRepository;
        private readonly ICurrentUserService _currentUser;

        public UpdateTaskStatusCommandHandler(
            IGenericRepository<Domain.Entities.Task> taskRepository,
            IUnitOfWork unitOfWork,
            IGenericRepository<Domain.Entities.Project> projectRepository,
            ICurrentUserService currentUser)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _projectRepository = projectRepository;
            _currentUser = currentUser;
        }

        public async Task<TaskDto> Handle(
            UpdateTaskStatusCommand request,
            CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (task is null)
            {
                throw new KeyNotFoundException(
                    $"Task with id {request.Id} was not found.");
            }

            var project = await _projectRepository.GetByIdAsync(task.ProjectId, cancellationToken);
            if (project is null)
                throw new KeyNotFoundException("The task's project was not found.");
            if (!_currentUser.IsAdmin && project?.OwnerId != _currentUser.UserId)
                throw new UnauthorizedAccessException("You cannot modify this task.");

            if (!IsValidTransition(task.Status, request.Status))
            {
                throw new InvalidOperationException(
                    $"Invalid task status transition from {task.Status} to {request.Status}.");
            }

            task.Status = request.Status;

            _taskRepository.Update(task);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                ProjectId = task.ProjectId,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }

        private static bool IsValidTransition(
            TaskStatusEnum currentStatus,
            TaskStatusEnum newStatus)
        {
            return currentStatus switch
            {
                TaskStatusEnum.Todo =>
                    newStatus == TaskStatusEnum.InProgress ||
                    newStatus == TaskStatusEnum.Cancelled,

                TaskStatusEnum.InProgress =>
                    newStatus == TaskStatusEnum.Completed ||
                    newStatus == TaskStatusEnum.Cancelled,

                TaskStatusEnum.Completed => false,

                TaskStatusEnum.Cancelled => false,

                _ => false
            };
        }
    }
}
