using webapi.DBOperations;
using webapi.Entities;

namespace webapi.UnitTests.TestSetup
{
    public static class Authors
    {
        public static void AddAuthors(this BookStoreDbContext context)
        {   //seed author
            context.Authors.AddRange(
                new Author
                {
                    FirstName = "Eric",
                    LastName = "Ries",
                    BirthDate = new DateTime(1978, 09, 04)
                },
                new Author
                {
                    FirstName = "Charlotte",
                    LastName = "Perkins Gilman",
                    BirthDate = new DateTime(1860, 07, 03)
                },
                new Author
                {
                    FirstName = "Frank",
                    LastName = "Herbert",
                    BirthDate = new DateTime(1920, 10, 08)
                }
            );
        }
    }
}
