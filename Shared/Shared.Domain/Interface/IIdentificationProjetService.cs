using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Interface
{
    public interface IIdentificationProjetService
    { /// <summary>
      /// Appel bas-niveau : exécute directement la procédure PL/SQL en lui passant le JSON complet attendu.
      /// Recommandation (option A) : les modules qui possèdent des DTOs spécifiques (ex. BanqueProjet)
      /// construisent le JSON complet et appellent cette méthode.
      /// </summary>
        Task AjouterProjetEtListesJsonAsync(string json);

        /// <summary>
        /// Confort : crée un JSON à partir du DTO partagé ( contient uniquement les parties partagées )
        /// et appelle la procédure. Ne gère PAS les DTOs spécifiques au module BanqueProjet.
        /// </summary>
        Task AjouterAsync(IdentificationProjetDto identificationProjet);

        /// <summary>
        /// Confort : met à jour à partir du DTO partagé. Id must be present.
        /// (implémentation : la proc est idempotente => supprime puis insert)
        /// </summary>
        Task MettreAJourAsync(IdentificationProjetDto identificationProjet);

        /// <summary>
        /// Supprime l'identification projet (appelera la proc avec JSON minimal ou fera suppression directe).
        /// </summary>
        Task SupprimerAsync(string idIdentificationProjet);

        Task<List<IdentificationProjetDto>> ObtenirTousAsync();
        Task<IdentificationProjetDto?> ObtenirParIdAsync(string idIdentificationProjet);
    }
}
