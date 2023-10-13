using AutoMapper;
using FluentAssertions;
using webapi.Applications.GenreOperations.Commands.UpdateGenre;
using webapi.DBOperations;
using webapi.Entities;
using webapi.UnitTests.TestSetup;
using Xunit;

namespace webapi.UnitTests.Applications.GenreOperations.Commands.UpdateGenre
{
    public class UpdateGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;  // Veritabaný baðlamýný temsil eden deðiþken.

        // Test sýnýfýnýn yapýsýný ayarlamak için CommonTestFixture sýnýfýný kullanarak sýnýfýn kurucusu.
        public UpdateGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;  // Veritabaný baðlamýný CommonTestFixture'den alýr.
        }

        // Verilen GenreId'nin veritabanýnda bulunmadýðý senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenGivenGenreIdIsNotinDB_InvalidOperationsException_ShouldBeReturn()
        {
            // UpdateGenreCommand sýnýfýnýn bir örneði oluþturulur ve gereken baðlam/enjeksiyonlar enjekte edilir.
            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = 0;  // Geçersiz bir GenreId ayarlanýr.

            // FluentActions kullanýlarak, bir iþlemi tetiklemek ve ardýndan hata durumunu test etmek için Invoking() yöntemi kullanýlýr.
            FluentActions
                .Invoking(() => command.Handle())  // Handle() yöntemi, Genre'ü güncellemeye çalýþýr.
                .Should()
                .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatasý olmalýdýr.
                .And.Message.Should()
                .Be("Genre is not found!");  // Hatanýn içeriði kontrol edilir ve "Genre is not found!" olmalýdýr.
        }

        // Verilen adýn veritabanýnda baþka bir tür ile ayný olduðu senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenGivenNameIsSameWithAnotherGenre_InvalidOperationException_ShouldBeReturn()
        {
            // Veritabanýna yeni bir tür eklenir.
            var genre = new Genre() { Name = "Science Fiction" };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            // UpdateGenreCommand sýnýfýnýn bir örneði oluþturulur ve gereken baðlam/enjeksiyonlar enjekte edilir.
            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = 2;
            command.Model = new UpdateGenreModel() { Name = "Science Fiction" };  // Mevcut bir türle ayný adý içeren bir tür eklenir.

            // FluentActions kullanýlarak, bir iþlemi tetiklemek ve ardýndan hata durumunu test etmek için Invoking() yöntemi kullanýlýr.
            FluentActions
                .Invoking(() => command.Handle())  // Handle() yöntemi, Genre'ü güncellemeye çalýþýr.
                .Should()
                .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatasý olmalýdýr.
                .And.Message.Should()
                .Be("Genre is already exist!");  // Hatanýn içeriði kontrol edilir ve "Genre is already exist!" olmalýdýr.
        }

        // Geçerli bir GenreId ile çaðrýldýðýnda Genre'ü güncellemesi gereken senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenGivenGenreIdinDB_ShouldBeUpdate()
        {
            // UpdateGenreCommand sýnýfýnýn bir örneði oluþturulur ve gereken baðlam/enjeksiyonlar enjekte edilir.
            UpdateGenreCommand command = new UpdateGenreCommand(_context);

            command.Model = new UpdateGenreModel() { Name = "WhenGivenGenreIdinDB_ShouldBeUpdate" };
            command.GenreId = 1;  // Varolan bir Genre'ü güncellemek için geçerli bir GenreId ayarlanýr.

            FluentActions.Invoking(() => command.Handle()).Invoke();

            // GenreId ile eþleþen bir türün varlýðýný kontrol eder.
            var genre = _context.Genres.SingleOrDefault(genre => genre.Id == command.GenreId);
            genre.Should().NotBeNull();  // Genre deðiþkeninin null olmamasý gerektiðini doðrular.
        }
    }
}
