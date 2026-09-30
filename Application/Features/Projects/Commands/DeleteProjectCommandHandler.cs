using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Projects.Commands
{
    public class DeleteProjectCommandHandler
        : IRequestHandler<DeleteProjectCommand, Unit>
    {
        private readonly IGenericRepository<Domain.Entities.Project> _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteProjectCommandHandler(
            IGenericRepository<Domain.Entities.Project> projectRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(
            DeleteProjectCommand request,
            CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (project is null)
            {
                throw new KeyNotFoundException(
                    $"Project with id {request.Id} was not found.");
            }

            if (!_currentUser.IsAdmin && project.OwnerId != _currentUser.UserId)
                throw new UnauthorizedAccessException("You cannot modify this project.");

            project.IsDeleted = true;

            _projectRepository.Update(project);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
