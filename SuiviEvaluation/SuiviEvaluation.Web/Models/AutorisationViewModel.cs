using Microsoft.AspNetCore.Mvc.Rendering;
using SuiviEvaluation.Application.Dtos;
using System.Collections.Generic;

namespace SuiviEvaluation.Web.Models
{
    public class AutorisationViewModel
    {
        public string IdIdentificationProjet { get; set; }
        public string NomProjet { get; set; }

        // Liste déroulante
        public List<SelectListItem> Activites { get; set; } = new();

        // Plusieurs autorisations possibles sur un projet
        public List<AutorisationSurProjetDto> AutorisationsActivite { get; set; } = new();

        // (optionnel) infos générales du projet si tu veux afficher plus
        public SuiviProjetDto SuiviProjets { get; set; }
    }
}
