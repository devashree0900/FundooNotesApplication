using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class FundooDBContext: DbContext
    {
        public FundooDBContext(DbContextOptions options) : base(options) { }

        //UserEntity- here in code, UserTable- Table name in DB
        public DbSet<UserEntity> UserTable { get; set; } //We are creating atable called UserTable in DB with columns as UserEntity
        public DbSet<NoteEntity> NoteTable { get; set; }

        public DbSet<LabelEntity> LabelTable { get; set; }
        public DbSet<CollabEntity> CollabTable { get; set; }
    }
}
