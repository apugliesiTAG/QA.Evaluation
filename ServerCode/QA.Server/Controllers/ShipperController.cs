using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        public ShipperController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
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
