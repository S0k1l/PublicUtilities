using Quartz.Spi;
using Quartz;
using PublicUtilities.Data;
using PublicUtilities.Models;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;

namespace PublicUtilities.Services
{
    public class MyJob : IJob
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MyJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                // Your task logic here
                var notification = new Notifications
                {
                    Header = "Нагадування",
                    Text = "Не забудьте передати показники лічильників!",
                    Date = DateTime.Now,
                    PlacesOfResidence = null,
                };

                dbContext.Add(notification);
                await dbContext.SaveChangesAsync();
            }
        }
    }
    public class SingletonJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SingletonJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            var jobType = jobDetail.JobType;
            return (IJob)_serviceProvider.GetRequiredService(jobType);
        }

        public void ReturnJob(IJob job)
        {
            // No-op
        }
    }

    public class QuartzHostedService : IHostedService
    {
        private readonly IScheduler _scheduler;

        public QuartzHostedService(ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            _scheduler = schedulerFactory.GetScheduler().Result;
            _scheduler.JobFactory = jobFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler.Start().Wait();


            // Define a trigger for the job
            ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("myTrigger", "myTriggerGroup")
                        .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 2, 6, 0))
                        .Build();

            _scheduler.ScheduleJob(JobBuilder.Create<MyJob>()
                .WithIdentity("myJob", "myJobGroup")
                .Build(), trigger);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _scheduler.Shutdown();
        }
    }
}