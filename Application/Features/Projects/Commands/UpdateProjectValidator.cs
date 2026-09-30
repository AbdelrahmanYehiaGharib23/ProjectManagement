using FluentValidation;

namespace Application.Features.Projects.Commands
{
    public class UpdateProjectValidator
        : AbstractValidator<Commands.UpdateProjectCommand>
    {
        public UpdateProjectValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(1000);
        }
    }
}
