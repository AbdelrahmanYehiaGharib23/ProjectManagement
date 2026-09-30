using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;
using Application.Features.Tasks.DTOs;
using MediatR;

namespace Application.Features.Tasks.Queries
{
    public class GetTasksQueryHandler
    : IRequestHandler<GetTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly IGenericRepository<Domain.Entities.Task> _taskRepository;
        private readonly IGenericRepository<Domain.Entities.Project> _projectRepository;
        private readonly ICurrentUserService _currentUser;

        public GetTasksQueryHandler(
            IGenericRepository<Domain.Entities.Task> taskRepository,
            IGenericRepository<Domain.Entities.Project> projectRepository,
            ICurrentUserService currentUser)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<TaskDto>> Handle(
            GetTasksQuery request,
            CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetAllAsync(false,cancellationToken);
            if (!_currentUser.IsAdmin)
            {
                var ownedProjectIds = (await _projectRepository.FindAsync(
                    p => p.OwnerId == _currentUser.UserId, cancellationToken))
                    .Select(p => p.Id).ToHashSet();
                tasks = tasks.Where(t => ownedProjectIds.Contains(t.ProjectId));
            }

            return tasks.Select(task => new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                ProjectId = task.ProjectId,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            });
        }
    }
}
