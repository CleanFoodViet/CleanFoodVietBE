//using System.Threading.Tasks;
//using CleanFoodVietAPI.Application.Services.Interfaces;
//using CleanFoodVietAPI.Data.Repositories.Interfaces;

//namespace CleanFoodVietAPI.Application.Jobs
//{
//    public static class SubscriptionJobs
//    {
//        public static async Task ReconcileAllGardenersAsync(
//            ISubscriptionReconciler reconciler,
//            IGardenerRepository gardenerRepo)
//        {
//            var ids = await gardenerRepo.GetAllGardenerIdsAsync();
//            foreach (var id in ids)
//                await reconciler.ReconcileAsync(id);
//        }
//    }
//}
