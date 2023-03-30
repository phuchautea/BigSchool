using BigSchool.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigSchool.Controllers.Api
{
    public class CoursesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Cancel(int Id)
        {
            var userID = User.Identity.GetUserId();
            if (userID == null)
            {
                return BadRequest("Please login first!");
            }
            BigSchoolContext context = new BigSchoolContext();
            var course = context.Courses.FirstOrDefault(x => x.Id == Id && x.LecturerId == userID);
            if (course.isCanceled)
            {
                return NotFound();
            }
            else
            {
                course.isCanceled = true;
                context.SaveChanges();
                return Ok();
            }
        }
    }
}
