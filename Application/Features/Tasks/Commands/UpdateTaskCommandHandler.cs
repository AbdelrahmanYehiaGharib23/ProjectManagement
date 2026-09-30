using Application.Common.Interfaces;
using Application.Features.Tasks.DTOs;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tasks.Commands
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
    {
        private readonly IGenericRepository<Domain.Entities.Task> _taskRepository;
        private readonly IGenericRepository<Project> _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateTaskCommandHandler(
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
            UpdateTaskCommand request,
            CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (task is null)
                throw new KeyNotFoundException(
                    $"Task with id {request.Id} was not found.");

            var currentProject = await _projectRepository.GetByIdAsync(task.ProjectId, cancellationToken);
            if (currentProject is null)
                throw new KeyNotFoundException("The task's project was not found.");
            if (!_currentUser.IsAdmin && currentProject?.OwnerId != _currentUser.UserId)
                throw new UnauthorizedAccessException("You cannot modify this task.");

            var project = await _projectRepository.GetByIdAsync(
                request.ProjectId,
                cancellationToken);

            if (project is null)
                throw new KeyNotFoundException(
                    $"Project with id {request.ProjectId} was not found.");

            if (!_currentUser.IsAdmin && project.OwnerId != _currentUser.UserId)
                throw new UnauthorizedAccessException("You cannot move tasks into this project.");

            task.Title = request.Title;
            task.Description = request.Description;
            task.ProjectId = request.ProjectId;

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
    }

}
