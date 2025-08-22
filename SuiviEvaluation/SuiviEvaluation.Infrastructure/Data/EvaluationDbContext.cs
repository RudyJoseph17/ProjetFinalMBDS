using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SuiviEvaluation.Infrastructure.Entities;

namespace SuiviEvaluation.Infrastructure.Data;

public partial class EvaluationDbContext : DbContext
{
    public EvaluationDbContext()
    {
    }

    public EvaluationDbContext(DbContextOptions<EvaluationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OViewQuantiteLivreParAnnee> OViewQuantiteLivreParAnnees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("User Id=josephrudy;Password=2025RUDY001;Data Source=DIP_SS_1:1521/banqueprojet1;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("JOSEPHRUDY")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<OViewQuantiteLivreParAnnee>(entity =>
        {
            entity.ToView("O_VIEW_QUANTITE_LIVRE_PAR_ANNEE");
        });
        modelBuilder.HasSequence("SEQ_ACTIVITES");
        modelBuilder.HasSequence("SEQ_ACTIVITES_ANNUELLES");
        modelBuilder.HasSequence("SEQ_ASPECTS_INSTITUTIONNELS");
        modelBuilder.HasSequence("SEQ_ASPECTS_LEGAUX");
        modelBuilder.HasSequence("SEQ_BAILLEURS_DE_FONDS");
        modelBuilder.HasSequence("SEQ_COUT_ANNUEL_DU_PROJET");
        modelBuilder.HasSequence("SEQ_COUT_TOTAL_PROJET");
        modelBuilder.HasSequence("SEQ_DDP_CADRE_LOGIQUE");
        modelBuilder.HasSequence("SEQ_DUREE_PROJET");
        modelBuilder.HasSequence("SEQ_ECHELON_TERRITORIALE");
        modelBuilder.HasSequence("SEQ_EFFETS_DU_PROJET");
        modelBuilder.HasSequence("SEQ_EMPLOIS_CREES");
        modelBuilder.HasSequence("SEQ_GESTION_DE_PROJET");
        modelBuilder.HasSequence("SEQ_IMPACTS_DU_PROJET");
        modelBuilder.HasSequence("SEQ_INDICATEURS_DE_RESULTATS");
        modelBuilder.HasSequence("SEQ_INSTITUTION_SECTORIELLE");
        modelBuilder.HasSequence("SEQ_LIVRABLES_DU_PROJET");
        modelBuilder.HasSequence("SEQ_LOCALISATION_GEOGRAPHIQUE_PROJ");
        modelBuilder.HasSequence("SEQ_OBJECTIFS_SPECIFIQUES");
        modelBuilder.HasSequence("SEQ_OBJECTIF_GENERAL");
        modelBuilder.HasSequence("SEQ_PARTIES_PRENANTES");
        modelBuilder.HasSequence("SEQ_PHASE_DU_PROJET");
        modelBuilder.HasSequence("SEQ_PREVISION");
        modelBuilder.HasSequence("SEQ_SECTEUR_D_ACTIVITE");
        modelBuilder.HasSequence("SEQ_SECTION_INSTITUTION");
        modelBuilder.HasSequence("SEQ_SUIVI_ET_CONTROLE");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
