using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace Shared.API.Controller
{
    public class DocumentsController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly ILogger<DocumentsController> _logger;

        public DocumentsController(ILogger<DocumentsController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Charge le .env (assure-toi d'avoir DotNetEnv dans ton projet et que .env est à la racine)
            DotNetEnv.Env.Load();

            var user = Environment.GetEnvironmentVariable("ORACLE_DB_USER");
            var password = Environment.GetEnvironmentVariable("ORACLE_DB_PASSWORD");
            var host = Environment.GetEnvironmentVariable("ORACLE_DB_HOST");
            var port = Environment.GetEnvironmentVariable("ORACLE_DB_PORT") ?? "1521";
            var service = Environment.GetEnvironmentVariable("ORACLE_DB_SERVICE");

            if (string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(service))
            {
                _logger.LogError("Variables d'environnement Oracle manquantes. Vérifie .env");
                throw new InvalidOperationException("Configuration Oracle manquante.");
            }

            // Chaîne de connexion Oracle en mode easy connect / description (préférer DESCRIPTION pour robustesse)
            _connectionString = $"User Id={user};Password={password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port}))(CONNECT_DATA=(SERVICE_NAME={service})))";
        }

        /// <summary>
        /// Upload d'un fichier et stockage en BLOB ou CLOB selon le type / contenu.
        /// </summary>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] int idDocument, [FromForm] string typeFichier)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Fichier invalide.");

            if (idDocument <= 0)
                return BadRequest("Identifiant de document invalide.");

            // Déterminer la colonne cible selon typeFichier (ne pas autoriser de valeurs arbitraires)
            string column = typeFichier?.ToLowerInvariant() switch
            {
                "contrat" => "CONTRAT_TEXTE",
                "correspondance" => "CORRESPONDANCE_TEXTE",
                _ => null
            };

            if (column == null)
                return BadRequest("Type de fichier inconnu. Utiliser 'contrat' ou 'correspondance'.");

            try
            {
                // Read file bytes
                byte[] fileBytes;
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                }

                // Détecter s'il s'agit d'un fichier texte (on utilisera CLOB) ou binaire (BLOB)
                // heuristique : content-type text/* ou extension .txt, .csv, .json
                var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant() ?? string.Empty;
                bool treatAsText =
                    file.ContentType.StartsWith("text/", StringComparison.OrdinalIgnoreCase)
                    || ext is ".txt" or ".csv" or ".json" or ".xml" or ".html";

                await using (var conn = new OracleConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // Préparer la commande (BindByName = true pour sécurité)
                    var sql = $"UPDATE DOCUMENTS_ANNEXES_O SET {column} = :fileData WHERE ID_DOCUMENTS_ANNEXES = :id";
                    await using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.BindByName = true;

                        if (treatAsText)
                        {
                            // Convertir en texte UTF-8 et stocker en CLOB
                            var text = Encoding.UTF8.GetString(fileBytes);

                            var pClob = new OracleParameter("fileData", OracleDbType.Clob)
                            {
                                Direction = ParameterDirection.Input,
                                Value = text
                            };
                            cmd.Parameters.Add(pClob);
                        }
                        else
                        {
                            // Stocker en BLOB
                            var pBlob = new OracleParameter("fileData", OracleDbType.Blob)
                            {
                                Direction = ParameterDirection.Input,
                                Value = fileBytes
                            };
                            cmd.Parameters.Add(pBlob);
                        }

                        var pId = new OracleParameter("id", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Input,
                            Value = idDocument
                        };
                        cmd.Parameters.Add(pId);

                        var rows = await cmd.ExecuteNonQueryAsync();

                        if (rows == 0)
                        {
                            _logger.LogWarning("Aucune ligne mise à jour pour ID_DOCUMENTS_ANNEXES = {Id}", idDocument);
                            return NotFound($"Document avec id {idDocument} introuvable.");
                        }
                    } // using cmd

                    await conn.CloseAsync();
                } // using conn

                return Ok("Fichier téléversé avec succès.");
            }
            catch (OracleException oex)
            {
                _logger.LogError(oex, "Erreur Oracle lors du téléversement du fichier. Id={Id}, Type={Type}", idDocument, typeFichier);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur base de données Oracle : " + oex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du téléversement du fichier. Id={Id}, Type={Type}", idDocument, typeFichier);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur serveur : " + ex.Message);
            }
        }
    }
}
