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

        public GetTaskByIdQueryHandler(
            IGenericRepository<Domain.Entities.Task> taskRepository)
        {
            _taskRepository = taskRepository;
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

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                ProjectId = task.ProjectId,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }
    }
}