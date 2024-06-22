using FluentValidation;
using TechLanchesLambda.DTOs;

namespace TechLanchesLambda.Validations;

public class UsuarioCadastroValidation : AbstractValidator<UsuarioDto>
{
    public UsuarioCadastroValidation()
    {
        RuleFor(x => x.Cpf)
           .NotEmpty().WithMessage("CPF deve ser informado.")
           .NotNull().WithMessage("CPF deve ser informado.");

        When(x => !string.IsNullOrEmpty(x.Cpf) && !string.IsNullOrWhiteSpace(x.Cpf), () =>
        {
            RuleFor(x => x.Cpf)
            .Must(ValidatorCPF.Validar).WithMessage("O CPF informado está inválido.");
        });

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email deve ser informado.")
            .NotNull().WithMessage("Email deve ser informado.");

        When(x => !string.IsNullOrEmpty(x.Email) && !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
            .Must(ValidatorEmail.Validar).WithMessage("O Email informado está inválido.");
        });

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome deve ser informado.")
            .NotNull().WithMessage("Nome deve ser informado.");

        RuleFor(x => x.Telefone)
          .NotEmpty().WithMessage("Telefone deve ser informado.")
          .NotNull().WithMessage("Telefone deve ser informado.");

        When(x => !string.IsNullOrEmpty(x.Telefone) && !string.IsNullOrWhiteSpace(x.Telefone), () =>
        {
            RuleFor(x => x.Telefone)
            .Must(ValidatorTelefone.Validar).WithMessage("O Telefone informado está inválido.");
        });

        RuleFor(x => x.Endereco)
         .NotEmpty().WithMessage("Endereço deve ser informado.")
         .NotNull().WithMessage("Endereço deve ser informado.");
    }
}