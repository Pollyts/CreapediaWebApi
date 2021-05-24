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
            Templateelement[] elements;
            if (id == 0)
            {
                id = await db.Templatefolders.Where(x => x.Parentfolderid == null).Select(x => x.Id).FirstAsync();
            }
            elements = await db.Templateelements.Where(x => x.Parentfolderid == id).ToArrayAsync();            
            if (elements == null)
                return NotFound();
            return elements;
        }
        public static List<FullCharacteristic> listofcharacteristics;

        //get /elements?idelement=5

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
        //public async Task<ActionResult<Characteristic[]>> GetElement(int idelement)
        //{
        //    Element[] elements = await db.Elements.Where(x => x.Parentfolderid == id).ToArrayAsync();
        //    if (elements == null)
        //        return NotFound();
        //    return elements;
        //}
        [HttpPost]
        public async Task<IActionResult> PostUser(Element telement)
        {
            db.Elements.Add(telement);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
