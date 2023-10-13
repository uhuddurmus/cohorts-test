using FluentAssertions;
using webapi.Applications.GenreOperations.Commands.CreateGenre;
using webapi.UnitTests.TestSetup;
using Xunit;

namespace webapi.UnitTests.Applications.GenreOperations.Commands.CreateGenre
{
    public class CreateGenreCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        // Ge�ersiz giri� de�erlerini i�eren senaryolar� test eden bir fakt metodu.
        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("a b")]
        [InlineData("a")]
        [InlineData("ab")]
        public void WhenInvalidInputsGiven_Validator_ShouldBeReturnErrors(String name)
        {
            // CreateGenreCommand s�n�f�n�n bir �rne�i olu�turulur ve gereken ba�lam/enjeksiyonlar null olarak ayarlan�r.
            CreateGenreCommand command = new CreateGenreCommand(null);
            command.Model = new CreateGenreModel() { Name = name };  // Ge�ersiz bir ad ile komut olu�turulur.

            // CreateGenreCommandValidator s�n�f�n�n bir �rne�i olu�turulur.
            CreateGenreCommandValidator validations = new CreateGenreCommandValidator();

            // Validasyon i�lemi ger�ekle�tirilir.
            var result = validations.Validate(command);

            // Hatalar�n say�s�n�n 0'dan b�y�k olmas� gerekti�i do�rulan�r.
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        // Ge�ersiz giri� de�erlerini i�eren senaryolar� test eden bir fakt metodu.
        [Theory]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("abc")]
        public void WhenInvalidInputsGiven_Validator_ShouldBeReturn(String name)
        {
            // CreateGenreCommand s�n�f�n�n bir �rne�i olu�turulur ve gereken ba�lam/enjeksiyonlar null olarak ayarlan�r.
            CreateGenreCommand command = new CreateGenreCommand(null);
            command.Model = new CreateGenreModel() { Name = name };  // Ge�ersiz bir ad ile komut olu�turulur.

            // CreateGenreCommandValidator s�n�f�n�n bir �rne�i olu�turulur.
            CreateGenreCommandValidator validations = new CreateGenreCommandValidator();

            // Validasyon i�lemi ger�ekle�tirilir.
            var result = validations.Validate(command);

            // Hatalar�n say�s�n�n 0'dan b�y�k olmas� gerekti�i do�rulan�r.
            result.Errors.Count.Should().BeGreaterThan(0);
        }
    }
}
