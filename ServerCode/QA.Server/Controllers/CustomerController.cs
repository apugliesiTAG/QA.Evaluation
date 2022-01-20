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
using System.Text;
using System.Threading.Tasks;

namespace QA.Server.Controllers
{
    [Route("api/Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        private IConfiguration _configuration;
        private string _apiurl;
        public CustomerController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IConfiguration configuration)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
            _apiurl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        [HttpGet("CustomersLookup")]
        public IActionResult CustomersLookup()
        {
            try
            {
                var customer = _repository.Customer.CustomersLookup();
                _logger.LogInfo($"Returned all shippers from database.");
                var CustomersResult = _mapper.Map<IEnumerable<CustomerDto>>(customer);
                return Ok(new { data = CustomersResult });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CustomersLookup action: {ex.Message}");
                return StatusCode(500, "Internal server error:" + ex.Message);
            }
        }
        [HttpGet("PopulateCustomer")]
        public async Task<IActionResult> PopulateCustomer()
        {
            try
            {
                var httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient
                                                .GetAsync(_apiurl + "/CustomersLookup").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var customers = JsonConvert.DeserializeObject<ExternalApiCustomerResponseDto>(result);
                    customers.data.ForEach(c => _repository.Customer.CreateCustomer(_mapper.Map<Customer>(c)));
                    _logger.LogInfo($"Customers table were populated.");
                    _repository.Save();
                    return NoContent();
                }
                else 
                {
                    return BadRequest("Cant populate customers data");
                }               
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PopulateCustomer action: {ex.Message}");
                return StatusCode(500, "Internal server error:" + ex.Message);
            }
        }
        [HttpPost("Create")]
        public IActionResult CreateCustomer([FromBody] CustomerDto customer)
        {
            try
            {
                if (customer == null)
                {
                    _logger.LogError("customer object sent from client is null.");
                    return BadRequest("shipper object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid customer object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var customerEntity = _mapper.Map<Customer>(customer);
                _repository.Customer.CreateCustomer(customerEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateCustomer action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
