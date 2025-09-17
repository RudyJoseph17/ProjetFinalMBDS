using InvestissementsPublics.Starter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace InvestissementsPublics.Starter.Autorisations
{
    public class PrivilegeService: IPrivilegeService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(30),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6)
        };

        public PrivilegeService(ApplicationDbContext db, IMemoryCache cache)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IList<string>> GetPrivilegesForRoleAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId)) return Array.Empty<string>();

            var cacheKey = GetRoleCacheKey(roleId);
            if (_cache.TryGetValue<IList<string>>(cacheKey, out var cached)) return cached;

            var list = await _db.RolePrivileges
                                .AsNoTracking()
                                .Where(rp => rp.RoleId == roleId)
                                .Include(rp => rp.Privilege)
                                .Select(rp => rp.Privilege!.Name)
                                .ToListAsync();

            var result = list.Distinct().ToList();
            _cache.Set(cacheKey, result, _cacheOptions);
            return result;
        }

        public async Task<IList<string>> GetPrivilegesForUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return Array.Empty<string>();

            // récupère roleIds depuis AspNetUserRoles
            var roleIds = await _db.UserRoles
                                   .AsNoTracking()
                                   .Where(ur => ur.UserId == userId)
                                   .Select(ur => ur.RoleId)
                                   .ToListAsync();

            if (!roleIds.Any()) return Array.Empty<string>();

            var all = new List<string>();
            foreach (var r in roleIds)
            {
                var p = await GetPrivilegesForRoleAsync(r);
                all.AddRange(p);
            }

            return all.Distinct().ToList();
        }

        public void InvalidateRoleCache(string roleId)
        {
            if (string.IsNullOrEmpty(roleId)) return;
            _cache.Remove(GetRoleCacheKey(roleId));
        }

        private static string GetRoleCacheKey(string roleId) => $"roleprivs_{roleId}";
    }
}
