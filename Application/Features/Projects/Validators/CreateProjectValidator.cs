using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Application.Features.Projects.Validators
{
    public class CreateProjectValidator
    : AbstractValidator<Commands.CreateProjectCommand>
    {
        public CreateProjectValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(1000);
        }
    }
}