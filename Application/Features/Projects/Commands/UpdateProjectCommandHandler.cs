using Application.Common.Interfaces;
using Application.Features.Projects.DTOs;
using MediatR;

namespace Application.Features.Projects.Commands
{
    public class UpdateProjectCommandHandler
         : IRequestHandler<UpdateProjectCommand, ProjectDto>
    {
        private readonly IGenericRepository<Domain.Entities.Project> _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateProjectCommandHandler(
            IGenericRepository<Domain.Entities.Project> projectRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<ProjectDto> Handle(
            UpdateProjectCommand request,
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

            project.Name = request.Name;
            project.Description = request.Description;

            _projectRepository.Update(project);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt
            };
        }
    }
}
