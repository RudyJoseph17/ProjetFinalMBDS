using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableauxDeBord.Infrastructure.Data
{
    public partial class ProjetDbContext : DbContext
    {
        public ProjetDbContext()
        {
        }

        public ProjetDbContext(DbContextOptions<ProjetDbContext> options)
            : base(options)
        {
        }

     //  public virtual DbSet<OViewCoutAnnuelProjetPlat> OViewCoutAnnuelProjetPlats { get; set; }

        /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
              => optionsBuilder.UseOracle("User Id=JOSEPHRUDY;Password=2025RUDY001;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=DIP_SS_1)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=banqueprojet1)))");
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDefaultSchema("JOSEPHRUDY")
                .UseCollation("USING_NLS_COMP");

            //modelBuilder.Entity<OViewCoutAnnuelProjetPlat>(entity =>
            //{
            //    entity.ToView("O_VIEW_COUT_ANNUEL_PROJET_PLAT");
            //});

            //OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
