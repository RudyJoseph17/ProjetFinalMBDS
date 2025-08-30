using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanqueProjet.Application.Enums
{
    public enum TypeProjet
    {
        Investissement = 1,
        PreInvestissement = 2,
        RenforcementInstitutionnel = 3
    }

    public enum RoleFirme
    {
        Execution = 1,
        Supervision = 2
    }

    public enum EchelonTerritorial
    {
        National = 1,
        InterDepartemental = 2,
        Departemental = 3,
        InterCommunal = 4,
        Communal = 5
    }
}
