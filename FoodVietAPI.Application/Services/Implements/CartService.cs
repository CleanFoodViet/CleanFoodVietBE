using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.CartDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class CartService : BaseService<CartService>, ICartService
    {
        public CartService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<CartService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<List<CartDTO>> GetRetailerCarts(string retailerId)
        {
            Ulid retailerID = Ulid.Parse(retailerId);
            var retailerCart = await _unitOfWork.GetRepository<Cart>()
                .GetListAsync(
                    include: ct => ct.Include(x => x.Gardener)
                                     .Include(x => x.CartItems).ThenInclude(x => x.Product),
                    predicate: ct => ct.RetailerId == retailerID,
                    selector: ct => new CartDTO
                    {
                        CartId = ct.CartId,
                        GardenerId = ct.GardenerId,
                        GardenerName = ct.Gardener.Name,
                        RetailerId = ct.RetailerId,
                        //UpdatedAt = ct.UpdatedAt,
                        CartItems = _mapper.Map<List<CartItemDTO>>(ct.CartItems)
                    }
                );

            return retailerCart.ToList();
        }

        public async Task ModifyCart(string reatilerId, List<CartDTO> request) 
        {
            Ulid retailerID = Ulid.Parse(reatilerId);

            //Modify Carts Information
            // 1: Load existing data from DB
            var existingCart = await _unitOfWork.GetRepository<Cart>()
                .GetListAsync(include: ca => ca.Include(x => x.CartItems),
                              predicate: ca => ca.RetailerId == retailerID);

            var incommingCarts = _mapper.Map<List<Cart>>(request);

            // 2: Split Carts and modify (add, update, remove)
            var (cartsToRemove, cartsToAdd, cartsToKeep) = CustomListUtil
                .SplitObjectsById(existingCart.ToList(), incommingCarts, cart => cart.CartId);

            // 3: Modify (Create, Update, Remove)
            // Remove:
            _unitOfWork.GetRepository<CartItem>().DeleteRangeAsync(cartsToRemove.SelectMany(x => x.CartItems));
            _unitOfWork.GetRepository<Cart>().DeleteRangeAsync(cartsToRemove);

            // Add:
            foreach (var cart in cartsToAdd)
            {
                cart.CreatedAt = DateTime.UtcNow;
                cart.UpdatedAt = DateTime.UtcNow;

                foreach (var item in cart.CartItems)
                {
                    item.CreatedAt = DateTime.UtcNow;
                    item.UpdatedAt = DateTime.UtcNow;
                }

                await _unitOfWork.GetRepository<Cart>().InsertAsync(cart);
            }

            // Update:
            foreach (var cartToUpdate in cartsToKeep)
            {
                var incomingCartDTO = request.First(c => c.CartId == cartToUpdate.CartId);
                var incomingItems = _mapper.Map<List<CartItem>>(incomingCartDTO.CartItems);
                var existingItems = cartToUpdate.CartItems.ToList();

                cartToUpdate.UpdatedAt = DateTime.UtcNow;

                var (itemsToRemove, itemsToAdd, itemsToKeep) = CustomListUtil.SplitObjectsById(
                    existingItems,
                    incomingItems,
                    i => i.CartItemId
                );

                _unitOfWork.GetRepository<CartItem>().DeleteRangeAsync(itemsToRemove);

                foreach (var item in itemsToAdd)
                {
                    item.CartId = cartToUpdate.CartId;
                    item.CreatedAt = DateTime.UtcNow;
                    item.UpdatedAt = DateTime.UtcNow;
                    await _unitOfWork.GetRepository<CartItem>().InsertAsync(item);
                }

                foreach (var keptItem in itemsToKeep)
                {
                    var incoming = incomingItems.First(x => x.CartItemId == keptItem.CartItemId);

                    keptItem.Price = incoming.Price;
                    keptItem.Quantity = incoming.Quantity;
                    keptItem.ProductUnit = incoming.ProductUnit;
                    keptItem.UpdatedAt = DateTime.UtcNow;
                }
            }

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when modify retailer Cart. (DB error)");
        }
    }
}