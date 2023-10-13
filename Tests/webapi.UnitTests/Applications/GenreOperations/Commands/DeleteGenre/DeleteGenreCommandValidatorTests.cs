using AutoMapper;
using FluentAssertions;
using webapi.Applications.GenreOperations.Commands.DeleteGenre;
using webapi.UnitTests.TestSetup;
using Xunit;

namespace webapi.UnitTests.Applications.GenreOperations.Commands.DeleteGenre
{
    public class DeleteGenreCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        // Ge�ersiz GenreId de�erlerini i�eren senaryolar� test eden bir fakt metodu.
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void WhenInvalidGenreIdisGiven_Validator_ShouldBeReturnErrors(int genreid)
        {
            // DeleteGenreCommand s�n�f�n�n bir �rne�i olu�turulur ve gereken ba�lam/enjeksiyonlar null olarak ayarlan�r.
            DeleteGenreCommand command = new DeleteGenreCommand(null);
            command.GenreId = genreid;  // Ge�ersiz bir GenreId ayarlan�r.

            // DeleteGenreCommandValidator s�n�f�n�n bir �rne�i olu�turulur.
            DeleteGenreCommandValidator validations = new DeleteGenreCommandValidator();

            // Validasyon i�lemi ger�ekle�tirilir.
            var result = validations.Validate(command);

            // Hatalar�n say�s�n�n 0'dan b�y�k olmas� gerekti�i do�rulan�r.
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        // Ge�ersiz GenreId de�erlerini i�eren senaryolar� test eden bir fakt metodu.
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void WhenInvalidGenreIdisGiven_Validator_ShouldNotBeReturnErrors(int genreid)
        {
            // DeleteGenreCommand s�n�f�n�n bir �rne�i olu�turulur ve gereken ba�lam/enjeksiyonlar null olarak ayarlan�r.
            DeleteGenreCommand command = new DeleteGenreCommand(null);
            command.GenreId = genreid;  // Ge�ersiz bir GenreId ayarlan�r.

            // DeleteGenreCommandValidator s�n�f�n�n bir �rne�i olu�turulur.
            DeleteGenreCommandValidator validations = new DeleteGenreCommandValidator();

            // Validasyon i�lemi ger�ekle�tirilir.
            var result = validations.Validate(command);

            // Hatalar�n say�s�n�n 0'dan b�y�k olmas� gerekti�i do�rulan�r.
            result.Errors.Count.Should().BeGreaterThan(0);
        }
    }
}
