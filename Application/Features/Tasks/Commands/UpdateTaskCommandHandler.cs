using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;
using Application.Features.Tasks.DTOs;
using MediatR;

namespace Application.Features.Tasks.Commands
{
    public class UpdateTaskCommandHandler
    : IRequestHandler<UpdateTaskCommand, TaskDto>
    {
        private readonly IGenericRepository<Domain.Entities.Task> _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTaskCommandHandler(
            IGenericRepository<Domain.Entities.Task> taskRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
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

            task.Title = request.Title;
            task.Description = request.Description;
            task.ProjectId = request.ProjectId;
            task.IsCompleted = request.IsCompleted;

            _taskRepository.Update(task);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

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