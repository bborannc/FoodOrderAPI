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
            // 1. Kullanıcı Kimlik Doğrulaması
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
                throw new ValidationException("Kullanıcı kimliği doğrulanamadı. Lütfen giriş yapınız.");

            // 2. Restoran Varlık ve Aktiflik Kontrolü
            var restaurant = await _context.Restaurants
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.RestaurantId && !r.IsDeleted, cancellationToken);

            if (restaurant == null)
                throw new ValidationException($"'{request.RestaurantId}' numaralı restoran bulunamadı.");

            if (!restaurant.IsActive)
                throw new ValidationException($"'{restaurant.Name}' şu anda sipariş kabul etmemektedir (Restoran kapalı/pasif).");

            // 3. Menü Ürünleri Kontrolü
            var requestedMenuItemIds = request.Items.Select(i => i.MenuItemId).Distinct().ToList();

            var menuItems = await _context.MenuItems
                .AsNoTracking()
                .Where(m => requestedMenuItemIds.Contains(m.Id) && m.RestaurantId == request.RestaurantId && !m.IsDeleted)
                .ToListAsync(cancellationToken);

            if (menuItems.Count != requestedMenuItemIds.Count)
                throw new ValidationException("Sepetteki bazı ürünler bu restorana ait değil veya bulunamadı.");

            var unavailableItem = menuItems.FirstOrDefault(m => !m.IsAvailable);
            if (unavailableItem != null)
                throw new ValidationException($"'{unavailableItem.Name}' şu anda satışta değildir.");

            // 4. Kalem Hesaplama
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

            // 5. Sipariş Nesnesini Oluşturma
            var order = new Order
            {
                UserId = userId.Value,
                RestaurantId = request.RestaurantId,
                Restaurant = null, // EF Core'un yeni restoran üretmesini garanti seviyesinde engellemek için null atanır
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
