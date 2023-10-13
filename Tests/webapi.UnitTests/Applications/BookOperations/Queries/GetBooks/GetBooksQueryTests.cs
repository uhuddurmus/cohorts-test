using AutoMapper;
using FluentAssertions;
using webapi.Applications.BookOperations.Queries.GetBooks;
using webapi.DBOperations;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Application.BookOperations.Queries.GetBooks;

public class GetBooksQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext context;  // Veritaban� ba�lam�n� temsil eden de�i�ken.
    private readonly IMapper mapper;  // Veri e�lemesini (mapping) ger�ekle�tirmek i�in kullan�lan de�i�ken.

    // Test s�n�f�n�n yap�s�n� ayarlamak i�in CommonTestFixture s�n�f�n� kullanarak s�n�f�n kurucusu.
    public GetBooksQueryTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;  // Veritaban� ba�lam�n� CommonTestFixture'den al�r.
        this.mapper = testFixture.Mapper;  // Veri e�lemesini CommonTestFixture'den al�r.
    }

    // GetBooksQuery i�lendi�inde kitap listesinin d�nd���n� test eden bir fakt metodu.
    [Fact]
    public void WhenGetBooksQueryIsHandled_BookListShouldBeReturned()
    {
        var query = new GetBooksQuery(context, mapper);  // GetBooksQuery s�n�f�n�n bir �rne�i olu�turulur ve ba�lam/enjeksiyonlar enjekte edilir.

        var result = query.Handle();  // Handle() y�ntemi, kitap listesini getirmek i�in �a�r�l�r.

        result.Should().NotBeNull();  // D�nen sonucun bo� olmad���n� do�rular.
    }
}
