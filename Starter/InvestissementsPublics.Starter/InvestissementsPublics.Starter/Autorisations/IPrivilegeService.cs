namespace InvestissementsPublics.Starter.Autorisations
{
    public interface IPrivilegeService
    {
        Task<IList<string>> GetPrivilegesForRoleAsync(string roleId);
        Task<IList<string>> GetPrivilegesForUserAsync(string userId);
        /// <summary>Supprime le cache côté service pour le rôle (appeler après update RolePrivileges).</summary>
        void InvalidateRoleCache(string roleId);
    }
}
