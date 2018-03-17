using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chat.DAL.Models;
using Chat.DAL.Repositories;

namespace Chat.Web.HtmlHelpers
{
    public static class UsersHelper
    {
        public static IEnumerable<User> Users(this HtmlHelper html, string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return new List<User>().ToArray();

            using (var unitOfWork = new UnitOfWork())
            {
                return unitOfWork.UserRepository.GetAll(userName);
            }
        }
    }
}