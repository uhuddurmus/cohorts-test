using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using webapi.Applications.GenreOperations.Queries.GetGenreDetail;
using webapi.DBOperations;
using webapi.UnitTests.TestSetup;
using Xunit;

namespace webapi.UnitTests.Applications.GenreOperations.Queries
{
    public class GetGenreDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;  // Veritabaný baðlamýný temsil eden deðiþken.
        private readonly IMapper _mappper;  // Veri eþleme iþlemlerini temsil eden deðiþken.

        // Test sýnýfýnýn yapýsýný ayarlamak için CommonTestFixture sýnýfýný kullanarak sýnýfýn kurucusu.
        public GetGenreDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;  // Veritabaný baðlamýný CommonTestFixture'den alýr.
            _mappper = testFixture.Mapper;   // Veri eþleme iþlemlerini CommonTestFixture'den alýr.
        }

        // Verilen GenreId'nin veritabanýnda bulunmadýðý senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenGivenGenreIdIsNotinDb_InvalidOperationException_ShouldBeReturn()
        {
            // GetGenreDetailQuery sýnýfýnýn bir örneði oluþturulur ve veritabaný baðlamý ve veri eþlemesi enjekte edilir.
            GetGenreDetailQuery Query = new GetGenreDetailQuery(_context, _mappper);

            Query.GenreId = 0;  // GenreId, veritabanýnda olmayan bir deðerle ayarlanýr.

            // FluentAssertions kullanýlarak, bir iþlemi tetiklemek ve ardýndan hata durumunu test etmek için Invoking() yöntemi kullanýlýr.
            FluentActions
                .Invoking(() => Query.Handle())  // Handle() yöntemi, GenreId'ye karþýlýk gelen detaylarý çeker.
                .Should()
                .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatasý olmalýdýr.
                .And.Message.Should()
                .Be("Genre is not found!");  // Hatanýn içeriði kontrol edilir ve "Genre is not found!" olmalýdýr.
        }

        // Verilen GenreId'nin veritabanýnda bulunduðu senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenGivenGenreIdIsinDB_InvalidOperationException_shouldBeReturn()
        {
            // GetGenreDetailQuery sýnýfýnýn bir örneði oluþturulur ve veritabaný baðlamý ve veri eþlemesi enjekte edilir.
            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mappper);

            query.GenreId = 2;  // GenreId, veritabanýnda bulunan bir deðerle ayarlanýr.

            // Veritabanýnda belirli bir GenreId ile eþleþen bir türün olup olmadýðýný kontrol eder.
            var genre = _context.Genres.SingleOrDefault(genre => genre.Id == query.GenreId);

            // genre deðiþkeninin null olmamasý gerektiðini doðrular.
            genre.Should().NotBeNull();
        }
    }
}
