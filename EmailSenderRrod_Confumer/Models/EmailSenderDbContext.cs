using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailSenderRrod_Confumer.Models
{
    class EmailSenderDbContext : DbContext
    {
        public DbSet<Mail> Mails { get; set; }

        public EmailSenderDbContext(DbContextOptions<EmailSenderDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
