using FluentValidation;
using OrcamentoMedico.Domain.Entities;

public class AtendenteValidator : AbstractValidator<Atendente>
{
    public AtendenteValidator()
    {
        RuleFor(a => a.IdUsuario).NotEmpty().WithMessage("IdUsuario é obrigatório.");
        RuleFor(a => a.CriadoEm).LessThanOrEqualTo(DateTime.Now).WithMessage("Data de criação não pode ser no futuro.");
        RuleFor(a => a.Usuario).NotNull().WithMessage("Usuário é obrigatório.");
    }
}