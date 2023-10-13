using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using webapi.Applications.GenreOperations.Commands.DeleteGenre;
using webapi.DBOperations;
using webapi.UnitTests.TestSetup;
using Xunit;

namespace webapi.UnitTests.Applications.GenreOperations.Commands.DeleteGenre
{
    public class DeleteGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;  // Veritaban� ba�lam�n� temsil eden de�i�ken.

        // Test s�n�f�n�n yap�s�n� ayarlamak i�in CommonTestFixture s�n�f�n� kullanarak s�n�f�n kurucusu.
        public DeleteGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;  // Veritaban� ba�lam�n� CommonTestFixture'den al�r.
        }

        // Verilen GenreId'nin veritaban�nda bulunmad��� senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenGivenGenreIdIsNotInDB_InvalidOperationsException_ShouldBeReturn()
        {
            // DeleteGenreCommand s�n�f�n�n bir �rne�i olu�turulur ve gereken ba�lam/enjeksiyonlar enjekte edilir.
            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            command.GenreId = 0;  // Ge�ersiz bir GenreId ayarlan�r.

            // FluentActions kullan�larak, bir i�lemi tetiklemek ve ard�ndan hata durumunu test etmek i�in Invoking() y�ntemi kullan�l�r.
            FluentActions
                .Invoking(() => command.Handle())  // Handle() y�ntemi, Genre'� silmeye �al���r.
                .Should()
                .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatas� olmal�d�r.
                .And.Message.Should()
                .Be("Genre is not found!");  // Hatan�n i�eri�i kontrol edilir ve "Genre is not found!" olmal�d�r.
        }

        // Verilen GenreId'nin veritaban�nda bulundu�u senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenGivenGenreIdIsNotInDB_ShouldBeRemove()
        {
            // DeleteGenreCommand s�n�f�n�n bir �rne�i olu�turulur ve gereken ba�lam/enjeksiyonlar enjekte edilir.
            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            command.GenreId = 1;  // Varolan bir Genre'� silmek i�in ge�erli bir GenreId ayarlan�r.

            FluentActions.Invoking(() => command.Handle()).Invoke();

            // GenreId ile e�le�en bir t�r�n hala varl���n� kontrol eder.
            var genre = _context.Genres.SingleOrDefault(genre => genre.Id == command.GenreId);
            genre.Should().Be(null);  // Genre de�i�keninin null olmas� gerekti�ini do�rular.
        }
    }
}
