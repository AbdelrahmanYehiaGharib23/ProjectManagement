using Application.Features.Projects.DTOs;
using MediatR;

namespace Application.Features.Projects.Queries;

public record GetProjectByIdQuery(int Id) : IRequest<ProjectDto>;
