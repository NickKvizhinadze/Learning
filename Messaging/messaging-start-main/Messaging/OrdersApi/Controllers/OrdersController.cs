using AutoMapper;
using Contracts.Events;
using Contracts.Models;
using Contracts.Responses;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Orders.Domain.Entities;
using Orders.Service;
using OrdersApi.Service.Clients;

namespace OrdersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IProductStockServiceClient productStockServiceClient;
        private readonly IMapper mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IRequestClient<VerifyOrder> _requestClient;

        public OrdersController(IOrderService orderService,
            IProductStockServiceClient productStockServiceClient,
            IMapper mapper,
            IPublishEndpoint publishEndpoint,
            ISendEndpointProvider sendEndpointProvider,
            IRequestClient<VerifyOrder> requestClient)
        {
            _orderService = orderService;
            this.productStockServiceClient = productStockServiceClient;
            this.mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
            _requestClient = requestClient;
        }


        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderModel model)
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:order-create-command"));
            await sendEndpoint.Send(model);

            return Accepted();
        }


        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var response = await _requestClient
                .GetResponse<OrderResult, OrderNotFoundResult>(new VerifyOrder
                {
                    Id = id
                });

            if (response.Is(out Response<OrderResult>? incomingMessage))
            {
                return Ok(incomingMessage.Message);
            }
            if (response.Is(out Response<OrderNotFoundResult>? notFound))
            {
                return NotFound(notFound.Message);
            }
            
            return BadRequest();
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            try
            {
                await _orderService.UpdateOrderAsync(order);
            }
            catch
            {
                if (!await _orderService.OrderExistsAsync(id))
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

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }


        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}