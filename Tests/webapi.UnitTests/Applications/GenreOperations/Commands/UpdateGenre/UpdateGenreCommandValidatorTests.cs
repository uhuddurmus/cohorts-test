using FluentAssertions;
using webapi.Applications.GenreOperations.Commands.UpdateGenre;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Applications.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        // Bo� bir genreName veya bo�luktan olu�an bir genreName gibi ge�ersiz giri�lerin test edildi�i senaryolar.
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("abc")]
        [InlineData("ab ")]
        [Theory]
        public void WhenInvalidInputsAreaGiven_Validator_ShouldBeReturnErrors(string genreName)
        {
            // UpdateGenreCommand s�n�f�n�n bir �rne�i olu�turulur ve gereken ba�lam/enjeksiyonlar null olarak ayarlan�r.
            UpdateGenreCommand command = new UpdateGenreCommand(null);
            command.Model = new UpdateGenreModel() { Name = genreName };

            // UpdateGenreCommandValidator s�n�f�n�n bir �rne�i olu�turulur.
            UpdateGenreCommandValidator validations = new UpdateGenreCommandValidator();

            // Validasyon i�lemi ger�ekle�tirilir.
            var result = validations.Validate(command);

            // Hatalar�n say�s�n�n 0'dan b�y�k olmas� gerekti�i do�rulan�r.
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        // Ge�erli bir genreName ile �a�r�ld���nda hata �retmemesi gereken senaryolar.
        [InlineData("abcd")]
        [InlineData("abc de")]
        [Theory]
        public void WhenInvalidInputsAreaGiven_Validator_ShouldNotBeReturnErrors(string genreName)
        {
            // UpdateGenreCommand s�n�f�n�n bir �rne�i olu�turulur ve gereken ba�lam/enjeksiyonlar null olarak ayarlan�r.
            UpdateGenreCommand command = new UpdateGenreCommand(null);
            command.Model = new UpdateGenreModel() { Name = genreName };

            // UpdateGenreCommandValidator s�n�f�n�n bir �rne�i olu�turulur.
            UpdateGenreCommandValidator validations = new UpdateGenreCommandValidator();

            // Validasyon i�lemi ger�ekle�tirilir.
            var result = validations.Validate(command);

            // Hatalar�n say�s�n�n 0 oldu�u do�rulan�r.
            result.Errors.Count.Should().Be(0);
        }
    }
}
