using Application.Common.Interfaces;
using Application.Features.Comment.DTOs;
using MediatR;

namespace Application.Features.Comment.Queries
{
    public class GetTaskCommentsQueryHandler
         : IRequestHandler<GetTaskCommentsQuery, IEnumerable<CommentDto>>
    {
        private readonly IGenericRepository<Domain.Entities.Comment> _commentRepository;
        private readonly IGenericRepository<Domain.Entities.Task> _taskRepository;
        private readonly IGenericRepository<Domain.Entities.Project> _projectRepository;
        private readonly ICurrentUserService _currentUser;

        public GetTaskCommentsQueryHandler(
            IGenericRepository<Domain.Entities.Comment> commentRepository,
            IGenericRepository<Domain.Entities.Task> taskRepository,
            IGenericRepository<Domain.Entities.Project> projectRepository,
            ICurrentUserService currentUser)
        {
            _commentRepository = commentRepository;
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<CommentDto>> Handle(
            GetTaskCommentsQuery request,
            CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(
                request.TaskId,
                cancellationToken);

            if (task is null)
            {
                throw new KeyNotFoundException(
                    $"Task with id {request.TaskId} was not found.");
            }

            var project = await _projectRepository.GetByIdAsync(task.ProjectId, cancellationToken);
            if (project is null)
                throw new KeyNotFoundException("The task's project was not found.");
            if (!_currentUser.IsAdmin && project?.OwnerId != _currentUser.UserId)
                throw new UnauthorizedAccessException("You cannot access comments on this task.");

            var comments = await _commentRepository.FindAsync(
                comment => comment.TaskId == request.TaskId,
                cancellationToken);

            return comments.Select(comment => new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                TaskId = comment.TaskId,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt
            });
        }
    }
}
