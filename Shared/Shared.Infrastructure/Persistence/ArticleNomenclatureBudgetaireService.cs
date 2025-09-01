using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Shared.Domain.Dtos;
using Shared.Domain.Interface;
using Shared.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Persistence
{
    public class ArticleNomenclatureBudgetaireService: IArticleNomenclatureBudgetaireService
    {
        private readonly SharedDbContext _db;
        private readonly ILogger<ArticleNomenclatureBudgetaireService> _logger;

        public ArticleNomenclatureBudgetaireService(
            SharedDbContext db,
            ILogger<ArticleNomenclatureBudgetaireService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task AjouterAsync(ArticleNomenclatureBudgetaireDto article)
        {
            var json = JsonConvert.SerializeObject(article);
            _logger.LogInformation(
                "📦 JSON envoyé à AJOUTER_ARTICLE_ET_PARAGRAPHES_ET_ALINEAS_JSON : {Json}",
                json);

            var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };
            await _db.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_ARTICLE_ET_PARAGRAPHES_ET_ALINEAS_JSON(:p_json); END;",
                param);

            _logger.LogInformation(
                "Procédure AJOUTER_ARTICLE_ET_PARAGRAPHES_ET_ALINEAS_JSON exécutée.");
        }

        public async Task<List<ArticleNomenclatureBudgetaireDto>> ObtenirTousAsync()
        {
            var articles = await _db.ViewArticleNomenclatureBudgetairePlats.ToListAsync();
            var paras = await _db.ViewParagrapheNomenclatureBudgetairePlats.ToListAsync();
            var alinéas = await _db.ViewAlineaNomenclatureBudgetairePlats.ToListAsync();

            return articles.Select(a => new ArticleNomenclatureBudgetaireDto
            {
                IdArticleNomenclatureBudgetaire = a.Idarticle,
                NomArticleNomenclatureBudgetaire = a.Nomarticle,
                listParagrapheNomenclatureBudgetaire = paras
                    .Where(p => p.Idarticle == a.Idarticle)
                    .Select(p => new ParagrapheNomenclatureBudgetaireDto
                    {
                        IdParagrapheNomenclatureBudgetaire = p.Idparagraphe,
                        NomParagrapheNomenclatureBudgetaire = p.Nomparagraphe,
                        listAlineaNomenclatureBudgetaire = alinéas
                            .Where(al => al.Idparagraphe == p.Idparagraphe)
                            .Select(al => new AlineaNomenclatureBudgetaireDto
                            {
                                IdAlineaNomenclatureBudgetaire = al.Idalinea,
                                NomAlineaNomenclatureBudgetaire = al.Nomalinea
                            })
                            .ToList()
                    })
                    .ToList()
            }).ToList();
        }

        public async Task<ArticleNomenclatureBudgetaireDto?> ObtenirParIdAsync(int id)
        {
            var a = await _db.ViewArticleNomenclatureBudgetairePlats
                .FirstOrDefaultAsync(x => x.Idarticle == id);
            if (a == null) return null;

            var paras = await _db.ViewParagrapheNomenclatureBudgetairePlats
                .Where(p => p.Idarticle == id).ToListAsync();
            var alinéas = await _db.ViewAlineaNomenclatureBudgetairePlats.ToListAsync();

            return new ArticleNomenclatureBudgetaireDto
            {
                IdArticleNomenclatureBudgetaire = a.Idarticle,
                NomArticleNomenclatureBudgetaire = a.Nomarticle,
                listParagrapheNomenclatureBudgetaire = paras
                    .Select(p => new ParagrapheNomenclatureBudgetaireDto
                    {
                        IdParagrapheNomenclatureBudgetaire = p.Idparagraphe,
                        NomParagrapheNomenclatureBudgetaire = p.Nomparagraphe,
                        listAlineaNomenclatureBudgetaire = alinéas
                            .Where(al => al.Idparagraphe == p.Idparagraphe)
                            .Select(al => new AlineaNomenclatureBudgetaireDto
                            {
                                IdAlineaNomenclatureBudgetaire = al.Idalinea,
                                NomAlineaNomenclatureBudgetaire = al.Nomalinea
                            })
                            .ToList()
                    })
                    .ToList()
            };
        }

        public async Task SupprimerAsync(int IdArticleNomenclatureBudgetaire)
        {
            var param = new OracleParameter("p_id", OracleDbType.Int32) { Value = IdArticleNomenclatureBudgetaire };

            await _db.Database.ExecuteSqlRawAsync(
                "DELETE FROM ARTICLE_NOMENCLATURE_BUDGETAIRE_O WHERE ID_ARTICLE_NOMENCLATURE_BUDGETAIRE = :p_id",
                param
            );
        }

        public async Task MettreAJourAsync(ArticleNomenclatureBudgetaireDto articleNomenclatureBudgetaire)
        {
            var json = JsonConvert.SerializeObject(articleNomenclatureBudgetaire);
            var param = new OracleParameter("p_json", OracleDbType.Clob) { Value = json };

            await _db.Database.ExecuteSqlRawAsync(
                "BEGIN AJOUTER_ARTICLE_ET_PARAGRAPHES_ET_ALINEAS_JSON(:p_json); END;",
                param
            );
        }

    }
}
