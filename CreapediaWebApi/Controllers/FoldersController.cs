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

        [HttpGet]
        [Route("/folders/main/{userid}")]
        public async Task<ActionResult<MainComponent[]>> GetMainComponents(int userid)
        {
            List<MainComponent> maincomponents = new List<MainComponent>();
            Folder mainfolder = await db.Folders.Where(x => x.Parentfolderid == null && x.Userid == userid).FirstAsync();
            Templatefolder maintfolder = await db.Templatefolders.Where(x => x.Parentfolderid == null && x.Userid == userid).FirstAsync();
            maincomponents.Add(new MainComponent() { Id = mainfolder.Id, Userid = mainfolder.Userid, Name = mainfolder.Name });
            maincomponents.Add(new MainComponent() { Id = maintfolder.Id, Userid = maintfolder.Userid, Name = maintfolder.Name });
            return maincomponents.ToArray();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Folder[]>> GetFoldersFromParent(int id, int iduser)
        {
            Folder[] folders;
            folders = await db.Folders.Where(x => x.Parentfolderid == id).ToArrayAsync();
            return folders;
        }

        

        [HttpGet]
        [Route("/folders/exporttouser")]
        public async Task<IActionResult> ExportToUser(int folderid, string usermail)
        {
            User user = await db.Users.Where(x => x.Mail == usermail).FirstAsync();
            if (user == null)
                return BadRequest("Нет такого пользователя");
            else
            {
                Folder Projects = await db.Folders.Where(x => x.Userid == user.Id && x.Parentfolderid == null).FirstOrDefaultAsync();
                Folder import = await db.Folders.Where(x => x.Name == "Импорт" && x.Userid == user.Id && x.Parentfolderid==Projects.Id).FirstOrDefaultAsync();
                if (import == null)
                {
                    import = new Folder()
                    {
                        Name = "Импорт",
                        Userid = user.Id,
                        Parentfolderid=Projects.Id
                };
                    db.Folders.Add(import);
                }
                await db.SaveChangesAsync();
                Folder oldfolder = await db.Folders.Where(x => x.Id == folderid).FirstAsync();
                Folder newfolder = new Folder()
                {
                    Name = oldfolder.Name,
                    Parentfolderid = import.Id,
                    Userid=user.Id
                };
                db.Folders.Add(newfolder);
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
                Folder newroot = await db.Folders.Where(x => x.Id == newrootid).FirstOrDefaultAsync();
                Folder oldfolder = await db.Folders.Where(x => x.Id == folderid).FirstAsync();
                Folder newfolder = new Folder()
                {
                    Name = oldfolder.Name,
                    Parentfolderid = newrootid,
                    Userid = oldfolder.Userid
                };
                db.Folders.Add(newfolder);
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
            Name=name,
            Componentid=folderid,
            Password=password,
            Typeofcomponent="папка с элементами"            
            });
            await db.SaveChangesAsync();
            return Ok();
        }


        //Копирование компонента в папку
        [HttpGet]
        [Route("/folders/importfromfolder")]
        public async Task<IActionResult> ImportFromFolder(int folderid, int importcompid,string type)
        {
            Folder newroot = await db.Folders.Where(x => x.Id == folderid).FirstOrDefaultAsync();
            if (type=="folder")
            {                
                Folder oldfolder = await db.Folders.Where(x => x.Id == importcompid).FirstAsync();
                Folder newfolder = new Folder()
                {
                    Name = oldfolder.Name,
                    Parentfolderid = newroot.Id,
                    Userid = oldfolder.Userid
                };
                db.Folders.Add(newfolder);
                await db.SaveChangesAsync();
                await AddFolders(oldfolder.Id, newfolder.Id, oldfolder.Userid);
                await AddElements(oldfolder.Id, newfolder.Id);
                return Ok();
            }
            else //если копировался элемент
            {
                Element oldelement = await db.Elements.Where(x => x.Id == importcompid).FirstAsync();
                Element newelement = new Element()
                {
                    Image=oldelement.Image,                    
                    Name = oldelement.Name,
                    Parentfolderid = newroot.Id,
                };
                db.Elements.Add(newelement);
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
                Folder newroot = await db.Folders.Where(x => x.Id == folderid).FirstOrDefaultAsync();
                if (lib.Typeofcomponent == "папка c элементами")
                {
                    Folder oldfolder = await db.Folders.Where(x => x.Id == lib.Componentid).FirstAsync();
                    Folder newfolder = new Folder()
                    {
                        Name = oldfolder.Name,
                        Parentfolderid = newroot.Id,
                        Userid = newroot.Userid
                    };
                    db.Folders.Add(newfolder);
                    await db.SaveChangesAsync();
                    await AddFolders(oldfolder.Id, newfolder.Id, newroot.Userid);
                    await AddElements(oldfolder.Id, newfolder.Id);
                    return Ok();
                }
                else
                {
                    Element oldelement = await db.Elements.Where(x => x.Id == lib.Componentid).FirstAsync();
                    Element newelement = new Element()
                    {
                        Image = oldelement.Image,
                        Name = oldelement.Name,
                        Parentfolderid = newroot.Id,
                    };
                    db.Elements.Add(newelement);
                    await db.SaveChangesAsync();
                    await AddCharacteristics(oldelement.Id, newelement.Id);
                    foreach (Characteristic c in listofcharacteristics)
                    {
                        db.Characteristics.Add(c);
                    }
                    await db.SaveChangesAsync();
                    //await AddRelations(oldelement.Id, newelement.Id);
                    return Ok();
                };                
            }
            else
            {
                return BadRequest("Неверный пароль");
            }
        }

        public async Task AddRelations(int oldelement, int newelement)
        {
            
        }


        public async Task AddFolders(int oldparentfolder, int newparentfolder, int userid)
        {
            Folder[] tfolders = await db.Folders.Where(x => x.Parentfolderid == oldparentfolder).ToArrayAsync();
            if (tfolders.Length > 0)
                foreach (Folder t in tfolders)
                {
                    Folder newfolder = new Folder()
                    {
                        Name = t.Name,
                        Parentfolderid = newparentfolder,
                        Userid=userid
                    };
                    db.Folders.Add(newfolder);
                    await db.SaveChangesAsync();
                    await AddFolders(t.Id, newfolder.Id,userid);
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
                        Image=el.Image
                    };
                    db.Elements.Add(newelement);
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
                        Value=c.Value,                        
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
                    Value=tc.Value,
                    Elementid=newelementid
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




        [HttpPost]       
        public async Task<IActionResult> PostFolder(Folder folder)
        {
            Folder parentfolder = await db.Folders.Where(x => x.Id == folder.Parentfolderid).FirstAsync();
            db.Folders.Add(new Folder
            {
                Name = folder.Name,
                Parentfolderid = parentfolder.Id,
                Userid = parentfolder.Userid
            });
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutFolder(FolderForEdit editfolder)
        {
            Folder folder = await db.Folders.Where(x => x.Id == editfolder.IdFolder).FirstAsync();
            folder.Name = editfolder.Name;
            folder.Parentfolderid = editfolder.IdParent;
            db.Entry(folder).State = EntityState.Modified;
            List <Folder> subfolders = await db.Folders.Where(x => x.Parentfolderid==editfolder.IdFolder).ToListAsync();
            foreach(Folder f in subfolders)
            {
                if (!Array.Exists(editfolder.subfolders, x => x.Id==f.Id))
                {
                    db.Folders.Remove(f);
                }
            }
            await db.SaveChangesAsync();
            Element[] elements = await db.Elements.Where(x => x.Parentfolderid == editfolder.IdFolder).ToArrayAsync();
            foreach (Element e in elements)
            {
                if (!Array.Exists(editfolder.elements, x => x.Id == e.Id))
                {
                    db.Elements.Remove(e);
                }
            }
            await db.SaveChangesAsync();
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

