using System;
using System.Linq;
using FluentAssertions;
using webapi.Applications.AuthorOperations.Commands.DeleteAuthor;
using webapi.DBOperations;
using webapi.Entities;
using webapi.UnitTests.TestSetup;

namespace WebApi.UnitTests.Application.AuthorOperations.Commands.DeleteAuthor;

public class DeleteAuthorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext context;

    public DeleteAuthorCommandTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;
    }

    // Var olmayan bir yazar�n silinmesi giri�iminin bir "InvalidOperationException" f�rlatmas� gerekti�ini do�rular.
    [Fact]
    public void WhenGivenAuthorIsNotFound_InvalidOperationException_ShouldBeReturn()
    {
        DeleteAuthorCommand command = new(context);
        command.AuthorId = 120;

        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And.Message.Should()
            .Be("Author not found!");
    }

    // Kitaplar� olan bir yazar�n silinmesi giri�iminin bir "InvalidOperationException" f�rlatmas� gerekti�ini do�rular.
    [Fact]
    public void WhenGivenAuthorHaveBook_InvalidOperationException_ShouldBeReturn()
    {
        DeleteAuthorCommand command = new(context);
        command.AuthorId = 1;

        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And.Message.Should()
            .Be("Author has books, cannot be deleted!");
    }

    // Ge�erli girdilerle bir yazar�n ba�ar�yla silinebilmesini do�rular.
    [Fact]
    public void WhenValidInputsAreGiven_Author_ShouldBeDeleted()
    {
        var newAuthor = new Author()
        {
            FirstName = "Uhud",
            LastName = "Durmus",
            BirthDate = new DateTime(1998, 09, 01)
        };
        context.Authors.Add(newAuthor);
        context.SaveChanges();

        DeleteAuthorCommand command = new(context);
        command.AuthorId = newAuthor.Id;

        FluentActions.Invoking(() => command.Handle()).Invoke();
        var author = context.Authors.SingleOrDefault(a => a.Id == command.AuthorId);
        author.Should().BeNull();
    }
}
