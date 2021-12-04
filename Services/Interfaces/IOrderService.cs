using DAL.ContextModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetById(int id);
        Task<List<Order>> GetAll();
        Task<Order> GetByUserId(string id);
        Task<bool> DeleteOrder(Order order);
    }
}
