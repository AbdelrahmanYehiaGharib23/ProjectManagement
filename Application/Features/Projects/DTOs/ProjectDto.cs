using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Projects.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
