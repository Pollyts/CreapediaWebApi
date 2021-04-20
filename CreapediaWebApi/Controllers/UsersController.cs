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
    public class UsersController : ControllerBase
    {
        d2v9eis2ivh7hhContext db;
        public UsersController(d2v9eis2ivh7hhContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<User[]> GetUsers()
        {
            return await db.Users.ToArrayAsync();
        }
    }
}
