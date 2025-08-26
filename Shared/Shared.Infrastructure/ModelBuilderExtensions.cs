using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Infrastructure.Entities;

namespace Shared.Infrastructure
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Enregistre toutes les vues partagées keyless.
        /// </summary>
        public static ModelBuilder ApplySharedViews(this ModelBuilder builder)
        {
            builder.Entity<OViewLivrablesDuProjet>(b =>
            {
                b.HasNoKey();
                b.ToView("O_VIEW_LIVRABLES_DU_PROJET");
            });

            builder.Entity<ViewIdentificationProjetPlat>(b =>
            {
                b.HasNoKey();
                b.ToView("VIEW_IDENTIFICATION_PROJET_PLAT");
            });

            return builder;
        }
    }
}
