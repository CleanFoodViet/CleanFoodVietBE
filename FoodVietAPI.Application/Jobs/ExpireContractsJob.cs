//using CleanFoodVietAPI.Application.Services.Interfaces;
//using CleanFoodVietAPI.Data.Entities;
//using CleanFoodVietAPI.Data.Repositories.Interfaces;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System.Threading.Tasks;

//public class ExpireContractsJob
//{
//    private readonly CleanFoodVietDbContext _db;
//    private readonly ILogger<ExpireContractsJob> _logger;

//    public ExpireContractsJob(
//        CleanFoodVietDbContext db,
//        ILogger<ExpireContractsJob> logger)
//    {
//        _db = db;
//        _logger = logger;
//    }

//    public async Task ExecuteAsync()
//    {
//        _logger.LogInformation("⏳ Calling stored procedure ReconcileSubscriptions() at {Time}", DateTime.UtcNow);

//        // ExecuteSqlRawAsync returns the number of rows affected by the last statement in the proc.
//        var affectedRows = await _db.Database
//                                    .ExecuteSqlRawAsync("CALL ReconcileSubscriptions();");

//        _logger.LogInformation(
//            "✅ Stored procedure ReconcileSubscriptions() completed at {Time}; affected {Count} rows",
//            DateTime.UtcNow,
//            affectedRows);
//    }
//}
