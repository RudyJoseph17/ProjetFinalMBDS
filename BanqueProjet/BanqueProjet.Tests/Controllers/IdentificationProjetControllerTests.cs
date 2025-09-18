using BanqueProjet.Application.Dtos;
using BanqueProjet.Application.Interfaces;
using BanqueProjet.Web.Areas.BanqueProjet.Controllers;
using BanqueProjet.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Tests.Controllers
{
    public class IdentificationProjetControllerTests
    {
        [Fact]
        public async Task Wizard_Post_Finish_GeneratesId_PropagatesAndCallsServices()
        {
            // Arrange
            var mockProjetService = new Mock<IProjetsBPService>();
            var mockCadre = new Mock<IDdpCadreLogiqueService>();
            var mockAspects = new Mock<IAspectsJuridiquesService>();
            var mockLocalisation = new Mock<ILocalisationGeographiqueProjService>();
            var mockGrilleDdp = new Mock<IGrilleDdpProjetService>();

            ProjetsBPDto? projetsCaptured = null;
            DdpCadreLogiqueDto? cadreCaptured = null;
            AspectsJuridiquesDto? aspectsCaptured = null;
            LocalisationGeographiqueProjDto? localisationCaptured = null;
            GrilleDdpProjetDto? grilleDdpCaptured = null;

            mockProjetService
                .Setup(s => s.AjouterAsync(It.IsAny<ProjetsBPDto>()))
                .Returns(Task.CompletedTask)
                .Callback<ProjetsBPDto>(p => projetsCaptured = p);

            mockCadre
                .Setup(s => s.AjouterAsync(It.IsAny<DdpCadreLogiqueDto>()))
                .Returns(Task.CompletedTask)
                .Callback<DdpCadreLogiqueDto>(d => cadreCaptured = d);

            mockAspects
                .Setup(s => s.AjouterAsync(It.IsAny<AspectsJuridiquesDto>()))
                .Returns(Task.CompletedTask)
                .Callback<AspectsJuridiquesDto>(a => aspectsCaptured = a);

            mockLocalisation
                .Setup(s => s.AjouterAsync(It.IsAny<LocalisationGeographiqueProjDto>()))
                .Returns(Task.CompletedTask)
                .Callback<LocalisationGeographiqueProjDto>(l => localisationCaptured = l);

            var logger = new NullLogger<IdentificationProjetController>();

            var controller = new IdentificationProjetController(
                mockProjetService.Object,
                mockCadre.Object,
                mockAspects.Object,
                mockLocalisation.Object,
                mockGrilleDdp.Object,
                logger
            );

            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            var model = new DdpViewModel
            {
                Projets = new ProjetsBPDto
                {
                    NomProjet = "Test projet"
                },
                CadreLogique = new DdpCadreLogiqueDto(),
                AspectsJuridiques = new List<AspectsJuridiquesDto>(),
                LocalisationGeographique = new LocalisationGeographiqueProjDto()

            };

            // Act
            var result = await controller.Wizard(model, step: 6, action: "Finish");

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(IdentificationProjetController.Index), redirect.ActionName);

            mockProjetService.Verify(s => s.AjouterAsync(It.IsAny<ProjetsBPDto>()), Times.Once);
            Assert.NotNull(projetsCaptured);
            Assert.False(string.IsNullOrWhiteSpace(projetsCaptured!.IdIdentificationProjet));

            mockCadre.Verify(s => s.AjouterAsync(It.IsAny<DdpCadreLogiqueDto>()), Times.Once);
            mockAspects.Verify(s => s.AjouterAsync(It.IsAny<AspectsJuridiquesDto>()), Times.Once);
            mockLocalisation.Verify(s => s.AjouterAsync(It.IsAny<LocalisationGeographiqueProjDto>()), Times.Once);

            Assert.NotNull(cadreCaptured);
            Assert.Equal(projetsCaptured.IdIdentificationProjet, cadreCaptured!.IdIdentificationProjet);

            Assert.NotNull(aspectsCaptured);
            Assert.Equal(projetsCaptured.IdIdentificationProjet, aspectsCaptured!.IdIdentificationProjet);

            Assert.NotNull(localisationCaptured);
            Assert.Equal(projetsCaptured.IdIdentificationProjet, localisationCaptured!.IdIdentificationProjet);
        }
    }
}
