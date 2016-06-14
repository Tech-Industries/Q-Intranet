﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Dashboard.Models;
using Dashboard.ViewModels;
using System.Web;

namespace Dashboard.APIControllers
{
    public class LogController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        [ResponseType(typeof(List<object>))]
        public List<object> Get()
        {
            return db.Logs.OrderByDescending(o => o.DateCreated).ToList<object>();
        }

        [ResponseType(typeof(List<object>))]
        public void Post(Log l)
        {
            var now = DateTime.Now;
            l.DateCreated = now;
            db.Logs.Add(l);
            db.SaveChangesAsync();
        }

        
    }
}