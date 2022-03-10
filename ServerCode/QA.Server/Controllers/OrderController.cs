using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Helpers;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace QA.Server.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        private IConfiguration _configuration;
        private string _apiurl;

        public OrderController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IConfiguration configuration)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
            _apiurl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        [HttpGet("Orders")]
        public IActionResult Orders( [FromQuery] string filter, [FromQuery] int skip,  [FromQuery] int take, 
                            [FromQuery] bool requireTotalCount, [FromQuery] string group, [FromQuery] string totalSummary,
                            [FromQuery] string sort)
        {
            try
            {
                
                var orders = _repository.Order.OrdersLookup(filter, skip, take, sort);
                _logger.LogInfo($"Returned all orders from database.");
                var ordersResult = _mapper.Map<IEnumerable<OrderDto>>(orders);
                var groupOptions = new List<OrderGroup>();
                if (group != null  )
                {
                    if (!group.Contains("OrderDate"))
                    {
                        groupOptions = JsonConvert.DeserializeObject<List<OrderGroup>>(group);
                        if (!groupOptions.FirstOrDefault().selector.Equals("OrderDate"))
                        {
                            return Ok(new { data = getGroupedResult(ordersResult, groupOptions) });
                        }
                    }
                }
                decimal[] sum = new decimal[1];
                sum[0] = ordersResult.Sum(o => o.Freight);
                if (requireTotalCount)
                {
                    int total = _repository.Order.totalCount();
                    return Ok(new { data = ordersResult, summary = sum, totalCount = total });
                }
                else
                {
                    if (totalSummary != null && totalSummary.Length > 0)
                    {
                        return Ok(new { data = ordersResult, summary = sum });
                    }
                    else
                    {
                        return Ok(new { data = ordersResult });
                    }
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside ShippersLookup action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private object getGroupedResult(IEnumerable<OrderDto> ordersResult, List<OrderGroup> groupOptions)
        {
            switch (groupOptions.FirstOrDefault().selector)
            {
                case "ShipCountry":
                    return ordersResult.Select(x => new { key = x.ShipCountry, items = 0, count = 1 }).ToList();
                    break;
                case "Freight":
                    return ordersResult.Select(x => new { key = x.Freight, items = 0, count = 1 }).ToList();
                    break;
            }
            return ordersResult;
        }

        [HttpGet("PopulateOrders")]
        public async Task<IActionResult> PopulateOrders()
        {
            try
            {
                var httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient
                                                .GetAsync(_apiurl + "/Orders").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var Orders = JsonConvert.DeserializeObject<ExternalApiOrderResponseDto>(result);
                    Orders.data.ForEach(c => _repository.Order.CreateOrder(_mapper.Map<Order>(c)));
                   _logger.LogInfo($"Orders table were populated.");
                    _repository.Save();
                    return NoContent();
                }
                else
                {
                    return BadRequest("Cant populate orders data");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PopulateOrders action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("Create")]
        public IActionResult CreateOrder([FromBody] OrderDto order)
        {
            try
            {
                if (order == null)
                {
                    _logger.LogError("order object sent from client is null.");
                    return BadRequest("order object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid order object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var orderEntity = _mapper.Map<Order>(order);
                _repository.Order.CreateOrder(orderEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateOrder action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("UpdateOrder")]
        public IActionResult UpdateOrder([FromForm] OrderFormDto order)
        {
            try
            {
                if (order == null)
                {
                    _logger.LogError("order object sent from client is null.");
                    return BadRequest("order object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid order object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var orderEntity = _repository.Order.FindOrder(order.key);
                if (orderEntity == null)
                {
                    _logger.LogError($"Order with id: {order.key}, hasn't been found in db.");
                    return NotFound();
                }
                JsonConvert.PopulateObject(order.values, orderEntity);
                orderEntity.Freight = 100;
                _repository.Order.UpdateOrder(orderEntity);
                _repository.Save();
                var orderresult = _mapper.Map<OrderDto>(orderEntity);
                return Ok(orderresult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateOrder action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("InsertOrder")]
        public IActionResult InsertOrder([FromForm] OrderFormDto order)
        {
            try
            {
                if (order == null)
                {
                    _logger.LogError("order object sent from client is null.");
                    return BadRequest("order object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid order object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var orderEntity = new Order();
                JsonConvert.PopulateObject(order.values, orderEntity);
                _repository.Order.CreateOrder(orderEntity);
                _repository.Save();
                var orderresult = _mapper.Map<OrderDto>(orderEntity);
                return Ok(orderresult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateOrder action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("DeleteOrder")]
        public IActionResult DeleteOrder([FromForm] OrderFormDto order)
        {
            try
            {
                if (order == null)
                {
                    _logger.LogError("order object sent from client is null.");
                    return BadRequest("order object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid order object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var orderEntity = _repository.Order.FindOrder(order.key);
                if (orderEntity == null)
                {
                    _logger.LogError($"Order with id: {order.key}, hasn't been found in db.");
                    return NotFound();
                }
                _repository.Order.DeleteOrder(orderEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateOrder action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
