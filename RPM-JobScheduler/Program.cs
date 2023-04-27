using Quartz;
using Quartz.Impl;
using RPM_JobScheduler.Jobs;

ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
var scheduler = await schedulerFactory.GetScheduler();

ITrigger trigger = TriggerBuilder.Create()
             .WithIdentity("savefueldatatrigger", "group1")
             .WithSchedule(CronScheduleBuilder.CronSchedule("0 0 9 ? * MON")).Build();

IJobDetail job = JobBuilder.Create<FuelSaveJob>()
    .WithIdentity(name: "savefueldatajob", group: "group1")
    .Build();

await scheduler.ScheduleJob(job, trigger);
await scheduler.Start();

DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
Console.WriteLine("Next Fire Time For Fuel Job:" + nextFireTime.Value);

Console.WriteLine("Press any key to close the application");
Console.ReadKey();