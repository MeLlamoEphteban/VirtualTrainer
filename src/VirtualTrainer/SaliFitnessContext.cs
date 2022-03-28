using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace VirtualTrainer
{
    public partial class SaliFitnessContext : DbContext
    {
        public SaliFitnessContext()
        {
        }

        public SaliFitnessContext(DbContextOptions<SaliFitnessContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<BodyGroup> BodyGroups { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<EquipmentExercise> EquipmentExercises { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<GroupEquipment> GroupEquipments { get; set; }
        public virtual DbSet<ProgramType> ProgramTypes { get; set; }
        public virtual DbSet<ProgramUser> ProgramUsers { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<TypeGroup> TypeGroups { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }
        public virtual DbSet<UsersExercise> UsersExercises { get; set; }
        public virtual DbSet<WorkProgram> WorkPrograms { get; set; }
        public virtual DbSet<WorkType> WorkTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=SaliFitness;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<BodyGroup>(entity =>
            {
                entity.HasKey(e => e.IdbodyGroup);

                entity.ToTable("BodyGroup");

                entity.Property(e => e.IdbodyGroup).HasColumnName("IDBodyGroup");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasKey(e => e.Idequipment);

                entity.Property(e => e.Idequipment).HasColumnName("IDEquipment");

                entity.Property(e => e.Details).HasMaxLength(100);

                entity.Property(e => e.EquipmentName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<EquipmentExercise>(entity =>
            {
                entity.ToTable("Equipment_Exercises");

                entity.HasIndex(e => e.Idequipment, "IX_Equipment_Exercises_IDEquipment");

                entity.HasIndex(e => e.Idexercise, "IX_Equipment_Exercises_IDExercise");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idequipment).HasColumnName("IDEquipment");

                entity.Property(e => e.Idexercise).HasColumnName("IDExercise");

                entity.HasOne(d => d.IdequipmentNavigation)
                    .WithMany(p => p.EquipmentExercises)
                    .HasForeignKey(d => d.Idequipment)
                    .HasConstraintName("FK_Equipment_Exercises_Equipment");

                entity.HasOne(d => d.IdexerciseNavigation)
                    .WithMany(p => p.EquipmentExercises)
                    .HasForeignKey(d => d.Idexercise)
                    .HasConstraintName("FK_Equipment_Exercises_Exercises");
            });

            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasKey(e => e.Idexercise);

                entity.Property(e => e.Idexercise).HasColumnName("IDExercise");

                entity.Property(e => e.ExerciseName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Instructions).HasMaxLength(50);

                entity.Property(e => e.Reps).HasDefaultValueSql("((12))");

                entity.Property(e => e.Sets).HasDefaultValueSql("((3))");

                entity.Property(e => e.Weight).HasDefaultValueSql("((5))");
            });

            modelBuilder.Entity<GroupEquipment>(entity =>
            {
                entity.ToTable("Group_Equipment");

                entity.HasIndex(e => e.IdbodyGroup, "IX_Group_Equipment_IDBodyGroup");

                entity.HasIndex(e => e.Idequipment, "IX_Group_Equipment_IDEquipment");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdbodyGroup).HasColumnName("IDBodyGroup");

                entity.Property(e => e.Idequipment).HasColumnName("IDEquipment");

                entity.HasOne(d => d.IdbodyGroupNavigation)
                    .WithMany(p => p.GroupEquipments)
                    .HasForeignKey(d => d.IdbodyGroup)
                    .HasConstraintName("FK_Group_Equipment_BodyGroup");

                entity.HasOne(d => d.IdequipmentNavigation)
                    .WithMany(p => p.GroupEquipments)
                    .HasForeignKey(d => d.Idequipment)
                    .HasConstraintName("FK_Group_Equipment_Equipment");
            });

            modelBuilder.Entity<ProgramType>(entity =>
            {
                entity.HasKey(e => e.IdprogramType);

                entity.ToTable("ProgramType");

                entity.Property(e => e.IdprogramType).HasColumnName("IDProgramType");

                entity.Property(e => e.ProgramTypeName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ProgramUser>(entity =>
            {
                entity.ToTable("Program_Users");

                entity.HasIndex(e => e.Iduser, "IX_Program_Users_IDUser");

                entity.HasIndex(e => e.IdworkProgram, "IX_Program_Users_IDWorkProgram");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.Property(e => e.IdworkProgram).HasColumnName("IDWorkProgram");

                entity.HasOne(d => d.IduserNavigation)
                    .WithMany(p => p.ProgramUsers)
                    .HasForeignKey(d => d.Iduser)
                    .HasConstraintName("FK_Program_Users_Users");

                entity.HasOne(d => d.IdworkProgramNavigation)
                    .WithMany(p => p.ProgramUsers)
                    .HasForeignKey(d => d.IdworkProgram)
                    .HasConstraintName("FK_Program_Users_WorkProgram");
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(e => e.Idsubscription);

                entity.Property(e => e.Idsubscription).HasColumnName("IDSubscription");

                entity.Property(e => e.AllowedTimeInterval)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SubName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TypeGroup>(entity =>
            {
                entity.ToTable("Type_Group");

                entity.HasIndex(e => e.IdbodyGroup, "IX_Type_Group_IDBodyGroup");

                entity.HasIndex(e => e.IdprogramType, "IX_Type_Group_IDProgramType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdbodyGroup).HasColumnName("IDBodyGroup");

                entity.Property(e => e.IdprogramType).HasColumnName("IDProgramType");

                entity.HasOne(d => d.IdbodyGroupNavigation)
                    .WithMany(p => p.TypeGroups)
                    .HasForeignKey(d => d.IdbodyGroup)
                    .HasConstraintName("FK_Type_Group_BodyGroup");

                entity.HasOne(d => d.IdprogramTypeNavigation)
                    .WithMany(p => p.TypeGroups)
                    .HasForeignKey(d => d.IdprogramType)
                    .HasConstraintName("FK_Type_Group_ProgramType");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Iduser);

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Cnp)
                    .IsRequired()
                    .HasMaxLength(13)
                    .HasColumnName("CNP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserSubscription>(entity =>
            {
                entity.ToTable("User_Subscription");

                entity.HasIndex(e => e.Idsubscription, "IX_User_Subscription_IDSubscription");

                entity.HasIndex(e => e.Iduser, "IX_User_Subscription_IDUser")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.Idsubscription).HasColumnName("IDSubscription");

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.IdsubscriptionNavigation)
                    .WithMany(p => p.UserSubscriptions)
                    .HasForeignKey(d => d.Idsubscription)
                    .HasConstraintName("FK_User_Subscription_Subscriptions");

                entity.HasOne(d => d.IduserNavigation)
                    .WithOne(p => p.UserSubscription)
                    .HasForeignKey<UserSubscription>(d => d.Iduser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Subscription_Users");
            });

            modelBuilder.Entity<UsersExercise>(entity =>
            {
                entity.ToTable("Users_Exercises");

                entity.HasIndex(e => e.Idexercise, "IX_Users_Exercises_IDExercise");

                entity.HasIndex(e => e.Iduser, "IX_Users_Exercises_IDUser");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idexercise).HasColumnName("IDExercise");

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.HasOne(d => d.IdexerciseNavigation)
                    .WithMany(p => p.UsersExercises)
                    .HasForeignKey(d => d.Idexercise)
                    .HasConstraintName("FK_Users_Exercises_Exercises");

                entity.HasOne(d => d.IduserNavigation)
                    .WithMany(p => p.UsersExercises)
                    .HasForeignKey(d => d.Iduser)
                    .HasConstraintName("FK_Users_Exercises_Users");
            });

            modelBuilder.Entity<WorkProgram>(entity =>
            {
                entity.HasKey(e => e.IdworkProgram);

                entity.ToTable("WorkProgram");

                entity.Property(e => e.IdworkProgram).HasColumnName("IDWorkProgram");

                entity.Property(e => e.ProgramName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<WorkType>(entity =>
            {
                entity.ToTable("Work_Type");

                entity.HasIndex(e => e.IdprogramType, "IX_Work_Type_IDProgramType");

                entity.HasIndex(e => e.IdworkProgram, "IX_Work_Type_IDWorkProgram");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdprogramType).HasColumnName("IDProgramType");

                entity.Property(e => e.IdworkProgram).HasColumnName("IDWorkProgram");

                entity.HasOne(d => d.IdprogramTypeNavigation)
                    .WithMany(p => p.WorkTypes)
                    .HasForeignKey(d => d.IdprogramType)
                    .HasConstraintName("FK_Work_Type_ProgramType");

                entity.HasOne(d => d.IdworkProgramNavigation)
                    .WithMany(p => p.WorkTypes)
                    .HasForeignKey(d => d.IdworkProgram)
                    .HasConstraintName("FK_Work_Type_WorkProgram");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
