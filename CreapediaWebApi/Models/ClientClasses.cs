using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;

namespace CreapediaWebApi.Models
{
    public class BodyofBreadCrumb
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ShortTelement
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ElementForEdit
    {
        public int IdElement { get; set; }
        public string Name { get; set; }
        public Characteristic[] characteristics { get; set; }
        public ShortTelement[] templatecharacteristics { get; set; }
        public FullRelation[] relations { get; set; }
    }

    public class TemplateFolderForEdit
    {
        public int IdFolder { get; set; }
        public string Name { get; set; }
        public int IdParent { get; set; }
        public Templatefolder[] subfolders { get; set; }
        public Templateelement[] elements { get; set; }
    }
    public class FolderForEdit
    {
        public int IdFolder { get; set; }
        public string Name { get; set; }
        public int IdParent { get; set; }
        public Folder [] subfolders { get; set; }
        public Element [] elements { get; set; }
    }
    public class SetOfNewCharacteristics
    {
        public NewCharacteristic[] characteristics { get; set; }
        public int elementid { get; set; }
    }
    public class FullRelation
    {
        public int Id { get; set; }
        public string NameFirstElement { get; set; }
        public string NameSecondElement { get; set; }
        public int IdFirst { get; set; }
        public int IdSecond { get; set; }
        public string Rel1to2 { get; set; }
        public string Rel2to1 { get; set; }
    }

    public class NewCharacteristic
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int Id { get; set; }
    }
    public class BreadCrumb
    {        
        public string title { get; set; }
        public string path { get; set; }               
        public BodyofBreadCrumb body { get; set; }
    }
    public class FullCharacteristic{
        public FullCharacteristic()
        {

        }
        public int IdParent { get; set; }
        public string NameParent { get; set; }
        public int IdCharacter { get; set; }
        public string NameCharacter { get; set; }
        public string ValueCharacter { get; set; }
    }
    public class ElementWithImage
    {
        public IFormFile Image { get; set; }
    }
    public class MainComponent
    {
        public MainComponent()
        {

        }
        public int Id { get; set; }
        public int Userid { get; set; }
        public string Name { get; set; }
    }

    public class EmailService
    {
        public async Task SendEmailAsync(string mail, int userid)
        {
            MailAddress from = new MailAddress("mycreapedia@gmail.com", "Creapedia");
            MailAddress to = new MailAddress(mail);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Подтверждение почты";
            m.Body = "Для подтверждения почтового адреса, пожалуйста, перейдите по ссылке:" + $"https://localhost:44348/mailconfirm?mail={mail}&name={userid}";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("mycreapedia@gmail.com", "ayorgzyeodcyakrw");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }
    }
public class ClientClasses
    {
    }
}
