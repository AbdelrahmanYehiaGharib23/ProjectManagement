using Application.Common.Interfaces;
using Application.Features.Projects.DTOs;
using MediatR;

namespace Application.Features.Projects.Queries;

public sealed class GetProjectByIdQueryHandler
    : IRequestHandler<GetProjectByIdQuery, ProjectDto>
{
    private readonly IGenericRepository<Domain.Entities.Project> _projects;
    private readonly ICurrentUserService _currentUser;

    public GetProjectByIdQueryHandler(
        IGenericRepository<Domain.Entities.Project> projects,
        ICurrentUserService currentUser)
    {
        _projects = projects;
        _currentUser = currentUser;
    }

    public async Task<ProjectDto> Handle(
        GetProjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        var project = await _projects.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Project with id {request.Id} was not found.");

        if (!_currentUser.IsAdmin && project.OwnerId != _currentUser.UserId)
            throw new UnauthorizedAccessException("You cannot access this project.");

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
