using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programmation.Infrastructure.Entities
{

    [Keyless]
    [Table("O_VIEW_INFORMATIONS_FINANCIERES_T")]
    public partial class OViewInformationsFinancieresProgrammeesProjet
    {
        [Column("ID_INFORMATIONS_FINANCIERES")]
        [Precision(6)]
        public int IdInformationsFinancieres { get; set; }

        [Column("EXERCICE_FISCAL_DEBUT")]
        [Precision(4)]
        public byte? ExerciceFiscalDebut { get; set; }

        [Column("EXERCICE_FISCAL_FIN")]
        [Precision(4)]
        public byte? ExerciceFiscalFin { get; set; }

        [Column("SOURCES_FINANCEMENT")]
        [StringLength(50)]
        [Unicode(false)]
        public string? SourcesFinancement { get; set; }

        [Column("ARTICLE")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Article { get; set; }

        [Column("ALINEA")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Alinea { get; set; }

        [Column("MONTANT_PREVU", TypeName = "NUMBER(12,2)")]
        public decimal? MontantPrevu { get; set; }

        [Column("MOIS_PREVISION")]
        [StringLength(12)]
        [Unicode(false)]
        public string? MoisPrevision { get; set; }

        [Column("ID_IDENTIFICATION_PROJET")]
        [StringLength(12)]
        [Unicode(false)]
        public string IdIdentificationProjet { get; set; } = null!;
    }
}
