using BigSchool.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Data;
using System.Threading.Tasks;

namespace BigSchool.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Create()
        {
            BigSchoolContext bigSchoolContext = new BigSchoolContext();
            Course course = new Course();
            course.Categories = bigSchoolContext.Categories.ToList();
            return View(course);
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Course course)
        {
            BigSchoolContext bigSchoolContext = new BigSchoolContext();
            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                course.Categories = bigSchoolContext.Categories.ToList();
                return View("Create", course);
            }

            // Get the UserManager from the Owin context
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            // Get the ID of the currently logged-in user
            string userId = User.Identity.GetUserId();
            course.LecturerId = userId;

            string dateString = course.Date;
            string timeString = course.Time;
            //string dateString = "2022-10-30 16:13:49";
            try
            {
                DateTime dateTime = DateTime.Parse(dateString + " " + timeString);

                if (dateTime > DateTime.Now)
                {
                    course.DateTime = dateTime;
                    bigSchoolContext.Courses.Add(course);
                    bigSchoolContext.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    course.Categories = bigSchoolContext.Categories.ToList();
                    ViewBag.Errors = "DateTime phải lớn hơn thời gian hiện tại";
                    return View(course);
                }
            }
            catch (Exception ex)
            {
                course.Categories = bigSchoolContext.Categories.ToList();
                ViewBag.Errors = "DateTime không hợp lệ";
                return View(course);
            }
            
            
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            var userID = User.Identity.GetUserId();
            var listAttendances = context.Attendances.Where(p => p.Attendee == userID).ToList();
            var courses = new List<Course>(); //Tìm chi tiết khóa học từ listAttendances ( mã khóa học, tên GV phải truy cập từ AspnetUser.Name )
            foreach (Attendance temp in listAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }
        public ActionResult Mine()
        {
            BigSchoolContext context = new BigSchoolContext();
            var userID = User.Identity.GetUserId();
            var user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userID);
            var courses = context.Courses.Where(p => p.LecturerId == userID).ToList();
            foreach (Course i in courses)
            {
                i.Name = user.Name;
            }
            return View(courses);
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            var loginUser = User.Identity.GetUserId(); //get login user
            var course = context.Courses.FirstOrDefault(c => c.LecturerId == loginUser && c.Id == id);
            if (course == null)
                return HttpNotFound("Không tìm thấy khóa học");
            course.Categories = context.Categories.ToList(); //lấy danh sách loại khóa học
            return View("Create", course);
        }
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            var userID = User.Identity.GetUserId(); //get login user
            BigSchoolContext context = new BigSchoolContext();
            var course = context.Courses.ToList();
            var findCourse = context.Courses.FirstOrDefault(p => p.Id == Id && p.LecturerId == userID);
            //findCourse.IsCanceled = true;
            if(findCourse != null)
            {
                course.Remove(findCourse);
                context.SaveChanges();
                return Json(new { status = true, message = "Xóa thành công" });
            }
            else
            {
                return Json(new { status = false, message = "Lỗi" });
            }
            
        }
        public ActionResult Following()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser loginUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listFollowings = context.Followings.Where(p => p.FollowerId == loginUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Following temp in listFollowings)
            {
                //lấy ra TẤT cả các khóa học của GV được theo dõi
                var listCourse = context.Courses.Where(p => p.LecturerId == temp. FolloweeId).ToList();
                if (listCourse.Count > 0)
                {
                    //tìm Name của GV
                    string Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(listCourse[0].LecturerId).Name;
                    foreach (Course i in listCourse)
                    {
                        i.Name = Name;
                    }
                    courses.AddRange(listCourse);
                }
            }
            return View(courses);
        }


    }
}