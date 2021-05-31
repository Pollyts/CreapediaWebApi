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
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        d2v9eis2ivh7hhContext db;
        public UsersController(d2v9eis2ivh7hhContext context)
        {
            db = context;
        }
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return NotFound();
            return new ObjectResult(user);
        }

        [HttpGet]
        [Route("/users/{userid}")]
        public async Task<ActionResult<MainComponent[]>> GetMainComponents(int userid)
        {
            List<MainComponent> maincomponents = new List<MainComponent>();
            Folder mainfolder = await db.Folders.Where(x => x.Parentfolderid == null && x.Userid == userid).FirstAsync();
            Templatefolder maintfolder = await db.Templatefolders.Where(x => x.Parentfolderid == null && x.Userid == userid).FirstAsync();
            maincomponents.Add(new MainComponent() { Id = mainfolder.Id, Userid = mainfolder.Userid, Name = mainfolder.Name });
            maincomponents.Add(new MainComponent() { Id = maintfolder.Id, Userid = maintfolder.Userid, Name = maintfolder.Name });
            return maincomponents.ToArray();
        }

        [HttpGet]
        [Route("/mailconfirm")]
        public async Task<ContentResult> MailConfirm([FromQuery] string mail, [FromQuery] int name)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Id == name && x.Mail == mail);
            if (user != null)
            {
                user.Mailconfirm = true;
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            return base.Content("<div>Адрес почты успешно подтвержден. <a href='http://localhost:3000/'>Вернуться на главную страницу приложения</a></div>", "text/html", System.Text.Encoding.UTF8);
        }

        [HttpGet]
        public async Task<object> GetUser(string? mail, string? pass)
        {
            //if (mail != null)
            //{
            User user = await db.Users.Where(x => x.Mail == mail && x.Password == pass).FirstAsync();
            if (user == null)
                return BadRequest("Неверная комбинация логина и пароля");
            //if (!user.MailConfirm)
            //    return BadRequest("Нужно подтверждение почты");
            return Ok(user);
            //}
            //else
            //    return db.Users.Select(x => new { x.Id, x.Name });
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPost]
        public async Task PostUser(User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(user.Mail, user.Id);
            db.Folders.Add(new Folder()
            {
                Name="Проекты",
                Userid=user.Id,
            });
            db.Templatefolders.Add(new Templatefolder()
            {
                Name = "Библиотека компонентов",
                Userid = user.Id,
            });
            await db.SaveChangesAsync();            
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return db.Users.Any(e => e.Id == id);
        }
    }
}
