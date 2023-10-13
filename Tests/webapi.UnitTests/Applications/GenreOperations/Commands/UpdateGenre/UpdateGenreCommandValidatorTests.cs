using FluentAssertions;
using webapi.Applications.GenreOperations.Commands.UpdateGenre;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Applications.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        // Boþ bir genreName veya boþluktan oluþan bir genreName gibi geçersiz giriþlerin test edildiði senaryolar.
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("abc")]
        [InlineData("ab ")]
        [Theory]
        public void WhenInvalidInputsAreaGiven_Validator_ShouldBeReturnErrors(string genreName)
        {
            // UpdateGenreCommand sýnýfýnýn bir örneði oluþturulur ve gereken baðlam/enjeksiyonlar null olarak ayarlanýr.
            UpdateGenreCommand command = new UpdateGenreCommand(null);
            command.Model = new UpdateGenreModel() { Name = genreName };

            // UpdateGenreCommandValidator sýnýfýnýn bir örneði oluþturulur.
            UpdateGenreCommandValidator validations = new UpdateGenreCommandValidator();

            // Validasyon iþlemi gerçekleþtirilir.
            var result = validations.Validate(command);

            // Hatalarýn sayýsýnýn 0'dan büyük olmasý gerektiði doðrulanýr.
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        // Geçerli bir genreName ile çaðrýldýðýnda hata üretmemesi gereken senaryolar.
        [InlineData("abcd")]
        [InlineData("abc de")]
        [Theory]
        public void WhenInvalidInputsAreaGiven_Validator_ShouldNotBeReturnErrors(string genreName)
        {
            // UpdateGenreCommand sýnýfýnýn bir örneði oluþturulur ve gereken baðlam/enjeksiyonlar null olarak ayarlanýr.
            UpdateGenreCommand command = new UpdateGenreCommand(null);
            command.Model = new UpdateGenreModel() { Name = genreName };

            // UpdateGenreCommandValidator sýnýfýnýn bir örneði oluþturulur.
            UpdateGenreCommandValidator validations = new UpdateGenreCommandValidator();

            // Validasyon iþlemi gerçekleþtirilir.
            var result = validations.Validate(command);

            // Hatalarýn sayýsýnýn 0 olduðu doðrulanýr.
            result.Errors.Count.Should().Be(0);
        }
    }
}
