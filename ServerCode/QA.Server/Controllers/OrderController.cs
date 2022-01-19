﻿using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
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
        public IActionResult Orders()
        {
            try
            {
                var orders = _repository.Order.OrdersLookup();
                _logger.LogInfo($"Returned all orders from database.");
                var ordersResult = _mapper.Map<IEnumerable<OrderDto>>(orders);
                decimal[] sum = new decimal[1];
                sum[0] = ordersResult.Sum(o => o.Freight);
                return Ok(new { data = ordersResult, summary = sum  });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside ShippersLookup action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
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
    }
}