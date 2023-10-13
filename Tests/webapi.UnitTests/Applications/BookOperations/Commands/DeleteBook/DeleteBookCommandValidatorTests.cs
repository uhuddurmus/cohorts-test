using FluentAssertions;
using webapi.Applications.BookOperations.Commands.DeleteBook;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Applications.BookOperations.Commands.DeleteBook;

public class DeleteBookCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    private DeleteBookCommandValidator _validator;  // DeleteBookCommandValidator s�n�f�n�n �rne�ini tutan de�i�ken.

    public DeleteBookCommandValidatorTests()
    {
        _validator = new DeleteBookCommandValidator();  // DeleteBookCommandValidator s�n�f�n�n bir �rne�i olu�turulur.
    }

    // BookId de�eri 0 veya daha k���k oldu�unda do�rulama i�leminin hatalar �retmesi gereken senaryoyu test eden bir fakt metodu.
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WhenBookIdLessThanOrEqualZero_ValidationShouldReturnError(int bookId)
    {
        DeleteBookCommand command = new DeleteBookCommand(null);
        command.BookId = bookId;  // Ge�ersiz bir BookId ayarlan�r.

        var result = _validator.Validate(command);  // Do�rulama i�lemi ger�ekle�tirilir.

        result.Errors.Count.Should().BeGreaterThan(0);  // Hatalar�n say�s�n�n 0'dan b�y�k olmas� gerekti�i do�rulan�r.
    }

    // Ge�erli bir BookId ile do�rulama i�leminin hata �retmemesi gereken senaryoyu test eden bir fakt metodu.
    [Fact]
    public void WhenBookIdGreaterThanZero_ValidationShouldNotReturnError()
    {
        DeleteBookCommand command = new DeleteBookCommand(null);
        command.BookId = 12;  // Ge�erli bir BookId ayarlan�r.

        var result = _validator.Validate(command);  // Do�rulama i�lemi ger�ekle�tirilir.

        result.Errors.Count.Should().Be(0);  // Hatalar�n say�s�n�n 0 olmas� gerekti�i do�rulan�r.
    }
}