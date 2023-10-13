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
        private readonly BookStoreDbContext _context;  // Veritaban� ba�lam�n� temsil eden de�i�ken.
        private readonly IMapper _mapper;  // AutoMapper nesnesini temsil eden de�i�ken.

        // Test s�n�f�n�n yap�s�n� ayarlamak i�in CommonTestFixture s�n�f�n� kullanarak s�n�f�n kurucusu.
        public CreateBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;  // Veritaban� ba�lam�n� CommonTestFixture'den al�r.
            _mapper = testFixture.Mapper;  // AutoMapper nesnesini CommonTestFixture'den al�r.
        }

        // Zaten varolan bir kitap ba�l��� ile yeni kitap olu�turma giri�i yap�ld���nda InvalidOperationException hatas� �retilmesi gereken senaryoyu test eden bir fakt metodu.
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
                .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatas� olmal�d�r.
                .And.Message.Should()
                .Be("Book already exists");  // Hatan�n i�eri�i kontrol edilir ve "Book already exists" olmal�d�r.
        }

        // Ge�erli giri� verileri ile yeni kitap olu�turuldu�unu ve veritaban�nda kaydedildi�ini do�rulayan fakt metodu.
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

            FluentActions.Invoking(() => command.Handle()).Invoke();  // Kitap olu�turma i�lemi �a�r�l�r.

            var book = _context.Books.SingleOrDefault(book => book.Title == model.Title);  // Olu�turulan kitap veritaban�nda aran�r.
            book.Should().NotBeNull();  // Kitab�n bo� olmamas� gerekti�i do�rulan�r.
            book.PageCount.Should().Be(model.PageCount);  // Sayfa say�s� do�rulan�r.
            book.PublishDate.Should().Be(model.PublishDate);  // Yay�n tarihi do�rulan�r.
            book.GenreId.Should().Be(model.GenreId);  // T�r kimli�i do�rulan�r.
            book.AuthorId.Should().Be(model.AuthorId);  // Yazar kimli�i do�rulan�r.
        }
    }
}
