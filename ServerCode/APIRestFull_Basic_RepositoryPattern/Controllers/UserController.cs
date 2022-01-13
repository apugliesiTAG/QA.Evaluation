using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRestFull_Basic_RepositoryPattern.Controllers
{
    [Route("api/user/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        public UserController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _repository.User.GetAllUsers();
                _logger.LogInfo($"Returned all users from database.");
                var usersResult = _mapper.Map<IEnumerable<UserDto>>(users);
                return Ok(usersResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllUsers action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("Get/{id}", Name = "GetUserById")]
        public IActionResult GetUserById(Guid id)
        {
            try
            {
                var user = _repository.User.GetUserById(id);
                if (user == null)
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned owner with id: {id}");
                    var userResult = _mapper.Map<UserDto>(user);
                    return Ok(userResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetUserById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Create")]
        public IActionResult CreateUser([FromBody] UserForCreationDto user)
        {
            try
            {
                if (user == null)
                {
                    _logger.LogError("User object sent from client is null.");
                    return BadRequest("User object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid user object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var userEntity = _mapper.Map<User>(user);
                _repository.User.CreateUser(userEntity);
                userEntity.State = false;
                userEntity.DateUpdatePassword = DateTime.Now;
                userEntity.DateValidityPassword = DateTime.Now.AddMonths(3);
                _repository.Save();
                var createdUser = _mapper.Map<UserDto>(userEntity);
                return CreatedAtRoute("GetUserById", new { id = createdUser.UserId }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateUser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("Update/{id}")]
        public IActionResult UpdateUser(Guid id, [FromBody] UserForUpdateDto user)
        {
            try
            {
                if (user == null)
                {
                    _logger.LogError("User object sent from client is null.");
                    return BadRequest("User object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid user object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var userEntity = _repository.User.GetUserById(id);
                if (userEntity == null)
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                _mapper.Map(user, userEntity);
                _repository.User.UpdateUser(userEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateUser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
