using Microsoft.EntityFrameworkCore;
using WebAPIDBLayer.Models;

namespace WebAPIDBLayer
{
    public class DbContextForRelation : DbContext
    {
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbContextForRelation() 
        {

        }
        public DbContextForRelation(DbContextOptions options) : base(options) 
        { 
        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = DESKTOP-0P56O06; Initial Catalog = WebAPITeacherAndClassroom; Integrated Security = True");
        }


    }
}