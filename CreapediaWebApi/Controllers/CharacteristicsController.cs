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
    public class CharacteristicsController : ControllerBase
    {
        d2v9eis2ivh7hhContext db;
        public CharacteristicsController(d2v9eis2ivh7hhContext context)
        {
            db = context;
        }

        //get /characteristics/12
        [HttpGet("{idelement}")]
        public async Task<ActionResult<Characteristic[]>> GetCharacteristics(int idelement)
        {
            Characteristic[] characteristics;
            characteristics = await db.Characteristics.Where(x => x.Elementid == idelement).ToArrayAsync();
            if (characteristics == null)
                return NotFound();
            return characteristics;
        }
        public static List<FullCharacteristic> listofcharacteristics;

        //get /characteristics?idelement=5

        [HttpGet]
        public async Task<ActionResult<FullCharacteristic[]>> GetTemplateCharacteristicsForElement(int idelement)
        {
            listofcharacteristics = new List<FullCharacteristic>();
            Elementlink[] elementlinks = await db.Elementlinks.Where(x => x.Childelementid == idelement).ToArrayAsync();
            foreach (Elementlink el in elementlinks)
            {
                await GetTemplateCharacteristicsFromChild(el.Parenttelementid);
            }
            return listofcharacteristics.ToArray();
        }

        public async Task GetTemplateCharacteristicsFromChild(int childid)
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
                await GetTemplateCharacteristicsFromChild(tl.Parenttelementid);
            }

        }
    }
}
