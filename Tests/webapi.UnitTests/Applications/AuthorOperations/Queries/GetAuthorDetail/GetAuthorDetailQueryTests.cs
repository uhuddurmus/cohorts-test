using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using webapi.Applications.AuthorOperations.Queries.GetAuthorDetail;
using webapi.DBOperations;
using webapi.UnitTests.TestSetup;

namespace WebApi.UnitTests.Application.AuthorOperations.Queries.GetAuthorDetail;

public class GetAuthorDetailQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext context;  // Veritaban� ba�lam�n� temsil eden de�i�ken.
    private readonly IMapper mapper;  // AutoMapper nesnesini temsil eden de�i�ken.

    // Test s�n�f�n�n yap�s�n� ayarlamak i�in CommonTestFixture s�n�f�n� kullanarak s�n�f�n kurucusu.
    public GetAuthorDetailQueryTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;  // Veritaban� ba�lam�n� CommonTestFixture'den al�r.
        this.mapper = testFixture.Mapper;  // AutoMapper nesnesini CommonTestFixture'den al�r.
    }

    // Ge�erli giri�ler verildi�inde bir yazar�n d�nd�r�lmesi gerekti�ini do�rulayan fakt metodu.
    [Fact]
    public void WhenValidInputsAreGiven_Author_ShouldBeReturned()
    {
        GetAuthorDetailQuery query = new(context, mapper);
        var AuthorId = query.AuthorId = 1;

        var author = context.Authors.Where(a => a.Id == AuthorId).SingleOrDefault();

        AuthorDetailViewModel vm = query.Handle();  // GetAuthorDetailQuery'nin i�lenmesi.

        // Sonu� do�rulamalar� yap�l�r.
        vm.Should().NotBeNull();
        vm.FirstName.Should().Be(author.FirstName);
        vm.LastName.Should().Be(author.LastName);
        vm.BirthDate.Should().Be(author.BirthDate);
    }

    // Var olmayan bir yazar kimli�i verildi�inde bir �zel durumun f�rlat�lmas� gerekti�ini do�rulayan fakt metodu.
    [Fact]
    public void WhenNonExistingAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn()
    {
        int authorId = 9999;

        GetAuthorDetailQuery query = new(context, mapper);
        query.AuthorId = authorId;

        // Bir �zel durumun f�rlat�lmas� gerekti�i do�rulan�r.
        query
            .Invoking(x => x.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And.Message.Should()
            .Be("Author not found!");
    }
}
