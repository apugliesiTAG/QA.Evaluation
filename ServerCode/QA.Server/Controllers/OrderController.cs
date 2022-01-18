using AutoMapper;
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
        [HttpGet("ShippersLookup")]
        public IActionResult ShippersLookup()
        {
            try
            {
                var shipper = _repository.Shipper.ShippersLookup();
                _logger.LogInfo($"Returned all shippers from database.");
                var shippersResult = _mapper.Map<IEnumerable<ShipperDto>>(shipper);
                return Ok(new { data = shippersResult });
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
                    _logger.LogError("shipper object sent from client is null.");
                    return BadRequest("shipper object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid shipper object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var orderEntity = _mapper.Map<Order>(order);
                _repository.Order.CreateOrder(orderEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateShipper action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
