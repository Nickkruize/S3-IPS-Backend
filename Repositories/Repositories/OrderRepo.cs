using DAL;
using DAL.ContextModels;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Repositories
{
    public class OrderRepo : GenericRepository<Order>, IOrderRepo
    {
        private readonly WebshopContext _context;

        public OrderRepo(WebshopContext db) : base(db)
        {
            _context = db;
        }
    }
}
