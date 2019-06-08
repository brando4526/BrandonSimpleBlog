using BrandonSimpleBlog.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BrandonSimpleBlog.Test.TestFixtures
{
    public class SqlLiteTestFixture : IDisposable
    {
        public ApplicationDbContext Context => SqlLiteInMemoryContext();

        public void Dispose()
        {
            Context?.Dispose();
        }

        private ApplicationDbContext SqlLiteInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            return context;
        }
    }
}
