﻿using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ReadingCorp;

namespace ReadingCorp
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}