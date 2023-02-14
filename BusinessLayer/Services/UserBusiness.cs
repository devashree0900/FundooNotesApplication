using System;
using System.Collections.Generic;
using System.Text;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using BusinessLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class UserBusiness: IUserBusiness
    {
        private readonly IUserRepository user;
        public UserBusiness(IUserRepository user)
        {
            this.user = user;
        }

        public UserEntity Register(RegisterModel model)
        {
            return user.Register(model);
        }

        public string Login(LoginModel login)
        {
            return user.Login(login);
        }

        public string ForgetPassword(string Email)
        {
            return user.ForgetPassword(Email);
        }

        public string ResetPassword(ResetPassword reset, string Email)
        {
            return user.ResetPassword(reset, Email);
        }

        public UserTicket CreateTicketForPassword(string EmailId, string token)
        {
            return user.CreateTicketForPassword(EmailId, token);
        }

        public List<UserEntity> GetAllUsers()
        {
            return user.GetAllUsers();  
        }
        public UserEntity GetUserWithId(int id)
        {
            return user.GetUserWithId(id);
        }

    }
}
