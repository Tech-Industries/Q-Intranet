
using Orizon.Web.Data;
//using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class CommentsViewModel
    {
        public long ID { get; set; }
        public int? TypeID { get; set; }
        public int? UserID { get; set; }        
        public DateTime? TimeSubmitted { get; set; }
        public string CommentText { get; set; }
        public string Type { get; set; }




        public static async Task<CommentsViewModel> MapFromAsync(Comment c)
        {
            return await Task.Run(() => { return MapFrom(c); });
        }

        public static async Task<List<CommentsViewModel>> MapFromAsync(ICollection<Comment> c)
        {
            return await Task.Run(() => { return MapFrom(c); });
        }

        public static CommentsViewModel MapFrom(Comment c)
        {
            return new CommentsViewModel
            {
                ID = c.ID,
                TypeID = c.TypeID,
                UserID = c.UserID,
                TimeSubmitted = c.TimeSubmitted,
                CommentText = c.CommentText,
                Type = c.Type

            };
        }

        public static List<CommentsViewModel> MapFrom(ICollection<Comment> c)
        {
            return c.Select(x => MapFrom(x)).ToList();
        }

    }
}