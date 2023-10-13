using FluentAssertions;
using webapi.Applications.BookOperations.Commands.UpdateBook;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Applications.BookOperations.Commands.UpdateBook;

public class UpdateBookCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    private UpdateBookCommandValidator _validator;  // UpdateBookCommandValidator s�n�f�n�n �rne�ini tutan de�i�ken.

    public UpdateBookCommandValidatorTests()
    {
        _validator = new UpdateBookCommandValidator();  // UpdateBookCommandValidator s�n�f�n�n bir �rne�i olu�turulur.
    }

    // BookId de�eri 0 veya daha k���k oldu�unda do�rulama i�leminin hatalar �retmesi gereken senaryolar� test eden bir fakt metodu.
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WhenBookIdIsInvalid_Validator_ShouldHaveError(int bookId)
    {
        var model = new UpdateBookModel
        {
            Title = "Right Title",
            GenreId = 3,
            AuthorId = 3
        };
        UpdateBookCommand command = new UpdateBookCommand(null);
        command.Model = model;
        command.BookId = bookId;  // Ge�ersiz bir BookId ayarlan�r.

        var result = _validator.Validate(command);  // Do�rulama i�lemi ger�ekle�tirilir.

        result.Errors.Count.Should().BeGreaterThan(0);  // Hatalar�n say�s�n�n 0'dan b�y�k olmas� gerekti�i do�rulan�r.
    }

    // Modelde ge�ersiz de�erler bulundu�unda do�rulama i�leminin hatalar �retmesi gereken senaryolar� test eden bir fakt metodu.
    [Theory]
    [InlineData("", 0, 0)]
    [InlineData(null, 0, 0)]
    [InlineData("x", 1, 1)]
    [InlineData("123", 2, 2)]
    public void WhenModelIsInvalid_Validator_ShouldHaveError(
        string title,
        int genreId,
        int authorId
    )
    {
        var model = new UpdateBookModel
        {
            Title = title,
            GenreId = genreId,
            AuthorId = authorId
        };
        UpdateBookCommand updateCommand = new UpdateBookCommand(null);
        updateCommand.BookId = 1;
        updateCommand.Model = model;

        var result = _validator.Validate(updateCommand);  // Do�rulama i�lemi ger�ekle�tirilir.

        result.Errors.Count.Should().BeGreaterThan(0);  // Hatalar�n say�s�n�n 0'dan b�
