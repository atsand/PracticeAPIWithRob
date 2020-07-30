using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeWebApi.CommonClasses.Exceptions;
using PracticeWebApi.CommonClasses.Users;
using PracticeWebApi.Services;
using System;
using System.Threading.Tasks;

namespace PracticeWebApi.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        
        [HttpPost("/user")]
        public async Task<IActionResult> AddUser([FromBody]User user)
        {
            try
            {
                var addedUser = await _userService.AddUser(user);
                return Ok(addedUser);
            }
            catch(DuplicateResourceException exception)
            {
                //What type of exception should be thrown here to give the inner message?
                return BadRequest(exception.Message);
            }
            catch(Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        [HttpPut("/user")]
        public async Task<IActionResult> UpdateUser([FromBody]User user)
        {
            try
            {
                await _userService.UpdateUser(user);
                return Ok();
            }
            catch (ResourceNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        [HttpDelete("/user/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute]string id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok();
            }
            catch (ResourceNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        [HttpGet("/user")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
               var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        [HttpGet("/user/{id}")]
        public async Task<IActionResult> FindUserById([FromRoute]string id)
        {
            try
            {
                var user = await _userService.FindUserById(id);
                return Ok(user);
            }
            catch (ResourceNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}