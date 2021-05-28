using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CreapediaWebApi.Models;

namespace CreapediaWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        d2v9eis2ivh7hhContext db;
        public FoldersController(d2v9eis2ivh7hhContext context)
        {
            db = context;
        }

        //[HttpGet]
        //public async Task<ActionResult<Folder[]>> GetMainFolders(int userid)
        //{
        //    Folder[] mfolders = await db.Folders.Where(x => x.Userid == userid&&x.Parentfolderid==null).ToArrayAsync();
        //    if (mfolders == null)
        //        return NotFound();
        //    return mfolders;
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<Folder[]>> GetFoldersFromParent(int id)
        {
            Folder[] folders;
            if (id == 0)
            {
                id = await db.Folders.Where(x => x.Parentfolderid == null).Select(x => x.Id).FirstAsync();
            }
            if (id == 0)
            {
                return null;
            }
            folders = await db.Folders.Where(x => x.Parentfolderid == id).ToArrayAsync();
            if (folders == null)
                return NotFound();
            return folders;
        }

        [HttpGet]
        [Route("/folders/export")]
        public async Task<IActionResult> ExportFolder(string? mail, int folderid)
        {
            User user = await db.Users.Where(x => x.Mail == mail).FirstAsync();
            if (user == null)
                return BadRequest("Нет такого пользователя");
            else
            {
                Folder import = await db.Folders.Where(x => x.Name == "Импорт" && x.Userid == user.Id && x.Parentfolderid==null).FirstOrDefaultAsync();
                if (import == null)
                {
                    import = new Folder()
                    {
                        Name = "Импорт",
                        Userid = user.Id
                    };
                    db.Folders.Add(import);
                }
                await db.SaveChangesAsync();
                Folder oldfolder = await db.Folders.Where(x => x.Id == folderid).FirstAsync();
                Folder newfolder = new Folder()
                {
                    Name = oldfolder.Name,
                    Parentfolderid = import.Id
                };
                db.Folders.Add(newfolder);
                await db.SaveChangesAsync();
                await AddFolders(oldfolder.Id, newfolder.Id);
                await AddElements(oldfolder.Id, newfolder.Id);
                return Ok();
            }
        }

        public async Task AddFolders(int oldparentfolder, int newparentfolder)
        {
            Folder[] tfolders = await db.Folders.Where(x => x.Parentfolderid == oldparentfolder).ToArrayAsync();
            if (tfolders.Length > 0)
                foreach (Folder t in tfolders)
                {
                    Folder newfolder = new Folder()
                    {
                        Name = t.Name,
                        Parentfolderid = newparentfolder
                    };
                    db.Folders.Add(newfolder);
                    await db.SaveChangesAsync();
                    await AddFolders(t.Id, newfolder.Id);
                    await AddElements(t.Id, newfolder.Id);
                }
        }
        public async Task AddElements(int oldparentfolder, int newparentfolder)
        {
            Element[] elements = await db.Elements.Where(x => x.Parentfolderid == oldparentfolder).ToArrayAsync();
            if (elements.Length > 0)
                foreach (Element el in elements)
                {
                    Element newelement = new Element()
                    {
                        Name = el.Name,
                        Parentfolderid = newparentfolder,
                    };
                    db.Elements.Add(newelement);
                    await db.SaveChangesAsync();
                }

        }
        [HttpPost]
        public async Task<IActionResult> PostTfolder(Folder tfolder)
        {
            db.Folders.Add(tfolder);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTFolder(int id, Folder folder)
        {
            if (id != folder.Id)
            {
                return BadRequest();
            }

            db.Entry(folder).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!db.Folders.Any(e => e.Id == id))
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
        public async Task<ActionResult<Folder>> DeleteFolder(int id)
        {
            var folder = await db.Folders.FindAsync(id);
            if (folder == null)
            {
                return NotFound();
            }
            db.Folders.Remove(folder);
            await db.SaveChangesAsync();

            return folder;
        }

    }
}
