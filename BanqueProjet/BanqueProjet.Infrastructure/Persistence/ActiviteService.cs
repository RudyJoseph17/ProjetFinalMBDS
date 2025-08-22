//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BanqueProjet.Application.Interfaces;
//using BanqueProjet.Application.Dtos;
//using BanqueProjet.Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;
//using Oracle.ManagedDataAccess.Client;
//using Shared.Infrastructure.Persistence;

//namespace BanqueProjet.Infrastructure.Persistence
//{
//    public class ActiviteService : BaseService, IActiviteService
//    {
//        public ActiviteService(BanquePDbContext dbContext) : base(dbContext) { }

//        public async Task AddAsync(ActiviteDto activite)
//        {
//            activite.IdActivites = await NextValAsync("SEQ_ACTIVITE");

//            var parameters = new[]
//            {
//                new OracleParameter("p_id_act", OracleDbType.Decimal) { Value = activite.IdActivites },
//                new OracleParameter("p_numero_act", OracleDbType.Decimal) { Value = activite.NumeroActivites },
//                new OracleParameter("p_nom_activ", OracleDbType.Varchar2, 100) { Value = activite.NomActivite ?? (object)DBNull.Value },
//                //new OracleParameter("p_id_projet", OracleDbType.Decimal) { Value = activite.IdIdentificationProjet }
//            };

//            await _dbContext.Database.ExecuteSqlRawAsync(
//                "BEGIN AJOUTER_ACTIVITE(:p_id_act, :p_numero_act, :p_nom_activ, :p_id_projet); END;",
//                parameters);
//            await _dbContext.Database.ExecuteSqlRawAsync("COMMIT");
//        }

//        public async Task UpdateAsync(ActiviteDto dto)
//        {
//            var parameters = new[]
//            {
//                new OracleParameter("p_id_act", OracleDbType.Decimal) { Value = dto.IdActivites },
//                new OracleParameter("p_numero_act", OracleDbType.Decimal) { Value = dto.NumeroActivites },
//                new OracleParameter("p_nom_activ", OracleDbType.Varchar2, 100) { Value = dto.NomActivite ?? (object)DBNull.Value },
//                //new OracleParameter("p_id_projet", OracleDbType.Decimal) { Value = dto.IdIdentificationProjet }
//            };

//            await _dbContext.Database.ExecuteSqlRawAsync(
//                "BEGIN MAJ_ACTIVITE(:p_id_act, :p_numero_act, :p_nom_activ, :p_id_projet); END;",
//                parameters);
//            await _dbContext.Database.ExecuteSqlRawAsync("COMMIT");
//        }

//        public async Task DeleteAsync(int idActivite)
//        {
//            var param = new OracleParameter("p_id_act", OracleDbType.Decimal) { Value = idActivite };
//            await _dbContext.Database.ExecuteSqlRawAsync(
//                "BEGIN SUPPRIMER_ACTIVITE(:p_id_act); END;", param);
//            await _dbContext.Database.ExecuteSqlRawAsync("COMMIT");
//        }

//        //Task IActiviteService.AddAsync(ActiviteDto activite)
//        //{
//        //    throw new NotImplementedException();
//        //}

//        Task<IEnumerable<ActiviteDto>> IActiviteService.GetActiviteAsync()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
