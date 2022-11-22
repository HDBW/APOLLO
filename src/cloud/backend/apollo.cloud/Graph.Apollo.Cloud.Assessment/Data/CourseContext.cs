using Invite.Apollo.App.Graph.Assessment.Models;
using Invite.Apollo.App.Graph.Assessment.Models.Course;
using Invite.Apollo.App.Graph.Common.Models.Course;
using Microsoft.EntityFrameworkCore;

namespace Invite.Apollo.App.Graph.Assessment.Data
{
    public class CourseContext : DbContext
    {
        #region DBSets
        public DbSet<Course> Courses { get; set; }
        public DbSet<Appointment> CourseAppointments { get; set; }
        public DbSet<Contact> CourseContacts { get; set; }

        #endregion

        private readonly ILoggerFactory _loggerFactory;

        public CourseContext(DbContextOptions<AssessmentContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Course
            builder.Entity<Course>()
                .HasKey(t => new { t.Id });
            builder.Entity<Course>()
                .HasIndex(t => new { t.ExternalId}).IsUnique();

            //CourseAppointments
            builder.Entity<Appointment>()
                .HasKey(t => new { t.Id });
            builder.Entity<Appointment>()
                .HasIndex(t => new { t.Schema }).IsUnique();
            //builder.Entity<Appointment>()
            //    .HasIndex(t => new { t.ExternalId }).IsUnique();
            builder.Entity<Appointment>()
                .HasOne(q => q.Course)
                .WithMany(a => a.Appointments) 
                .HasForeignKey(q => q.CourseId);

            //builder.Entity<Contact>()
            //    .HasOne(q => q.Course)
            //    .WithMany(a => a.Questions) //used to be AssessmentQuestions
            //    .HasForeignKey(q => q.CourseId);



            //Contact
            builder.Entity<Contact>()
                .HasKey(t => new { t.Id });
            builder.Entity<Contact>()
                .HasIndex(t => new { t.Schema }).IsUnique();

            builder.Entity<CourseHasContacts>().HasIndex(
                t => new { t.CourseId, t.ContactId }).IsUnique();

            builder.Entity<CourseHasContacts>()
                .HasOne(pt => pt.Course)
                .WithMany(p => p.CourseContacts)
                .HasForeignKey(pt => pt.CourseId);

            builder.Entity<CourseHasContacts>()
                .HasOne(pt => pt.Contact)
                .WithMany(p => p.CourseContacts)
                .HasForeignKey(pt => pt.ContactId);
            
            base.OnModelCreating(builder);
            // Add your customizations after calling base.OnModelCreating(builder);
        }

    }
}
