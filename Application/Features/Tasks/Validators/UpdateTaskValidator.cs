using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Application.Features.Tasks.Validators
{
    public class UpdateTaskValidator : AbstractValidator<Commands.UpdateTaskCommand>
    {
        public UpdateTaskValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(1000);

            RuleFor(x => x.ProjectId)
                .GreaterThan(0);
        }
    }
}