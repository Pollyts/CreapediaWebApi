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
//    public class MainFoldersController : ControllerBase
//    {
//        d2v9eis2ivh7hhContext db;
//        public MainFoldersController(d2v9eis2ivh7hhContext context)
//        {
//            db = context;
//        }
//        [HttpGet]
//        public async Task<ActionResult<Mainfolder[]>> GetMainFolders(int userid)
//        {
//            Mainfolder[] mfolders = await db.Mainfolders.Where(x => x.Userid==userid).ToArrayAsync();
//            if (mfolders == null)
//                return NotFound();
//            return mfolders;
//        }
//        //public async Task<ActionResult<Mainfolder[]>> CreateMainFolders(int userid)
//        //{
            
//        //    Mainfolder[] mfolders = await db.Mainfolders.Where(x => x.Userid == userid).ToArrayAsync();
//        //    if (mfolders == null)
//        //        return NotFound();
//        //    return mfolders;
//        //}
//    }
//}
