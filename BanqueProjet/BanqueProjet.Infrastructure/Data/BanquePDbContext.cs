using System;
using System.Collections.Generic;
using BanqueProjet.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BanqueProjet.Infrastructure.Data;

public partial class BanquePDbContext : DbContext
{
    public BanquePDbContext(DbContextOptions<BanquePDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OViewActivite> OViewActivites { get; set; }

    public virtual DbSet<OViewAspectsJuridique> OViewAspectsJuridiques { get; set; }

    public virtual DbSet<OViewBailleursDeFond> OViewBailleursDeFonds { get; set; }

    public virtual DbSet<OViewCoutAnnuelDuProjet> OViewCoutAnnuelDuProjets { get; set; }

    public virtual DbSet<OViewEffetsDuProjet> OViewEffetsDuProjets { get; set; }

    public virtual DbSet<OViewImpactsDuProjet> OViewImpactsDuProjets { get; set; }

    public virtual DbSet<OViewIndicateursDeResultat> OViewIndicateursDeResultats { get; set; }

    public virtual DbSet<OViewLivrablesDuProjet> OViewLivrablesDuProjets { get; set; }

    public virtual DbSet<OViewPartiesPrenante> OViewPartiesPrenantes { get; set; }

    public virtual DbSet<ViewActivitesIformationsFinanciere> ViewActivitesIformationsFinancieres { get; set; }

    public virtual DbSet<ViewIdentProjetActivitesPlat> ViewIdentProjetActivitesPlats { get; set; }

    public virtual DbSet<ViewIdentProjetAspectsJuridiquesPlat> ViewIdentProjetAspectsJuridiquesPlats { get; set; }

    public virtual DbSet<ViewIdentProjetBailleursPlat> ViewIdentProjetBailleursPlats { get; set; }

    public virtual DbSet<ViewIdentProjetEffetsPlat> ViewIdentProjetEffetsPlats { get; set; }

    public virtual DbSet<ViewIdentProjetImpactsPlat> ViewIdentProjetImpactsPlats { get; set; }

    public virtual DbSet<ViewIdentProjetIndicateursPlat> ViewIdentProjetIndicateursPlats { get; set; }

    public virtual DbSet<ViewIdentProjetLivrablesPlat> ViewIdentProjetLivrablesPlats { get; set; }

    public virtual DbSet<ViewIdentProjetObjectifsSpecifiquesPlat> ViewIdentProjetObjectifsSpecifiquesPlats { get; set; }

    public virtual DbSet<ViewIdentProjetPartiesPrenantesPlat> ViewIdentProjetPartiesPrenantesPlats { get; set; }

    public virtual DbSet<ViewIdentificationProjetPlat> ViewIdentificationProjetPlats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            //.HasDefaultSchema("JOSEPHRUDY")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<OViewActivite>(entity =>
        {
            entity.ToView("O_VIEW_ACTIVITES");
        });

        modelBuilder.Entity<OViewAspectsJuridique>(entity =>
        {
            entity.ToView("O_VIEW_ASPECTS_JURIDIQUES");
        });

        modelBuilder.Entity<OViewBailleursDeFond>(entity =>
        {
            entity.ToView("O_VIEW_BAILLEURS_DE_FONDS");
        });

        modelBuilder.Entity<OViewCoutAnnuelDuProjet>(entity =>
        {
            entity.ToView("O_VIEW_COUT_ANNUEL_DU_PROJET");
        });

        modelBuilder.Entity<OViewEffetsDuProjet>(entity =>
        {
            entity.ToView("O_VIEW_EFFETS_DU_PROJET");
        });

        modelBuilder.Entity<OViewImpactsDuProjet>(entity =>
        {
            entity.ToView("O_VIEW_IMPACTS_DU_PROJET");
        });

        modelBuilder.Entity<OViewIndicateursDeResultat>(entity =>
        {
            entity.ToView("O_VIEW_INDICATEURS_DE_RESULTATS");
        });

        modelBuilder.Entity<OViewLivrablesDuProjet>(entity =>
        {
            entity.ToView("O_VIEW_LIVRABLES_DU_PROJET");
        });

        modelBuilder.Entity<OViewPartiesPrenante>(entity =>
        {
            entity.ToView("O_VIEW_PARTIES_PRENANTES");
        });

        modelBuilder.Entity<ViewActivitesIformationsFinanciere>(entity =>
        {
            entity.ToView("VIEW_ACTIVITES_IFORMATIONS_FINANCIERES");
        });

        modelBuilder.Entity<ViewIdentProjetActivitesPlat>(entity =>
        {
            entity.ToView("VIEW_IDENT_PROJET_ACTIVITES_PLAT");
        });

        modelBuilder.Entity<ViewIdentProjetAspectsJuridiquesPlat>(entity =>
        {
            entity.ToView("VIEW_IDENT_PROJET_ASPECTS_JURIDIQUES_PLAT");
        });

        modelBuilder.Entity<ViewIdentProjetBailleursPlat>(entity =>
        {
            entity.ToView("VIEW_IDENT_PROJET_BAILLEURS_PLAT");
        });

        modelBuilder.Entity<ViewIdentProjetEffetsPlat>(entity =>
        {
            entity.ToView("VIEW_IDENT_PROJET_EFFETS_PLAT");
        });

        modelBuilder.Entity<ViewIdentProjetImpactsPlat>(entity =>
        {
            entity.ToView("VIEW_IDENT_PROJET_IMPACTS_PLAT");
        });

        modelBuilder.Entity<ViewIdentProjetIndicateursPlat>(entity =>
        {
            entity.ToView("VIEW_IDENT_PROJET_INDICATEURS_PLAT");
        });

        modelBuilder.Entity<ViewIdentProjetLivrablesPlat>(entity =>
        {
            entity.ToView("VIEW_IDENT_PROJET_LIVRABLES_PLAT");
        });

        modelBuilder.Entity<ViewIdentProjetObjectifsSpecifiquesPlat>(entity =>
        {
            entity.ToView("VIEW_IDENT_PROJET_OBJECTIFS_SPECIFIQUES_PLAT");
        });

        modelBuilder.Entity<ViewIdentProjetPartiesPrenantesPlat>(entity =>
        {
            entity.ToView("VIEW_IDENT_PROJET_PARTIES_PRENANTES_PLAT");
        });

        modelBuilder.Entity<ViewIdentificationProjetPlat>(entity =>
        {
            entity.ToView("VIEW_IDENTIFICATION_PROJET_PLAT");
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
