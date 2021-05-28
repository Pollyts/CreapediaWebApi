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
    public class TemplateCharacteristicsController : ControllerBase
    {
        d2v9eis2ivh7hhContext db;
        public TemplateCharacteristicsController(d2v9eis2ivh7hhContext context)
        {
            db = context;
        }
        [HttpGet("{idelement}")]
        public async Task<ActionResult<Templatecharacteristic[]>> GetTemplateCharacteristics(int idelement)
        {
            Templatecharacteristic[] tcharacteristics;
            tcharacteristics = await db.Templatecharacteristics.Where(x => x.Telementid == idelement).ToArrayAsync();
            if (tcharacteristics == null)
                return NotFound();
            return tcharacteristics;
        }
        public static List<FullCharacteristic> listofcharacteristics;


        //get /characteristics?idelement=5
        [HttpGet]
        public async Task<ActionResult<FullCharacteristic[]>> GetTemplateCharacteristicsForElement(int idelement)
        {
            listofcharacteristics = new List<FullCharacteristic>();
            Templatelink[] templatelinks = await db.Templatelinks.Where(x => x.Childelementid == idelement).ToArrayAsync();
            foreach (Templatelink tl in templatelinks)
            {
                await GetTemplatesFromChild(tl.Parenttelementid);
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

    }
}
