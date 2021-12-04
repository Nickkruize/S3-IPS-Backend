using DAL.ContextModels;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IOrderItemRepo _ordeItemRepo;

        public OrderService(IOrderItemRepo orderItemRepo, IOrderRepo orderRepo)
        {
            _ordeItemRepo = orderItemRepo;
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

        public async Task<Order> GetByUserId(string id)
        {
            return await _orderRepo.GetOrderByUserId(id);
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
    }
}
