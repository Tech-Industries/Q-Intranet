using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class MachineStateCurrentViewModel
    {
        public int ID { get; set; }
        public string Machine { get; set; }
        public string Status { get; set; }
        public string Mode { get; set; }

        public static async Task<MachineStateCurrentViewModel> MapFromAsync(MachineStateCurrent m)
        {
            return await Task.Run(() => { return MapFrom(m); });
        }

        public static async Task<List<MachineStateCurrentViewModel>> MapFromAsync(ICollection<MachineStateCurrent> m)
        {
            return await Task.Run(() => { return MapFrom(m); });
        }

        public static MachineStateCurrentViewModel MapFrom(MachineStateCurrent m)
        {
            return new MachineStateCurrentViewModel
            {
                ID = m.ID,
                Machine = m.Machine,
                Status = m.Status,
                Mode = m.Mode
            };
        }

        public static List<MachineStateCurrentViewModel> MapFrom(ICollection<MachineStateCurrent> m)
        {
            return m.Select(x => MapFrom(x)).ToList();
        }

    }

}
