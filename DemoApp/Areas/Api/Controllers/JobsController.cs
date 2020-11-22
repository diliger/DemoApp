using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Areas.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class JobsController : ControllerBase
    {
        DemoContext db;

        private readonly ILogger logger;

        public JobsController(DemoContext db, ILogger<JobsController> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        [HttpGet(Name = "Summary")]
        public IEnumerable<JobsPerRoomType> Summary()
        {
            List<JobsPerRoomType> summary = new List<JobsPerRoomType>();

            var grouped = db.RxJobs.Include(val => val.RoomType).GroupBy(val => new { val.RoomTypeId, val.Status });

            foreach (var g in grouped)
            {
                var row = new JobsPerRoomType();
                row.Count = g.Count();
                row.RoomType = g.First().RoomType?.Name;
                row.Status = g.Key.Status;
                summary.Add(row);
            }

            return summary;
        }
    }    
}