﻿using AutoMapper;
using DAL;
using DAL.ContextModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using S3_webshop.Resources;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly WebshopContext _context;
        private readonly IMapper _mapper;

        public OrderController(WebshopContext context, IMapper mapper, IOrderService orderService)
        {
            _orderService = orderService;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Orders
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OrdersResource>>> GetOrders()
        {
            try
            {
                List<Order> orders = await _orderService.GetAll();
                List<OrdersResource> result = _mapper.Map<List<Order>, List<OrdersResource>>(orders);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }

        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            try
            {
                var order = await _orderService.GetById(id);

                if (order == null)
                {
                    return NotFound();
                }

                OrdersResource result = _mapper.Map<Order, OrdersResource>(order);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        [HttpGet("GetOrders/{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<OrdersResource>>> GetOrders(string userId)
        {
            try
            {
                var order = await _orderService.GetOrdersByUserId(userId);

                if (order == null)
                {
                    return NotFound();
                }

                List<OrdersResource> result = _mapper.Map<List<Order>, List<OrdersResource>>(order);
                if (result.Count == 0)
                {
                    return NotFound("No orders found for this user");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException.Message);
            }
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order )
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_orderService.GetById(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            Order result = await _orderService.CreateOrder(order);

            if (result == null)
            {
                return BadRequest("Order could not be created");
            }

            return CreatedAtAction("GetOrder", new { id = result.Id }, result);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _orderService.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            bool isDeleted = await _orderService.DeleteOrder(order);
            if (isDeleted)
            {
                return NoContent();
            }

            return StatusCode(500, "Error deleting the order");
        }
    }
}
