using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;
using Application.Features.Projects.DTOs;
using MediatR;

namespace Application.Features.Projects.Queries
{
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IEnumerable<ProjectDto>>
    {
        private readonly IGenericRepository<Domain.Entities.Project> _projectRepository;
        private readonly ICurrentUserService _currentUser;

        public GetProjectsQueryHandler(
            IGenericRepository<Domain.Entities.Project> projectRepository,
            ICurrentUserService currentUser)
        {
            _projectRepository = projectRepository;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<ProjectDto>> Handle(
            GetProjectsQuery request,
            CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetAllAsync(false,cancellationToken);
            if (!_currentUser.IsAdmin)
                projects = projects.Where(p => p.OwnerId == _currentUser.UserId);

            return projects.Select(project => new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt
            });
        }
    }
}
