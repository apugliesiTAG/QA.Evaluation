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
    [Route("api/Shipper")]
    [ApiController]
    public class ShipperController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        private IConfiguration _configuration;
        private string _apiurl;

        public ShipperController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IConfiguration configuration)
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
        [HttpGet("PopulateShipper")]
        public async Task<IActionResult> PopulateShipper()
        {
            try
            {
                var httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient
                                                .GetAsync(_apiurl + "/ShippersLookup").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var customers = JsonConvert.DeserializeObject<ExternalApiShipperResponseDto>(result);
                    customers.data.ForEach(c => _repository.Shipper.CreateShipper(_mapper.Map<Shipper>(c)));
                    _logger.LogInfo($"Shippers table were populated.");
                    _repository.Save();
                    return NoContent();
                }
                else
                {
                    return BadRequest("Cant populate shippers data");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PopulateShipper action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("Create")]
        public IActionResult CreateShipper([FromBody] ShipperDto shipper)
        {
            try
            {
                if (shipper == null)
                {
                    _logger.LogError("shipper object sent from client is null.");
                    return BadRequest("shipper object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid shipper object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var shipperEntity = _mapper.Map<Shipper>(shipper);
                _repository.Shipper.CreateShipper(shipperEntity);
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
