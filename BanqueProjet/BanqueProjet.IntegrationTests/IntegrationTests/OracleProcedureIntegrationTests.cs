using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.IntegrationTests.IntegrationTests
{
    public class OracleProcedureIntegrationTests
    {
        // Lecture de la connection string depuis une variable d'environnement :
        // ex: "User Id=TEST;Password=Passw0rd!;Data Source=localhost:1521/XEPDB1"
        private string GetConnectionString()
        {
            var cs = Environment.GetEnvironmentVariable("ORACLE_TEST_CONN");
            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException("Veuillez définir la variable d'environnement ORACLE_TEST_CONN pour les tests d'intégration.");
            return cs!;
        }

        [Fact(Skip = "Integration test - requires Oracle test DB. Enable manually.")]
        public async Task Call_AjouterProcedure_WithSimpleJson_ShouldNotThrow()
        {
            var connStr = GetConnectionString();

            var simpleJsonObj = new
            {
                IdIdentificationProjet = (string?)null,
                NomProjet = "IT Test - " + Guid.NewGuid().ToString("N").Substring(0, 6),
                // ajouter d'autres champs minimaux requis par la proc si nécessaire
            };

            var json = JsonConvert.SerializeObject(simpleJsonObj);

            await using var conn = new OracleConnection(connStr);
            await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "AJOUTER_IDENTIFICATION_PROJET_ET_LISTES_JSON";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            var p = new OracleParameter("p_json", OracleDbType.Clob, ParameterDirection.Input);
            p.Value = json;
            cmd.Parameters.Add(p);

            // exécuter la proc
            await cmd.ExecuteNonQueryAsync();

            // Optionnel : vérifier insertion via SELECT (adaptez selon votre vue/table)
            // var checkCmd = conn.CreateCommand();
            // checkCmd.CommandText = "SELECT COUNT(*) FROM VIEW_IDENTIFICATION_PROJET_PLAT WHERE NOM_PROJET = :p";
            // checkCmd.Parameters.Add(new OracleParameter("p", jsonObj.NomProjet));
            // var count = Convert.ToInt32(await checkCmd.ExecuteScalarAsync());
            // Assert.True(count >= 1);
        }
    }
}
