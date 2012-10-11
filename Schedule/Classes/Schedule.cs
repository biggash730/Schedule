using System.Collections.Generic;
using System.Linq;
using Schedule.Models;

namespace Schedule.Classes
{
    public class Schedule
    {
        RemindEntities RE = new RemindEntities();
        public List<Models.Schedule> AllSchedules(int id)
        {
            var allSchedules = RE.Schedules.Where(p => p.userId == id && p.state == "ACTIVE");
            return allSchedules.ToList();
        }

        public object OneSchedule(int id)
        {
            var oneSchedule = RE.Schedules.Single(p => p.id == id && p.state == "ACTIVE");
            return oneSchedule;
        }

        public int OneSchedule(string name, string description, int uid)
        {
            int schId = RE.Schedules.First(p => p.name == name && p.description == description && p.userId == uid).id;
            return schId;
        }

        public void CreateSchedule(Models.Schedule schedule)
        {
            RE.Schedules.AddObject(schedule);
            RE.SaveChanges();
        }

        public void EditSchedule(Models.Schedule schedule)
        {
            var sch = RE.Schedules.Single(p => p.id == schedule.id);
            
            sch.name = schedule.name;
            sch.description = schedule.description;
            sch.type = schedule.type;
            sch.state = schedule.state;
            RE.SaveChanges();
        }

        public void DeleteSchedule(int id)
        {
            var sch = RE.Schedules.Single(p => p.id == id);
            sch.state = "INACTIVE";
            RE.SaveChanges();

        }
    }
}