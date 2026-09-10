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

        public GetTasksQueryHandler(
            IGenericRepository<Domain.Entities.Task> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> Handle(
            GetTasksQuery request,
            CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetAllAsync(false,cancellationToken);

            return tasks.Select(task => new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                ProjectId = task.ProjectId,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            });
        }
    }
}