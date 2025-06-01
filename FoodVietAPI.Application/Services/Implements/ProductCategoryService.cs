using AutoMapper;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Implements
{

    public class ProductCategoryService : BaseService<ProductCategoryService>, IProductCategoryService
    {
        public ProductCategoryService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<ProductCategoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task GetProductCategoryList()
        {

        }
    }
}
