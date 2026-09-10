using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Application.Features.Tasks.Validators
{
    public class CreateTaskValidator : AbstractValidator<Commands.CreateTaskCommand>
    {
        public CreateTaskValidator()
        {
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