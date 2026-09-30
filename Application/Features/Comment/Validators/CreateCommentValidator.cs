using FluentValidation;

namespace Application.Features.Comment.Validators
{
    public class CreateCommentValidator
         : AbstractValidator<Commands.CreateCommentCommand>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(2000);

            RuleFor(x => x.TaskId)
                .GreaterThan(0);
        }
    }
}
