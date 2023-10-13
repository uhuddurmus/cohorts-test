using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using webapi.Applications.BookOperations.Commands.UpdateBook;
using webapi.DBOperations;
using webapi.Entities;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Applications.BookOperations.Commands.UpdateBook;

public class UpdateBookCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext context;  // Veritabaný baðlamýný temsil eden deðiþken.
    private readonly IMapper mapper;  // Veri eþlemesini (mapping) gerçekleþtirmek için kullanýlan deðiþken.

    // Test sýnýfýnýn yapýsýný ayarlamak için CommonTestFixture sýnýfýný kullanarak sýnýfýn kurucusu.
    public UpdateBookCommandTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;  // Veritabaný baðlamýný CommonTestFixture'den alýr.
        this.mapper = testFixture.Mapper;  // Veri eþlemesini CommonTestFixture'den alýr.
    }

    // Varolmayan bir kitap için güncelleme iþlemi yapýldýðýnda InvalidOperationException hatasý üretilmesi gereken senaryoyu test eden bir fakt metodu.
    [Fact]
    public void WhenGivenBookIsNotFound_InvalidOperationException_ShouldBeReturn()
    {
        UpdateBookCommand command = new UpdateBookCommand(context);
        command.BookId = 999;  // Varolmayan bir kitap için güncelleme iþlemi yapýlýr.

        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatasý olmalýdýr.
            .And.Message.Should()
            .Be("Book not found");  // Hatanýn içeriði kontrol edilir ve "Book not found" olmalýdýr.
    }

    // Geçerli giriþlerle kitap güncelleme iþleminin baþarýyla gerçekleþtirildiðini test eden bir fakt metodu.
    [Fact]
    public void WhenValidInputsAreGiven_Book_ShouldBeUpdated()
    {
        UpdateBookCommand command = new UpdateBookCommand(context);
        var book = new Book
        {
            Title = "Test",
            GenreId = 1,
            AuthorId = 1,
            PageCount = 100,
            PublishDate = new DateTime(2022, 1, 1)
        };

        context.Books.Add(book);
        context.SaveChanges();

        command.BookId = book.Id;  // Varolan bir kitap için güncelleme iþlemi yapýlýr.
        UpdateBookModel model = new UpdateBookModel
        {
            Title = "Updated",
            GenreId = 2,
            AuthorId = 2
        };
        command.Model = model;

        FluentActions.Invoking(() => command.Handle()).Invoke();  // Güncelleme iþlemi çaðrýlýr.

        var updatedBook = context.Books.SingleOrDefault(b => b.Id == book.Id);  // Güncellenmiþ kitap veritabanýndan alýnýr.
        updatedBook.Should().NotBeNull();  // Güncellenmiþ kitabýn boþ olmadýðýný doðrular.
        updatedBook.PageCount.Should().Be(book.PageCount);  // Sayfa sayýsýnýn doðru olduðunu doðrular.
        updatedBook.PublishDate.Should().Be(book.PublishDate);  // Yayýn tarihinin doðru olduðunu doðrular.
        updatedBook.Title.Should().Be(model.Title);  // Baþlýðýn güncellendiðini ve doðru olduðunu doðrular.
        updatedBook.GenreId.Should().Be(model.GenreId);  // Tür ID'sinin güncellendiðini ve doðru olduðunu doðrular.
        updatedBook.AuthorId.Should().Be(model.AuthorId);  // Yazar ID'sinin güncellendiðini ve doðru olduðunu doðrular.
    }
}
