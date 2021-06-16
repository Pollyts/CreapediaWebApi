using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CreapediaWebApi.Models;
using System.IO;

namespace CreapediaWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ElementsController : ControllerBase
    {
        d2v9eis2ivh7hhContext db;
        public ElementsController(d2v9eis2ivh7hhContext context)
        {
            db = context;
        }
        //GET: elements/5
        // Возвращает список элементов в текущей папке
        [HttpGet("{id}")]
        public async Task<ActionResult<Element[]>> GetElements(int id)
        {
            Element[] elements;
            if (id == 0)
            {
                id = await db.Folders.Where(x => x.Parentfolderid == null).Select(x => x.Id).FirstAsync();
            }
            elements = await db.Elements.Where(x => x.Parentfolderid == id).ToArrayAsync();
            if (elements == null)
                return NotFound();
            return elements;
        }

        [HttpGet]
        [Route("/elements/templatefolder")]
        public async Task<IActionResult> SetTemplateElement(int templateid, int elementid)
        {
            db.Elementlinks.Add(new Elementlink()
            {
                Parenttelementid = templateid,
                Childelementid = elementid
            });
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("/elements/image/{id}")]
        public async Task<IActionResult> PostElementWithImage(int id, [FromForm] ElementWithImage el)
        {
            Element element = await db.Elements.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (element == null)
                return Ok();
            byte[] imageData = null;
            using (var binaryReader = new BinaryReader(el.Image.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)el.Image.Length);
            }
            element.Image = imageData;
            db.Entry(element).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        [Route("/elements/breadcrumbs/{templateelementid}")]
        public async Task<ActionResult<BreadCrumb[]>> GetBreadCrumbs(int templateelementid)
        {
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
            Templateelement telement = await db.Templateelements.Where(x => x.Id == templateelementid).FirstAsync();
            breadCrumbs.Add(new BreadCrumb()
            {
                body=new BodyofBreadCrumb()
                {
                    Id = telement.Id,
                    Name = telement.Name,
                },                
                path = "/telement",
                title = telement.Name
            });
            int? folderid = telement.Parentfolderid;
            while (folderid != null)
            {
                Templatefolder templatefolder = await db.Templatefolders.Where(x => x.Id == folderid).FirstAsync();
                breadCrumbs.Add(new BreadCrumb()
                {
                    body = new BodyofBreadCrumb()
                    {
                        Id = templatefolder.Id,
                        Name = templatefolder.Name
                    },
                    path = "/tfolder",
                    title = telement.Name
                });
                folderid = templatefolder.Parentfolderid;
            }
            return breadCrumbs.ToArray();
            
        }
        public static List<FullCharacteristic> listofcharacteristics;

        //get /elements?idelement=5
        //Возвращает родительские характеристики элементу
        [HttpGet]
        public async Task<ActionResult<FullCharacteristic[]>> GetTemplatesForElement(int idelement)
        {
            listofcharacteristics = new List<FullCharacteristic>();
            Elementlink[] elementlinks = await db.Elementlinks.Where(x => x.Childelementid == idelement).ToArrayAsync();
            foreach (Elementlink el in elementlinks)
            {
                await GetTemplatesFromChild(el.Parenttelementid);
            }
            return listofcharacteristics.ToArray();
        }

        public async Task GetTemplatesFromChild(int childid)
        {
            //Найти все характеристики
            Templatecharacteristic[] templatecharacteristics = await db.Templatecharacteristics.Where(x => x.Telementid == childid).ToArrayAsync();
            Templateelement currentelement = await db.Templateelements.Where(x => x.Id == childid).FirstAsync();
            //Добавить характеристики родителя
            foreach (Templatecharacteristic tc in templatecharacteristics)
            {
                FullCharacteristic fc = new FullCharacteristic()
                {
                    IdParent = currentelement.Id,
                    NameParent = currentelement.Name,
                    IdCharacter = tc.Id,
                    NameCharacter = tc.Name,
                    ValueCharacter = tc.Value
                };
                listofcharacteristics.Add(fc);
            }
            //Найти всех родителей
            Templatelink[] templatelinks = await db.Templatelinks.Where(x => x.Childelementid == childid).ToArrayAsync();
            //Рекурсия по родителям
            foreach (Templatelink tl in templatelinks)
            {
                await GetTemplatesFromChild(tl.Parenttelementid);
            }

        }
        [HttpPost]
        public async Task<ActionResult<int>> PostElement(Element telement)
        {
            db.Elements.Add(telement);
            await db.SaveChangesAsync();
            return telement.Id;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Element>> DeleteElement(int id)
        {
            var element = await db.Elements.FindAsync(id);
            if (element == null)
            {
                return NotFound();
            }
            db.Elements.Remove(element);
            await db.SaveChangesAsync();
            return element;
        }

        [HttpPut]
        public async Task<ActionResult<int>> PutElement(ElementForEdit editelem)
        {

            //изменить имя
            Element el = await db.Elements.Where(x => x.Id == editelem.IdElement).FirstAsync();
            el.Name = editelem.Name;
            db.Entry(el).State = EntityState.Modified;
            //List<Characteristic> chars = await db.Characteristics.Where(x => x.Elementid == editelem.IdElement).ToListAsync();
            //foreach (Characteristic c in chars)
            //{
            //    if (!Array.Exists(editelem.characteristics, x => x.Id == c.Id))
            //    {
            //        db.Characteristics.Remove(c);
            //    }
            //}
            //await db.SaveChangesAsync();
            //изменить классы
            Elementlink[] elementlinks = await db.Elementlinks.Where(x => x.Childelementid == editelem.IdElement).ToArrayAsync();
            foreach (Elementlink e in elementlinks)
            {
                if (!Array.Exists(editelem.templatecharacteristics, x => x.Id == e.Parenttelementid))
                {
                    db.Elementlinks.Remove(e);
                }
            }
            await db.SaveChangesAsync();
            //изменить характеристики
            Characteristic[] characteristics = await db.Characteristics.Where(x => x.Elementid == editelem.IdElement).ToArrayAsync();
            foreach (Characteristic c in characteristics)
            {
                if (!Array.Exists(editelem.characteristics, x => x.Id == c.Id))
                {
                    db.Characteristics.Remove(c);
                }
            }
            await db.SaveChangesAsync();
            //изменить связи
            Relation[] relations = await db.Relations.Where(x => x.Firstelementid == editelem.IdElement || x.Secondelementid == editelem.IdElement).ToArrayAsync();
            foreach (Relation r in relations)
            {
                if (!Array.Exists(editelem.relations, x => x.Id == r.Id))
                {
                    db.Relations.Remove(r);
                }
            }
            await db.SaveChangesAsync();
            return el.Id;
        }

        [HttpGet]
        [Route("/elements/exporttouser")]
        public async Task<IActionResult> ExportToUser(int elementid, string usermail)
        {
            User user = await db.Users.Where(x => x.Mail == usermail).FirstAsync();
            if (user == null)
                return BadRequest("Нет такого пользователя");
            else
            {
                Folder Projects = await db.Folders.Where(x => x.Userid == user.Id && x.Parentfolderid == null).FirstOrDefaultAsync();
                Folder import = await db.Folders.Where(x => x.Name == "Импорт" && x.Userid == user.Id && x.Parentfolderid == Projects.Id).FirstOrDefaultAsync();
                if (import == null)
                {
                    import = new Folder()
                    {
                        Name = "Импорт",
                        Userid = user.Id,
                        Parentfolderid = Projects.Id
                    };
                    db.Folders.Add(import);
                }
                await db.SaveChangesAsync();
                Element oldelem = await db.Elements.Where(x => x.Id == elementid).FirstAsync();
                Element newelement = new Element()
                {
                    Name = oldelem.Name,
                    Image = oldelem.Image,   
                    Parentfolderid = import.Id,
                };
                db.Elements.Add(newelement);
                await db.SaveChangesAsync();
                await AddCharacteristics(oldelem.Id, newelement.Id);
                return Ok();
            }
        }
        public async Task AddCharacteristics(int oldelement, int newelement)
        {
            //получила все характеристики старого
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
                    db.Characteristics.Add(newcharacteristic);
                }
            Elementlink[] elementlinks = await db.Elementlinks.Where(x => x.Childelementid == oldelement).ToArrayAsync();
            foreach (Elementlink el in elementlinks)
            {
                Elementlink newlink = new Elementlink()
                {
                    Parenttelementid = el.Parenttelementid,
                    Childelementid = newelement
                };
                db.Elementlinks.Add(newlink);
            }
        }

        [HttpGet]
        [Route("/elements/exporttofolder")]
        public async Task<IActionResult> ExportToFolder(int elementid, int newrootid)
        {
            Folder newroot = await db.Folders.Where(x => x.Id == newrootid).FirstOrDefaultAsync();
            Element oldelem = await db.Elements.Where(x => x.Id == elementid).FirstAsync();
            Element newelement = new Element()
            {
                Name = oldelem.Name,
                Image = oldelem.Image,
                Parentfolderid = newroot.Id,
            };
            db.Elements.Add(newelement);
            await db.SaveChangesAsync();
            await AddCharacteristics(oldelem.Id, newelement.Id);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("/elements/exporttolibrary")]
        public async Task<IActionResult> ExportToLibrary(int elementid, string name, string password)
        {
            db.Libraries.Add(new Library()
            {
                Name = name,
                Componentid = elementid,
                Password = password,
                Typeofcomponent = "папка"
            });
            await db.SaveChangesAsync();
            return Ok();
        }


    }
}
