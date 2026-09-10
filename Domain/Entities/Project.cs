using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Project:BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
