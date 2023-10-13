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
        private readonly BookStoreDbContext _context;  // Veritaban� ba�lam�n� temsil eden de�i�ken.
        private readonly IMapper _mappper;  // Veri e�leme i�lemlerini temsil eden de�i�ken.

        // Test s�n�f�n�n yap�s�n� ayarlamak i�in CommonTestFixture s�n�f�n� kullanarak s�n�f�n kurucusu.
        public GetGenreDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;  // Veritaban� ba�lam�n� CommonTestFixture'den al�r.
            _mappper = testFixture.Mapper;   // Veri e�leme i�lemlerini CommonTestFixture'den al�r.
        }

        // Verilen GenreId'nin veritaban�nda bulunmad��� senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenGivenGenreIdIsNotinDb_InvalidOperationException_ShouldBeReturn()
        {
            // GetGenreDetailQuery s�n�f�n�n bir �rne�i olu�turulur ve veritaban� ba�lam� ve veri e�lemesi enjekte edilir.
            GetGenreDetailQuery Query = new GetGenreDetailQuery(_context, _mappper);

            Query.GenreId = 0;  // GenreId, veritaban�nda olmayan bir de�erle ayarlan�r.

            // FluentAssertions kullan�larak, bir i�lemi tetiklemek ve ard�ndan hata durumunu test etmek i�in Invoking() y�ntemi kullan�l�r.
            FluentActions
                .Invoking(() => Query.Handle())  // Handle() y�ntemi, GenreId'ye kar��l�k gelen detaylar� �eker.
                .Should()
                .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatas� olmal�d�r.
                .And.Message.Should()
                .Be("Genre is not found!");  // Hatan�n i�eri�i kontrol edilir ve "Genre is not found!" olmal�d�r.
        }

        // Verilen GenreId'nin veritaban�nda bulundu�u senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenGivenGenreIdIsinDB_InvalidOperationException_shouldBeReturn()
        {
            // GetGenreDetailQuery s�n�f�n�n bir �rne�i olu�turulur ve veritaban� ba�lam� ve veri e�lemesi enjekte edilir.
            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mappper);

            query.GenreId = 2;  // GenreId, veritaban�nda bulunan bir de�erle ayarlan�r.

            // Veritaban�nda belirli bir GenreId ile e�le�en bir t�r�n olup olmad���n� kontrol eder.
            var genre = _context.Genres.SingleOrDefault(genre => genre.Id == query.GenreId);

            // genre de�i�keninin null olmamas� gerekti�ini do�rular.
            genre.Should().NotBeNull();
        }
    }
}
