using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Comment.Commands
{
    public class DeleteCommentCommandHandler
        : IRequestHandler<DeleteCommentCommand, Unit>
    {
        private readonly IGenericRepository<Domain.Entities.Comment> _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Domain.Entities.Task> _taskRepository;
        private readonly IGenericRepository<Domain.Entities.Project> _projectRepository;
        private readonly ICurrentUserService _currentUser;

        public DeleteCommentCommandHandler(
            IGenericRepository<Domain.Entities.Comment> commentRepository,
            IUnitOfWork unitOfWork,
            IGenericRepository<Domain.Entities.Task> taskRepository,
            IGenericRepository<Domain.Entities.Project> projectRepository,
            ICurrentUserService currentUser)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(
            DeleteCommentCommand request,
            CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (comment is null)
            {
                throw new KeyNotFoundException(
                    $"Comment with id {request.Id} was not found.");
            }

            var task = await _taskRepository.GetByIdAsync(comment.TaskId, cancellationToken);
            var project = task is null ? null : await _projectRepository.GetByIdAsync(task.ProjectId, cancellationToken);
            if (project is null)
                throw new KeyNotFoundException("The comment's task or project was not found.");
            if (!_currentUser.IsAdmin && project?.OwnerId != _currentUser.UserId)
                throw new UnauthorizedAccessException("You cannot delete this comment.");
            comment.IsDeleted = true;

            _commentRepository.Update(comment);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
