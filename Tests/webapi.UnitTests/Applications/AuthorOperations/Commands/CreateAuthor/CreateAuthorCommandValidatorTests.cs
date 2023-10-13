using System;
using FluentAssertions;
using webapi.Applications.AuthorOperations.Commands.CreateAuthor;
using webapi.UnitTests.TestSetup;

namespace WebApi.UnitTests.Application.AuthorOperations.Commands.CreateAuthor;

public class CreateAuthorCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    [Theory]
    [InlineData("")]
    [InlineData("0")]
    [InlineData("a")]
    public void WhenNameIsInvalid_Validator_ShouldReturnError(string name)
    {
        var command = new CreateAuthorCommand(null, null);
        var Model = new CreateAuthorModel
        {
            FirstName = name,
            LastName = "Test",
            BirthDate = new DateTime(1990, 1, 1)
        };

        command.Model = Model;

        var validator = new CreateAuthorCommandValidator();

        var result = validator.Validate(command);

        result.Errors.Count.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData("")]
    [InlineData("0")]
    [InlineData("a")]
    public void WhenSurnameHasLessThan4Characters_Validator_ShouldReturnError(string surname)
    {
        var command = new CreateAuthorCommand(null, null);
        var Model = new CreateAuthorModel
        {
            FirstName = "Test",
            LastName = surname,
            BirthDate = new DateTime(1990, 1, 1)
        };

        command.Model = Model;

        var validator = new CreateAuthorCommandValidator();

        var result = validator.Validate(command);

        result.Errors.Count should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenBirthdayIsAfterToday_Validator_ShouldReturnError()
    {
        var command = new CreateAuthorCommand(null, null);
        var Model = new CreateAuthorModel
        {
            FirstName = "Test",
            LastName = "Testoglu",
            BirthDate = DateTime.Now.AddDays(1)
        };

        command.Model = Model;

        var validator = new CreateAuthorCommandValidator();

        var result = validator.Validate(command);

        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void WhenModelIsValid_Validator_ShouldNotReturnError()
    {
        var command = new CreateAuthorCommand(null, null);
        var Model = new CreateAuthorModel
        {
            FirstName = "Test",
            LastName = "Testoglu",
            BirthDate = new DateTime(1990, 1, 1)
        };

        command.Model = Model;

        var validator = new CreateAuthorCommandValidator();

        var result = validator.Validate(command);

        result.Errors.Should().BeEmpty();
    }
}
