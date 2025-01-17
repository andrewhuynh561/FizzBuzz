﻿using FizzBuzz.Models;
using Microsoft.EntityFrameworkCore;

namespace FizzBuzz.Data
{
    public class FizzBuzzDbContext : DbContext
    {

        public FizzBuzzDbContext(DbContextOptions<FizzBuzzDbContext> options)
            : base(options)
        {
        }

        public DbSet<Games> Games { get; set; }
    }
}
