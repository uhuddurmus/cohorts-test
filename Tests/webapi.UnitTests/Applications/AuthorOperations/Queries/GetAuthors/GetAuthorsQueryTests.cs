using AutoMapper;
using FluentAssertions;
using webapi.Applications.AuthorOperations.Queries.GetAuthor;
using webapi.DBOperations;
using webapi.UnitTests.TestSetup;

namespace WebApi.UnitTests.Application.AuthorOperations.Queries.GetAuthors;

public class GetAuthorsQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext context;  // Veritabaný baðlamýný temsil eden deðiþken.
    private readonly IMapper mapper;  // AutoMapper nesnesini temsil eden deðiþken.

    // Test sýnýfýnýn yapýsýný ayarlamak için CommonTestFixture sýnýfýný kullanarak sýnýfýn kurucusu.
    public GetAuthorsQueryTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;  // Veritabaný baðlamýný CommonTestFixture'den alýr.
        this.mapper = testFixture.Mapper;  // AutoMapper nesnesini CommonTestFixture'den alýr.
    }

    // GetAuthorsQuery sýnýfýnýn iþlendiðinde bir yazar listesi döndüðünü doðrulayan fakt metodu.
    [Fact]
    public void WhenGetAuthorsQueryIsHandled_AuthorListShouldBeReturned()
    {
        var query = new GetAuthorsQuery(context, mapper);

        var result = query.Handle();  // GetAuthorsQuery'nin iþlenmesi.

        result.Should().NotBeNull();  // Sonucun boþ olmamasý gerektiði doðrulanýr.
    }
}