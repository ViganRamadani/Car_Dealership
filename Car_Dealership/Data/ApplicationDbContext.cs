using Car_Dealership.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Car_Dealership.Models;

namespace Car_Dealership.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)

        {
        }
        public DbSet<Car_Dealership.Models.News> News { get; set; }


        public DbSet<Post> Posts { get; set; }
        public DbSet<UserPostLike> UserPostLikes { get; set; }

    }
}
