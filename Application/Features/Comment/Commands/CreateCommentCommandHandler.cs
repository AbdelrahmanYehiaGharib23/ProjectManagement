using Application.Common.Interfaces;
using Application.Features.Comment.DTOs;
using MediatR;

namespace Application.Features.Comment.Commands
{
    public class CreateCommentCommandHandler
        : IRequestHandler<CreateCommentCommand, CommentDto>
    {
        private readonly IGenericRepository<Domain.Entities.Comment> _commentRepository;
        private readonly IGenericRepository<Domain.Entities.Task> _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Domain.Entities.Project> _projectRepository;
        private readonly ICurrentUserService _currentUser;

        public CreateCommentCommandHandler(
            IGenericRepository<Domain.Entities.Comment> commentRepository,
            IGenericRepository<Domain.Entities.Task> taskRepository,
            IUnitOfWork unitOfWork,
            IGenericRepository<Domain.Entities.Project> projectRepository,
            ICurrentUserService currentUser)
        {
            _commentRepository = commentRepository;
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _projectRepository = projectRepository;
            _currentUser = currentUser;
        }

        public async Task<CommentDto> Handle(
            CreateCommentCommand request,
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
                throw new UnauthorizedAccessException("You cannot comment on this task.");

            var comment = new Domain.Entities.Comment
            {
                Content = request.Content,
                TaskId = request.TaskId
            };

            _commentRepository.Add(comment);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                TaskId = comment.TaskId,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt
            };
        }
    }
}
