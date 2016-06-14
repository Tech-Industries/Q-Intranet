using System;
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

namespace Dashboard.APIControllers
{
    public class SlideController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();
        
        public object Get(int ID, string Year, string Month)
        {
            return db.IncentiveRollupByPlants.Where(x => x.PlantID == ID && x.Year == Year && x.Month == Month);
        }

        public object Get(int PlantID, string Type)
        {
            if(Type == "Meeting")
            {
                return db.Meetings1.Where(x => x.PlantID == PlantID);
            }
            else
            {
                return null;
            }
        }


    }
}