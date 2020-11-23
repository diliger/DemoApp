using DemoApp.Areas.Api.Controllers;
using DemoApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DemoApp.Tests
{
    public class JobsApiTests
    {
        private DemoContext context;

        public JobsApiTests()
        {
            InitContext();
        }

        [Fact]
        public void SummaryNotNull()
        {
            ILogger<JobsController> logger = Mock.Of<ILogger<JobsController>>();

            JobsController controller = new JobsController(context, logger);

            var result = controller.Summary();
            // Assert

            Assert.NotNull(result);
        }

        [Fact]
        public void SummaryAny()
        {
            ILogger<JobsController> logger = Mock.Of<ILogger<JobsController>>();

            JobsController controller = new JobsController(context, logger);

            var result = controller.Summary();
            // Assert

            Assert.True(result.Any());
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
