using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mail;
using System.Web.Helpers;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using Schedule.Models;

namespace Schedule.Classes
{
    public class SendMail
    {
        RemindEntities RE = new RemindEntities();
        public void sendMail()
        {
            var ActiveSchedules = RE.Schedules.Where(p => p.state == "ACTIVE");

            foreach (var scheduleItem in ActiveSchedules)
            {
                var ActiveUser = RE.Users.Single(p => p.id == scheduleItem.userId);
                if (ActiveUser.state == "ACTIVE")
                {
                    var ActiveScheduleTimes = RE.ScheduleTimes.Where(p => p.scheduleId == scheduleItem.id && p.state == "ACTIVE");
                    foreach (var scheduleTimeItem in ActiveScheduleTimes)
                    {
                        if (scheduleTimeItem.taskTime <= DateTime.Now)
                        {
                            string body, subject, email, from;
                            from = "biggash730@gmail.com";
                            email = ActiveUser.email;
                            subject = "Reminder Notification";
                            body = "Reminder name: " + scheduleItem.name + "\r\n"
                                + "Description: " + scheduleItem.description + "\r\n"
                                + "Time: " + scheduleTimeItem.taskTime.ToString();

                            SmtpClient sm = new SmtpClient("smtp.gmail.com", 587);
                            sm.EnableSsl = true;
                            sm.Credentials = new NetworkCredential("biggash730", "kp0l3miah");
                            sm.Send(from, email, subject, body);


                            //WebMail.SmtpServer = Global.SMTP;                                
                            //WebMail.EnableSsl = true;
                            //WebMail.SmtpPort = 587;
                            //WebMail.UserName = "biggash730";
                            //WebMail.Password = "kp0l3miah";
                            //WebMail.From = "biggash@gmail.com";
                            //WebMail.Send(email, subject, body, "biggash730@gmail.com");

                            var oldscheduleTime = RE.ScheduleTimes.Single(p => p.id == scheduleTimeItem.id);
                            var newScheduleTime = new ScheduleTime();
                            newScheduleTime.state = "ACTIVE";
                            newScheduleTime.scheduleId = oldscheduleTime.scheduleId;

                            if (scheduleItem.type == "ONCE")
                            {
                                oldscheduleTime.state = "INACTIVE";

                                var oldschedule = RE.Schedules.Single(p => p.id == scheduleItem.id);
                                oldschedule.state = "INACTIVE";

                            }
                            else if (scheduleItem.type == "DAILY")
                            {
                                oldscheduleTime.state = "INACTIVE";

                                newScheduleTime.taskTime = oldscheduleTime.taskTime.AddDays(1);
                                newScheduleTime.
                                RE.ScheduleTimes.AddObject(newScheduleTime);


                            }
                            else if (scheduleItem.type == "WEEKLY")
                            {
                                oldscheduleTime.state = "INACTIVE";

                                newScheduleTime.taskTime = oldscheduleTime.taskTime.AddDays(7);

                                RE.ScheduleTimes.AddObject(newScheduleTime);

                            }
                            else if (scheduleItem.type == "MONTHLY")
                            {
                                oldscheduleTime.state = "INACTIVE";

                                newScheduleTime.taskTime = oldscheduleTime.taskTime.AddMonths(1);

                                RE.ScheduleTimes.AddObject(newScheduleTime);

                            }
                            else if (scheduleItem.type == "YEARLY")
                            {
                                oldscheduleTime.state = "INACTIVE";

                                newScheduleTime.taskTime = oldscheduleTime.taskTime.AddYears(1);

                                RE.ScheduleTimes.AddObject(newScheduleTime);

                            }
                        }
                    }
                }
            }
            RE.SaveChanges();
        }
    }
}