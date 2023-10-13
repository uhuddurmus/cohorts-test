using FluentAssertions;
using webapi.Applications.BookOperations.Queries.GetBookDetail;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Application.BookOperations.Queries.GetBookDetail;

public class GetBookDetailQueryValidatorTests : IClassFixture<CommonTestFixture>
{
    private GetBookDetailQueryValidator _validator;  // GetBookDetailQueryValidator sýnýfýnýn örneðini tutan deðiþken.

    public GetBookDetailQueryValidatorTests()
    {
        _validator = new GetBookDetailQueryValidator();  // GetBookDetailQueryValidator sýnýfýnýn bir örneði oluþturulur.
    }

    // BookId deðeri 0 veya daha küçük olduðunda doðrulama iþleminin hatalar üretmesi gereken senaryolarý test eden bir fakt metodu.
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WhenBookIdLessThanOrEqualZero_ValidationShouldReturnError(int bookId)
    {
        GetBookDetailQuery query = new GetBookDetailQuery(null, null);
        query.BookId = bookId;  // Geçersiz bir BookId ayarlanýr.

        var result = _validator.Validate(query);  // Doðrulama iþlemi gerçekleþtirilir.

        result.Errors.Count.Should().BeGreaterThan(0);  // Hatalarýn sayýsýnýn 0'dan büyük olmasý gerektiði doðrulanýr.
    }

    // BookId deðeri 0'dan büyük olduðunda doðrulama iþleminin hata üretmemesi gereken senaryoyu test eden bir fakt metodu.
    [Fact]
    public void WhenBookIdGreaterThanZero_ValidationShouldNotReturnError()
    {
        GetBookDetailQuery query = new GetBookDetailQuery(null, null);
        query.BookId = 12;  // Geçerli bir BookId ayarlanýr.

        var result = _validator.Validate(query);  // Doðrulama iþlemi gerçekleþtirilir.

        result.Errors.Count.Should().Be(0);  // Hatalarýn sayýsýnýn 0 olmasý gerektiði doðrulanýr.
    }
}