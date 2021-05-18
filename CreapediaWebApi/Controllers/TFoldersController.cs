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

        [HttpGet]
        [Route("/tfolders/export")]
        public async Task<IActionResult> ExportFolder(string? mail, int folderid)
        {
            User user = await db.Users.Where(x => x.Mail == mail).FirstAsync();
            if (user == null)
                return BadRequest("Нет такого пользователя");
            else
            {
                Templatefolder import= await db.Templatefolders.Where(x => x.Name=="Импорт"&&x.ParentfolderId==null&&x.Userid==user.Id).FirstOrDefaultAsync();
                if(import==null)
                {
                    import = new Templatefolder()
                    {
                        Name = "Импорт",
                        Userid = user.Id
                    };
                    db.Templatefolders.Add(import);
                }
                await db.SaveChangesAsync();
                Templatefolder oldfolder = await db.Templatefolders.Where(x => x.Id==folderid).FirstAsync();
                Templatefolder newfolder = new Templatefolder()
                {
                    Name = oldfolder.Name,
                    ParentfolderId = import.Id,
                    Userid = user.Id
                };
                db.Templatefolders.Add(newfolder);
                await db.SaveChangesAsync();
                await AddFolders(oldfolder.Id, newfolder.Id, user.Id);
                await AddElements(oldfolder.Id, newfolder.Id, user.Id);                
                return Ok();
            }
        }

        public async Task AddFolders(int oldparentfolder, int newparentfolder, int userid)
        {
            Templatefolder[] tfolders = await db.Templatefolders.Where(x => x.ParentfolderId == oldparentfolder).ToArrayAsync();
            if (tfolders.Length>0)
            foreach (Templatefolder t in tfolders)
            {
                    Templatefolder newfolder = new Templatefolder()
                    {
                        Name = t.Name,
                        ParentfolderId = newparentfolder,
                        Userid = userid
                    };
                    db.Templatefolders.Add(newfolder);
                    await db.SaveChangesAsync();
                    await AddFolders(t.Id, newfolder.Id, userid);
                    await AddElements(t.Id, newfolder.Id, userid);
                }
        }
        public async Task AddElements(int oldparentfolder, int newparentfolder, int userid)
        {
            Templateelement[] telements = await db.Templateelements.Where(x => x.TemplatefolderId == oldparentfolder).ToArrayAsync();
            if (telements.Length>0)
                foreach (Templateelement el in telements)
                {
                    Templateelement newelement = new Templateelement()
                    {
                        Name = el.Name,
                        TemplatefolderId = newparentfolder,
                    };
                    db.Templateelements.Add(newelement);
                    await db.SaveChangesAsync();
                }

        }
        [HttpPost]
        public async Task<IActionResult> PostTfolder(Templatefolder tfolder)
        {
            db.Templatefolders.Add(tfolder);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTFolder(int id, Templatefolder tfolder)
        {
            if (id != tfolder.Id)
            {
                return BadRequest();
            }

            db.Entry(tfolder).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!db.Templatefolders.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Templatefolder>> DeleteUser(int id)
        {
            var tfolder = await db.Templatefolders.FindAsync(id);
            if (tfolder == null)
            {
                return NotFound();
            }
            db.Templatefolders.Remove(tfolder);
            await db.SaveChangesAsync();

            return tfolder;
        }
        
    }
}

