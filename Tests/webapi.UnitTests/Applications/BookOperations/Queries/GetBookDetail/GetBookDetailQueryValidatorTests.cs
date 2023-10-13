using FluentAssertions;
using webapi.Applications.BookOperations.Queries.GetBookDetail;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Application.BookOperations.Queries.GetBookDetail;

public class GetBookDetailQueryValidatorTests : IClassFixture<CommonTestFixture>
{
    private GetBookDetailQueryValidator _validator;  // GetBookDetailQueryValidator s�n�f�n�n �rne�ini tutan de�i�ken.

    public GetBookDetailQueryValidatorTests()
    {
        _validator = new GetBookDetailQueryValidator();  // GetBookDetailQueryValidator s�n�f�n�n bir �rne�i olu�turulur.
    }

    // BookId de�eri 0 veya daha k���k oldu�unda do�rulama i�leminin hatalar �retmesi gereken senaryolar� test eden bir fakt metodu.
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WhenBookIdLessThanOrEqualZero_ValidationShouldReturnError(int bookId)
    {
        GetBookDetailQuery query = new GetBookDetailQuery(null, null);
        query.BookId = bookId;  // Ge�ersiz bir BookId ayarlan�r.

        var result = _validator.Validate(query);  // Do�rulama i�lemi ger�ekle�tirilir.

        result.Errors.Count.Should().BeGreaterThan(0);  // Hatalar�n say�s�n�n 0'dan b�y�k olmas� gerekti�i do�rulan�r.
    }

    // BookId de�eri 0'dan b�y�k oldu�unda do�rulama i�leminin hata �retmemesi gereken senaryoyu test eden bir fakt metodu.
    [Fact]
    public void WhenBookIdGreaterThanZero_ValidationShouldNotReturnError()
    {
        GetBookDetailQuery query = new GetBookDetailQuery(null, null);
        query.BookId = 12;  // Ge�erli bir BookId ayarlan�r.

        var result = _validator.Validate(query);  // Do�rulama i�lemi ger�ekle�tirilir.

        result.Errors.Count.Should().Be(0);  // Hatalar�n say�s�n�n 0 olmas� gerekti�i do�rulan�r.
    }
}