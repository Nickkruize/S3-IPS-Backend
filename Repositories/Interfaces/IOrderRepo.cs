using DAL.ContextModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IOrderRepo : IGenericRepository<Order>
    {
        Task<List<Order>> GetAllOrdersWithRelatedData();
        Task<Order> GetOrderByIdWithRelatedData(int id);
        Task<Order> GetOrderByUserId(string id);
    }
}
