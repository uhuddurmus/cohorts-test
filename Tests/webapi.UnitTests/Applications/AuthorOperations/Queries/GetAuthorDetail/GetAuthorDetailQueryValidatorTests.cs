using FluentAssertions;
using webapi.Applications.AuthorOperations.Queries.GetAuthorDetail;
using webapi.UnitTests.TestSetup;

namespace WebApi.UnitTests.Application.AuthorOperations.Queries.GetAuthorDetail;

public class GetAuthorDetailQueryValidatorTests : IClassFixture<CommonTestFixture>
{
    private GetAuthorDetailQueryValidator _validator;  // Do�rulama s�n�f�n� temsil eden de�i�ken.

    public GetAuthorDetailQueryValidatorTests()
    {
        _validator = new();  // Do�rulama s�n�f�n�n �rne�i olu�turulur.
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WhenAuthorIdLessThanOrEqualZero_ValidationShouldReturnError(int authorId)
    {
        GetAuthorDetailQuery query = new(null, null);  // GetAuthorDetailQuery s�n�f�n�n �rne�i olu�turulur.
        query.AuthorId = authorId;

        var result = _validator.Validate(query);  // Do�rulama i�lemi yap�l�r.

        result.Errors.Count.Should().BeGreaterThan(0);  // Hata say�s�n�n s�f�rdan b�y�k oldu�u do�rulan�r.
    }

    [Fact]
    public void WhenAuthorIdGreaterThanZero_ValidationShouldNotReturnError()
    {
        GetAuthorDetailQuery query = new(null, null);  // GetAuthorDetailQuery s�n�f�n�n �rne�i olu�turulur.
        query.AuthorId = 12;

        var result = _validator.Validate(query);  // Do�rulama i�lemi yap�l�r.

        result.Errors.Count.Should().Be(0);  // Hata say�s�n�n s�f�r oldu�u do�rulan�r.
    }
}
