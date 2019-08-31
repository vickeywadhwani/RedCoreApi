using System;
using System.Data.Entity;
using RedCoreApi.Models;

namespace RedCoreUnitTest
{
    public class TestRedCoreContext : ISRedCoreContext
    {
        public TestRedCoreContext()
        {
            this.user = new TestUserDbSet();
        }

        public DbSet<user> user { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(user item) { }
        public void Dispose() { }
    }
}
