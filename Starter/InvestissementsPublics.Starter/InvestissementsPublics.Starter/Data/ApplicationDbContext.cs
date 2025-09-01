using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.ApplicationUsers;
using InvestissementsPublics.Starter.ApplicationUsers;
using Microsoft.AspNetCore.Identity;
using Shared.Domain.Authorization;

namespace InvestissementsPublics.Starter.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Privilege> Privileges { get; set; }
            public DbSet<RolePrivilege> RolePrivileges { get; set; }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

                // === 0) conversion bool -> NUMBER(1) (ta logique existante) ===
                foreach (var entityType in builder.Model.GetEntityTypes())
                {
                    var boolProps = entityType.ClrType
                        .GetProperties()
                        .Where(p => p.PropertyType == typeof(bool)
                                 || p.PropertyType == typeof(bool?));

                    foreach (var prop in boolProps)
                    {
                        builder.Entity(entityType.ClrType)
                               .Property(prop.Name)
                               .HasColumnType("NUMBER(1)");
                    }
                }

                // === 1) Privilege table ===
                builder.Entity<Privilege>(b =>
                {
                    b.ToTable("Privileges");
                    b.HasKey(p => p.Id);
                    // Oracle : Id numeric identity
                    b.Property(p => p.Id)
                     .HasColumnType("NUMBER(10)")
                     .ValueGeneratedOnAdd()
                     .HasAnnotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

                    b.Property(p => p.Name)
                     .HasColumnType("NVARCHAR2(200)")
                     .HasMaxLength(200)
                     .IsRequired();

                    b.Property(p => p.Description)
                     .HasColumnType("NVARCHAR2(500)")
                     .HasMaxLength(500);
                });

                // === 2) RolePrivilege (many-to-many role <-> privilege) ===
                builder.Entity<RolePrivilege>(b =>
                {
                    b.ToTable("RolePrivileges");
                    b.HasKey(rp => new { rp.RoleId, rp.PrivilegeId });

                    b.Property(rp => rp.RoleId)
                     .HasColumnType("NVARCHAR2(450)")
                     .IsRequired();

                    b.Property(rp => rp.PrivilegeId)
                     .HasColumnType("NUMBER(10)")
                     .IsRequired();

                    // FK -> AspNetRoles (IdentityRole)
                    b.HasOne<IdentityRole>(rp => rp.Role)
                     .WithMany() // IdentityRole does not have collection mapped here
                     .HasForeignKey(rp => rp.RoleId)
                     .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne(rp => rp.Privilege)
                     .WithMany(p => p.RolePrivileges)
                     .HasForeignKey(rp => rp.PrivilegeId)
                     .OnDelete(DeleteBehavior.Cascade);
                });

                // === 3) tes autres configurations existantes ===
                // builder.Entity<…>().ToView("…");
                // …

                // appel partial pour extension si tu en as besoin
                OnModelCreatingPartial(builder);
            }

            partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        }
}
