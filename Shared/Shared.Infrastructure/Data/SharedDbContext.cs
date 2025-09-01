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

    public virtual DbSet<ViewAlineaNomenclatureBudgetairePlat> ViewAlineaNomenclatureBudgetairePlats { get; set; }

    public virtual DbSet<ViewArrondissementPlat> ViewArrondissementPlats { get; set; }

    public virtual DbSet<ViewArticleNomenclatureBudgetairePlat> ViewArticleNomenclatureBudgetairePlats { get; set; }

    public virtual DbSet<ViewAttributionsInstitutionPlat> ViewAttributionsInstitutionPlats { get; set; }

    public virtual DbSet<ViewBailleursDeFondsPlat> ViewBailleursDeFondsPlats { get; set; }

    public virtual DbSet<ViewCommunePlat> ViewCommunePlats { get; set; }

    public virtual DbSet<ViewDepartementPlat> ViewDepartementPlats { get; set; }

    public virtual DbSet<ViewIdentProjetLivrablesPlat> ViewIdentProjetLivrablesPlats { get; set; }

    public virtual DbSet<ViewIdentificationProjetPlat> ViewIdentificationProjetPlats { get; set; }

    public virtual DbSet<ViewInstitutionSectoriellePlat> ViewInstitutionSectoriellePlats { get; set; }

    public virtual DbSet<ViewNotificationPlat> ViewNotificationPlats { get; set; }

    public virtual DbSet<ViewParagrapheNomenclatureBudgetairePlat> ViewParagrapheNomenclatureBudgetairePlats { get; set; }

    public virtual DbSet<ViewProgrammePlat> ViewProgrammePlats { get; set; }

    public virtual DbSet<ViewSecteurActivitePlat> ViewSecteurActivitePlats { get; set; }

    public virtual DbSet<ViewSectionCommunalePlat> ViewSectionCommunalePlats { get; set; }

    public virtual DbSet<ViewSectionInstitutionPlat> ViewSectionInstitutionPlats { get; set; }

    public virtual DbSet<ViewSousProgrammePlat> ViewSousProgrammePlats { get; set; }

    public virtual DbSet<ViewSousSecteurActivitePlat> ViewSousSecteurActivitePlats { get; set; }

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

        modelBuilder.Entity<ViewAlineaNomenclatureBudgetairePlat>(entity =>
        {
            entity.ToView("VIEW_ALINEA_NOMENCLATURE_BUDGETAIRE_PLAT");
        });

        modelBuilder.Entity<ViewArrondissementPlat>(entity =>
        {
            entity.ToView("VIEW_ARRONDISSEMENT_PLAT");
        });

        modelBuilder.Entity<ViewArticleNomenclatureBudgetairePlat>(entity =>
        {
            entity.ToView("VIEW_ARTICLE_NOMENCLATURE_BUDGETAIRE_PLAT");
        });

        modelBuilder.Entity<ViewAttributionsInstitutionPlat>(entity =>
        {
            entity.ToView("VIEW_ATTRIBUTIONS_INSTITUTION_PLAT");
        });

        modelBuilder.Entity<ViewBailleursDeFondsPlat>(entity =>
        {
            entity.ToView("VIEW_BAILLEURS_DE_FONDS_PLAT");
        });

        modelBuilder.Entity<ViewCommunePlat>(entity =>
        {
            entity.ToView("VIEW_COMMUNE_PLAT");
        });

        modelBuilder.Entity<ViewDepartementPlat>(entity =>
        {
            entity.ToView("VIEW_DEPARTEMENT_PLAT");
        });

        modelBuilder.Entity<ViewIdentProjetLivrablesPlat>(entity =>
        {
            entity.ToView("VIEW_IDENT_PROJET_LIVRABLES_PLAT");
        });

        modelBuilder.Entity<ViewIdentificationProjetPlat>(entity =>
        {
            entity.ToView("VIEW_IDENTIFICATION_PROJET_PLAT");
        });

        modelBuilder.Entity<ViewInstitutionSectoriellePlat>(entity =>
        {
            entity.ToView("VIEW_INSTITUTION_SECTORIELLE_PLAT");
        });

        modelBuilder.Entity<ViewNotificationPlat>(entity =>
        {
            entity.ToView("VIEW_NOTIFICATION_PLAT");

            entity.Property(e => e.EstLuChar).IsFixedLength();
        });

        modelBuilder.Entity<ViewParagrapheNomenclatureBudgetairePlat>(entity =>
        {
            entity.ToView("VIEW_PARAGRAPHE_NOMENCLATURE_BUDGETAIRE_PLAT");
        });

        modelBuilder.Entity<ViewProgrammePlat>(entity =>
        {
            entity.ToView("VIEW_PROGRAMME_PLAT");
        });

        modelBuilder.Entity<ViewSecteurActivitePlat>(entity =>
        {
            entity.ToView("VIEW_SECTEUR_ACTIVITE_PLAT");
        });

        modelBuilder.Entity<ViewSectionCommunalePlat>(entity =>
        {
            entity.ToView("VIEW_SECTION_COMMUNALE_PLAT");
        });

        modelBuilder.Entity<ViewSectionInstitutionPlat>(entity =>
        {
            entity.ToView("VIEW_SECTION_INSTITUTION_PLAT");
        });

        modelBuilder.Entity<ViewSousProgrammePlat>(entity =>
        {
            entity.ToView("VIEW_SOUS_PROGRAMME_PLAT");
        });

        modelBuilder.Entity<ViewSousSecteurActivitePlat>(entity =>
        {
            entity.ToView("VIEW_SOUS_SECTEUR_ACTIVITE_PLAT");
        });
        modelBuilder.HasSequence("NOTIFICATION_SEQ");
        modelBuilder.HasSequence("SEQ_ACTIVITES");
        modelBuilder.HasSequence("SEQ_ACTIVITES_ANNUELLES");
        modelBuilder.HasSequence("SEQ_ALINEA_NOMENCLATURE_BUDGETAIRE");
        modelBuilder.HasSequence("SEQ_ARRONDISSEMENT");
        modelBuilder.HasSequence("SEQ_ARTICLE_NOMENCLATURE_BUDGETAIRE");
        modelBuilder.HasSequence("SEQ_ASPECTS_JURIDIQUES");
        modelBuilder.HasSequence("SEQ_ATTRIBUTIONS_INSTITUTION");
        modelBuilder.HasSequence("SEQ_BAILLEURS_DE_FONDS");
        modelBuilder.HasSequence("SEQ_COMMUNE");
        modelBuilder.HasSequence("SEQ_COUT_ANNUEL_DU_PROJET");
        modelBuilder.HasSequence("SEQ_COUT_TOTAL_PROJET");
        modelBuilder.HasSequence("SEQ_DEPARTEMENT");
        modelBuilder.HasSequence("SEQ_EFFETS_DU_PROJET");
        modelBuilder.HasSequence("SEQ_IMPACTS_DU_PROJET");
        modelBuilder.HasSequence("SEQ_INDICATEURS_DE_RESULTATS");
        modelBuilder.HasSequence("SEQ_INSTITUTION_SECTORIELLE");
        modelBuilder.HasSequence("SEQ_LIVRABLES_DU_PROJET");
        modelBuilder.HasSequence("SEQ_OBJECTIFS_SPECIFIQUES");
        modelBuilder.HasSequence("SEQ_PARAGRAPHE_NOMENCLATURE_BUDGETAIRE");
        modelBuilder.HasSequence("SEQ_PARTIES_PRENANTES");
        modelBuilder.HasSequence("SEQ_PROGRAMME");
        modelBuilder.HasSequence("SEQ_SECTEUR_ACTIVITE");
        modelBuilder.HasSequence("SEQ_SECTION_COMMUNALE");
        modelBuilder.HasSequence("SEQ_SECTION_INSTITUTION");
        modelBuilder.HasSequence("SEQ_SOUS_PROGRAMME");
        modelBuilder.HasSequence("SEQ_SOUS_SECTEUR_ACTIVITE");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
