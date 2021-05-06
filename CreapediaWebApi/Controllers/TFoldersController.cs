using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CreapediaWebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CreapediaWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TFoldersController : ControllerBase
    {
        d2v9eis2ivh7hhContext db;
        public TFoldersController(d2v9eis2ivh7hhContext context)
        {
            db = context;
        }
        [HttpGet]
        public async Task<ActionResult<Templatefolder[]>> GetMainFolders(int userid)
        {
            Templatefolder[] tfolders = await db.Templatefolders.Where(x => x.ParentfolderId == null && x.Userid==userid).ToArrayAsync();
            if (tfolders == null)
                return NotFound();
            return tfolders;
        }
        // GET: api/Tfolders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Templatefolder []>> GetTfolder(int id)
        {
            Templatefolder [] tfolders = await db.Templatefolders.Where(x => x.ParentfolderId == id).ToArrayAsync();
            if (tfolders == null)
                return NotFound();
            return tfolders;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id, User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    return NoContent();
        //}

        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //    db.Users.Add(user);
        //    await db.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.Id }, user);
        //}

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<User>> DeleteUser(int id)
        //{
        //    var user = await db.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    db.Users.Remove(user);
        //    await db.SaveChangesAsync();

        //    return user;
        //}

        //private bool UserExists(int id)
        //{
        //    return db.Users.Any(e => e.Id == id);
        //}
    }
}

