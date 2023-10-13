using FluentAssertions;
using webapi.Applications.GenreOperations.Commands.CreateGenre;
using webapi.DBOperations;
using webapi.Entities;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Applications.GenreOperations.Commands.CreateGenre
{
    public class CreateGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;  // Veritaban� ba�lam�n� temsil eden de�i�ken.

        // Test s�n�f�n�n yap�s�n� ayarlamak i�in CommonTestFixture s�n�f�n� kullanarak s�n�f�n kurucusu.
        public CreateGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;  // Veritaban� ba�lam�n� CommonTestFixture'den al�r.
        }

        // Ayn� ad ile bir t�r�n veritaban�nda zaten mevcut oldu�u senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenAlreadyExitGenreTitleIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Veritaban�na yeni bir t�r eklenir.
            var genre = new Genre()
            {
                Name = "WhenAlreadyExitGenreTitleIsGiven_InvalidOperationException_ShouldBeReturn"
            };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            // CreateGenreCommand s�n�f�n�n bir �rne�i olu�turulur ve gereken ba�lam/enjeksiyonlar enjekte edilir.
            CreateGenreCommand command = new CreateGenreCommand(_context);
            command.Model = new CreateGenreModel() { Name = genre.Name };  // Ayn� ad ile bir t�r eklemeye �al���l�r.

            // FluentActions kullan�larak, bir i�lemi tetiklemek ve ard�ndan hata durumunu test etmek i�in Invoking() y�ntemi kullan�l�r.
            FluentActions
                .Invoking(() => command.Handle())  // Handle() y�ntemi, Genre eklemeye �al���r.
                .Should()
                .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatas� olmal�d�r.
                .And.Message.Should()
                .Be("Genre is already exist!");  // Hatan�n i�eri�i kontrol edilir ve "Genre is already exist!" olmal�d�r.
        }

        // Ge�erli giri�lerle bir t�r�n ba�ar�yla olu�turuldu�u senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenValidInputsAreaGiven_Genre_shouldBeCreated()
        {
            // CreateGenreCommand s�n�f�n�n bir �rne�i olu�turulur ve gereken ba�lam/enjeksiyonlar enjekte edilir.
            CreateGenreCommand command = new CreateGenreCommand(_context);
            command.Model = new CreateGenreModel()
            {
                Name = "WhenValidInputIsGiven_ShouldBeCreated"
            };

            // FluentActions kullan�larak, bir i�lemi tetiklemek ve i�lemi ger�ekle�tirmek i�in Invoke() y�ntemi kullan�l�r.
            FluentActions.Invoking(() => command.Handle()).Invoke();

            // Eklenen t�r� veritaban�nda arar ve bulunup bulunmad���n� kontrol eder.
            var genre = _context.Genres.SingleOrDefault(genre => genre.Name == command.Model.Name);
            genre.Should().NotBeNull();  // Genre de�i�keninin null olmamas� gerekti�ini do�rular.
        }
    }
}
