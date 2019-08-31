using System;
using System.Data.Entity;

namespace RedCoreApi.Models
{
    public interface ISRedCoreContext : IDisposable
    {
        DbSet<user> user { get; }
        int SaveChanges();
        void MarkAsModified(user item);
    }
}

