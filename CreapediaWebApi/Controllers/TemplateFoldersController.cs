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
    public class TemplateFoldersController : ControllerBase
    {
        d2v9eis2ivh7hhContext db;
        public TemplateFoldersController(d2v9eis2ivh7hhContext context)
        {
            db = context;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Templatefolder[]>> GetTemplateFoldersFromParent(int id)
        {
            Templatefolder[] templatefolders;
            if (id == 0)
            {
                id = await db.Templatefolders.Where(x => x.Parentfolderid == null).Select(x => x.Id).FirstAsync();
            }
            if (id == 0)
            {
                return null;
            }
            templatefolders = await db.Templatefolders.Where(x => x.Parentfolderid == id).ToArrayAsync();
            if (templatefolders == null)
                return NotFound();
            return templatefolders;
        }
       
        [HttpPost]
        public async Task<IActionResult> PostTfolder(Templatefolder tfolder)
        {
            Templatefolder parentfolder = await db.Templatefolders.Where(x => x.Id == tfolder.Parentfolderid).FirstAsync();
            db.Templatefolders.Add(new Templatefolder
            { 
            Name=tfolder.Name,
            Parentfolderid=parentfolder.Id,
            Userid = parentfolder.Userid
            });
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutFolder(TemplateFolderForEdit editfolder)
        {
            Templatefolder folder = await db.Templatefolders.Where(x => x.Id == editfolder.IdFolder).FirstAsync();
            folder.Name = editfolder.Name;
            folder.Parentfolderid = editfolder.IdParent;
            db.Entry(folder).State = EntityState.Modified;
            List<Templatefolder> subfolders = await db.Templatefolders.Where(x => x.Parentfolderid == editfolder.IdFolder).ToListAsync();
            foreach (Templatefolder f in subfolders)
            {
                if (!Array.Exists(editfolder.subfolders, x => x.Id == f.Id))
                {
                    db.Templatefolders.Remove(f);
                }
            }
            await db.SaveChangesAsync();
            Templateelement[] elements = await db.Templateelements.Where(x => x.Parentfolderid == editfolder.IdFolder).ToArrayAsync();
            foreach (Templateelement e in elements)
            {
                if (!Array.Exists(editfolder.elements, x => x.Id == e.Id))
                {
                    db.Templateelements.Remove(e);
                }
            }
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Templatefolder>> DeleteTFolder(int id)
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

        [HttpGet]
        [Route("/templatefolders/exporttouser")]
        public async Task<IActionResult> ExportToUser(int folderid, string usermail)
        {
            User user = await db.Users.Where(x => x.Mail == usermail).FirstAsync();
            if (user == null)
                return BadRequest("Нет такого пользователя");
            else
            {
                Templatefolder Library = await db.Templatefolders.Where(x => x.Userid == user.Id && x.Parentfolderid == null).FirstOrDefaultAsync();
                Templatefolder import = await db.Templatefolders.Where(x => x.Name == "Импорт" && x.Userid == user.Id && x.Parentfolderid == Library.Id).FirstOrDefaultAsync();
                if (import == null)
                {
                    import = new Templatefolder()
                    {
                        Name = "Импорт",
                        Userid = user.Id,
                        Parentfolderid = Library.Id
                    };
                    db.Templatefolders.Add(import);
                }
                await db.SaveChangesAsync();
                Templatefolder oldfolder = await db.Templatefolders.Where(x => x.Id == folderid).FirstAsync();
                Templatefolder newfolder = new Templatefolder()
                {
                    Name = oldfolder.Name,
                    Parentfolderid = import.Id,
                    Userid = user.Id
                };
                db.Templatefolders.Add(newfolder);
                await db.SaveChangesAsync();
                await AddFolders(oldfolder.Id, newfolder.Id, user.Id);
                await AddElements(oldfolder.Id, newfolder.Id);
                return Ok();
            }
        }

        [HttpGet]
        [Route("/folders/exporttofolder")]
        public async Task<IActionResult> ExportToFolder(int folderid, int newrootid)
        {
            Templatefolder newroot = await db.Templatefolders.Where(x => x.Id == newrootid).FirstOrDefaultAsync();
            Templatefolder oldfolder = await db.Templatefolders.Where(x => x.Id == folderid).FirstAsync();
            Templatefolder newfolder = new Templatefolder()
            {
                Name = oldfolder.Name,
                Parentfolderid = newrootid,
                Userid = oldfolder.Userid
            };
            db.Templatefolders.Add(newfolder);
            await db.SaveChangesAsync();
            await AddFolders(oldfolder.Id, newfolder.Id, oldfolder.Userid);
            await AddElements(oldfolder.Id, newfolder.Id);
            return Ok();
        }

        [HttpGet]
        [Route("/folders/exporttolibrary")]
        public async Task<IActionResult> ExportToLibrary(int folderid, string name, string password)
        {
            db.Libraries.Add(new Library()
            {
                Name = name,
                Componentid = folderid,
                Password = password,
                Typeofcomponent = "папка с классами"
            });
            await db.SaveChangesAsync();
            return Ok();
        }


        //Копирование компонента в папку
        [HttpGet]
        [Route("/folders/importfromfolder")]
        public async Task<IActionResult> ImportFromFolder(int folderid, int importcompid, string type)
        {
            Templatefolder newroot = await db.Templatefolders.Where(x => x.Id == folderid).FirstOrDefaultAsync();
            if (type == "folder")
            {
                Templatefolder oldfolder = await db.Templatefolders.Where(x => x.Id == importcompid).FirstAsync();
                Templatefolder newfolder = new Templatefolder()
                {
                    Name = oldfolder.Name,
                    Parentfolderid = newroot.Id,
                    Userid = oldfolder.Userid
                };
                db.Templatefolders.Add(newfolder);
                await db.SaveChangesAsync();
                await AddFolders(oldfolder.Id, newfolder.Id, oldfolder.Userid);
                await AddElements(oldfolder.Id, newfolder.Id);
                return Ok();
            }
            else //если копировался элемент
            {
                Templateelement oldelement = await db.Templateelements.Where(x => x.Id == importcompid).FirstAsync();
                Templateelement newelement = new Templateelement()
                {                    
                    Name = oldelement.Name,
                    Parentfolderid = newroot.Id,
                };
                db.Templateelements.Add(newelement);
                await db.SaveChangesAsync();
                await AddCharacteristics(oldelement.Id, newelement.Id);
                await AddRelations(oldelement.Id, newelement.Id);
                return Ok();
            }

        }

        [HttpGet]
        [Route("/folders/importfromlib")]
        public async Task<IActionResult> ImportFromLibrary(int folderid, int importcompid, string password)
        {
            Library lib = await db.Libraries.Where(x => x.Id == importcompid).FirstOrDefaultAsync();
            if (lib.Password == password)
            {
                Templatefolder newroot = await db.Templatefolders.Where(x => x.Id == folderid).FirstOrDefaultAsync();
                if (lib.Typeofcomponent == "папка с классами")
                {
                    Templatefolder oldfolder = await db.Templatefolders.Where(x => x.Id == lib.Componentid).FirstAsync();
                    Templatefolder newfolder = new Templatefolder()
                    {
                        Name = oldfolder.Name,
                        Parentfolderid = newroot.Id,
                        Userid = newroot.Userid
                    };
                    db.Templatefolders.Add(newfolder);
                    await db.SaveChangesAsync();
                    await AddFolders(oldfolder.Id, newfolder.Id, newroot.Userid);
                    await AddElements(oldfolder.Id, newfolder.Id);
                    return Ok();
                }
                else
                {
                    Templateelement oldelement = await db.Templateelements.Where(x => x.Id == lib.Componentid).FirstAsync();
                    Templateelement newelement = new Templateelement()
                    {
                        Name = oldelement.Name,
                        Parentfolderid = newroot.Id,
                    };
                    db.Templateelements.Add(newelement);
                    await db.SaveChangesAsync();
                    await AddCharacteristics(oldelement.Id, newelement.Id);
                    return Ok();
                };
            }
            else
            {
                return BadRequest("Неверный пароль");
            }
        }
        public async Task AddFolders(int oldparentfolder, int newparentfolder, int userid)
        {
            Templatefolder[] tfolders = await db.Templatefolders.Where(x => x.Parentfolderid == oldparentfolder).ToArrayAsync();
            if (tfolders.Length > 0)
                foreach (Templatefolder t in tfolders)
                {
                    Templatefolder newfolder = new Templatefolder()
                    {
                        Name = t.Name,
                        Parentfolderid = newparentfolder,
                        Userid = userid
                    };
                    db.Templatefolders.Add(newfolder);
                    await db.SaveChangesAsync();
                    await AddFolders(t.Id, newfolder.Id, userid);
                    await AddElements(t.Id, newfolder.Id);
                }
        }
        public async Task AddElements(int oldparentfolder, int newparentfolder)
        {
            Templateelement[] elements = await db.Templateelements.Where(x => x.Parentfolderid == oldparentfolder).ToArrayAsync();
            if (elements.Length > 0)
                foreach (Templateelement el in elements)
                {
                    Templateelement newelement = new Templateelement()
                    {
                        Name = el.Name,
                        Parentfolderid = newparentfolder
                    };
                    db.Templateelements.Add(newelement);
                    await db.SaveChangesAsync();
                    await AddCharacteristics(el.Id, newelement.Id);
                    foreach (Characteristic c in listofcharacteristics)
                    {
                        db.Characteristics.Add(c);
                    }
                    await db.SaveChangesAsync();
                }

        }
        public static List<Characteristic> listofcharacteristics;

        public async Task AddCharacteristics(int oldelement, int newelement)
        {
            listofcharacteristics = new List<Characteristic>();
            Characteristic[] chars = await db.Characteristics.Where(x => x.Elementid == oldelement).ToArrayAsync();
            if (chars.Length > 0)
                foreach (Characteristic c in chars)
                {
                    Characteristic newcharacteristic = new Characteristic()
                    {
                        Value = c.Value,
                        Name = c.Name,
                        Elementid = newelement,
                    };
                    listofcharacteristics.Add(newcharacteristic);
                }
            Elementlink[] elementlinks = await db.Elementlinks.Where(x => x.Childelementid == oldelement).ToArrayAsync();
            foreach (Elementlink el in elementlinks)
            {
                await GetTemplateCharacteristicsFromChild(el.Parenttelementid, newelement);
            }
        }

        public async Task GetTemplateCharacteristicsFromChild(int childid, int newelementid)
        {
            //Найти все характеристики
            Templatecharacteristic[] templatecharacteristics = await db.Templatecharacteristics.Where(x => x.Telementid == childid).ToArrayAsync();
            Templateelement currentelement = await db.Templateelements.Where(x => x.Id == childid).FirstAsync();
            //Добавить характеристики родителя
            foreach (Templatecharacteristic tc in templatecharacteristics)
            {
                Characteristic c = new Characteristic()
                {
                    Name = tc.Name,
                    Value = tc.Value,
                    Elementid = newelementid
                };
                listofcharacteristics.Add(c);
            }
            //Найти всех родителей
            Templatelink[] templatelinks = await db.Templatelinks.Where(x => x.Childelementid == childid).ToArrayAsync();
            //Рекурсия по родителям
            foreach (Templatelink tl in templatelinks)
            {
                await GetTemplateCharacteristicsFromChild(tl.Parenttelementid, newelementid);
            }

        }
    }
}
