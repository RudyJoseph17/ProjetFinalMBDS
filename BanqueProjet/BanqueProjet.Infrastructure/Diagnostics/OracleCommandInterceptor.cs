using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Data.Common;
using System.Diagnostics;

namespace BanqueProjet.Infrastructure.Diagnostics
{
    public class OracleCommandInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            Debug.WriteLine("➡️ SQL exécuté (Reader) : " + command.CommandText);
            return base.ReaderExecuting(command, eventData, result);
        }

        public override InterceptionResult<int> NonQueryExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<int> result)
        {
            Debug.WriteLine("➡️ SQL exécuté (NonQuery) : " + command.CommandText);
            return base.NonQueryExecuting(command, eventData, result);
        }

        public override InterceptionResult<object> ScalarExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<object> result)
        {
            Debug.WriteLine("➡️ SQL exécuté (Scalaire) : " + command.CommandText);
            return base.ScalarExecuting(command, eventData, result);
        }

        public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
        {
            Debug.WriteLine("❌ SQL en erreur : " + command.CommandText);
            Debug.WriteLine("Erreur Oracle : " + eventData.Exception.Message);
            base.CommandFailed(command, eventData);
        }
    }
}
