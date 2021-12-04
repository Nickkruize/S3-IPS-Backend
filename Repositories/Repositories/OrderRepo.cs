using DAL;
using DAL.ContextModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class OrderRepo : GenericRepository<Order>, IOrderRepo
    {
        private readonly WebshopContext _context;

        public OrderRepo(WebshopContext db) : base(db)
        {
            _context = db;
        }

        public async Task<List<Order>> GetAllOrdersWithRelatedData()
        {
            return await _context.Orders
                .IgnoreQueryFilters()
                .Include(e => e.OrderItems)
                .ThenInclude(o => o.Product)
                .Include(e => e.User)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdWithRelatedData(int id)
        {
            return await _context.Orders
                .Include(e => e.OrderItems)
                .ThenInclude(o => o.Product)
                .Include(e => e.User)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> GetOrderByUserId(string id)
        {
            return await _context.Orders
                .Include(e => e.OrderItems)
                .ThenInclude(o => o.Product)
                .Include(e => e.User)
                .FirstOrDefaultAsync(o => o.User.Id == id);
        }
    }
}
