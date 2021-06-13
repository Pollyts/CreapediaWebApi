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
    public class RelationsController : ControllerBase
    {
        d2v9eis2ivh7hhContext db;
        public RelationsController(d2v9eis2ivh7hhContext context)
        {
            db = context;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<FullRelation[]>> GetRelations(int id)
        {
            Relation[] relations = await db.Relations.Where(x => x.Firstelementid == id || x.Secondelementid==id).ToArrayAsync();
            List<FullRelation> fullRelations = new List<FullRelation>();
            foreach(Relation r in relations)
            {
                fullRelations.Add(new FullRelation()
                {
                    NameFirstElement = await db.Elements.Where(x => x.Id == r.Firstelementid).Select(x => x.Name).FirstAsync(),
                    NameSecondElement = await db.Elements.Where(x => x.Id == r.Secondelementid).Select(x => x.Name).FirstAsync(),
                    Rel1to2 = r.Rel1to2,
                    Rel2to1 = r.Rel2to1,
                    IdFirst = r.Firstelementid,
                    IdSecond = r.Secondelementid
                });
            }
            return fullRelations.ToArray();
        }

        [HttpPost]
        [Route("/relations/relation1")]
        public async Task<IActionResult> PostRelation1(Relation rel)
        {
            db.Relations.Add(rel);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("/relations/relation2")]
        public async Task<IActionResult> PostRelation2(Relation rel)
        {
            db.Relations.Add(rel);
            await db.SaveChangesAsync();
            return Ok();
        }


    }
}
