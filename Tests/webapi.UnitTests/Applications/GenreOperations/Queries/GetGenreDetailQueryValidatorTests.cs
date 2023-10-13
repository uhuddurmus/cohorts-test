using FluentAssertions;
using webapi.Applications.GenreOperations.Queries.GetGenreDetail;
using webapi.UnitTests.TestSetup;
using Xunit;

namespace webapi.UnitTests.Applications.GenreOperations.Queries
{
    public class GetGenreDetailQueryValidatorTests : IClassFixture<CommonTestFixture>
    {
        // Geçerli olmayan GenreId deðerlerini test etmek için bir dizi senaryo.
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-114)]
        [Theory]
        public void WhenInvalidGenreIdisGiven_Validator_ShouldBeReturnErrors(int Genreid)
        {
            // Test senaryosu için bir GetGenreDetailQuery nesnesi oluþturulur.
            GetGenreDetailQuery query = new GetGenreDetailQuery(null, null);
            query.GenreId = Genreid;

            // Doðrulama iþlemini yapacak olan GetGenreDetailQueryValidator sýnýfý örneði oluþturulur.
            GetGenreDetailQueryValidator validations = new GetGenreDetailQueryValidator();

            // Validasyon iþlemi gerçekleþtirilir.
            var result = validations.Validate(query);

            // Hatalarýn sayýsýnýn 0'dan büyük olduðunu doðrular.
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        // Geçerli GenreId deðerlerini test etmek için bir dizi senaryo.
        [InlineData(1)]
        [InlineData(2)]
        [Theory]
        public void WhenInvalidGenreidIsGiven_Validator_ShouldNotBeReturnErrors(int genreid)
        {
            // Test senaryosu için bir GetGenreDetailQuery nesnesi oluþturulur.
            GetGenreDetailQuery query = new GetGenreDetailQuery(null, null);
            query.GenreId = genreid;

            // Doðrulama iþlemini yapacak olan GetGenreDetailQueryValidator sýnýfý örneði oluþturulur.
            GetGenreDetailQueryValidator validations = new GetGenreDetailQueryValidator();

            // Validasyon iþlemi gerçekleþtirilir.
            var result = validations.Validate(query);

            // Hatalarýn sayýsýnýn 0 olduðunu doðrular.
            result.Errors.Count.Should().Be(0);
        }
    }
}