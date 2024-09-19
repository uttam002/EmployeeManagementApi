using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data_Layer;
using Bussiness_Logic_Layer.Services;
using Data_Layer.Entities;
using Microsoft.AspNetCore.Authorization;
using Bussiness_Logic_Layer.Interfaces;

namespace EmpManApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IBALLogin _balLogin;
        public AuthController(IBALLogin balLogin)
        { 
            _balLogin = balLogin;
        }
        ResponseResult result = new ResponseResult();

        [HttpPost("login")]
        public ResponseResult LoginUser([FromBody] LoginModel loginModel)
        {
            try
            {
                result = _balLogin.LoginUser(loginModel.Username, loginModel.Password);
                if (result.Data == null)
                {
                    result.Message = "Login failed. Invalid username or password.";
                    result.Result = ResponseStatus.Error;
                }
                else
                {
                    result.Result = ResponseStatus.Success;
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Error in Login. Check details and try again! {ex.Message}";
                result.Result = ResponseStatus.Error;
            }

            return result;
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public string UserDetails()
        {
            return "Authorized";
        }

    }
    
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
