using Application.Common.Interfaces;
using Application.Features.Tasks.DTOs;
using Domain.Entities;
using MediatR;
using TaskStatusEnum = Domain.Entities.Enum.TaskStatus;


namespace Application.Features.Tasks.Commands
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        private readonly IGenericRepository<Domain.Entities.Task> _taskRepository;
        private readonly IGenericRepository<Project> _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public CreateTaskCommandHandler(
            IGenericRepository<Domain.Entities.Task> taskRepository,
            IGenericRepository<Project> projectRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<TaskDto> Handle(
            CreateTaskCommand request,
            CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(
                request.ProjectId,
                cancellationToken);

            if (project is null)
                throw new KeyNotFoundException(
                    $"Project with id {request.ProjectId} was not found.");

            if (!_currentUser.IsAdmin && project.OwnerId != _currentUser.UserId)
                throw new UnauthorizedAccessException("You cannot add tasks to this project.");

            var task = new Domain.Entities.Task
            {
                Title = request.Title,
                Description = request.Description,
                ProjectId = request.ProjectId,
                Status = TaskStatusEnum.Todo
            };

            _taskRepository.Add(task);

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
    }
}
