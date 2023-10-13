using FluentAssertions;
using webapi.Applications.BookOperations.Commands.DeleteBook;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Applications.BookOperations.Commands.DeleteBook;

public class DeleteBookCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    private DeleteBookCommandValidator _validator;  // DeleteBookCommandValidator sýnýfýnýn örneðini tutan deðiþken.

    public DeleteBookCommandValidatorTests()
    {
        _validator = new DeleteBookCommandValidator();  // DeleteBookCommandValidator sýnýfýnýn bir örneði oluþturulur.
    }

    // BookId deðeri 0 veya daha küçük olduðunda doðrulama iþleminin hatalar üretmesi gereken senaryoyu test eden bir fakt metodu.
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WhenBookIdLessThanOrEqualZero_ValidationShouldReturnError(int bookId)
    {
        DeleteBookCommand command = new DeleteBookCommand(null);
        command.BookId = bookId;  // Geçersiz bir BookId ayarlanýr.

        var result = _validator.Validate(command);  // Doðrulama iþlemi gerçekleþtirilir.

        result.Errors.Count.Should().BeGreaterThan(0);  // Hatalarýn sayýsýnýn 0'dan büyük olmasý gerektiði doðrulanýr.
    }

    // Geçerli bir BookId ile doðrulama iþleminin hata üretmemesi gereken senaryoyu test eden bir fakt metodu.
    [Fact]
    public void WhenBookIdGreaterThanZero_ValidationShouldNotReturnError()
    {
        DeleteBookCommand command = new DeleteBookCommand(null);
        command.BookId = 12;  // Geçerli bir BookId ayarlanýr.

        var result = _validator.Validate(command);  // Doðrulama iþlemi gerçekleþtirilir.

        result.Errors.Count.Should().Be(0);  // Hatalarýn sayýsýnýn 0 olmasý gerektiði doðrulanýr.
    }
}