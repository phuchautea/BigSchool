using BigSchool.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigSchool.Controllers
{
    public class FollowingsController : ApiController
    {
        [HttpPost]

        public IHttpActionResult Follow(Following followDTO)
        {
            var userID = User.Identity.GetUserId(); //get login user
            followDTO.FollowerId = userID;
            if (userID == null)
            {
                return BadRequest("Please login first!");
            }
            BigSchoolContext context = new BigSchoolContext();
            var fl = context.Followings.FirstOrDefault(x => x.FollowerId == followDTO.FollowerId && x.FolloweeId == followDTO.FolloweeId);
            if (fl == null)
            {
                context.Followings.Add(followDTO);
                context.SaveChanges();
                return Ok();
            }
            else
            {
                context.Followings.Remove(fl);
                context.SaveChanges();
                return NotFound();
            }
            

        }
    }
}
