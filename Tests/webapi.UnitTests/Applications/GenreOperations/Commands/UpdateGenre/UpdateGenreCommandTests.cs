using AutoMapper;
using FluentAssertions;
using webapi.Applications.GenreOperations.Commands.UpdateGenre;
using webapi.DBOperations;
using webapi.Entities;
using webapi.UnitTests.TestSetup;
using Xunit;

namespace webapi.UnitTests.Applications.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;  // Veritaban� ba�lam�n� temsil eden de�i�ken.

        // Test s�n�f�n�n yap�s�n� ayarlamak i�in CommonTestFixture s�n�f�n� kullanarak s�n�f�n kurucusu.
        public UpdateGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;  // Veritaban� ba�lam�n� CommonTestFixture'den al�r.
        }

        // Verilen GenreId'nin veritaban�nda bulunmad��� senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenGivenGenreIdIsNotinDB_InvalidOperationsException_ShouldBeReturn()
        {
            // UpdateGenreCommand s�n�f�n�n bir �rne�i olu�turulur ve gereken ba�lam/enjeksiyonlar enjekte edilir.
            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = 0;  // Ge�ersiz bir GenreId ayarlan�r.

            // FluentActions kullan�larak, bir i�lemi tetiklemek ve ard�ndan hata durumunu test etmek i�in Invoking() y�ntemi kullan�l�r.
            FluentActions
                .Invoking(() => command.Handle())  // Handle() y�ntemi, Genre'� g�ncellemeye �al���r.
                .Should()
                .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatas� olmal�d�r.
                .And.Message.Should()
                .Be("Genre is not found!");  // Hatan�n i�eri�i kontrol edilir ve "Genre is not found!" olmal�d�r.
        }

        // Verilen ad�n veritaban�nda ba�ka bir t�r ile ayn� oldu�u senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenGivenNameIsSameWithAnotherGenre_InvalidOperationException_ShouldBeReturn()
        {
            // Veritaban�na yeni bir t�r eklenir.
            var genre = new Genre() { Name = "Science Fiction" };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            // UpdateGenreCommand s�n�f�n�n bir �rne�i olu�turulur ve gereken ba�lam/enjeksiyonlar enjekte edilir.
            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = 2;
            command.Model = new UpdateGenreModel() { Name = "Science Fiction" };  // Mevcut bir t�rle ayn� ad� i�eren bir t�r eklenir.

            // FluentActions kullan�larak, bir i�lemi tetiklemek ve ard�ndan hata durumunu test etmek i�in Invoking() y�ntemi kullan�l�r.
            FluentActions
                .Invoking(() => command.Handle())  // Handle() y�ntemi, Genre'� g�ncellemeye �al���r.
                .Should()
                .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatas� olmal�d�r.
                .And.Message.Should()
                .Be("Genre is already exist!");  // Hatan�n i�eri�i kontrol edilir ve "Genre is already exist!" olmal�d�r.
        }

        // Ge�erli bir GenreId ile �a�r�ld���nda Genre'� g�ncellemesi gereken senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenGivenGenreIdinDB_ShouldBeUpdate()
        {
            // UpdateGenreCommand s�n�f�n�n bir �rne�i olu�turulur ve gereken ba�lam/enjeksiyonlar enjekte edilir.
            UpdateGenreCommand command = new UpdateGenreCommand(_context);

            command.Model = new UpdateGenreModel() { Name = "WhenGivenGenreIdinDB_ShouldBeUpdate" };
            command.GenreId = 1;  // Varolan bir Genre'� g�ncellemek i�in ge�erli bir GenreId ayarlan�r.

            FluentActions.Invoking(() => command.Handle()).Invoke();

            // GenreId ile e�le�en bir t�r�n varl���n� kontrol eder.
            var genre = _context.Genres.SingleOrDefault(genre => genre.Id == command.GenreId);
            genre.Should().NotBeNull();  // Genre de�i�keninin null olmamas� gerekti�ini do�rular.
        }
    }
}
