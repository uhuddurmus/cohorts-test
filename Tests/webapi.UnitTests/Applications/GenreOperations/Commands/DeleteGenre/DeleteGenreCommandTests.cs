using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using webapi.Applications.GenreOperations.Commands.DeleteGenre;
using webapi.DBOperations;
using webapi.UnitTests.TestSetup;
using Xunit;

namespace webapi.UnitTests.Applications.GenreOperations.Commands.DeleteGenre
{
    public class DeleteGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;  // Veritabaný baðlamýný temsil eden deðiþken.

        // Test sýnýfýnýn yapýsýný ayarlamak için CommonTestFixture sýnýfýný kullanarak sýnýfýn kurucusu.
        public DeleteGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;  // Veritabaný baðlamýný CommonTestFixture'den alýr.
        }

        // Verilen GenreId'nin veritabanýnda bulunmadýðý senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenGivenGenreIdIsNotInDB_InvalidOperationsException_ShouldBeReturn()
        {
            // DeleteGenreCommand sýnýfýnýn bir örneði oluþturulur ve gereken baðlam/enjeksiyonlar enjekte edilir.
            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            command.GenreId = 0;  // Geçersiz bir GenreId ayarlanýr.

            // FluentActions kullanýlarak, bir iþlemi tetiklemek ve ardýndan hata durumunu test etmek için Invoking() yöntemi kullanýlýr.
            FluentActions
                .Invoking(() => command.Handle())  // Handle() yöntemi, Genre'ü silmeye çalýþýr.
                .Should()
                .Throw<InvalidOperationException>()  // Beklenen bir InvalidOperationException hatasý olmalýdýr.
                .And.Message.Should()
                .Be("Genre is not found!");  // Hatanýn içeriði kontrol edilir ve "Genre is not found!" olmalýdýr.
        }

        // Verilen GenreId'nin veritabanýnda bulunduðu senaryoyu test eden bir fakt metodu.
        [Fact]
        public void WhenGivenGenreIdIsNotInDB_ShouldBeRemove()
        {
            // DeleteGenreCommand sýnýfýnýn bir örneði oluþturulur ve gereken baðlam/enjeksiyonlar enjekte edilir.
            DeleteGenreCommand command = new DeleteGenreCommand(_context);
            command.GenreId = 1;  // Varolan bir Genre'ü silmek için geçerli bir GenreId ayarlanýr.

            FluentActions.Invoking(() => command.Handle()).Invoke();

            // GenreId ile eþleþen bir türün hala varlýðýný kontrol eder.
            var genre = _context.Genres.SingleOrDefault(genre => genre.Id == command.GenreId);
            genre.Should().Be(null);  // Genre deðiþkeninin null olmasý gerektiðini doðrular.
        }
    }
}
