using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using CleanFoodVietAPI.Data;
using Microsoft.EntityFrameworkCore;
using CleanFoodVietAPI.Data.Entities;

namespace CleanFoodVietAPI.Functions
{
    public class ExpireContractsFn
    {
        private readonly CleanFoodVietDbContext _db;
        private readonly ILogger<ExpireContractsFn> _log;

        public ExpireContractsFn(
            CleanFoodVietDbContext db,
            ILogger<ExpireContractsFn> log)
        {
            _db = db;
            _log = log;
        }

        [Function("ExpireContractsFn")]
        public async Task RunAsync(
            [TimerTrigger("0 */15 * * * *")] TimerInfo timer)
        {
            _log.LogInformation("⏳ ExpireContractsFn started at {Time}", DateTime.UtcNow);

            var affected = await _db.Database
                .ExecuteSqlRawAsync("CALL ReconcileSubscriptions();");

            _log.LogInformation("✅ ExpireContractsFn finished at {Time}; {Count} rows", DateTime.UtcNow, affected);
        }
    }
}
