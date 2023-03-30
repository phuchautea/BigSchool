using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BigSchoolContext bigSchoolContext = new BigSchoolContext();
            var upcommingCourse = bigSchoolContext.Courses.Where(p => p.DateTime > DateTime.Now && p.isCanceled == false).OrderBy(p => p.DateTime).ToList();
            foreach (Course i in upcommingCourse)
            {
                ApplicationUser user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LecturerId);
                i.Name = user.Name;
                var getUser = User.Identity.GetUserId();
                Attendance at = bigSchoolContext.Attendances.FirstOrDefault(p => p.CourseId == i.Id && p.Attendee == getUser);
                if(at == null)
                {
                    i.isShowGoing = true;
                }

                Following fl = bigSchoolContext.Followings.FirstOrDefault(p => p.FollowerId == getUser && p.FolloweeId == i.LecturerId);
                if(fl != null)
                {
                    i.isShowFollow = true;
                }
            }
            ViewBag.User = User.Identity.GetUserId();
            return View(upcommingCourse);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}