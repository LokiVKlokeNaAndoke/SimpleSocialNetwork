using System;
using System.ComponentModel.Design;
using System.IO;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    public class ServicesHelper : IDisposable
    {
        private SqliteConnection _connection;
        private DbContextOptions<SocialDbContext> _options;

        public SocialDbContext Context { get; private set; }

        public ServicesHelper()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<SocialDbContext>()
                .UseLazyLoadingProxies()
                .UseSqlite(_connection)
                .Options;

            Context = new SocialDbContext(_options);

            Context.Database.EnsureCreated();

            // File.WriteAllText("/tmp/create_script.sql",Context.Database.GenerateCreateScript());
        }

        /// <summary>
        /// When adding to db context without explicitly calling CreateProxy()
        /// proxies aren't created even when Find()'ing entities
        /// so reloading is required
        /// </summary>
        public void ReloadContext()
        {
            Context.SaveChanges();
            Context.Dispose();
            Context = new SocialDbContext(_options);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public async Task SeedUsers()
        {
            await Context.Users.AddAsync(new ApplicationUser
            {
                Login = "petya",
                IsAdmin = true,
            });

            await Context.Users.AddAsync(new ApplicationUser
            {
                Login = "vasya",
                About = "vaysa stuff asdasdasdsd"
            });

            await Context.Users.AddAsync(new ApplicationUser
            {
                Login = "george",
            });

            await Context.Users.AddAsync(new ApplicationUser
            {
                Login = "steve",
                About = "steve stuff asdasdasdsd",
                IsDeleted = true,
            });
            await Context.SaveChangesAsync();
            ReloadContext();
        }
    }
}