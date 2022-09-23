using CoreLayer.Dtos;
using FluentValidation;

namespace AuthServer.API.Validation
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(i => i.Email).NotEmpty().WithMessage("Email boş olamaz");
            RuleFor(i => i.Password).NotEmpty().WithMessage("Password boş olamaz");
            RuleFor(i => i.UserName).NotEmpty().WithMessage("Username boş olamaz");

        }
    }
}
