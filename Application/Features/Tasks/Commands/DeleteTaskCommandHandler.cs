using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Tasks.Commands
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly IGenericRepository<Domain.Entities.Task> _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Domain.Entities.Project> _projectRepository;
        private readonly ICurrentUserService _currentUser;

        public DeleteTaskCommandHandler(
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

        public async Task<Unit> Handle(
            DeleteTaskCommand request,
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
                throw new UnauthorizedAccessException("You cannot modify this task.");

            task.IsDeleted = true;

            _taskRepository.Update(task);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
