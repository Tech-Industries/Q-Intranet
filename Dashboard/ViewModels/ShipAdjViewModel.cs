using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class ShipAdjViewModel
    {
        public long ID { get; set; }
        public string OrderNo { get; set; }
        public DateTime? AdjShipDate { get; set; }
        public string OrderLine { get; set; }
        public DateTime? DateDue { get; set; }



        public static async Task<ShipAdjViewModel> MapFromAsync(ShippingPlanAdjShipDate d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<ShipAdjViewModel>> MapFromAsync(ICollection<ShippingPlanAdjShipDate> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static ShipAdjViewModel MapFrom(ShippingPlanAdjShipDate d)
        {
            return new ShipAdjViewModel
            {
                ID = d.ID,
                OrderNo = d.OrderNo,
                AdjShipDate = d.AdjShipDate,
                OrderLine = d.OrderLine,
                DateDue = d.DateDue
            };
        }

        public static List<ShipAdjViewModel> MapFrom(ICollection<ShippingPlanAdjShipDate> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }

    }
}