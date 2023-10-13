using FluentAssertions;
using webapi.Applications.GenreOperations.Commands.CreateGenre;
using webapi.DBOperations;
using webapi.Entities;
using webapi.UnitTests.TestSetup;

namespace webapi.UnitTests.Applications.GenreOperations.Commands.CreateGenre
{
    public class CreateGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;  // Veritabaný baðlamýný temsil eden deðiþken.

        // Test sýnýfýnýn yapýsýný ayarlamak için CommonTestFixture sýnýfýný kullanarak sýnýfýn kurucusu.
        public CreateGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;  // Veritabaný baðlamýný CommonTestFixture'den alýr.
        }

        // Ayný ad ile bir türün veritabanýnda zaten mevcut olduðu senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenAlreadyExitGenreTitleIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Veritabanýna yeni bir tür eklenir.
            var genre = new Genre()
            {
                Name = "WhenAlreadyExitGenreTitleIsGiven_InvalidOperationException_ShouldBeReturn"
            };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            // CreateGenreCommand sýnýfýnýn bir örneði oluþturulur ve gereken baðlam/enjeksiyonlar enjekte edilir.
            CreateGenreCommand command = new CreateGenreCommand(_context);
            command.Model = new CreateGenreModel() { Name = genre.Name };  // Ayný ad ile bir tür eklemeye çalýþýlýr.

            // FluentActions kullanýlarak, bir iþlemi tetiklemek ve ardýndan hata durumunu test etmek için Invoking() yöntemi kullanýlýr.
            FluentActions
                .Invoking(() => command.Handle())  // Handle() yöntemi, Genre eklemeye çalýþýr.
                .Should()
                .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatasý olmalýdýr.
                .And.Message.Should()
                .Be("Genre is already exist!");  // Hatanýn içeriði kontrol edilir ve "Genre is already exist!" olmalýdýr.
        }

        // Geçerli giriþlerle bir türün baþarýyla oluþturulduðu senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenValidInputsAreaGiven_Genre_shouldBeCreated()
        {
            // CreateGenreCommand sýnýfýnýn bir örneði oluþturulur ve gereken baðlam/enjeksiyonlar enjekte edilir.
            CreateGenreCommand command = new CreateGenreCommand(_context);
            command.Model = new CreateGenreModel()
            {
                Name = "WhenValidInputIsGiven_ShouldBeCreated"
            };

            // FluentActions kullanýlarak, bir iþlemi tetiklemek ve iþlemi gerçekleþtirmek için Invoke() yöntemi kullanýlýr.
            FluentActions.Invoking(() => command.Handle()).Invoke();

            // Eklenen türü veritabanýnda arar ve bulunup bulunmadýðýný kontrol eder.
            var genre = _context.Genres.SingleOrDefault(genre => genre.Name == command.Model.Name);
            genre.Should().NotBeNull();  // Genre deðiþkeninin null olmamasý gerektiðini doðrular.
        }
    }
}
