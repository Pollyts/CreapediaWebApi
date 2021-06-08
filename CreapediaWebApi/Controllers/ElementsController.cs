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
        [HttpPost]
        [Route("/elements/image")]
        public async Task<IActionResult> PostElementWithImage([FromForm] ElementWithImage el)
        {
            Element element = await db.Elements.Where(x => x.Parentfolderid == el.parentfolderid).FirstOrDefaultAsync();
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
        public async Task<IActionResult> PostElement(Element telement)
        {
            db.Elements.Add(telement);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
