using DAL.ContextModels;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;

        public OrderService( IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<List<Order>> GetAll()
        {
            return await _orderRepo.GetAllOrdersWithRelatedData();
        }

        public async Task<Order> GetById(int id)
        {
            return await _orderRepo.GetOrderByIdWithRelatedData(id);
        }

        public async Task<List<Order>> GetOrdersByUserId(string id)
        {
            var result =  await _orderRepo.GetOrdersByUserId(id);
            return result;
        }

        public async Task<bool> DeleteOrder(Order order)
        {
            if (order != null)
            {
                try
                {
                    _orderRepo.Delete(order);
                    await _orderRepo.Save();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            Order result = await _orderRepo.Create(order);
            await _orderRepo.Save();
            return result;

        }
    }
}
