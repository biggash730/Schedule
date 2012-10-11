using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Schedule.Models;

namespace Schedule.Classes
{
    public class ScheduleTime
    {
        RemindEntities RE = new RemindEntities();
        public List<Models.ScheduleTime> AllScheduleTimes(int id)
        {
            var allScheduleTimes = RE.ScheduleTimes.Where(p => p.scheduleId == id && p.state == "ACTIVE");
            return allScheduleTimes.ToList();
        }

        public object OneScheduleTime(int id)
        {
            var oneScheduleTime = RE.ScheduleTimes.Single(p => p.id == id && p.state == "ACTIVE");
            return oneScheduleTime;
        }

        public void CreateScheduleTime(Models.ScheduleTime scheduleTime)
        {
            RE.ScheduleTimes.AddObject(scheduleTime);
            RE.SaveChanges();
        }

        public void EditScheduleTime(Models.ScheduleTime scheduleTime)
        {
            var sch = RE.ScheduleTimes.Single(p => p.id == scheduleTime.id);

            sch.scheduleId = scheduleTime.scheduleId;
            sch.taskTime = scheduleTime.taskTime;
            sch.state = scheduleTime.state;
            RE.SaveChanges();
        }

        public void DeleteScheduleTime(int id)
        {
            var sch = RE.ScheduleTimes.Single(p => p.id == id);
            sch.state = "INACTIVE";
            RE.SaveChanges();

        }
    }
}