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
    public class TElementsController : ControllerBase
    {
        d2v9eis2ivh7hhContext db;
        public TElementsController(d2v9eis2ivh7hhContext context)
        {
            db = context;
        }
        // GET: api/Tfolders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Templateelement[]>> GetTemplateElements(int id)
        {
            Templateelement[] telements = await db.Templateelements.Where(x => x.TemplatefolderId == id).ToArrayAsync();
            if (telements == null)
                return NotFound();
            return telements;
        }
        [HttpPost]
        public async Task<IActionResult> PostUser(Templateelement telement)
        {
            db.Templateelements.Add(telement);
            await db.SaveChangesAsync();
            return Ok();
        }

    }
}
