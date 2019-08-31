using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using RedCoreApi.Models;
using System.Web.Http.OData;

namespace RedCoreApi.Controllers
{
    
    public class UsersController : ApiController
    {
        
        
        private ISRedCoreContext db = new UserDBModel();
       
        // add these constructors
        public UsersController() { }

        public UsersController(ISRedCoreContext context)
        {
            db = context;
        }
        

        [Authorize(Roles = "admin")]
        // GET: api/Users
        public IQueryable<user> Getuser()
        {   
            return db.user;
        }

        [Authorize(Roles = "admin")]
        // GET: api/Users/5
        [ResponseType(typeof(user))]
        public IHttpActionResult Getuser(int id)
        {
            user user = db.user.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize(Roles = "admin")]
        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putuser(int id, user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.userid)
            {
                return BadRequest();
            }

            var usr = db.user.Where(t => t.userid != user.userid && t.email == user.email).ToList();
            if (usr.Count >= 1) {
                return Content(HttpStatusCode.Conflict, new { message = "An existing record with the email was already found." });
            } 

            //db.Entry(user).State = EntityState.Modified;
            db.MarkAsModified(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "admin")]
        // PATCH: api/Users/5
        [ResponseType(typeof(void))]
        [HttpPatch]
        public IHttpActionResult Patchuser(int id, Delta<user> patch)
        {
            //Validate(patch.GetEntity());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user user = db.user.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            patch.Patch(user);

            if (id != user.userid)
            {
                return BadRequest();
            }

            var usr = db.user.Where(t =>t.userid != user.userid && t.email == user.email).ToList();
            if (usr.Count >= 1)
            {
                return Content(HttpStatusCode.Conflict, new { message = "An existing record with the email was already found." });
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "admin")]
        // POST: api/Users
        [ResponseType(typeof(user))]
        public IHttpActionResult Postuser(user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          
            var usr = db.user.Where(t => t.email == user.email).ToList();
            if(usr.Count >= 1) {
                return Content(HttpStatusCode.Conflict, new { message = "An existing record with the email was already found." });
            }

            db.user.Add(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (userExists(user.userid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = user.userid }, user);
        }

        [Authorize(Roles = "admin")]
        // DELETE: api/Users/5
        [ResponseType(typeof(user))]
        public IHttpActionResult Deleteuser(int id)
        {
            user user = db.user.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.user.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool userExists(int id)
        {
            return db.user.Count(e => e.userid == id) > 0;
        }
    }
}