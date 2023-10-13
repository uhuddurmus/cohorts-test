using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using webapi.Applications.AuthorOperations.Commands.CreateAuthor;
using webapi.DBOperations;
using webapi.Entities;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Application.AuthorOperations.Commands.CreateAuthor;

public class CreateAuthorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext context;
    private readonly IMapper mapper;

    public CreateAuthorCommandTests(CommonTestFixture testFixture)
    {
        this.context = testFixture.Context;
        this mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenAlreadyExitsAuthorFullNameIsGiven_InvalidOperationException_ShouldBeReturn()
    {
        // arrange (Hazırlık)
        var author = new Author()
        {
            FirstName = "Uhud",
            LastName = "Durmus",
            BirthDate = new DateTime(1998, 09, 01)
        };
        context.Authors.Add(author);
        context.SaveChanges();

        CreateAuthorCommand command = new CreateAuthorCommand(context, mapper);
        command.Model = new CreateAuthorModel
        {
            FirstName = author.FirstName,
            LastName = author.LastName,
            BirthDate = author.BirthDate
        };

        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And.Message.Should()
            .Be("Author already exists!");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Author_ShouldBeCreated()
    {
        CreateAuthorCommand command = new CreateAuthorCommand(context, mapper);
        CreateAuthorModel model = new CreateAuthorModel()
        {
            FirstName = "Uhud",
            LastName = "Durmus",
            BirthDate = new DateTime(1998, 09, 01)
        };

        command.Model = model;

        FluentActions.Invoking(() => command.Handle()).Invoke();
        var author = context.Authors.SingleOrDefault(g => g.FirstName == model.FirstName);
        author.Should().NotBeNull();
        author.FirstName.Should().Be(model.FirstName);
        author.LastName.Should().Be(model.LastName);
        author.BirthDate.Should().Be(model.BirthDate);
    }
}
