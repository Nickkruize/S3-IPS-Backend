using DAL;
using DAL.ContextModels;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Repositories
{
    public class OrderItemRepo : GenericRepository<OrderItem>, IOrderItemRepo
    {
        private readonly WebshopContext _context;

        public OrderItemRepo(WebshopContext db) : base(db)
        {
            _context = db;
        }
    }
}
