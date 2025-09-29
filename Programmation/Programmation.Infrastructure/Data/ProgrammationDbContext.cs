using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Programmation.Infrastructure.Entities;

namespace Programmation.Infrastructure.Data;

public partial class ProgrammationDbContext : DbContext
{

    public ProgrammationDbContext(DbContextOptions<ProgrammationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OViewHypothesesEtRisque> OViewHypothesesEtRisques { get; set; }
    public virtual DbSet<OViewPrevisionActivitesAnnuelle> OViewPrevisionActivitesAnnuelles { get; set; }
    public virtual DbSet<OViewGestionDeProjetEtSuivi> OViewGestionEtSuivis { get; set; }
    public virtual DbSet<OViewInformationsFinancieresProgrammeesProjet> OViewInformationsFinancieresProgrammeesProjets { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("JOSEPHRUDY")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<OViewHypothesesEtRisque>(entity =>
        {
            entity.ToView("O_VIEW_HYPOTHESES_ET_RISQUES");
        });

        modelBuilder.Entity<OViewGestionDeProjetEtSuivi>(entity =>
        {
            entity.ToView("O_VIEW_GESTION_DE_PROJET_ET_SUIVI");
        });

        modelBuilder.Entity<OViewPrevisionActivitesAnnuelle>(entity =>
        {
            entity.ToView("O_VIEW_ACTIVITES_ANNUELLES");
        });


        modelBuilder.HasSequence("NOTIFICATION_SEQ");
        modelBuilder.HasSequence("SEQ_ACTIVITES");
        modelBuilder.HasSequence("SEQ_ACTIVITES_ANNUELLES");
        modelBuilder.HasSequence("SEQ_ALINEA_NOMENCLATURE_BUDGETAIRE");
        modelBuilder.HasSequence("SEQ_ARRONDISSEMENT");
        modelBuilder.HasSequence("SEQ_ARTICLE_NOMENCLATURE_BUDGETAIRE");
        modelBuilder.HasSequence("SEQ_ASPECTS_INSTITUTIONNELS");
        modelBuilder.HasSequence("SEQ_ASPECTS_JURIDIQUES");
        modelBuilder.HasSequence("SEQ_ASPECTS_LEGAUX");
        modelBuilder.HasSequence("SEQ_ATTRIBUTIONS_INSTITUTION");
        modelBuilder.HasSequence("SEQ_BAILLEURS_DE_FONDS");
        modelBuilder.HasSequence("SEQ_COMMUNE");
        modelBuilder.HasSequence("SEQ_COUT_ANNUEL_DU_PROJET");
        modelBuilder.HasSequence("SEQ_COUT_ANNUEL_PROJET");
        modelBuilder.HasSequence("SEQ_COUT_TOTAL_PROJET");
        modelBuilder.HasSequence("SEQ_DDP_CADRE_LOGIQUE");
        modelBuilder.HasSequence("SEQ_DEPARTEMENT");
        modelBuilder.HasSequence("SEQ_DOCUMENTS_ANNEXES");
        modelBuilder.HasSequence("SEQ_DUREE_PROJET");
        modelBuilder.HasSequence("SEQ_ECHELON_TERRITORIALE");
        modelBuilder.HasSequence("SEQ_EFFETS_DU_PROJET");
        modelBuilder.HasSequence("SEQ_EMPLOIS_CREES");
        modelBuilder.HasSequence("SEQ_GESTION_DE_PROJET");
        modelBuilder.HasSequence("SEQ_IDENTIFICATION_PROJET");
        modelBuilder.HasSequence("SEQ_IMPACTS_DU_PROJET");
        modelBuilder.HasSequence("SEQ_INDICATEURS_DE_RESULTATS");
        modelBuilder.HasSequence("SEQ_INFORMATIONS_FINANCIERES");
        modelBuilder.HasSequence("SEQ_INSTITUTION_SECTORIELLE");
        modelBuilder.HasSequence("SEQ_LIVRABLES_DU_PROJET");
        modelBuilder.HasSequence("SEQ_LIVRABLES_PROJET");
        modelBuilder.HasSequence("SEQ_LOCALISATION_GEOGRAPHIQUE_PROJ");
        modelBuilder.HasSequence("SEQ_OBJECTIFS_SPECIFIQUES");
        modelBuilder.HasSequence("SEQ_OBJECTIF_GENERAL");
        modelBuilder.HasSequence("SEQ_PARAGRAPHE_NOMENCLATURE_BUDGETAIRE");
        modelBuilder.HasSequence("SEQ_PARTIES_PRENANTES");
        modelBuilder.HasSequence("SEQ_PHASE_DU_PROJET");
        modelBuilder.HasSequence("SEQ_PREVISION");
        modelBuilder.HasSequence("SEQ_PROGRAMME");
        modelBuilder.HasSequence("SEQ_SECTEUR_ACTIVITE");
        modelBuilder.HasSequence("SEQ_SECTEUR_D_ACTIVITE");
        modelBuilder.HasSequence("SEQ_SECTION_COMMUNALE");
        modelBuilder.HasSequence("SEQ_SECTION_INSTITUTION");
        modelBuilder.HasSequence("SEQ_SOUS_PROGRAMME");
        modelBuilder.HasSequence("SEQ_SOUS_SECTEUR_ACTIVITE");
        modelBuilder.HasSequence("SEQ_SUIVI_ET_CONTROLE");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
