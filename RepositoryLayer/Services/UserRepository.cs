using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using ModelLayer;
using RepositoryLayer.Interfaces;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace RepositoryLayer.Services
{
    public class UserRepository: IUserRepository
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;
        public UserRepository(FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        //Making an entry in the database
        public UserEntity Register(RegisterModel model)
        {
            UserEntity  entity = new UserEntity();
            entity.FirstName= model.FirstName;
            entity.LastName= model.LastName;
            entity.EmailId= model.EmailId;
            entity.Password= EncryptPassword(model.Password);
            context.UserTable.Add(entity);
            int result = context.SaveChanges();
            if(result>0)
            {
                return entity;
            }
            else
            {
                return null;
            }
        }

        public string EncryptPassword(string Password)
        {
            var EncryptPass = System.Text.Encoding.UTF8.GetBytes(Password);
            return System.Convert.ToBase64String(EncryptPass);
        }
        //Token will be in string format
        //This will be http method
        public string Login(LoginModel login)
        {
            string encodedPassword = EncryptPassword(login.Password);
            var IfExists =  this.context.UserTable.Where(a=> a.EmailId==login.EmailId && a.Password==encodedPassword).FirstOrDefault();
            if(IfExists != null)
            {
                var token = GenerateJwtToken(IfExists.EmailId, IfExists.UserId);
                return token;
            }
            else
            {
                return null;
                
            }
        }
        public string ForgetPassword(string Email)
        {
            var EmailCheck = this.context.UserTable.Where(b=>b.EmailId == Email).FirstOrDefault();
            if(EmailCheck != null)
            {
                var token = GenerateJwtToken(EmailCheck.EmailId, EmailCheck.UserId);
                new MSMQ().SendMessage(token,EmailCheck.EmailId,EmailCheck.FirstName);
                return token;
            }
            else
            {
                return null;
            }
        }

        public List<UserEntity> GetAllUsers()
        {
            var userList = this.context.UserTable.ToList();
            return userList;
        }


        public UserEntity GetUserWithId(int id)
        {
            var user = this.context.UserTable.Where(a => a.UserId== id).FirstOrDefault();
            return user;
        }

        public string ResetPassword(ResetPassword reset,string Email)
        {
            try
            {
                if(reset.Password.Equals(reset.ConfirmPassword))
                {
                    var EmailCheck = this.context.UserTable.Where(b => b.EmailId == Email).FirstOrDefault();
                    EmailCheck.Password = EncryptPassword(reset.Password); //database table password
                    context.SaveChanges();
                    return "Reset Done";
                }
                else
                {
                    return "Password does not match";
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateJwtToken(string EmailId, long UserId)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])); // configuration we use to take data from appsetting
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("EmailId",EmailId),
                new Claim("UserId",UserId.ToString())
            };
            var token = new JwtSecurityToken(
               issuer: configuration["Jwt:Issuer"],
               audience: configuration["Jwt:Audience"],
               //claims,
               claims:claims,
               expires: DateTime.Now.AddMinutes(10),
               signingCredentials: credentials
                ) ;
            return new JwtSecurityTokenHandler().WriteToken(token) ;
        }

        public UserTicket CreateTicketForPassword(string EmailId, string token)
        {
            var userCheck = context.UserTable.FirstOrDefault(context => context.EmailId == EmailId);
            if (userCheck != null)
            {
                UserTicket userTicket = new UserTicket
                {
                    FirstName = userCheck.FirstName,
                    LastName = userCheck.LastName,
                    EmailId = EmailId,
                    Token = token,
                    createdAt = DateTime.Now
                };
                return userTicket;
            }
            else { return null; }
        }
    }
}
