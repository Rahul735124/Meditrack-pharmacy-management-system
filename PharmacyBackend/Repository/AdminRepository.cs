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

    public class AdminRepository : IAdminRepository
    {
        private readonly PharmacyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManager<ApplicationUser> UserManager => _userManager;

        public AdminRepository(PharmacyDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<string> AddSupplierAsync(SupplierDTO supplierDto)
        {
            try
            {
                var existingUser = await UserManager.FindByEmailAsync(supplierDto.Email);
                if (existingUser != null)
                    return "Supplier email already exists.";

                var user = new ApplicationUser
                {
                    FullName = supplierDto.Name,
                    Email = supplierDto.Email,
                    UserName = supplierDto.Email,
                    Contact = supplierDto.Contact,
                    NormalizedEmail = supplierDto.Email.ToUpper(),
                    NormalizedUserName = supplierDto.Email.ToUpper()
                };

                var result = await UserManager.CreateAsync(user, supplierDto.Password);

                if (!result.Succeeded)
                    return "Failed to create supplier.";

                await UserManager.AddToRoleAsync(user, "Supplier");

                var supplier = new Supplier
                {
                    UserId = user.Id,
                    Name = supplierDto.Name,
                    Email = supplierDto.Email,
                    Contact = supplierDto.Contact
                };

                await _context.Suppliers.AddAsync(supplier);
                await _context.SaveChangesAsync();
                return "Supplier added successfully.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<List<DrugResponseDTO>> GetAllDrugsAsync()
        {
            try
            {
                var drugs = await _context.Drugs
                .Include(d => d.Supplier)
                    .Select(d => new DrugResponseDTO
                    {
                        Id = d.DrugId,
                        Name = d.Name,
                        Description = d.Description,
                        QuantityAvailable = d.QuantityAvailable,
                        Price = d.Price,
                        ExpiryDate = d.ExpiryDate,
                        SupplierId = d.SupplierId,
                        SupplierName = d.Supplier.Name

                    })
                    .ToListAsync();

                return drugs;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetAllDrugsAsync: {ex.Message}");
            }
        }

        public async Task<string> AddDrugAsync(DrugDTO dto)
        {
            // Assuming you are fetching the supplier by ID
            var supplier = await _context.Suppliers.FindAsync(dto.SupplierId);
            if (supplier == null)
            {
                return "Supplier not found!";
            }

            var drug = new Drug
            {
                Name = dto.Name,
                Description = dto.Description,
                QuantityAvailable = dto.QuantityAvailable,
                Price = dto.Price,
                ExpiryDate = dto.ExpiryDate,
                SupplierId = dto.SupplierId,
                Supplier = supplier  // Associate the drug with the supplier
            };

            _context.Drugs.Add(drug);
            await _context.SaveChangesAsync();

            return "Drug added successfully!";
        }


        public async Task<List<OrderDTO>> GetNewOrdersAsync()
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => !o.IsVerified)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Drug)
                    .Where(o => !o.IsVerified)
                    .ToListAsync();

                return orders.Select(o => new OrderDTO
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemsResponseDTO
                    {
                        OrderItemId = oi.OrderItemId,
                        DrugId = oi.DrugId,
                        DrugName = oi.Drug.Name,
                        Quantity = oi.Quantity,
                        IsInStock = oi.Drug.QuantityAvailable >= oi.Quantity,
                        IsRequestedFromSupplier = oi.IsRequestedFromSupplier
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetNewOrdersAsync: {ex.Message}");
            }
        }


        public async Task<string> ClassifyOrderItemAsync(int orderItemId)
        {
            try
            {
                var orderItem = await _context.OrderItems.FindAsync(orderItemId);
                if (orderItem == null)
                    return "Order Item not found.";

                orderItem.IsRequestedFromSupplier = true;
                await _context.SaveChangesAsync();
                return "Order item classified and supplier requested.";
            }
            catch (Exception ex)
            {
                return $"Error in ClassifyOrderItemAsync: {ex.Message}";
            }
        }

        public async Task<string> VerifyOrderAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Drug)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                    return "Order not found.";

                foreach (var orderItem in order.OrderItems)
                {
                    var drug = await _context.Drugs.FindAsync(orderItem.DrugId);
                    if (drug == null)
                        continue;

                    if (drug.QuantityAvailable < orderItem.Quantity)
                    {
                        return $"Insufficient stock for drug: {drug.Name}. Cannot verify order.";
                    }

                    drug.QuantityAvailable -= orderItem.Quantity;
                }

                order.IsVerified = true;
                await _context.SaveChangesAsync();

                return "Order verified and stock updated.";
            }
            catch (Exception ex)
            {
                return $"Error in VerifyOrderAsync: {ex.Message}";
            }
        }


        public async Task<string> PickOrderAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                    return "Order not found.";

                if (!order.IsVerified)
                    return "Cannot pick up order before verification.";

                if (order.IsPickedUp)
                    return "Order already picked up.";

                order.IsPickedUp = true;
                await _context.SaveChangesAsync();
                return "Order marked as picked.";
            }
            catch (Exception ex)
            {
                return $"Error in PickOrderAsync: {ex.Message}";
            }
        }

        public async Task<List<SupplierResponseDTO>> GetAllSuppliersAsync()
        {
            var suppliers = await _context.Suppliers
                .Select(s => new SupplierResponseDTO
                {
                    Id = s.SupplierId,
                    Name = s.Name,
                    Email = s.Email,
                    Contact = s.Contact
                }).ToListAsync();

            return suppliers;
        }

        public async Task<string> EditDrugAsync(int drugId, DrugDTO dto)
        {
            var drug = await _context.Drugs.FindAsync(drugId);
            if (drug == null)
                return "Drug not found.";

            drug.Name = dto.Name;
            drug.Description = dto.Description;
            drug.QuantityAvailable = dto.QuantityAvailable;
            drug.Price = dto.Price;
            drug.ExpiryDate = dto.ExpiryDate;
            drug.SupplierId = dto.SupplierId;

            await _context.SaveChangesAsync();
            return "Drug updated successfully.";
        }

        public async Task<string> DeleteDrugAsync(int drugId)
        {
            var drug = await _context.Drugs.FindAsync(drugId);
            if (drug == null)
                return "Drug not found.";

            _context.Drugs.Remove(drug);
            await _context.SaveChangesAsync();
            return "Drug deleted successfully.";
        }

        public async Task<string> EditSupplierAsync(int supplierId, SupplierDTO dto)
        {
            var supplier = await _context.Suppliers.FindAsync(supplierId);
            if (supplier == null)
                return "Supplier not found.";

            supplier.Name = dto.Name;
            supplier.Email = dto.Email;
            supplier.Contact = dto.Contact;

            await _context.SaveChangesAsync();
            return "Supplier updated successfully.";
        }

        public async Task<string> DeleteSupplierAsync(int supplierId)
        {
            try
            {
                var supplier = await _context.Suppliers.FindAsync(supplierId);
                if (supplier == null)
                    return "Supplier not found.";

                // Get user by email or userId
                var user = await _userManager.FindByEmailAsync(supplier.Email);
                if (user == null)
                    return "Associated user not found.";

                // Step 1: Delete user (Identity does its own SaveChanges)
                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    return $"User delete failed: {string.Join(", ", deleteResult.Errors.Select(e => e.Description))}";
                }

                // Step 2: Reload the supplier entity again, so EF has a fresh tracked instance
                var freshSupplier = await _context.Suppliers.FindAsync(supplierId);
                if (freshSupplier != null)
                {
                    _context.Suppliers.Remove(freshSupplier);
                    await _context.SaveChangesAsync();
                }

                return "Supplier and user deleted successfully.";
            }
            catch (Exception ex)
            {
                return $"Error in DeleteSupplierAsync: {ex.Message}";
            }
        }



        // public async Task<SalesReportResponseDTO> GetSalesReportAsync(DateTime startDate, DateTime endDate)
        // {
        //     try
        //     {
        //         endDate = endDate.Date.AddDays(1).AddTicks(-1); // Include full day

        //         var orders = await _context.Orders
        //             .Where(o => o.IsVerified == true && o.OrderDate >= startDate && o.OrderDate <= endDate)
        //             .Include(o => o.OrderItems)
        //                 .ThenInclude(oi => oi.Drug)
        //             .ToListAsync();

        //         var reportOrders = orders.Select(order => new SalesReportDTO
        //         {
        //             OrderId = order.OrderId,
        //             OrderDate = order.OrderDate,
        //             Items = order.OrderItems.Select(item => new SalesReportItemDTO
        //             {
        //                 DrugName = item.Drug.Name,
        //                 Quantity = item.Quantity,
        //                 PricePerUnit = item.Drug.Price,
        //                 ItemTotalPrice = item.Quantity * item.Drug.Price
        //             }).ToList(),
        //             TotalOrderAmount = order.OrderItems.Sum(item => item.Quantity * item.Drug.Price)
        //         }).ToList();

        //         var totalSales = reportOrders.Sum(o => o.TotalOrderAmount);

        //         return new SalesReportResponseDTO
        //         {
        //             TotalSales = totalSales,
        //             Orders = reportOrders
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new Exception($"Error generating sales report: {ex.Message}");
        //     }
        // }

        public async Task<SalesReportResponseDTO> GetSalesReportAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                endDate = endDate.Date.AddDays(1).AddTicks(-1); // Include full day

                var orders = await _context.Orders
                    .Where(o => o.IsVerified == true && o.OrderDate >= startDate && o.OrderDate <= endDate)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Drug)
                    .Include(o => o.User)
                    .ToListAsync();

                var reportOrders = orders.Select(order => new SalesReportDTO
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    DoctorName = order.User.FullName,
                    DoctorEmail = order.User.Email,
                    DoctorContact = order.User.Contact,
                    Items = order.OrderItems.Select(item => new SalesReportItemDTO
                    {
                        DrugName = item.Drug.Name,
                        Quantity = item.Quantity,
                        PricePerUnit = item.Drug.Price,
                        ItemTotalPrice = item.Quantity * item.Drug.Price
                    }).ToList(),
                    TotalOrderAmount = order.OrderItems.Sum(item => item.Quantity * item.Drug.Price)
                }).ToList();

                var totalSales = reportOrders.Sum(o => o.TotalOrderAmount);

                return new SalesReportResponseDTO
                {
                    TotalSales = totalSales,
                    Orders = reportOrders
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating sales report: {ex.Message}");
            }
        }

        public async Task<List<SupplierOrderStatusDTO>> GetSupplierOrderItemStatusesAsync()
        {
            return await _context.OrderItems
                .Include(i => i.Drug)
                .Include(i => i.Order)
                .Where(i => i.IsRequestedFromSupplier)
                .Select(i => new SupplierOrderStatusDTO
                {
                    OrderItemId = i.OrderItemId,
                    DrugName = i.Drug.Name,
                    Quantity = i.Quantity,
                    OrderDate = i.Order.OrderDate,
                    Status = i.IsSupplierConfirmed ? "Confirmed" : "Requested"
                })
                .ToListAsync();
        }

        public async Task<List<OrderDTO>> GetVerifiedOrdersAsync()
        {
            var orders = await _context.Orders
                .Where(o => o.IsVerified)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Drug)
                .ToListAsync();

            return orders.Select(o => new OrderDTO
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                IsPickedUp = o.IsPickedUp,
                IsVerified = o.IsVerified,
                OrderItems = o.OrderItems.Select(oi => new OrderItemsResponseDTO
                {
                    DrugId = oi.DrugId,
                    DrugName = oi.Drug.Name,
                    Quantity = oi.Quantity
                }).ToList()
            }).ToList();
        }

        public async Task<IEnumerable<DrugResponseDTO>> GetExpiredDrugsAsync()
        {
            var indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var indiaTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, indiaTimeZone);

            var expiredDrugs = await _context.Drugs
                .Where(d => d.ExpiryDate < indiaTime)
                .Include(d => d.Supplier)
                .ToListAsync();

            return expiredDrugs.Select(d => new DrugResponseDTO
            {
                Id = d.DrugId,
                Name = d.Name,
                Description = d.Description,
                QuantityAvailable = d.QuantityAvailable,
                Price = d.Price,
                ExpiryDate = d.ExpiryDate,
                SupplierId = d.SupplierId,
                SupplierName = d.Supplier?.Name
            }).ToList();
        }





    }
}

