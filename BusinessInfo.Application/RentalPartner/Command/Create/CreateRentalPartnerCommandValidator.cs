using FluentValidation;

namespace BusinessInfo.Application.RentalPartner.Command.Create
{
    public class CreateRentalPartnerCommandValidator : AbstractValidator<CreateRentalPartnerCommandRequest>
    {
        public CreateRentalPartnerCommandValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O campo Nome do Parceiro é obrigatório");

            RuleFor(x => x.Document)
                .NotEmpty()
                .WithMessage("Campo documento é obrigatório");

        }
    }
}
