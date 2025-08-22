using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.ApplicationUsers;
using InvestissementsPublics.Starter.ApplicationUsers;

namespace InvestissementsPublics.Starter.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // === 1) Forcer tous les bool en NUMBER(1) pour Oracle ===
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

            // === 2) Vos autres configurations (vues, séquences…) ===
            // builder.Entity<…>().ToView("…");
            // …
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
