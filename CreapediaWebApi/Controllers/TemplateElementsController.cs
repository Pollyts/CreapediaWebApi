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
    }
}
