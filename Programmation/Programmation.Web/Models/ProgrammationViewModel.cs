// ViewModel pour /Programmation/Views
using Programmation.Application.Dtos;

public class ProgrammationViewModel
{
    public ProgrammationProjetDto Programmation { get; set; } = new();
    public List<HypothesesEtRisquesDto> Hypotheses { get; set; } = new();
    public List<InformationsFinancieresProgrammeesProjetDto> InformationsFinancieres { get; set; } = new();
    public List<LivrablesProgrameProjetDto> Livrables { get; set; } = new();
    public List<PrevisionActiviteAnnuelleDto> Previsions { get; set; } = new();
    public List<GestionEtSuiviProjetDto> Gestion { get; set; } = new();
}