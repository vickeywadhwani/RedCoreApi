namespace RedCoreApi.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class UserDBModel : DbContext, ISRedCoreContext
    {
        public UserDBModel()
            : base("name=RedCoreDBConn")
        {
        }

        public virtual DbSet<user> user { get; set; }

        /*public void MarkAsModified(user item)
        {
            throw new NotImplementedException();
        }*/

        public void MarkAsModified(user item)
        {
            Entry(item).State = EntityState.Modified;
        }
    

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<user>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.telephone)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.city)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.post_code)
                .IsUnicode(false);
        }
    }
}
