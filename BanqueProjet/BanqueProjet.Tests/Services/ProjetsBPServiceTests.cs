using BanqueProjet.Application.Dtos;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Shared.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Tests.Services
{
    public class ProjetsBPServiceTests
    {
        [Fact]
        public async Task AjouterAsync_GeneratesIdAndSerializesJson_PreservesCasingAndIncludesNestedCollections()
        {
            var logger = new NullLogger<TestableProjetsBPService>();
            var svc = new TestableProjetsBPService(logger);

            var dto = new ProjetsBPDto
            {
                NomProjet = "Un test",
                Activites = new List<ActiviteBPDto>
                {
                    new ActiviteBPDto { NomActivite = "A1" }
                },
                BailleursDeFonds = new List<BailleursDeFondsDto>
                {
                    new BailleursDeFondsDto { NomBailleur = "B1" }
                }
            };

            await svc.AjouterAsync(dto);

            Assert.False(string.IsNullOrWhiteSpace(dto.IdIdentificationProjet));
            Assert.Equal("AJOUTER_IDENTIFICATION_PROJET_ET_LISTES_JSON", svc.LastProcedureName);

            var parsed = JObject.Parse(svc.LastJsonSent!);

            Assert.Equal("Un test", parsed["NomProjet"]?.ToString());
            Assert.True(parsed["IdIdentificationProjet"] != null || parsed["ID_IDENTIFICATION_PROJET"] != null);
            Assert.True(parsed["Activites"] != null || parsed["activites"] != null);
            Assert.True(parsed["BailleursDeFonds"] != null || parsed["bailleursDeFonds"] != null);
        }

    }
}
