using System;
using System.Linq;
using FluentAssertions;
using webapi.Applications.BookOperations.Commands.DeleteBook;
using webapi.DBOperations;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Applications.BookOperations.Commands.DeleteBook;

public class DeleteBookCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext context;  // Veritabaný baðlamýný temsil eden deðiþken.

    // Test sýnýfýnýn yapýsýný ayarlamak için CommonTestFixture sýnýfýný kullanarak sýnýfýn kurucusu.
    public DeleteBookCommandTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;  // Veritabaný baðlamýný CommonTestFixture'den alýr.
    }

    // Varolmayan bir kitap için silme iþlemi yapýldýðýnda InvalidOperationException hatasý üretilmesi gereken senaryoyu test eden bir fakt metodu.
    [Fact]
    public void WhenGivenBookIsNotFound_InvalidOperationException_ShouldBeReturn()
    {
        DeleteBookCommand command = new DeleteBookCommand(context);
        command.BookId = 9999999;  // Varolmayan bir kitap için silme iþlemi yapýlýr.

        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatasý olmalýdýr.
            .And.Message.Should()
            .Be("Book not found");  // Hatanýn içeriði kontrol edilir ve "Book not found" olmalýdýr.
    }

    // Geçerli bir BookId ile kitap silme iþleminin baþarýyla gerçekleþtirildiðini test eden bir fakt metodu.
    [Fact]
    public void WhenValidInputsAreGiven_Book_ShouldBeDeleted()
    {
        DeleteBookCommand command = new DeleteBookCommand(context);
        command.BookId = 2;  // Varolan bir kitap için silme iþlemi yapýlýr.

        FluentActions.Invoking(() => command.Handle()).Invoke();  // Silme iþlemi çaðrýlýr.

        var book = context.Books.SingleOrDefault(b => b.Id == command.BookId);  // Silinmiþ kitap veritabanýnda aranýr.
        book.Should().BeNull();  // Kitabýn boþ olmasý gerektiði doðrulanýr.
    }
}

