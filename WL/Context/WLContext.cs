using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using WL.Model;

namespace WL.Context
{
    public class WLContext : DbContext
    {

        //private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=EFCore;Trusted_Connection=True;";
        private const string connectionString = "@Server=./SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;";

        public WLContext() {}

        public WLContext(DbContextOptions<WLContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public WLContext CreateDbContext(string[] args)//
        {
            var optionsBuilder = new DbContextOptionsBuilder<WLContext>();
            optionsBuilder.UseSqlite("Data Source=WL.db");
            //optionsBuilder.UseSqlServer(connectionString);

            return new WLContext(optionsBuilder.Options);
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=WL.db");
            //optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CardDeck>()
                .HasKey(cd => new { cd.CardId, cd.DeckId });

            modelBuilder.Entity<CardDeck>()
                .HasOne(cd => cd.Card)
                .WithMany(cd => cd.Decks)
                .HasForeignKey(cd => cd.CardId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CardDeck>()
                .HasOne(cd => cd.Deck)
                .WithMany(cd => cd.Cards)
                .HasForeignKey(cd => cd.DeckId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Card>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Cards)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<Category>()
                .HasMany(cat => cat.Cards)
                .WithOne(cat => cat.Category)
                .HasForeignKey(cat => cat.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Category>()
                .HasKey(c => new { c.Id });
        }
    }
}
