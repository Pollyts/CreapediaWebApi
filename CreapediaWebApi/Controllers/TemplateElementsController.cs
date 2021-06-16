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
    public class TemplateElementsController : ControllerBase
    {
        d2v9eis2ivh7hhContext db;
        public TemplateElementsController(d2v9eis2ivh7hhContext context)
        {
            db = context;
        }
        //GET: elements/5
        // Возвращает список элементов в текущей папке
        [HttpGet("{id}")]
        public async Task<ActionResult<Templateelement[]>> GetTemplateElements(int id)
        {
            Templateelement[] telements;
            if (id == 0)
            {
                id = await db.Templatefolders.Where(x => x.Parentfolderid == null).Select(x => x.Id).FirstAsync();
            }
            telements = await db.Templateelements.Where(x => x.Parentfolderid == id).ToArrayAsync();            
            if (telements == null)
                return NotFound();
            return telements;
        }
        public static List<FullCharacteristic> listofcharacteristics;
       
        [HttpPost]
        public async Task<IActionResult> PostTelement(Templateelement telement)
        {
            db.Templateelements.Add(telement);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("/templateelements/templatefolder")]
        public async Task<IActionResult> SetTemplateElement(int templateid, int elementid)
        {
            db.Templatelinks.Add(new Templatelink()
            {
                Parenttelementid = templateid,
                Childelementid = elementid
            });
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<int>> PutElement(TemplateElementForEdit editelem)
        {

            //изменить имя
            Templateelement el = await db.Templateelements.Where(x => x.Id == editelem.IdElement).FirstAsync();
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
            Templatelink[] elementlinks = await db.Templatelinks.Where(x => x.Childelementid == editelem.IdElement).ToArrayAsync();
            foreach (Templatelink e in elementlinks)
            {
                if (!Array.Exists(editelem.templatecharacteristics, x => x.Id == e.Parenttelementid))
                {
                    db.Templatelinks.Remove(e);
                }
            }
            await db.SaveChangesAsync();
            //изменить характеристики
            Templatecharacteristic[] characteristics = await db.Templatecharacteristics.Where(x => x.Telementid == editelem.IdElement).ToArrayAsync();
            foreach (Templatecharacteristic c in characteristics)
            {
                if (!Array.Exists(editelem.characteristics, x => x.Id == c.Id))
                {
                    db.Templatecharacteristics.Remove(c);
                }
            }
            await db.SaveChangesAsync();            
            return el.Id;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Templateelement>> DeleteElement(int id)
        {
            var element = await db.Templateelements.FindAsync(id);
            if (element == null)
            {
                return NotFound();
            }
            db.Templateelements.Remove(element);
            await db.SaveChangesAsync();
            return element;
        }
    }
}
