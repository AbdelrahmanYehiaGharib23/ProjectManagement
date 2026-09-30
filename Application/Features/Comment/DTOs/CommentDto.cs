using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Comment.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public int TaskId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
