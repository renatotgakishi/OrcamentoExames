using Xunit;
using FluentAssertions;
using FluentValidation.TestHelper;
using OrcamentoMedico.Domain.Entities;

public class AtendenteValidatorTests
{
    private readonly AtendenteValidator _validator = new();

    [Fact]
    public void Deve_Falhar_Quando_IdUsuario_Estiver_Vazio()
    {
        var atendente = new Atendente
        {
            IdUsuario = Guid.Empty,
            CriadoEm = DateTime.Now,
            Usuario = new Usuario()
        };

        var result = _validator.TestValidate(atendente);
        result.ShouldHaveValidationErrorFor(a => a.IdUsuario);
    }

    [Fact]
    public void Deve_Passar_Quando_Todos_Os_Campos_Estao_Certos()
    {
        var atendente = new Atendente
        {
            IdUsuario = Guid.NewGuid(),
            CriadoEm = DateTime.Now.AddSeconds(-1),
            Usuario = new Usuario()
        };

        var result = _validator.TestValidate(atendente);
        result.ShouldNotHaveAnyValidationErrors();
    }
}