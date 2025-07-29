// Application/DTOs/Gardener/SubscriptionContractDetailDTO.cs
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Application.DTOs.AccountDTOs;
using CleanFoodVietAPI.Application.DTOs.ServicePackageDTOs;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace CleanFoodVietAPI.Application.DTOs.Gardener
{
    public class SubscriptionContractDetailDTO
    {
        public Ulid SubscriptionId { get; set; }
        public Ulid GardenerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationInDays { get; set; }
        public string Status { get; set; } = null!;
        public string SubscriptionType { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public AccountInfoDTO Account { get; set; } = null!;
        public ServicePackageDTO ServicePackage { get; set; } = null!;

        public static Expression<Func<SubscriptionContract, SubscriptionContractDetailDTO>> Projection =>
            sc => new SubscriptionContractDetailDTO
            {
                SubscriptionId = sc.SubscriptionId,
                GardenerId = sc.GardenerId,
                StartDate = sc.StartDate,
                EndDate = sc.EndDate,
                DurationInDays = sc.DurationInDays,
                Status = sc.Status!,
                SubscriptionType = sc.SubscriptionType!,
                CreatedAt = sc.CreatedAt,

                Account = AccountInfoDTO.Projection.Compile().Invoke(sc.Account),

                ServicePackage = ServicePackageDTO.Projection.Compile().Invoke(sc.ServicePackage)
            };
    }
}
