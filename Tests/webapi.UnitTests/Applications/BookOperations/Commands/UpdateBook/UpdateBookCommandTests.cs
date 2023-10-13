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
    private readonly BookStoreDbContext context;  // Veritaban� ba�lam�n� temsil eden de�i�ken.
    private readonly IMapper mapper;  // Veri e�lemesini (mapping) ger�ekle�tirmek i�in kullan�lan de�i�ken.

    // Test s�n�f�n�n yap�s�n� ayarlamak i�in CommonTestFixture s�n�f�n� kullanarak s�n�f�n kurucusu.
    public UpdateBookCommandTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;  // Veritaban� ba�lam�n� CommonTestFixture'den al�r.
        this.mapper = testFixture.Mapper;  // Veri e�lemesini CommonTestFixture'den al�r.
    }

    // Varolmayan bir kitap i�in g�ncelleme i�lemi yap�ld���nda InvalidOperationException hatas� �retilmesi gereken senaryoyu test eden bir fakt metodu.
    [Fact]
    public void WhenGivenBookIsNotFound_InvalidOperationException_ShouldBeReturn()
    {
        UpdateBookCommand command = new UpdateBookCommand(context);
        command.BookId = 999;  // Varolmayan bir kitap i�in g�ncelleme i�lemi yap�l�r.

        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatas� olmal�d�r.
            .And.Message.Should()
            .Be("Book not found");  // Hatan�n i�eri�i kontrol edilir ve "Book not found" olmal�d�r.
    }

    // Ge�erli giri�lerle kitap g�ncelleme i�leminin ba�ar�yla ger�ekle�tirildi�ini test eden bir fakt metodu.
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

        command.BookId = book.Id;  // Varolan bir kitap i�in g�ncelleme i�lemi yap�l�r.
        UpdateBookModel model = new UpdateBookModel
        {
            Title = "Updated",
            GenreId = 2,
            AuthorId = 2
        };
        command.Model = model;

        FluentActions.Invoking(() => command.Handle()).Invoke();  // G�ncelleme i�lemi �a�r�l�r.

        var updatedBook = context.Books.SingleOrDefault(b => b.Id == book.Id);  // G�ncellenmi� kitap veritaban�ndan al�n�r.
        updatedBook.Should().NotBeNull();  // G�ncellenmi� kitab�n bo� olmad���n� do�rular.
        updatedBook.PageCount.Should().Be(book.PageCount);  // Sayfa say�s�n�n do�ru oldu�unu do�rular.
        updatedBook.PublishDate.Should().Be(book.PublishDate);  // Yay�n tarihinin do�ru oldu�unu do�rular.
        updatedBook.Title.Should().Be(model.Title);  // Ba�l���n g�ncellendi�ini ve do�ru oldu�unu do�rular.
        updatedBook.GenreId.Should().Be(model.GenreId);  // T�r ID'sinin g�ncellendi�ini ve do�ru oldu�unu do�rular.
        updatedBook.AuthorId.Should().Be(model.AuthorId);  // Yazar ID'sinin g�ncellendi�ini ve do�ru oldu�unu do�rular.
    }
}
