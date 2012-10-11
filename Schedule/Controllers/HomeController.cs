using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Schedule.Models;
using Schedule.Classes;
using ScheduleTime = Schedule.Models.ScheduleTime;
using User = Schedule.Classes.User;



namespace Schedule.Controllers
{
    public class HomeController : Controller
    {
        User Usr = new User();
        Classes.Schedule Sch = new Classes.Schedule();
        private Classes.ScheduleTime SchTime = new Classes.ScheduleTime();
        
        //
        // GET: /Home/

        public ActionResult Index(int id)
        {
            var user = Usr.OneUser(id);
            var fullname = (user.firstName + " " + user.lastName);
            Session.Add("Username", fullname);

            List<Models.Schedule> listSchedueles = Sch.AllSchedules(id);
            return View(listSchedueles);
        }

        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(string loginUsername, string loginPassword)
        {
            var newLogin = new UserLogin()
                               {
                                   Username=loginPassword,
                                   Password=loginPassword
                               };
            int isTrue = Usr.Login(newLogin);

            if (isTrue > 0)
            {
                Session.Add("UserId", isTrue);
                var user = Usr.OneUser(isTrue);
                if(user.userType == "ADMINISTRATOR")
                    return RedirectToAction("Index","Administrator", new { id = isTrue });
                else
                {
                    return RedirectToAction("Index", new { id = isTrue });
                }
            }
            else
            {
                Session.Add("UserId", 0);
                return View();
            }

        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session.Add("UserId", 0);
            return RedirectToAction("Login");
        }

        public void CreateSchedule(string schName, string schDescription, string schType)
        {
            var userId = (int)Session["UserId"];
            var newSchedule = new Models.Schedule()
                             {
                                 userId = userId,
                                 name=schName,
                                 description=schDescription,
                                 type=schType,
                                 state="ACTIVE"
                             };
            Sch.CreateSchedule(newSchedule);
            int newSchId = Sch.OneSchedule(schName, schDescription, userId);
            Session.Add("SchId", newSchId);
        }

        [HttpPost]
        public ActionResult CreateScheduleTimeOnce(string schNameOnce, string schDescriptionOnce, DateTime schDateOnce, string schHourOnce, string schMinOnce)
        {
            if (schNameOnce == null) throw new ArgumentNullException("schNameOnce");
            if (schDescriptionOnce == null) throw new ArgumentNullException("schDescriptionOnce");
            if (schHourOnce == null) throw new ArgumentNullException("schHourOnce");
            if (schMinOnce == null) throw new ArgumentNullException("schMinOnce");
            
            var userId = (int)Session["UserId"];
            const string schType = "ONCE";
            CreateSchedule(schNameOnce, schDescriptionOnce, schType);
            var schId = (int)Session["SchId"];
            
            var taskTime = new DateTime(schDateOnce.Year, schDateOnce.Month, schDateOnce.Day, int.Parse(schHourOnce), int.Parse(schMinOnce), 0);
            var newSchTime = new ScheduleTime
                                 {
                                     scheduleId = schId, 
                                     state = "ACTIVE",
                                     taskTime = taskTime
                                 };
            
            SchTime.CreateScheduleTime(newSchTime);
            return RedirectToAction("Index", new { id = userId });
        }

        [HttpPost]
        public ActionResult CreateScheduleTimeDaily(string schNameDaily, string schDescriptionDaily, string schHourDaily, string schMinDaily)
        {
            if (schNameDaily == null) throw new ArgumentNullException("schNameDaily");
            if (schDescriptionDaily == null) throw new ArgumentNullException("schDescriptionDaily");
            if (schHourDaily == null) throw new ArgumentNullException("schHourDaily");
            if (schMinDaily == null) throw new ArgumentNullException("schMinDaily");

            var userId = (int)Session["UserId"];
            const string schType = "DAILY";
            CreateSchedule(schNameDaily, schDescriptionDaily, schType);
            var schId = (int)Session["SchId"];

            DateTime taskTime;
            if (DateTime.Now.Hour >= int.Parse(schHourDaily) || DateTime.Now.Minute >= int.Parse(schMinDaily))
            {
                taskTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, (DateTime.Now.Day) + 1, int.Parse(schHourDaily), int.Parse(schMinDaily), 0);
            }
            else
            {
                taskTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(schHourDaily), int.Parse(schMinDaily), 0);
            }
            var newSchTime = new ScheduleTime
            {
                scheduleId = schId,
                state = "ACTIVE",
                taskTime = taskTime
            };

            SchTime.CreateScheduleTime(newSchTime);
            return RedirectToAction("Index", new { id = userId });
        }

        [HttpPost]
        public ActionResult CreateScheduleTimeWeekly(string schNameWeekly, string schDescriptionWeekly, string schHourWeekly, string schMinWeekly, string schMonWeekly,
            string schTueWeekly, string schWedWeekly, string schThuWeekly, string schFriWeekly, string schSatWeekly, string schSunWeekly)
        {
            if (schNameWeekly == null) throw new ArgumentNullException("schNameWeekly");
            if (schDescriptionWeekly == null) throw new ArgumentNullException("schDescriptionWeekly");
            if (schHourWeekly == null) throw new ArgumentNullException("schHourWeekly");
            if (schMinWeekly == null) throw new ArgumentNullException("schMinWeekly");

            var userId = (int)Session["UserId"];
            const string schType = "WEEKLY";
            CreateSchedule(schNameWeekly, schDescriptionWeekly, schType);
            var schId = (int)Session["SchId"];

            var taskTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(schHourWeekly), int.Parse(schMinWeekly), 0);

            var day = taskTime.DayOfWeek;
            var days = new DateTime[7];
            switch (day)
            {
                case DayOfWeek.Sunday:
                    for (var i = 0; i < 7; i++)
                    {
                        days[i] = taskTime.AddDays(i + 1);
                    }
                    break;
                case DayOfWeek.Monday:
                    days[0] = taskTime.AddDays(7);
                    for (var i = 1; i < 7; i++)
                    {
                        days[i] = taskTime.AddDays(i);
                    }
                    break;
                case DayOfWeek.Tuesday:
                    days[0] = taskTime.AddDays(6);
                    days[1] = taskTime.AddDays(7);
                    for (int i = 2; i < 7; i++)
                    {
                        days[i] = taskTime.AddDays(i - 1);
                    }
                    break;
                case DayOfWeek.Wednesday:
                    days[0] = taskTime.AddDays(5);
                    days[1] = taskTime.AddDays(6);
                    days[2] = taskTime.AddDays(7);
                    for (int i = 3; i < 7; i++)
                    {
                        days[i] = taskTime.AddDays(i - 2);
                    }
                    break;
                case DayOfWeek.Thursday:
                    days[0] = taskTime.AddDays(4);
                    days[1] = taskTime.AddDays(5);
                    days[2] = taskTime.AddDays(6);
                    days[3] = taskTime.AddDays(7);
                    for (int i = 4; i < 7; i++)
                    {
                        days[i] = taskTime.AddDays(i - 3);
                    }
                    break;
                case DayOfWeek.Friday:
                    days[5] = taskTime.AddDays(1);
                    days[6] = taskTime.AddDays(2);
                    for (int i = 0; i < 5; i++)
                    {
                        days[i] = taskTime.AddDays(i + 3);
                    }
                    break;
                case DayOfWeek.Saturday:
                    days[6] = taskTime.AddDays(1);
                    for (int i = 0; i < 6; i++)
                    {
                        days[i] = taskTime.AddDays(i + 2);
                    }
                    break;
            }

            if (schMonWeekly == "schMonWeekly")
            {
                var newSchTime = new ScheduleTime
                {
                    scheduleId = schId,
                    state = "ACTIVE",
                    taskTime = days[0]
                };
                SchTime.CreateScheduleTime(newSchTime);
                
            }
            if (schTueWeekly == "schTueWeekly")
            {
                var newSchTime = new ScheduleTime
                {
                    scheduleId = schId,
                    state = "ACTIVE",
                    taskTime = days[1]
                };
                SchTime.CreateScheduleTime(newSchTime);
                
            }
            if (schWedWeekly == "schWedWeekly")
            {
                var newSchTime = new ScheduleTime
                {
                    scheduleId = schId,
                    state = "ACTIVE",
                    taskTime = days[2]
                };
                SchTime.CreateScheduleTime(newSchTime);
            }
            if (schThuWeekly == "schThuWeekly")
            {
                var newSchTime = new ScheduleTime
                {
                    scheduleId = schId,
                    state = "ACTIVE",
                    taskTime = days[3]
                };
                SchTime.CreateScheduleTime(newSchTime);
            }
            if (schFriWeekly == "schFriWeekly")
            {

                var newSchTime = new ScheduleTime
                {
                    scheduleId = schId,
                    state = "ACTIVE",
                    taskTime = days[4]
                };
                SchTime.CreateScheduleTime(newSchTime);
            }
            if (schSatWeekly == "schSatWeekly")
            {
                var newSchTime = new ScheduleTime
                {
                    scheduleId = schId,
                    state = "ACTIVE",
                    taskTime = days[5]
                };
                SchTime.CreateScheduleTime(newSchTime);
                
            }
            if (schSunWeekly == "schSunWeekly")
            {
                var newSchTime = new ScheduleTime
                {
                    scheduleId = schId,
                    state = "ACTIVE",
                    taskTime = days[6]
                };
                SchTime.CreateScheduleTime(newSchTime);
            }

            return RedirectToAction("Index", new { id = userId });
        }

        [HttpPost]
        public JsonResult ViewSchedule(int id)
        {
            var oneShedule = Sch.OneSchedule(id);
            return Json(oneShedule, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AllScheduleTimes(int schId)
        {
            var allSheduleTimes = SchTime.AllScheduleTimes(schId);
            return Json(allSheduleTimes, JsonRequestBehavior.AllowGet);
        }

        

    }
}
