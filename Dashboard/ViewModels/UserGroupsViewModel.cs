using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class UserGroupsViewModel
    {
        public int ID { get; set; }
        public int? UserID { get; set; }
        public int? GroupID { get; set; }


        public static async Task<UserGroupsViewModel> MapFromAsync(UserGroup f)
        {
            return await Task.Run(() => { return MapFrom(f); });
        }

        public static async Task<List<UserGroupsViewModel>> MapFromAsync(ICollection<UserGroup> f)
        {
            return await Task.Run(() => { return MapFrom(f); });
        }

        public static UserGroupsViewModel MapFrom(UserGroup f)
        {
            return new UserGroupsViewModel
            {
                ID = f.ID,
                UserID = f.UserID,
                GroupID = f.GroupID
            };
        }

        public static List<UserGroupsViewModel> MapFrom(ICollection<UserGroup> m)
        {
            return m.Select(x => MapFrom(x)).ToList();
        }



    }

}
