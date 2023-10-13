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
    private readonly BookStoreDbContext context;  // Veritabaný baðlamýný temsil eden deðiþken.
    private readonly IMapper mapper;  // AutoMapper nesnesini temsil eden deðiþken.

    // Test sýnýfýnýn yapýsýný ayarlamak için CommonTestFixture sýnýfýný kullanarak sýnýfýn kurucusu.
    public GetAuthorDetailQueryTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;  // Veritabaný baðlamýný CommonTestFixture'den alýr.
        this.mapper = testFixture.Mapper;  // AutoMapper nesnesini CommonTestFixture'den alýr.
    }

    // Geçerli giriþler verildiðinde bir yazarýn döndürülmesi gerektiðini doðrulayan fakt metodu.
    [Fact]
    public void WhenValidInputsAreGiven_Author_ShouldBeReturned()
    {
        GetAuthorDetailQuery query = new(context, mapper);
        var AuthorId = query.AuthorId = 1;

        var author = context.Authors.Where(a => a.Id == AuthorId).SingleOrDefault();

        AuthorDetailViewModel vm = query.Handle();  // GetAuthorDetailQuery'nin iþlenmesi.

        // Sonuç doðrulamalarý yapýlýr.
        vm.Should().NotBeNull();
        vm.FirstName.Should().Be(author.FirstName);
        vm.LastName.Should().Be(author.LastName);
        vm.BirthDate.Should().Be(author.BirthDate);
    }

    // Var olmayan bir yazar kimliði verildiðinde bir özel durumun fýrlatýlmasý gerektiðini doðrulayan fakt metodu.
    [Fact]
    public void WhenNonExistingAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn()
    {
        int authorId = 9999;

        GetAuthorDetailQuery query = new(context, mapper);
        query.AuthorId = authorId;

        // Bir özel durumun fýrlatýlmasý gerektiði doðrulanýr.
        query
            .Invoking(x => x.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And.Message.Should()
            .Be("Author not found!");
    }
}
