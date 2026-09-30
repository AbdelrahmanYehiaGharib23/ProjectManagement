using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;
using Application.Features.Tasks.DTOs;
using MediatR;

namespace Application.Features.Tasks.Queries
{
    public class GetTaskByIdQueryHandler
    : IRequestHandler<GetTaskByIdQuery, TaskDto>
    {
        private readonly IGenericRepository<Domain.Entities.Task> _taskRepository;
        private readonly IGenericRepository<Domain.Entities.Project> _projectRepository;
        private readonly ICurrentUserService _currentUser;

        public GetTaskByIdQueryHandler(
            IGenericRepository<Domain.Entities.Task> taskRepository,
            IGenericRepository<Domain.Entities.Project> projectRepository,
            ICurrentUserService currentUser)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _currentUser = currentUser;
        }

        public async Task<TaskDto> Handle(
            GetTaskByIdQuery request,
            CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (task is null)
                throw new KeyNotFoundException(
                    $"Task with id {request.Id} was not found.");
            var project = await _projectRepository.GetByIdAsync(task.ProjectId, cancellationToken);
            if (project is null)
                throw new KeyNotFoundException("The task's project was not found.");
            if (!_currentUser.IsAdmin && project?.OwnerId != _currentUser.UserId)
                throw new UnauthorizedAccessException("You cannot access this task.");
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
