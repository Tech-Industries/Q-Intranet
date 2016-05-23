using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class LoginViewModel
    {
        public string path { get; set; }
        public string UserName { get; set; }
        public string password { get; set; }
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public static async Task<LoginViewModel> MapFromAsync(User g)
        {
            return await Task.Run(() => { return MapFrom(g); });
        }

        public static async Task<List<LoginViewModel>> MapFromAsync(ICollection<User> g)
        {
            return await Task.Run(() => { return MapFrom(g); });
        }

        public static LoginViewModel MapFrom(User g)
        {
            return new LoginViewModel
            {
                ID = g.ID,
                UserName = g.UserName,
                FirstName = g.FirstName,
                LastName = g.LastName,
                Email = g.Email                    
            };
        }

        public static List<LoginViewModel> MapFrom(ICollection<User> g)
        {
            return g.Select(x => MapFrom(x)).ToList();
        }
    }

}
