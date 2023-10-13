using AutoMapper;
using FluentAssertions;
using webapi.Applications.BookOperations.Queries.GetBooks;
using webapi.DBOperations;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Application.BookOperations.Queries.GetBooks;

public class GetBooksQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext context;  // Veritabaný baðlamýný temsil eden deðiþken.
    private readonly IMapper mapper;  // Veri eþlemesini (mapping) gerçekleþtirmek için kullanýlan deðiþken.

    // Test sýnýfýnýn yapýsýný ayarlamak için CommonTestFixture sýnýfýný kullanarak sýnýfýn kurucusu.
    public GetBooksQueryTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;  // Veritabaný baðlamýný CommonTestFixture'den alýr.
        this.mapper = testFixture.Mapper;  // Veri eþlemesini CommonTestFixture'den alýr.
    }

    // GetBooksQuery iþlendiðinde kitap listesinin döndüðünü test eden bir fakt metodu.
    [Fact]
    public void WhenGetBooksQueryIsHandled_BookListShouldBeReturned()
    {
        var query = new GetBooksQuery(context, mapper);  // GetBooksQuery sýnýfýnýn bir örneði oluþturulur ve baðlam/enjeksiyonlar enjekte edilir.

        var result = query.Handle();  // Handle() yöntemi, kitap listesini getirmek için çaðrýlýr.

        result.Should().NotBeNull();  // Dönen sonucun boþ olmadýðýný doðrular.
    }
}
