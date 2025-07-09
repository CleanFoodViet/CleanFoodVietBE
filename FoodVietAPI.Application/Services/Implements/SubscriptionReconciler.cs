//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using CleanFoodVietAPI.Application.Services.Interfaces;
//using CleanFoodVietAPI.Data;
//using CleanFoodVietAPI.Data.Entities;
//using CleanFoodVietAPI.Data.Enums.ServiceFeatureEnums;
//using Microsoft.EntityFrameworkCore;

//namespace CleanFoodVietAPI.Application.Services.Implements
//{
//    public class SubscriptionReconciler : ISubscriptionReconciler
//    {
//        private readonly CleanFoodVietDbContext _db;

//        public SubscriptionReconciler(CleanFoodVietDbContext db)
//        {
//            _db = db;
//        }

//        public async Task ReconcileAsync(Ulid gardenerId)
//        {
//            var now = DateTime.UtcNow;

//            // 1) expire by date
//            var active = await _db.SubscriptionContracts
//                .Where(c => c.GardenerId == gardenerId && c.Status == "ACTIVE")
//                .OrderBy(c => c.StartDate)
//                .FirstOrDefaultAsync();

//            if (active != null && active.EndDate <= now)
//            {
//                active.Status = "EXPIRED";
//                await _db.SaveChangesAsync();
//            }

//            // 2) expire by quota
//            if (active != null)
//            {
//                var quota = await _db.SubscriptionContractBenefits
//                    .Where(b =>
//                        b.SubscriptionContractId == active.SubscriptionId &&
//                        b.BenefitType == ServiceFeatureActionEnum.POST_QUOTA.ToString())
//                    .FirstOrDefaultAsync();

//                if (quota != null && quota.RemainingValue <= 0)
//                {
//                    active.Status = "EXPIRED";
//                    await _db.SaveChangesAsync();
//                }
//            }

//            // 3) flip next pending → ACTIVE, insert benefits
//            if (active == null || active.Status == "EXPIRED")
//            {
//                var next = await _db.SubscriptionContracts
//                    .Where(c =>
//                        c.GardenerId == gardenerId &&
//                        c.Status == "PENDING" &&
//                        c.StartDate <= now)
//                    .OrderBy(c => c.StartDate)
//                    .FirstOrDefaultAsync();

//                if (next != null)
//                {
//                    next.Status = "ACTIVE";
//                    next.StartDate = now;
//                    next.EndDate = now.AddDays(next.DurationInDays);

//                    // ←— UPDATED: join through PackageServiceFeature
//                    var packageFeatures = await _db.PackageServiceFeatures
//                        .Include(psf => psf.ServiceFeature)
//                        .Where(psf =>
//                            psf.ServicePackageId == next.ServicePackageId &&
//                            psf.ServiceFeature.Status == ServiceFeatureStatusEnum.ACTIVE.ToString())
//                        .ToListAsync();

//                    foreach (var psf in packageFeatures)
//                    {
//                        var feature = psf.ServiceFeature;
//                        _db.SubscriptionContractBenefits.Add(new SubscriptionContractBenefit
//                        {
//                            SubscriptionContractId = next.SubscriptionId,
//                            BenefitType = feature.Action,       // or feature.FeatureType.ToString()
//                            DefaultValue = feature.DefaultValue,
//                            RemainingValue = feature.DefaultValue
//                        });
//                    }

//                    await _db.SaveChangesAsync();
//                }
//            }
//        }
//    }
//}