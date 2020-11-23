using DemoApp.Controllers;
using DemoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using Xunit;

namespace DemoApp.Tests
{
    public class JobsControllerTests
    {
        private DemoContext context;

        public JobsControllerTests()
        {
            InitContext();
        }

        [Fact]
        public void IndexViewData()
        {
            ILogger<JobsController> logger = Mock.Of<ILogger<JobsController>>();

            JobsController controller = new JobsController(context, logger);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result.ViewData["Jobs"]);
            Assert.NotNull(result.ViewData["RoomTypes"]);
        }

        [Fact]
        public void IndexViewResultNotNull()
        {
            ILogger<JobsController> logger = Mock.Of<ILogger<JobsController>>();

            JobsController controller = new JobsController(context, logger);

            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void CompleteDelayedJobSuccess()
        {
            ILogger<JobsController> logger = Mock.Of<ILogger<JobsController>>();

            JobsController controller = new JobsController(context, logger);

            RxJob job = await context.RxJobs.FirstOrDefaultAsync(val => val.StatusNum == 4);
            // Act
            JsonResult result = controller.Complete(job.Id) as JsonResult;
            // Assert

            Assert.NotNull(result);
            Assert.NotNull(result.Value);

            JObject response = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(result.Value));
            Assert.True(response.Value<bool>("Success"));
        }

        [Fact]
        public async void CompleteCompletedJobError()
        {
            ILogger<JobsController> logger = Mock.Of<ILogger<JobsController>>();

            JobsController controller = new JobsController(context, logger);

            RxJob job = await context.RxJobs.FirstOrDefaultAsync(val => val.StatusNum == 1);
            // Act
            JsonResult result = controller.Complete(job.Id) as JsonResult;
            // Assert

            Assert.NotNull(result);
            Assert.NotNull(result.Value);

            JObject response = JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(result.Value));
            Assert.False(response.Value<bool>("Success"));
        }

        private async void InitContext()
        {
            var options = new DbContextOptionsBuilder<DemoContext>().UseInMemoryDatabase(databaseName: "Demo").Options;

            this.context = new DemoContext(options);

            if (!(await context.RxJobs.AnyAsync()))
            {
                context.RxJobs.Add(new RxJob
                {
                    Id = Guid.NewGuid(),
                    Name = "Job15_0_0",
                    Status = "Complete",
                    StatusNum = 1,
                    DateCreated = DateTime.Now,
                    RoomTypeId = new Guid("50AFD651-40D4-4704-8C06-DFED2F922AB9")
                });

                context.RxJobs.Add(new RxJob
                {
                    Id = Guid.NewGuid(),
                    Name = "Job15_0_1",
                    Status = "Delayed",
                    StatusNum = 4,
                    DateCreated = DateTime.Now,
                    RoomTypeId = new Guid("911C7AF6-2D20-4B06-AD06-BD835A3871F1")
                });
            }

            if (!(await context.RxRoomTypes.AnyAsync()))
            {
                context.RxRoomTypes.Add(new RxRoomType()
                {
                    Id = new Guid("50AFD651-40D4-4704-8C06-DFED2F922AB9"),
                    Name = "Type1"
                });

                context.RxRoomTypes.Add(new RxRoomType()
                {
                    Id = new Guid("911C7AF6-2D20-4B06-AD06-BD835A3871F1"),
                    Name = "Type2"
                });
            }

            context.SaveChanges();
        }
    }
}
