using System;
using System.Linq;
using FluentAssertions;
using webapi.Applications.BookOperations.Commands.DeleteBook;
using webapi.DBOperations;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Applications.BookOperations.Commands.DeleteBook;

public class DeleteBookCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext context;  // Veritaban� ba�lam�n� temsil eden de�i�ken.

    // Test s�n�f�n�n yap�s�n� ayarlamak i�in CommonTestFixture s�n�f�n� kullanarak s�n�f�n kurucusu.
    public DeleteBookCommandTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;  // Veritaban� ba�lam�n� CommonTestFixture'den al�r.
    }

    // Varolmayan bir kitap i�in silme i�lemi yap�ld���nda InvalidOperationException hatas� �retilmesi gereken senaryoyu test eden bir fakt metodu.
    [Fact]
    public void WhenGivenBookIsNotFound_InvalidOperationException_ShouldBeReturn()
    {
        DeleteBookCommand command = new DeleteBookCommand(context);
        command.BookId = 9999999;  // Varolmayan bir kitap i�in silme i�lemi yap�l�r.

        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatas� olmal�d�r.
            .And.Message.Should()
            .Be("Book not found");  // Hatan�n i�eri�i kontrol edilir ve "Book not found" olmal�d�r.
    }

    // Ge�erli bir BookId ile kitap silme i�leminin ba�ar�yla ger�ekle�tirildi�ini test eden bir fakt metodu.
    [Fact]
    public void WhenValidInputsAreGiven_Book_ShouldBeDeleted()
    {
        DeleteBookCommand command = new DeleteBookCommand(context);
        command.BookId = 2;  // Varolan bir kitap i�in silme i�lemi yap�l�r.

        FluentActions.Invoking(() => command.Handle()).Invoke();  // Silme i�lemi �a�r�l�r.

        var book = context.Books.SingleOrDefault(b => b.Id == command.BookId);  // Silinmi� kitap veritaban�nda aran�r.
        book.Should().BeNull();  // Kitab�n bo� olmas� gerekti�i do�rulan�r.
    }
}

