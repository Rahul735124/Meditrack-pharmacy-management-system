using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PharmacyBackend.Data;
using PharmacyBackend.DTOs;
using PharmacyBackend.Interface;
using PharmacyBackend.Models;

namespace PharmacyBackend.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly PharmacyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorRepository(PharmacyDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<DoctorDrugDTO>> GetAllDrugsAsync()
        {
            var drugs = await _context.Drugs
                .Select(d => new DoctorDrugDTO
                {
                    Id = d.DrugId,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    ExpiryDate = d.ExpiryDate,

                }).ToListAsync();

            return drugs;
        }

        public async Task<OrderSummaryDTO> PlaceOrderAsync(string userId, PlaceOrderDTO placeOrderDTO)
        {
            try
            {
                // Validate inputs
                foreach (var item in placeOrderDTO.OrderItems)
                {
                    if (item.Quantity <= 0)
                        throw new ArgumentException($"Quantity must be greater than 0 for DrugId: {item.DrugId}");

                    var drugExists = await _context.Drugs.AnyAsync(d => d.DrugId == item.DrugId);
                    if (!drugExists)
                        throw new ArgumentException($"Invalid DrugId: {item.DrugId}. Drug does not exist.");
                }

                // Create Order
                var indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                var order = new Order
                {
                    UserId = userId,
                    OrderDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, indianTimeZone),
                    IsVerified = false,
                    IsPickedUp = false
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync(); // So that OrderId is generated

                List<OrderItemSummaryDTO> itemSummaries = new List<OrderItemSummaryDTO>();
                decimal totalCost = 0;

                // Add OrderItems
                foreach (var item in placeOrderDTO.OrderItems)
                {
                    var drug = await _context.Drugs.FindAsync(item.DrugId);

                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderId,
                        DrugId = item.DrugId,
                        Quantity = item.Quantity,
                        IsRequestedFromSupplier = false,
                        IsSupplierConfirmed = false
                    };

                    await _context.OrderItems.AddAsync(orderItem);

                    var itemTotal = drug.Price * item.Quantity;
                    totalCost += itemTotal;

                    itemSummaries.Add(new OrderItemSummaryDTO
                    {
                        DrugName = drug.Name,
                        Quantity = item.Quantity,
                        PricePerUnit = drug.Price,
                        ItemTotalCost = itemTotal
                    });
                }

                await _context.SaveChangesAsync();

                return new OrderSummaryDTO
                {
                    OrderItems = itemSummaries,
                    TotalCost = totalCost
                };
            }
            catch (ArgumentException argEx)
            {
                throw new Exception($"Validation error: {argEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error placing order: {ex.Message}");
            }
        }


        public async Task<List<DoctorOrderStatusDTO>> GetMyOrdersAsync(string userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Drug)
                .Select(o => new DoctorOrderStatusDTO
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    IsVerified = o.IsVerified,
                    IsPickedUp = o.IsPickedUp,
                    Items = o.OrderItems.Select(oi => new DoctorOrderItemDTO
                    {
                        DrugName = oi.Drug.Name,
                        Quantity = oi.Quantity,
                        Price = oi.Drug.Price
                    }).ToList()
                })
                .ToListAsync();

            return orders;
        }

        public async Task<ApplicationUser> GetDoctorByIdAsync(string userId)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> UpdateDoctorProfileAsync(ApplicationUser user, string newEmail)
        {
            if (user.Email != newEmail)
            {
                var emailResult = await _userManager.SetEmailAsync(user, newEmail);
                if (!emailResult.Succeeded)
                    return false;
            }

            var updateResult = await _userManager.UpdateAsync(user);
            return updateResult.Succeeded;
        }

    }
}