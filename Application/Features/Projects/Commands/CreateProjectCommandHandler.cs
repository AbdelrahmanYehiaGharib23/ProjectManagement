using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;
using Application.Features.Projects.DTOs;
using MediatR;

namespace Application.Features.Projects.Commands
{
    public class CreateProjectCommandHandler
     : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        private readonly IGenericRepository<Domain.Entities.Project> _projectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProjectCommandHandler(
            IGenericRepository<Domain.Entities.Project> projectRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectDto> Handle(
            CreateProjectCommand request,
            CancellationToken cancellationToken)
        {
            var project = new Domain.Entities.Project
            {
                Name = request.Name,
                Description = request.Description
            };

            _projectRepository.Add(project);

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