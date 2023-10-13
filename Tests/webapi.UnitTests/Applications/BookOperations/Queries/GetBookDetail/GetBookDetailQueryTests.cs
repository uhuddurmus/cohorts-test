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
    private readonly BookStoreDbContext context;  // Veritabaný baðlamýný temsil eden deðiþken.
    private readonly IMapper mapper;  // Veri eþlemesini (mapping) gerçekleþtirmek için kullanýlan deðiþken.

    // Test sýnýfýnýn yapýsýný ayarlamak için CommonTestFixture sýnýfýný kullanarak sýnýfýn kurucusu.
    public GetBookDetailQueryTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;  // Veritabaný baðlamýný CommonTestFixture'den alýr.
        this.mapper = testFixture.Mapper;  // Veri eþlemesini CommonTestFixture'den alýr.
    }

    // Geçerli giriþlerle bir kitabýn detaylarýnýn baþarýyla alýndýðýný test eden bir fakt metodu.
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

        BookDetailViewModel vm = query.Handle();  // Handle() yöntemi, kitap detaylarýný getirmek için çaðrýlýr.

        // assert (doðrulama)
        vm.Should().NotBeNull();  // Dönen sonucun boþ olmadýðýný doðrular.
        vm.Title.Should().Be(book.Title);  // Kitap baþlýðýnýn doðru olduðunu doðrular.
        vm.PageCount.Should().Be(book.PageCount);  // Sayfa sayýsýnýn doðru olduðunu doðrular.
        vm.Genre.Should().Be(book.Genre.Name);  // Türün doðru olduðunu doðrular.
        vm.Author.Should().Be(book.Author.FirstName + " " + book.Author.LastName);  // Yazarýn doðru olduðunu doðrular.
        vm.PublishDate.Should().Be(book.PublishDate.ToString("dd/MM/yyyy"));  // Yayýn tarihinin doðru olduðunu doðrular.
    }

    // Varolmayan bir BookId verildiðinde InvalidOperationException hatasý üretilmesi gereken senaryoyu test eden bir fakt metodu.
    [Fact]
    public void WhenNonExistingBookIdIsGiven_InvalidOperationException_ShouldBeReturn()
    {
        int bookId = 99999;

        GetBookDetailQuery query = new GetBookDetailQuery(context, mapper);
        query.BookId = bookId;

        // FluentActions kullanýlarak, bir iþlemi tetiklemek ve ardýndan hata durumunu test etmek için Invoking() yöntemi kullanýlýr.
        query
            .Invoking(x => x.Handle())
            .Should()
            .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatasý olmalýdýr.
            .And.Message.Should()
            .Be("Book not found");  // Hatanýn içeriði kontrol edilir ve "Book not found" olmalýdýr.
    }
}
