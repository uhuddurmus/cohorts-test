using FluentAssertions;
using webapi.Applications.GenreOperations.Commands.CreateGenre;
using webapi.UnitTests.TestSetup;
using Xunit;

namespace webapi.UnitTests.Applications.GenreOperations.Commands.CreateGenre
{
    public class CreateGenreCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        // Geçersiz giriþ deðerlerini içeren senaryolarý test eden bir fakt metodu.
        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("a b")]
        [InlineData("a")]
        [InlineData("ab")]
        public void WhenInvalidInputsGiven_Validator_ShouldBeReturnErrors(String name)
        {
            // CreateGenreCommand sýnýfýnýn bir örneði oluþturulur ve gereken baðlam/enjeksiyonlar null olarak ayarlanýr.
            CreateGenreCommand command = new CreateGenreCommand(null);
            command.Model = new CreateGenreModel() { Name = name };  // Geçersiz bir ad ile komut oluþturulur.

            // CreateGenreCommandValidator sýnýfýnýn bir örneði oluþturulur.
            CreateGenreCommandValidator validations = new CreateGenreCommandValidator();

            // Validasyon iþlemi gerçekleþtirilir.
            var result = validations.Validate(command);

            // Hatalarýn sayýsýnýn 0'dan büyük olmasý gerektiði doðrulanýr.
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        // Geçersiz giriþ deðerlerini içeren senaryolarý test eden bir fakt metodu.
        [Theory]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("abc")]
        public void WhenInvalidInputsGiven_Validator_ShouldBeReturn(String name)
        {
            // CreateGenreCommand sýnýfýnýn bir örneði oluþturulur ve gereken baðlam/enjeksiyonlar null olarak ayarlanýr.
            CreateGenreCommand command = new CreateGenreCommand(null);
            command.Model = new CreateGenreModel() { Name = name };  // Geçersiz bir ad ile komut oluþturulur.

            // CreateGenreCommandValidator sýnýfýnýn bir örneði oluþturulur.
            CreateGenreCommandValidator validations = new CreateGenreCommandValidator();

            // Validasyon iþlemi gerçekleþtirilir.
            var result = validations.Validate(command);

            // Hatalarýn sayýsýnýn 0'dan büyük olmasý gerektiði doðrulanýr.
            result.Errors.Count.Should().BeGreaterThan(0);
        }
    }
}
