using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DemoApp.Controllers
{
    public class JobsController : Controller
    {
        DemoContext db;

        private readonly ILogger logger;

        public JobsController(DemoContext db, ILogger<JobsController> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            //list of jobs
            //for better performance I'd recommend to use API for ajax loading rows on demand
            //by pages or on scroll, but it is out of scope of the demo task
            List<RxJob> jobs = db.RxJobs.ToList();

            List<RxRoomType> roomTypes = db.RxRoomTypes.ToList();

            ViewBag.Jobs = jobs;
            ViewBag.RoomTypes = roomTypes;

            return View();
        }

        public IActionResult Complete(Guid id)
        {
            try
            {
                RxJob job = db.RxJobs.FirstOrDefault(val => val.Id == id);
                if (job == null)
                {
                    return Json(new { Success = false, Error = "Not found!" });
                }

                //assume it should be a refbook the same as RoomTypes with parameters CanComplete and Completed for optimizing querying and code readability
                //but it is also out of scope of the test task
                //I wouldn't recomment to use this approach
                Dictionary<int, string> statuses = new Dictionary<int, string>() { { 1, "Complete" }, { 2, "Not Started" }, { 3, "In Progress" }, { 4, "Delayed" } };

                int completedStatusId = 1;
                int[] canCompleteStatuses = new int[] { 3, 4 };

                if (!job.StatusNum.HasValue || !canCompleteStatuses.Contains(job.StatusNum.Value))
                {
                    return Json(new { Success = false, Error = "Not allowed!" });
                }

                job.StatusNum = completedStatusId;
                job.Status = statuses[completedStatusId];

                db.SaveChanges();

                return Json(new { Success = true, Job = job });
            }
            catch (Exception ex)
            {
                //Log exception here
                logger.LogError(ex, "Exception in Jobs/Complete");

                //Send more details about exception if necessary 
                return Json(new { Success = false, Error = "Unexpected exception!" });
            }
        }
    }
}