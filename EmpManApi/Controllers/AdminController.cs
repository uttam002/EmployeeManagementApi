using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using Data_Layer.Entities;
using Bussiness_Logic_Layer.Interfaces;

namespace EmpManApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public AdminController(IUserService userService, IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _userService = userService;
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/Admin/Employees
        [HttpGet("Employees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        // GET: api/Admin/Departments
        [HttpGet("Departments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        // GET: api/Admin/Employees/Department/{departmentId}
        [HttpGet("Employees/Department/{departmentId}")]
        public async Task<IActionResult> GetEmployeesByDepartment(int departmentId)
        {
            var employees = await _employeeService.GetEmployeesByDepartmentAsync(departmentId);
            return Ok(employees);
        }

        // Additional CRUD operations and services

        // GET: api/Admin/User/{id}
        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // PUT: api/Admin/User/{id}
        [HttpPut("User/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest("User ID in the URL does not match the ID in the body.");
            }

            var result = await _userService.UpdateUserAsync(id, user);
            if (result.Message == "User with ID {id} not found.")
            {
                return NotFound(result);
            }
            else if (result.Message.Contains("Another user with the username"))
            {
                return Conflict(result);
            }
            else if (result.Message == "User updated successfully.")
            {
                return NoContent();
            }

            return StatusCode(500, "A problem happened while handling your request.");
        }
        // DELETE: api/Admin/User/{id}
        [HttpDelete("User/{userName}")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            var result = await _userService.DeleteUserAsync(userName);
            if (result.Message == $"User with UserName {userName} not found.")
            {
                return NotFound(result);
            }
            else if (result.Message == "User deleted successfully.")
            {
                return NoContent();
            }

            return StatusCode(500, "A problem happened while handling your request.");
        }

    }
}
