using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Schedule.Models;

namespace Schedule.Classes
{
    public class User
    {
        RemindEntities RE = new RemindEntities();

        public void AllUsers()
        {

            
        }
        
        public Models.User OneUser(int id)
        {
            var auser = RE.Users.First(p => p.id == id);
            return auser;
        }
        
        public void CreateUser()
        {

           
        }
        
        public void EditUser()
        {

            
        }
        
        public void DeleteUser()
        {

            
        }

        public int Login(UserLogin userLogin)
        {
            var user = RE.Users.First(p => p.username == userLogin.Username);
            int isTrue = user.password == userLogin.Password ? user.id : 0;

            return isTrue;
        }
    }
}