using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class UsersViewModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? SupervisorID { get; set; }

        public static async Task<UsersViewModel> MapFromAsync(User g)
        {
            return await Task.Run(() => { return MapFrom(g); });
        }

        public static async Task<List<UsersViewModel>> MapFromAsync(ICollection<User> g)
        {
            return await Task.Run(() => { return MapFrom(g); });
        }

        public static UsersViewModel MapFrom(User g)
        {
            return new UsersViewModel
            {
                ID = g.ID,
                UserName = g.UserName,
                FirstName = g.FirstName,
                LastName = g.LastName,
                Email = g.Email,
                SupervisorID = g.SupervisorID
            };
        }

        public static List<UsersViewModel> MapFrom(ICollection<User> g)
        {
            return g.Select(x => MapFrom(x)).ToList();
        }

    }

}
