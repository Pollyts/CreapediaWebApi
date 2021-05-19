//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using CreapediaWebApi.Models;

//namespace CreapediaWebApi.Controllers
//{
//    [Route("[controller]")]
//    [ApiController]
//    public class ElementsController : ControllerBase
//    {
//        d2v9eis2ivh7hhContext db;
//        public ElementsController(d2v9eis2ivh7hhContext context)
//        {
//            db = context;
//        }
//        // GET: api/Tfolders/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Element[]>> GetTemplateElements(int id)
//        {
//            Element[] elements = await db.Elements.Where(x => x.IdParentfolder == id).ToArrayAsync();
//            if (elements == null)
//                return NotFound();
//            return elements;
//        }
//        [HttpPost]
//        public async Task<IActionResult> PostUser(Element telement)
//        {
//            db.Elements.Add(telement);
//            await db.SaveChangesAsync();
//            return Ok();
//        }
//    }
//}
