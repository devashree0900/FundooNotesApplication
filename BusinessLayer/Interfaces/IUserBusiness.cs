﻿using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IUserBusiness
    {
        public UserEntity Register(RegisterModel model);
        public string Login(LoginModel model);

        public string ForgetPassword(string Email);

        public string ResetPassword(ResetPassword reset, string Email);

        public UserTicket CreateTicketForPassword(string EmailId, string token);

        public List<UserEntity> GetAllUsers();

        public UserEntity GetUserWithId(int id);
    }
}
