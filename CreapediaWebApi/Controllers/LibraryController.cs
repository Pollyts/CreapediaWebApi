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
            if ((type=="папка с элементами")|| (type == "элемент"))
            {
                Library[] libs = await db.Libraries.Where(x => x.Typeofcomponent == "папка с элементами" || x.Typeofcomponent == "элемент").ToArrayAsync();
                return libs;
            }
            else{
                Library[] libs = await db.Libraries.Where(x => x.Typeofcomponent == "папка с классами" || x.Typeofcomponent == "класс").ToArrayAsync();
                return libs;
            }
        }
        [HttpGet]
        [Route("/library/all")]
        public async Task<ActionResult<Library[]>>  GetClasses()
        {
            Library[] libs = await db.Libraries.ToArrayAsync();
            return libs;
        }

        //[HttpGet]
        //[Route("/library/importfromlib")]
        //public async Task<IActionResult> ImportFromLib(int userid, int importcompid)
        //{
        //    Library lib = await db.Libraries.Where(x => x.Id == importcompid).FirstAsync();
        //    switch (lib.Typeofcomponent)
        //    {
        //        case "папка с элементами":

        //            break;
        //        case "элемент":

        //            break;
        //        case "папка с классами":

        //            break;
        //        case "класс":

        //            break;

        //    }
        //    return Ok();
        //}
    }
}
