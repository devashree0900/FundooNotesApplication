using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces;
using RepositoryLayer.Entity;
using ModelLayer;
using System.Linq.Expressions;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;

namespace FundooNotesApplication.Controllers
{
    //[Route("api/user")]  //attribute routed actions 
    [Route("api/[controller]")]

    [ApiController] //to enable behaviours like:Attribute routing requirement. Automatic HTTP 400 responses. Binding source parameter inference
    //here actions will return data instead of views

    //Controller base can return Status code
    //controller alone return view or jason but not status code
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness user;
        public UserController(IUserBusiness user)
        {
            this.user = user;
        }
        [HttpPost]
        [Route("Register")] //whenever any request with this url that is api/register will come will be routed to this controller
        public IActionResult Register(RegisterModel model)
        {
            var registerData = user.Register(model);
            if(registerData != null)
            {
                return Ok(new ResponseModel<UserEntity>{Status = true, Message ="Register Successfull", Data = registerData});
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "Register Failed"});
            }
        }
        [HttpPost("Login")]
        public IActionResult Login(LoginModel model)
        {
            var loginData = user.Login(model);
            if (loginData != null)
            {
                return Ok(new ResponseModel<String> { Status = true, Message = "Login Successfull", Data=loginData });
            }
            else
            {
                return BadRequest(new ResponseModel<String> { Status = false, Message = "Login Failed" });
            }
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var allUsersData = user.GetAllUsers();
            if (allUsersData != null)
            {
                return Ok(new ResponseModel<List<UserEntity>> { Status = true, Message = "Successfull operation", Data = allUsersData });
            }
            else
            {
                return BadRequest(new ResponseModel<List<UserEntity>> { Status = false, Message = "Unsuccessful Operation" });
            }
        }

        [HttpGet("GetUserWithId")]
        public IActionResult GetUserWIthId(int id)
        {
            var User = user.GetUserWithId(id);
            if (User != null)
            {
                return Ok(new ResponseModel<UserEntity> { Status = true, Message = "Successfull", Data = User });
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "Unsuccessful" });
            }
        }

        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(string Email)
        {
            var forget = user.ForgetPassword(Email);
            if (forget != null)
            {
                return Ok(new ResponseModel<String> { Status = true, Message = "Mail sent successfully", Data = forget });
            }
            else
            {
                return BadRequest(new ResponseModel<String> { Status = false, Message = "Mail not sent" });
            }
        }
        //update all the data - put, partial data update - patch
        //whenever u want to update some data not all the data
        [Authorize] //this API will be locked- then u r required to provide a token, otherwise we will get error 401
        [HttpPatch("ResetPassword")]
        public IActionResult ResetPassword(ResetPassword reset)
        {
            var Email = User.Claims.FirstOrDefault(e => e.Type == "EmailId").Value;
           // var Email = User.FindFirst(ClaimTypes.Email).Value;
            var resetData = user.ResetPassword(reset,Email);
            if (resetData != null)
            {
                return Ok(new ResponseModel<String> { Status = true, Message = "Password Reset successful", Data = resetData });
            }
            else
            {
                return BadRequest(new ResponseModel<String> { Status = false, Message = "Password rest unsuccessful" });
            }
        }

        


    }
}
