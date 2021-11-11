using JYTGameStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public ApplicationDbContext()
        {
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<FavoriteCategory> FavoriteCategory { get; set; }
        public DbSet<FavoritePlatform> FavoritePlatform { get; set; }
        public DbSet<FriendsAndFamily> FriendsAndFamily { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Platform> Platform { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<GameRatings> GameRatings { get; set; }
        public DbSet<GameReview> GameReview { get; set; }
        public DbSet<RegisterEvent> RegisterEvents { get; set; }
        public DbSet<CreditCard> CreditCard { get; set; }
    }
}
