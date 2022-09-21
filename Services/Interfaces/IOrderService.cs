using DAL.ContextModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetById(int id);
        Task<List<Order>> GetAll();
        Task<Order> GetByUserId(string id);
        Task<List<Order>> GetOrdersByUserId(string id);
        Task<Order> GetOrderByUserId(string id);
        Task<bool> DeleteOrder(Order order);
    }
}
