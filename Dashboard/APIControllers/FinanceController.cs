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
using System.Web;

namespace Dashboard.APIControllers
{
    public class FinanceController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        [ResponseType(typeof(List<FinanceViewModel>))]
        public async Task<IHttpActionResult> Get(string fType, string nameContain, string month, string year)
        {
            if (nameContain == null)
            {
                nameContain = "";
            }

            if (fType == "Overtime")
            {
                return Ok(await FinanceViewModel.MapFromAsync(db.OTMonthlyByCompanies.Where(x => x.CompanyCode.Equals(nameContain) && x.Year.Equals(year) && x.Month.Equals(month)).ToList()));
            }
            else if (fType == "Sales")
            {
                return Ok(await FinanceViewModel.MapFromAsync(db.SalesDailyByCompanies.Where(x => x.CompanyCode.Equals(nameContain) && x.Year.Equals(year) && x.Month.Equals(month)).OrderBy(o => o.Date).ToList()));
            }
            else if (fType == "CWO")
            {
                return Ok(await FinanceViewModel.MapFromAsync(db.CWODailyByPlants.Where(x => x.PlantCode.Equals(nameContain) && x.Year.Equals(year) && x.Month.Equals(month)).OrderBy(o => o.Day).ToList()));
            }

            return null;


        }
        public async Task<IHttpActionResult> Get(string fType, string nameContain, string month, string year, string day)
        {
            if (nameContain == "All")
            {
                return Ok(await FinanceViewModel.MapFromAsync(db.SalesStages.Where(x => x.CompanyCode.Contains("") && x.DateInvoiced.Equals(year + month + day)).ToList()));
            }
            else
            {
                if (fType == "MonthSnapShot")
                {
                    return Ok(await FinanceViewModel.MapFromAsync(db.SalesStages.Where(x => (x.CompanyCode.Equals(nameContain) || x.PlantCode.Equals(nameContain)) &&  x.DateInvoiced.Substring(0,6).Equals(year + month)).OrderBy(o => o.DateInvoiced).ToList()));
                }
                else
                {
                    return Ok(await FinanceViewModel.MapFromAsync(db.SalesStages.Where(x => (x.CompanyCode.Equals(nameContain) || x.PlantCode.Equals(nameContain)) && x.DateInvoiced.Equals(year + month + day)).ToList()));
                }


            }


        }
    }
}