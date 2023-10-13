using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using webapi.Applications.BookOperations.Queries.GetBookDetail;
using webapi.DBOperations;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Application.BookOperations.Queries.GetBookDetail;

public class GetBookDetailQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext context;  // Veritaban� ba�lam�n� temsil eden de�i�ken.
    private readonly IMapper mapper;  // Veri e�lemesini (mapping) ger�ekle�tirmek i�in kullan�lan de�i�ken.

    // Test s�n�f�n�n yap�s�n� ayarlamak i�in CommonTestFixture s�n�f�n� kullanarak s�n�f�n kurucusu.
    public GetBookDetailQueryTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;  // Veritaban� ba�lam�n� CommonTestFixture'den al�r.
        this.mapper = testFixture.Mapper;  // Veri e�lemesini CommonTestFixture'den al�r.
    }

    // Ge�erli giri�lerle bir kitab�n detaylar�n�n ba�ar�yla al�nd���n� test eden bir fakt metodu.
    [Fact]
    public void WhenValidInputsAreGiven_Book_ShouldBeReturned()
    {
        GetBookDetailQuery query = new GetBookDetailQuery(context, mapper);
        var BookId = query.BookId = 1;

        var book = context.Books
            .Include(x => x.Genre)
            .Include(x => x.Author)
            .Where(b => b.Id == BookId)
            .SingleOrDefault();

        BookDetailViewModel vm = query.Handle();  // Handle() y�ntemi, kitap detaylar�n� getirmek i�in �a�r�l�r.

        // assert (do�rulama)
        vm.Should().NotBeNull();  // D�nen sonucun bo� olmad���n� do�rular.
        vm.Title.Should().Be(book.Title);  // Kitap ba�l���n�n do�ru oldu�unu do�rular.
        vm.PageCount.Should().Be(book.PageCount);  // Sayfa say�s�n�n do�ru oldu�unu do�rular.
        vm.Genre.Should().Be(book.Genre.Name);  // T�r�n do�ru oldu�unu do�rular.
        vm.Author.Should().Be(book.Author.FirstName + " " + book.Author.LastName);  // Yazar�n do�ru oldu�unu do�rular.
        vm.PublishDate.Should().Be(book.PublishDate.ToString("dd/MM/yyyy"));  // Yay�n tarihinin do�ru oldu�unu do�rular.
    }

    // Varolmayan bir BookId verildi�inde InvalidOperationException hatas� �retilmesi gereken senaryoyu test eden bir fakt metodu.
    [Fact]
    public void WhenNonExistingBookIdIsGiven_InvalidOperationException_ShouldBeReturn()
    {
        int bookId = 99999;

        GetBookDetailQuery query = new GetBookDetailQuery(context, mapper);
        query.BookId = bookId;

        // FluentActions kullan�larak, bir i�lemi tetiklemek ve ard�ndan hata durumunu test etmek i�in Invoking() y�ntemi kullan�l�r.
        query
            .Invoking(x => x.Handle())
            .Should()
            .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatas� olmal�d�r.
            .And.Message.Should()
            .Be("Book not found");  // Hatan�n i�eri�i kontrol edilir ve "Book not found" olmal�d�r.
    }
}
