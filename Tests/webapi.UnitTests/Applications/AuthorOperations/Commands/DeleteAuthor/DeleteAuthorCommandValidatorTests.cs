using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using webapi.Applications.AuthorOperations.Commands.DeleteAuthor;
using webapi.UnitTests.TestSetup;

namespace WebApi.UnitTests.Application.AuthorOperations.Commands.DeleteAuthor;

public class DeleteAuthorCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    private DeleteAuthorCommandValidator _validator;

    public DeleteAuthorCommandValidatorTests()
    {
        _validator = new();
    }

    // Ge�ersiz yazar kimlik numaras� (0 veya negatif) ile do�rulaman�n hata d�nd�rmesini sa�lar.
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WhenAuthorIdLessThanOrEqualZero_ValidationShouldReturnError(int authorId)
    {
        // arrange
        DeleteAuthorCommand command = new(null);
        command.AuthorId = authorId;

        // act
        var result = _validator.Validate(command);

        // assert
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // Ge�erli yazar kimlik numaras� (> 0) ile do�rulaman�n hata d�nd�rmemesi gerekti�ini do�rular.
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(100)]
    public void WhenAuthorIdGreaterThanZero_ValidationShouldNotReturnError(int authorId)
    {
        // arrange
        DeleteAuthorCommand command = new(null);
        command.AuthorId = authorId;

        // act
        var result = _validator.Validate(command);

        // assert
        result.Errors.Count.Should().Be(0);
    }
}
