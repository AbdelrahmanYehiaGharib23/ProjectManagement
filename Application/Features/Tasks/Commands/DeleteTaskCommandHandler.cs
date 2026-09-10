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

        public DeleteTaskCommandHandler(
            IGenericRepository<Domain.Entities.Task> taskRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
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

            _taskRepository.Remove(task);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}