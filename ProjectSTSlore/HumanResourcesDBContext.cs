using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ProjectSTSlore
{
    public partial class HumanResourcesDBContext : DbContext
    {
        public DbSet<Group> Groups { set; get; }
        public DbSet<Student> Students { set; get; }
        public DbSet<Person> Persons { set; get; }

        public HumanResourcesDBContext(DbContextOptions<HumanResourcesDBContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            /*if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-7KCTEMM\\SQLEXPRESS;Database=EntityFrameworkDB;Trusted_Connection=True;");
            }*/
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher_Subject>().HasOne(x => x.teacher).WithMany().OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Teacher_Subject>().HasOne(x => x.subject).WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Group_TeacherSubject>().HasOne(x => x.teacherSubject).WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Group_TeacherSubject>().HasOne(x => x.group).WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Marks>().HasOne(x => x.subjectForMarks).WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Marks>().HasOne(x => x.student).WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Student>().HasOne(x => x.group).WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Student>().HasOne(x => x.person).WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Teacher>().HasOne(x => x.person).WithMany().OnDelete(DeleteBehavior.Cascade);
            OnModelCreatingPartial(modelBuilder);

            
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        //* устанавливаем фабрику логгера
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(new Logger());    //* указываем наш провайдер логгирования
            /*builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name
                        && level == LogLevel.Information)   //* adding constraints for logs(in this case only "Command" will be in log and with "Information" level of access)
                    .AddProvider(new MyLoggerProvider());*/
        });
    }
}
