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
    public class LibraryController : ControllerBase
    {
        d2v9eis2ivh7hhContext db;
        public LibraryController(d2v9eis2ivh7hhContext context)
        {
            db = context;
        }
        [HttpGet("{type}")]
        public async Task<ActionResult<Library[]>> GetCompsFromLibrary(string type)
        {
            Library[] libs = await db.Libraries.Where(x => x.Typeofcomponent == type).ToArrayAsync();
            return libs;
        }
    }
}
