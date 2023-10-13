using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using webapi.Applications.BookOperations.Commands.CreateBook;
using webapi.DBOperations;
using webapi.Entities;
using webapi.UnitTests.TestSetup;
using Xunit;
using static webapi.Applications.BookOperations.Commands.CreateBook.CreateBookCommand;

namespace webapi.UnitTests.Applications.BookOperations.Commands.CreateBook
{
    public class CreateBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;  // Veritabaný baðlamýný temsil eden deðiþken.
        private readonly IMapper _mapper;  // AutoMapper nesnesini temsil eden deðiþken.

        // Test sýnýfýnýn yapýsýný ayarlamak için CommonTestFixture sýnýfýný kullanarak sýnýfýn kurucusu.
        public CreateBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;  // Veritabaný baðlamýný CommonTestFixture'den alýr.
            _mapper = testFixture.Mapper;  // AutoMapper nesnesini CommonTestFixture'den alýr.
        }

        // Zaten varolan bir kitap baþlýðý ile yeni kitap oluþturma giriþi yapýldýðýnda InvalidOperationException hatasý üretilmesi gereken senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            var book = new Book()
            {
                Title = "WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn",
                PageCount = 100,
                PublishDate = new DateTime(1990, 01, 10),
                GenreId = 1,
                AuthorId = 1
            };
            _context.Books.Add(book);
            _context.SaveChanges();

            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
            command.Model = new CreateBookModel() { Title = book.Title };

            FluentActions
                .Invoking(() => command.Handle())
                .Should()
                .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatasý olmalýdýr.
                .And.Message.Should()
                .Be("Book already exists");  // Hatanýn içeriði kontrol edilir ve "Book already exists" olmalýdýr.
        }

        // Geçerli giriþ verileri ile yeni kitap oluþturulduðunu ve veritabanýnda kaydedildiðini doðrulayan fakt metodu.
        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeCreated()
        {
            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
            CreateBookModel model = new CreateBookModel()
            {
                Title = "Booknamehere",
                PageCount = 1000,
                PublishDate = DateTime.Now.Date.AddYears(-10),
                GenreId = 1,
                AuthorId = 1
            };
            command.Model = model;

            FluentActions.Invoking(() => command.Handle()).Invoke();  // Kitap oluþturma iþlemi çaðrýlýr.

            var book = _context.Books.SingleOrDefault(book => book.Title == model.Title);  // Oluþturulan kitap veritabanýnda aranýr.
            book.Should().NotBeNull();  // Kitabýn boþ olmamasý gerektiði doðrulanýr.
            book.PageCount.Should().Be(model.PageCount);  // Sayfa sayýsý doðrulanýr.
            book.PublishDate.Should().Be(model.PublishDate);  // Yayýn tarihi doðrulanýr.
            book.GenreId.Should().Be(model.GenreId);  // Tür kimliði doðrulanýr.
            book.AuthorId.Should().Be(model.AuthorId);  // Yazar kimliði doðrulanýr.
        }
    }
}
