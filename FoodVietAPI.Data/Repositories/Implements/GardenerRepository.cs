//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using CleanFoodVietAPI.Data.Entities;
//using CleanFoodVietAPI.Data.Enums.AccountEnums;
//using CleanFoodVietAPI.Data.Repositories.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace CleanFoodVietAPI.Data.Repositories.Implements
//{
//    public class GardenerRepository : IGardenerRepository
//    {
//        private readonly CleanFoodVietDbContext _db;

//        public GardenerRepository(CleanFoodVietDbContext db)
//        {
//            _db = db;
//        }

//        public async Task<IReadOnlyList<Ulid>> GetAllGardenerIdsAsync()
//        {
//            var gardenerRole = AccountRoleEnum.GARDENER.ToString();

//            return await _db.Accounts
//                .AsNoTracking()
//                .Where(a => a.Role != null && a.Role.Name == gardenerRole)
//                .Select(a => a.AccountId)
//                .ToListAsync();
//        }
//    }
//}
