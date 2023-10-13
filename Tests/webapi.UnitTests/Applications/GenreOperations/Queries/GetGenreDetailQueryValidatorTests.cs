using FluentAssertions;
using webapi.Applications.GenreOperations.Queries.GetGenreDetail;
using webapi.UnitTests.TestSetup;
using Xunit;

namespace webapi.UnitTests.Applications.GenreOperations.Queries
{
    public class GetGenreDetailQueryValidatorTests : IClassFixture<CommonTestFixture>
    {
        // Ge�erli olmayan GenreId de�erlerini test etmek i�in bir dizi senaryo.
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-114)]
        [Theory]
        public void WhenInvalidGenreIdisGiven_Validator_ShouldBeReturnErrors(int Genreid)
        {
            // Test senaryosu i�in bir GetGenreDetailQuery nesnesi olu�turulur.
            GetGenreDetailQuery query = new GetGenreDetailQuery(null, null);
            query.GenreId = Genreid;

            // Do�rulama i�lemini yapacak olan GetGenreDetailQueryValidator s�n�f� �rne�i olu�turulur.
            GetGenreDetailQueryValidator validations = new GetGenreDetailQueryValidator();

            // Validasyon i�lemi ger�ekle�tirilir.
            var result = validations.Validate(query);

            // Hatalar�n say�s�n�n 0'dan b�y�k oldu�unu do�rular.
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        // Ge�erli GenreId de�erlerini test etmek i�in bir dizi senaryo.
        [InlineData(1)]
        [InlineData(2)]
        [Theory]
        public void WhenInvalidGenreidIsGiven_Validator_ShouldNotBeReturnErrors(int genreid)
        {
            // Test senaryosu i�in bir GetGenreDetailQuery nesnesi olu�turulur.
            GetGenreDetailQuery query = new GetGenreDetailQuery(null, null);
            query.GenreId = genreid;

            // Do�rulama i�lemini yapacak olan GetGenreDetailQueryValidator s�n�f� �rne�i olu�turulur.
            GetGenreDetailQueryValidator validations = new GetGenreDetailQueryValidator();

            // Validasyon i�lemi ger�ekle�tirilir.
            var result = validations.Validate(query);

            // Hatalar�n say�s�n�n 0 oldu�unu do�rular.
            result.Errors.Count.Should().Be(0);
        }
    }
}