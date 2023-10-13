using System;
using FluentAssertions;
using webapi.Applications.AuthorOperations.Commands.UpdateAuthor;
using webapi.UnitTests.TestSetup;

namespace WebApi.UnitTests.Application.AuthorOperations.Commands.UpdateAuthor;

public class UpdateAuthorCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    private UpdateAuthorCommandValidator _validator;  // UpdateAuthorCommand doğrulayıcı nesnesini temsil eden değişken.

    public UpdateAuthorCommandValidatorTests()
    {
        _validator = new();
    }

    // Geçersiz bir yazar kimliği verildiğinde doğrulama hatası olup olmadığını doğrulayan fakt metodu.
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WhenAuthorIdIsInvalid_Validator_ShouldHaveError(int authorId)
    {
        var model = new UpdateAuthorModel
        {
            FirstName = "Uhud",
            LastName = "Durmus",
            BirthDate = new DateTime(1998, 09, 01)
        };
        UpdateAuthorCommand command = new(null);
        command.Model = model;
        command.AuthorId = authorId;

        var result = _validator.Validate(command);

        result.Errors.Should().ContainSingle();
    }

    // Geçersiz bir yazar modeli verildiğinde doğrulama hatası olup olmadığını doğrulayan fakt metodu.
    [Theory]
    [InlineData("", "")]
    [InlineData("123", "")]
    public void WhenModelIsInvalid_Validator_ShouldHaveError(string name, string surname)
    {
        var model = new UpdateAuthorModel
        {
            FirstName = name,
            LastName = surname,
            BirthDate = new DateTime(2000, 11, 22)
        };
        UpdateAuthorCommand updateCommand = new(null);
        updateCommand.AuthorId = 3;
        updateCommand.Model = model;

        var result = _validator.Validate(updateCommand);

        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // Doğum tarihi olarak şu anın verildiğinde doğrulama hatası olup olmadığını doğrulayan fakt metodu.
    [Fact]
    public void WhenVirthDayEqualNowGiven_Validator_ShouldBeReturnError()
    {
        UpdateAuthorCommand command = new(null);
        command.Model = new UpdateAuthorModel
        {
            FirstName = "Ufuk",
            LastName = "Durmus",
            BirthDate = DateTime.Now.Date
        };

        UpdateAuthorCommandValidator validator = new();
        var result = validator.Validate(command);

        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // Geçerli girişler verildiğinde doğrulama hatası olmadığını doğrulayan fakt metodu.
    [Fact]
    public void WhenInputsAreValid_Validator_ShouldNotHaveError()
    {
        var model = new UpdateAuthorModel
        {
            FirstName = "Uhud",
            LastName = "Durmus",
            BirthDate = new DateTime(1998, 09, 01)
        };
        UpdateAuthorCommand updateCommand = new(null);
        updateCommand.AuthorId = 2;
        updateCommand.Model = model;

        var result = _validator.Validate(updateCommand);

        result.Errors.Count.Should().Be(0);
    }
}
