using FluentValidation;
using FoodOrderApi.Application.Security;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Core.Entities;
using FoodOrderApi.Core.Enums;
using FoodOrderApi.Core.ValueObjects;
using FoodOrderApi.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CustomResponseDto<int>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateOrderCommandHandler(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<CustomResponseDto<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
                throw new ValidationException("Kullanıcı kimliği doğrulanamadı. Lütfen giriş yapınız.");

            var restaurantExists = await _context.Restaurants
                .AnyAsync(r => r.Id == request.RestaurantId && !r.IsDeleted, cancellationToken);

            if (!restaurantExists)
                throw new ValidationException("Belirtilen restoran bulunamadı.");

            var requestedMenuItemIds = request.Items.Select(i => i.MenuItemId).Distinct().ToList();

            var menuItems = await _context.MenuItems
                .Where(m => requestedMenuItemIds.Contains(m.Id) && m.RestaurantId == request.RestaurantId && !m.IsDeleted)
                .ToListAsync(cancellationToken);

            if (menuItems.Count != requestedMenuItemIds.Count)
                throw new ValidationException("Sepetteki bazı ürünler bu restorana ait değil veya bulunamadı.");

            var unavailableItem = menuItems.FirstOrDefault(m => !m.IsAvailable);
            if (unavailableItem != null)
                throw new ValidationException($"'{unavailableItem.Name}' şu anda satışta değildir.");

            decimal totalPrice = 0;
            var orderItems = new List<OrderItem>();

            foreach (var itemDto in request.Items)
            {
                var menuItem = menuItems.First(m => m.Id == itemDto.MenuItemId);
                totalPrice += menuItem.Price * itemDto.Quantity;

                orderItems.Add(new OrderItem
                {
                    MenuItemId = menuItem.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = menuItem.Price
                });
            }

            var order = new Order
            {
                UserId = userId.Value,
                RestaurantId = request.RestaurantId,
                TotalPrice = totalPrice,
                Status = OrderStatus.Pending,
                DeliveryAddress = new Address
                {
                    City = request.DeliveryAddress.City,
                    District = request.DeliveryAddress.District,
                    Neighborhood = request.DeliveryAddress.Neighborhood,
                    Street = request.DeliveryAddress.Street,
                    BuildingNumber = request.DeliveryAddress.BuildingNumber,
                    DoorNumber = request.DeliveryAddress.DoorNumber,
                    AddressDirections = request.DeliveryAddress.AddressDirections
                },
                OrderItems = orderItems
            };

            await _context.Orders.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return CustomResponseDto<int>.Success(201, order.Id);
        }
    }
}
