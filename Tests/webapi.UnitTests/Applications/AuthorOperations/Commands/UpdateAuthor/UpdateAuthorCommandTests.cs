using System;
using System.Linq;
using FluentAssertions;
using webapi.Applications.AuthorOperations.Commands.UpdateAuthor;
using webapi.DBOperations;
using webapi.Entities;
using webapi.UnitTests.TestSetup;

namespace WebApi.UnitTests.Application.AuthorOperations.Commands.UpdateAuthor;

public class UpdateAuthorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext context;

    public UpdateAuthorCommandTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;
    }

    [Fact]
    public void WhenGivenAuthorIsNotFound_InvalidOperationException_ShouldBeReturn()
    {

        UpdateAuthorCommand command = new(context);
        command.AuthorId = 999;

        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And.Message.Should()
            .Be("Author not found!");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Author_ShouldBeUpdated()
    {
        UpdateAuthorCommand command = new(context);
        var author = new Author
        {
            FirstName = "Mustafa Uhud",
            LastName = "Durmus",
            BirthDate = new DateTime(1998, 09, 01)
        };

        context.Authors.Add(author);
        context.SaveChanges();

        command.AuthorId = author.Id;
        UpdateAuthorModel model = new UpdateAuthorModel
        {
            FirstName = "Uhud",
            LastName = "Durmus",
            BirthDate = new DateTime(1998, 09, 01)
        };
        command.Model = model;

        FluentActions.Invoking(() => command.Handle()).Invoke();
        var updatedAuthor = context.Authors.SingleOrDefault(a => a.Id == author.Id);
        updatedAuthor.Should().NotBeNull();
        updatedAuthor.FirstName.Should().Be(model.FirstName);
        updatedAuthor.LastName.Should().Be(model.LastName);
        updatedAuthor.BirthDate.Should().Be(model.BirthDate);
    }
}
