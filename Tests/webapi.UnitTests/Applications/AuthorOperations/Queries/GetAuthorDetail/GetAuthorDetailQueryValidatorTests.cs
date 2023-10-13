using FluentAssertions;
using webapi.Applications.AuthorOperations.Queries.GetAuthorDetail;
using webapi.UnitTests.TestSetup;

namespace WebApi.UnitTests.Application.AuthorOperations.Queries.GetAuthorDetail;

public class GetAuthorDetailQueryValidatorTests : IClassFixture<CommonTestFixture>
{
    private GetAuthorDetailQueryValidator _validator;  // Doðrulama sýnýfýný temsil eden deðiþken.

    public GetAuthorDetailQueryValidatorTests()
    {
        _validator = new();  // Doðrulama sýnýfýnýn örneði oluþturulur.
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WhenAuthorIdLessThanOrEqualZero_ValidationShouldReturnError(int authorId)
    {
        GetAuthorDetailQuery query = new(null, null);  // GetAuthorDetailQuery sýnýfýnýn örneði oluþturulur.
        query.AuthorId = authorId;

        var result = _validator.Validate(query);  // Doðrulama iþlemi yapýlýr.

        result.Errors.Count.Should().BeGreaterThan(0);  // Hata sayýsýnýn sýfýrdan büyük olduðu doðrulanýr.
    }

    [Fact]
    public void WhenAuthorIdGreaterThanZero_ValidationShouldNotReturnError()
    {
        GetAuthorDetailQuery query = new(null, null);  // GetAuthorDetailQuery sýnýfýnýn örneði oluþturulur.
        query.AuthorId = 12;

        var result = _validator.Validate(query);  // Doðrulama iþlemi yapýlýr.

        result.Errors.Count.Should().Be(0);  // Hata sayýsýnýn sýfýr olduðu doðrulanýr.
    }
}
