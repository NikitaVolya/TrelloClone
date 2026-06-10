
using Domain.Boards;
using Domain.Common;
using Domain.Projects;
using Domain.Tasks;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DAL.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Board> Boards => Set<Board>();
        public DbSet<Column> Columns => Set<Column>();
        public DbSet<Invitation> Invitations => Set<Invitation>();
        public DbSet<Domain.Tasks.Task> Tasks => Set<Domain.Tasks.Task>();
        public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
        public DbSet<TaskAssignee> TaskAssignees => Set<TaskAssignee>();
        public DbSet<TaskComment> TaskComments => Set<TaskComment>();


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // one ApplicationUser with many Project 
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Projects)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            // one Project with many Board
            builder.Entity<Project>()
                .HasMany(p => p.Boards)
                .WithOne(b => b.Project)
                .HasForeignKey(b => b.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // one Board with many Column
            builder.Entity<Board>()
                .HasMany(b => b.Columns)
                .WithOne(c => c.Board)
                .HasForeignKey(c => c.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            // one Column with many Task
            // one Task with one or zero Column
            builder.Entity<Column>()
                .HasMany(c => c.Tasks)
                .WithOne(t => t.Column)
                .HasForeignKey(t => t.ColumnId)
                .OnDelete(DeleteBehavior.Restrict);

            // one Board with many Task
            builder.Entity<Board>()
                .HasMany(b => b.Tasks)
                .WithOne(t => t.Board)
                .HasForeignKey(t => t.BoardId)
                .OnDelete(DeleteBehavior.Cascade);


            // ============== START Inivation ===================== 
            // many ApplicationUser to many Project

            // one Project with many Invitation
            builder.Entity<Project>()
                .HasMany(p => p.Invitations)
                .WithOne(inv => inv.Project)
                .HasForeignKey(inv => inv.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // one ApplicationUser with many Invitation
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Invitations)
                .WithOne(inv => inv.User)
                .HasForeignKey(inv => inv.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Invitation>()
                .HasKey(pm => new
                {
                    pm.ProjectId,
                    pm.UserId
                });

            // ============== END   Inivation ===================== 



            // ============== START ProjectMember ===================== 
            // many ApplicationUser to many Project

            // one Project with many ProjectMember
            builder.Entity<Project>()
                .HasMany(p => p.Members)
                .WithOne(pm => pm.Project)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // one ApplicationUser with many ProjectMember
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.ProjectMembers)
                .WithOne(pm => pm.Member)
                .HasForeignKey(pm => pm.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProjectMember>()
                .HasKey(pm => new
                {
                    pm.ProjectId,
                    pm.MemberId
                });

            // ============== END   ProjectMember ===================== 


            // ============== START TaskComment ===================== 
            // many ApplicationUser to many Task

            // one Project with many TaskComment
            builder.Entity<Domain.Tasks.Task>()
                .HasMany(t => t.Comments)
                .WithOne(tc => tc.Task)
                .HasForeignKey(tc => tc.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // one ApplicationUser with many TaskComment
            // On Delete ApplicationUser set null in TaskComment.SenderId
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Comments)
                .WithOne(tc => tc.Sender)
                .HasForeignKey(tc => tc.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // ============== END   TaskComment ===================== 


            // ============== START TaskAssigne ===================== 
            // many ApplicationUser to many Task

            // one Project with many TaskComment
            builder.Entity<Domain.Tasks.Task>()
                .HasMany(t => t.Assignees)
                .WithOne(ta => ta.Task)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // one ApplicationUser with many TaskComment
            // On Delete ApplicationUser set null in TaskComment.SenderId
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Assignees)
                .WithOne(ta => ta.User)
                .HasForeignKey(ta => ta.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TaskAssignee>()
                .HasKey(pm => new
                {
                    pm.TaskId,
                    pm.UserId
                });

            // ============== END   TaskAssigne ===================== 
        }

    }
}
