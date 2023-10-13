using AutoMapper;
using FluentAssertions;
using webapi.Applications.GenreOperations.Commands.DeleteGenre;
using webapi.UnitTests.TestSetup;
using Xunit;

namespace webapi.UnitTests.Applications.GenreOperations.Commands.DeleteGenre
{
    public class DeleteGenreCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        // Geçersiz GenreId deðerlerini içeren senaryolarý test eden bir fakt metodu.
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void WhenInvalidGenreIdisGiven_Validator_ShouldBeReturnErrors(int genreid)
        {
            // DeleteGenreCommand sýnýfýnýn bir örneði oluþturulur ve gereken baðlam/enjeksiyonlar null olarak ayarlanýr.
            DeleteGenreCommand command = new DeleteGenreCommand(null);
            command.GenreId = genreid;  // Geçersiz bir GenreId ayarlanýr.

            // DeleteGenreCommandValidator sýnýfýnýn bir örneði oluþturulur.
            DeleteGenreCommandValidator validations = new DeleteGenreCommandValidator();

            // Validasyon iþlemi gerçekleþtirilir.
            var result = validations.Validate(command);

            // Hatalarýn sayýsýnýn 0'dan büyük olmasý gerektiði doðrulanýr.
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        // Geçersiz GenreId deðerlerini içeren senaryolarý test eden bir fakt metodu.
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void WhenInvalidGenreIdisGiven_Validator_ShouldNotBeReturnErrors(int genreid)
        {
            // DeleteGenreCommand sýnýfýnýn bir örneði oluþturulur ve gereken baðlam/enjeksiyonlar null olarak ayarlanýr.
            DeleteGenreCommand command = new DeleteGenreCommand(null);
            command.GenreId = genreid;  // Geçersiz bir GenreId ayarlanýr.

            // DeleteGenreCommandValidator sýnýfýnýn bir örneði oluþturulur.
            DeleteGenreCommandValidator validations = new DeleteGenreCommandValidator();

            // Validasyon iþlemi gerçekleþtirilir.
            var result = validations.Validate(command);

            // Hatalarýn sayýsýnýn 0'dan büyük olmasý gerektiði doðrulanýr.
            result.Errors.Count.Should().BeGreaterThan(0);
        }
    }
}
