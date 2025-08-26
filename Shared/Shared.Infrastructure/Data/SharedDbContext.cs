using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Entities;

namespace Shared.Infrastructure.Data;

public partial class SharedDbContext : DbContext
{
    public SharedDbContext(DbContextOptions<SharedDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OViewActivite> OViewActivites { get; set; }

    public virtual DbSet<OViewLivrablesDuProjet> OViewLivrablesDuProjets { get; set; }

    public virtual DbSet<ViewActivitesIformationsFinanciere> ViewActivitesIformationsFinancieres { get; set; }

    public virtual DbSet<ViewIdentProjetLivrablesPlat> ViewIdentProjetLivrablesPlats { get; set; }

    public virtual DbSet<ViewIdentificationProjetPlat> ViewIdentificationProjetPlats { get; set; }

    public virtual DbSet<ViewNotificationPlat> ViewNotificationPlats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("JOSEPHRUDY")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<OViewActivite>(entity =>
        {
            entity.ToView("O_VIEW_ACTIVITES");
        });

        modelBuilder.Entity<OViewLivrablesDuProjet>(entity =>
        {
            entity.ToView("O_VIEW_LIVRABLES_DU_PROJET");
        });

        modelBuilder.Entity<ViewActivitesIformationsFinanciere>(entity =>
        {
            entity.ToView("VIEW_ACTIVITES_IFORMATIONS_FINANCIERES");
        });

        modelBuilder.Entity<ViewIdentProjetLivrablesPlat>(entity =>
        {
            entity.ToView("VIEW_IDENT_PROJET_LIVRABLES_PLAT");
        });

        modelBuilder.Entity<ViewIdentificationProjetPlat>(entity =>
        {
            entity.ToView("VIEW_IDENTIFICATION_PROJET_PLAT");
        });

        modelBuilder.Entity<ViewNotificationPlat>(entity =>
        {
            entity.ToView("VIEW_NOTIFICATION_PLAT");

            entity.Property(e => e.EstLuChar).IsFixedLength();
        });
        modelBuilder.HasSequence("NOTIFICATION_SEQ");
        modelBuilder.HasSequence("SEQ_ACTIVITES");
        modelBuilder.HasSequence("SEQ_ACTIVITES_ANNUELLES");
        modelBuilder.HasSequence("SEQ_ASPECTS_JURIDIQUES");
        modelBuilder.HasSequence("SEQ_BAILLEURS_DE_FONDS");
        modelBuilder.HasSequence("SEQ_COUT_ANNUEL_DU_PROJET");
        modelBuilder.HasSequence("SEQ_COUT_TOTAL_PROJET");
        modelBuilder.HasSequence("SEQ_EFFETS_DU_PROJET");
        modelBuilder.HasSequence("SEQ_IMPACTS_DU_PROJET");
        modelBuilder.HasSequence("SEQ_INDICATEURS_DE_RESULTATS");
        modelBuilder.HasSequence("SEQ_LIVRABLES_DU_PROJET");
        modelBuilder.HasSequence("SEQ_OBJECTIFS_SPECIFIQUES");
        modelBuilder.HasSequence("SEQ_PARTIES_PRENANTES");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
