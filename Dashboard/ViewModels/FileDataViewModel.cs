using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class FileDataViewModel
    {
        public string remoteFile { get; set; }
        public string localFile { get; set; }
    }
}