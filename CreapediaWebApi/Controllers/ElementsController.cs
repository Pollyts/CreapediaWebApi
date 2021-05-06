using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CreapediaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElementsController : ControllerBase
    {
        //d2v9eis2ivh7hhContext db;
        //public ElementsController(d2v9eis2ivh7hhContext context)
        //{
        //    db = context;
        //}
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Templatefolder[]>> GetTfolder(int id)
        //{
        //    Templatefolder[] tfolders = await db.Templatefolders.Where(x => x.ParentfolderId == id).ToArrayAsync();
        //    if (tfolders == null)
        //        return NotFound();
        //    return tfolders;
        //}
    }
}
