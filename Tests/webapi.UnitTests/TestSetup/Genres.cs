using webapi.DBOperations;
using webapi.Entities;

namespace webapi.UnitTests.TestSetup
{
    public static class Genres
    {
        public static void AddGenres(this BookStoreDbContext context)
        {       //seed Genre
            context.Genres.AddRange(
                new Genre { Name = "Personal Growth", },
                new Genre { Name = "Science Fiction", },
                new Genre { Name = "Romance", }
            );
        }
    }
}
