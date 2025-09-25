using System.Collections.Generic;
using System.Threading.Tasks;
using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Web.Areas.BanqueProjet.Controllers;
using BanqueProjet.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace BanqueProjet.Tests.Controllers
{
    public class IdentificationProjetControllerTests
    {
        [Fact]
        public async Task Wizard_Post_Finish_GeneratesId_PropagatesAndCallsServices()
        {
            // Arrange
            var mockProjetService = new Mock<IProjetsBPService>();
            var mockCadreService = new Mock<IDdpCadreLogiqueService>();
            var mockAspectsService = new Mock<IAspectsJuridiquesService>();
            var mockLocalisationService = new Mock<ILocalisationGeographiqueProjService>();
            var mockGrilleService = new Mock<IGrilleDdpProjetService>();
            var mockPartiesService = new Mock<IPartiesPrenantesService>();
            var mockIndicateursService = new Mock<IIndicateursDeResultatService>();
            var mockLivrablesService = new Mock<IDefinitionLivrablesDuProjetService>();
            var mockEffetsService = new Mock<IEffetsDuProjetService>();
            var mockObjectifsService = new Mock<IObjectifsSpecifiquesService>();
            var mockImpactsService = new Mock<IImpactsDuProjetService>();
            var mockBailleursService = new Mock<IBailleursDeFondService>();
            var mockCoutsService = new Mock<ICoutAnnuelDuProjetService>();
            var mockActivitesAnnuellesService = new Mock<IActivitesAnnuellesService>();
            var mockActiviteBPService = new Mock<IActiviteBPService>();

            ProjetsBPDto projetsCaptured = null!;
            DdpCadreLogiqueDto cadreCaptured = null!;
            AspectsJuridiquesDto aspectsCaptured = null!;
            LocalisationGeographiqueProjDto localisationCaptured = null!;

            // Stub des services qu'on vérifie
            mockProjetService
                .Setup(s => s.AjouterAsync(It.IsAny<ProjetsBPDto>()))
                .Callback<ProjetsBPDto>(p => projetsCaptured = p)
                .Returns(Task.CompletedTask);

            mockCadreService
                .Setup(s => s.AjouterAsync(It.IsAny<DdpCadreLogiqueDto>()))
                .Callback<DdpCadreLogiqueDto>(d => cadreCaptured = d)
                .Returns(Task.CompletedTask);

            mockAspectsService
                .Setup(s => s.AjouterAsync(It.IsAny<AspectsJuridiquesDto>()))
                .Callback<AspectsJuridiquesDto>(a => aspectsCaptured = a)
                .Returns(Task.CompletedTask);

            mockLocalisationService
                .Setup(s => s.AjouterAsync(It.IsAny<LocalisationGeographiqueProjDto>()))
                .Callback<LocalisationGeographiqueProjDto>(l => localisationCaptured = l)
                .Returns(Task.CompletedTask);

            // Stub des autres services pour qu'ils retournent toujours Task.CompletedTask
            mockGrilleService
                .Setup(s => s.AjouterAsync(It.IsAny<GrilleDdpProjetDto>()))
                .Returns(Task.CompletedTask);
            mockPartiesService
                .Setup(s => s.AjouterAsync(It.IsAny<PartiesPrenantesDto>()))
                .Returns(Task.CompletedTask);
            mockIndicateursService
                .Setup(s => s.AjouterAsync(It.IsAny<IndicateursDeResultatDto>()))
                .Returns(Task.CompletedTask);
            mockLivrablesService
                .Setup(s => s.AjouterAsync(It.IsAny<DefinitionLivrablesDuProjetDto>()))
                .Returns(Task.CompletedTask);
            mockEffetsService
                .Setup(s => s.AjouterAsync(It.IsAny<EffetsDuProjetDto>()))
                .Returns(Task.CompletedTask);
            mockObjectifsService
                .Setup(s => s.AjouterAsync(It.IsAny<ObjectifsSpecifiquesDto>()))
                .Returns(Task.CompletedTask);
            mockImpactsService
                .Setup(s => s.AjouterAsync(It.IsAny<ImpactsDuProjetDto>()))
                .Returns(Task.CompletedTask);
            mockBailleursService
                .Setup(s => s.AjouterAsync(It.IsAny<BailleursDeFondsDto>()))
                .Returns(Task.CompletedTask);
            mockCoutsService
                .Setup(s => s.AjouterAsync(It.IsAny<CoutAnnuelDuProjetDto>()))
                .Returns(Task.CompletedTask);
            mockActivitesAnnuellesService
                .Setup(s => s.AjouterAsync(It.IsAny<ActivitesAnnuellesDto>()))
                .Returns(Task.CompletedTask);

            // Pour la partie ActiviteBP, comme on ne l'appelle pas dans PersistAllAsync, on stub juste l'appel Get
            mockActiviteBPService
                .Setup(s => s.ObtenirTousAsync())
                .ReturnsAsync(new List<ActiviteBPDto>());

            var logger = new NullLogger<IdentificationProjetController>();

            var controller = new IdentificationProjetController(
                mockProjetService.Object,
                mockCadreService.Object,
                mockAspectsService.Object,
                mockLocalisationService.Object,
                mockGrilleService.Object,
                mockPartiesService.Object,
                mockIndicateursService.Object,
                mockLivrablesService.Object,
                mockEffetsService.Object,
                mockObjectifsService.Object,
                mockImpactsService.Object,
                mockBailleursService.Object,
                mockCoutsService.Object,
                mockActivitesAnnuellesService.Object,
                mockActiviteBPService.Object,
                logger
            );

            controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>()
            );

            // On ajoute un élément dans AspectsJuridiques pour que le service soit bien appelé
            var model = new DdpViewModel
            {
                Projets = new ProjetsBPDto { NomProjet = "Test projet" },
                CadreLogique = new DdpCadreLogiqueDto(),
                AspectsJuridiques = new List<AspectsJuridiquesDto> { new AspectsJuridiquesDto() },
                LocalisationGeographique = new LocalisationGeographiqueProjDto(),
                PartiesPrenantesProjets = new List<PartiesPrenantesDto>(),
                IndicateursResultats = new List<IndicateursDeResultatDto>(),
                DefinitionLivrables = new List<DefinitionLivrablesDuProjetDto>(),
                EffetsProjets = new List<EffetsDuProjetDto>(),
                ObjectifsSpecifiques = new List<ObjectifsSpecifiquesDto>(),
                ImpactsDuProjet = new List<ImpactsDuProjetDto>(),
                BailleursDeFonds = new List<BailleursDeFondsDto>(),
                CoutAnnuelDuProjet = new List<CoutAnnuelDuProjetDto>(),
                ActivitesAnnuelles = new List<ActivitesAnnuellesDto>(),
                ActiviteBP = new List<ActiviteBPDto>()
            };

            // Act
            var result = await controller.Wizard(model, step: 6, action: "Finish");

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(IdentificationProjetController.Index), redirect.ActionName);

            mockProjetService.Verify(s => s.AjouterAsync(It.IsAny<ProjetsBPDto>()), Times.Once);
            Assert.False(string.IsNullOrWhiteSpace(projetsCaptured.IdIdentificationProjet));

            mockCadreService.Verify(s => s.AjouterAsync(It.IsAny<DdpCadreLogiqueDto>()), Times.Once);
            Assert.Equal(projetsCaptured.IdIdentificationProjet, cadreCaptured.IdIdentificationProjet);

            mockAspectsService.Verify(s => s.AjouterAsync(It.IsAny<AspectsJuridiquesDto>()), Times.Once);
            Assert.Equal(projetsCaptured.IdIdentificationProjet, aspectsCaptured.IdIdentificationProjet);

            mockLocalisationService.Verify(s => s.AjouterAsync(It.IsAny<LocalisationGeographiqueProjDto>()), Times.Once);
            Assert.Equal(projetsCaptured.IdIdentificationProjet, localisationCaptured.IdIdentificationProjet);
        }
    }
}
