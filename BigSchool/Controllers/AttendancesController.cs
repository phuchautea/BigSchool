using BigSchool.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigSchool.Controllers
{
    public class AttendancesController : ApiController
    {
        [HttpPost]

        public IHttpActionResult Attend(Course courseDTO)
        {
            var userID = User.Identity.GetUserId(); //get login user
            if (userID == null)
            {
                return BadRequest("Please login first!");
            }
            BigSchoolContext context = new BigSchoolContext();
            var at = context.Attendances.FirstOrDefault(x => x.CourseId == courseDTO.Id && x.Attendee == userID);
            if(at == null)
            {
                var attendance = new Attendance()
                {
                    CourseId = courseDTO.Id,
                    Attendee = userID //nguoi dang ki khoa học chinh la login user
                };
                context.Attendances.Add(attendance);
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
            
        }
    }
}
