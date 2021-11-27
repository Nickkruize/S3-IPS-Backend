using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
