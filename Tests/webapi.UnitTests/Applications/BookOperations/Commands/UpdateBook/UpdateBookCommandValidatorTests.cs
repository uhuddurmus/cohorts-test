using FluentAssertions;
using webapi.Applications.BookOperations.Commands.UpdateBook;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Applications.BookOperations.Commands.UpdateBook;

public class UpdateBookCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    private UpdateBookCommandValidator _validator;  // UpdateBookCommandValidator sýnýfýnýn örneðini tutan deðiþken.

    public UpdateBookCommandValidatorTests()
    {
        _validator = new UpdateBookCommandValidator();  // UpdateBookCommandValidator sýnýfýnýn bir örneði oluþturulur.
    }

    // BookId deðeri 0 veya daha küçük olduðunda doðrulama iþleminin hatalar üretmesi gereken senaryolarý test eden bir fakt metodu.
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
        command.BookId = bookId;  // Geçersiz bir BookId ayarlanýr.

        var result = _validator.Validate(command);  // Doðrulama iþlemi gerçekleþtirilir.

        result.Errors.Count.Should().BeGreaterThan(0);  // Hatalarýn sayýsýnýn 0'dan büyük olmasý gerektiði doðrulanýr.
    }

    // Modelde geçersiz deðerler bulunduðunda doðrulama iþleminin hatalar üretmesi gereken senaryolarý test eden bir fakt metodu.
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

        var result = _validator.Validate(updateCommand);  // Doðrulama iþlemi gerçekleþtirilir.

        result.Errors.Count.Should().BeGreaterThan(0);  // Hatalarýn sayýsýnýn 0'dan bü
