using AutoMapper;
using FluentAssertions;
using webapi.Applications.AuthorOperations.Queries.GetAuthor;
using webapi.DBOperations;
using webapi.UnitTests.TestSetup;

namespace WebApi.UnitTests.Application.AuthorOperations.Queries.GetAuthors;

public class GetAuthorsQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext context;  // Veritaban� ba�lam�n� temsil eden de�i�ken.
    private readonly IMapper mapper;  // AutoMapper nesnesini temsil eden de�i�ken.

    // Test s�n�f�n�n yap�s�n� ayarlamak i�in CommonTestFixture s�n�f�n� kullanarak s�n�f�n kurucusu.
    public GetAuthorsQueryTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;  // Veritaban� ba�lam�n� CommonTestFixture'den al�r.
        this.mapper = testFixture.Mapper;  // AutoMapper nesnesini CommonTestFixture'den al�r.
    }

    // GetAuthorsQuery s�n�f�n�n i�lendi�inde bir yazar listesi d�nd���n� do�rulayan fakt metodu.
    [Fact]
    public void WhenGetAuthorsQueryIsHandled_AuthorListShouldBeReturned()
    {
        var query = new GetAuthorsQuery(context, mapper);

        var result = query.Handle();  // GetAuthorsQuery'nin i�lenmesi.

        result.Should().NotBeNull();  // Sonucun bo� olmamas� gerekti�i do�rulan�r.
    }
}