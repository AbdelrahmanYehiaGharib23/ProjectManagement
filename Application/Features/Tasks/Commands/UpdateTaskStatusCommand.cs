using Application.Features.Tasks.DTOs;
using MediatR;
using FluentValidation;

using TaskStatusEnum = Domain.Entities.Enum.TaskStatus;

namespace Application.Features.Tasks.Commands
{
    public record UpdateTaskStatusCommand(
        int Id,
        TaskStatusEnum Status
    ) : IRequest<TaskDto>;

    public sealed class UpdateTaskStatusValidator : AbstractValidator<UpdateTaskStatusCommand>
    {
        public UpdateTaskStatusValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0);
            RuleFor(command => command.Status).IsInEnum();
        }
    }
}
