using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IOrderRepo : IGenericRepository<Order>
    {
        Task<List<Order>> GetAllOrdersWithRelatedData();
    }
}
