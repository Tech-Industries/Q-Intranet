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
using System.Web.Http.Results;
using PdfSharp;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Pdf;
using MigraDoc.Rendering;
using System.Diagnostics;
using System.IO;

namespace Dashboard.APIControllers
{


    public class UtilityController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        #region ADInfo

        [Route("api/v1/utility/adinfo")]
        [HttpGet]
        public async Task<IHttpActionResult> GetADInfo()
        {

            var ADInfo = await db.ADInfo.ToListAsync();
            if (!ADInfo.Any())
            {
                return NotFound();
            }

            //foreach (ADInfo i in ADInfo)
            //{

            //    byte[] thumbnailInBytes = (byte[])i.thumbnailPhoto;
            //    i.thumbnailPhoto = thumbnailInBytes;
            //}
            return Ok(ADInfo);
        }

        //[Route("api/v1/operations/opportunities/{id:int}")]
        //[HttpGet]
        //public async Task<IHttpActionResult> GetOpportunity(int id)
        //{

        //    var opps = await db.Opportunities.FindAsync(id);
        //    if (opps == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(opps);
        //}

        //[Route("api/v1/operations/opportunities")]
        //[HttpPost]
        //public async Task<IHttpActionResult> PostOpportunities(Opportunity o)
        //{
        //    db.Opportunities.Add(o);
        //    await db.SaveChangesAsync();

        //    return Ok(o);
        //}

        //[Route("api/v1/operations/opportunities/{id:int}")]
        //[HttpPut]
        //public async Task<IHttpActionResult> PutOpportunities(int id, Opportunity o)
        //{
        //    Opportunity oldOp = await db.Opportunities.FindAsync(id);


        //    if (oldOp == null || o == null)
        //    {
        //        return NotFound();
        //    }
        //    try
        //    {
        //        var entry = db.Entry(oldOp);

        //        oldOp = o;

        //        await db.SaveChangesAsync();
        //    }
        //    catch (Exception e)
        //    {
        //        return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
        //    }

        //    return Ok(oldOp);
        //}

        //[Route("api/v1/operations/opportunities/{id:int}")]
        //[HttpDelete]
        //public async Task<IHttpActionResult> DeleteOpportunity(int id)
        //{
        //    try
        //    {
        //        db.Opportunities.Remove(await db.Opportunities.FindAsync(id));
        //        await db.SaveChangesAsync();
        //    }
        //    catch
        //    {
        //        return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
        //    }
        //    return Ok();
        //}
        #endregion

        #region ZipFiles

        public class Folder
        {
            public string Parent { get; set; }
            public string Name { get; set; }
            public string FullPath { get; set; }
            public int Level { get; set; }
        }

        [Route("api/v1/utility/zipfiles/filestructure")]
        [HttpGet]
        public async Task<IHttpActionResult> GetFileStructure()
        {
            var root = @"\\crp-utl01.orizonaero.local\FTP";
            DirectoryInfo dirInfo = new DirectoryInfo(root);
            var subsLeft = true;
            
            DirSearch(@"\\crp-utl01.orizonaero.local\FTP");
            


            return Ok(subDirs);
        }
        #endregion
        
        private List<Folder> subDirs = new List<Folder>();
        private void DirSearch(string sDir)
        {
            foreach (string d in Directory.GetDirectories(sDir))
            {
                int levels = d.Split('\\').Count()-5;
                subDirs.Add(new Folder { Parent =  sDir.Split('\\').Last(), Name = d.Split('\\').Last(), FullPath = d, Level = levels });
                this.DirSearch(d);
            }
        }

    }
}