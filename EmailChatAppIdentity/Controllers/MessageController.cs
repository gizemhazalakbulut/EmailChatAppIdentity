using EmailChatAppIdentity.Context;
using EmailChatAppIdentity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace EmailChatAppIdentity.Controllers
{
    public class MessageController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Inbox(string search) //gelen kutusu
        {

            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            var messageList = _context.Messages.Where(x => x.ReceiverEmail == values.Email);

            // Gelen kutusu (alıcı olarak bu kullanıcı var)
            ViewBag.InboxCount = _context.Messages.Count(m => m.ReceiverEmail == values.Email);

            // Giden kutusu (gönderen olarak bu kullanıcı var)
            ViewBag.SendboxCount = _context.Messages.Count(m => m.SenderEmail == values.Email);

            if (!string.IsNullOrEmpty(search))
            {
                string term = search.ToLower();
                messageList = messageList.Where(x =>
                    x.Subject.ToLower().Contains(term) ||
                    x.ReceiverEmail.ToLower().Contains(term) ||
                    x.MessageDetail.ToLower().Contains(term));
            }

            ViewBag.SearchTerm = search;
            return View(await messageList.ToListAsync());
        }

        public async Task<IActionResult> SendBox(string search) //giden kutusu
        {

            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            string emailValue = values.Email;

            // Gelen kutusu (alıcı olarak bu kullanıcı var)
            ViewBag.InboxCount = _context.Messages.Count(m => m.ReceiverEmail == values.Email);

            // Giden kutusu (gönderen olarak bu kullanıcı var)
            ViewBag.SendboxCount = _context.Messages.Count(m => m.SenderEmail == values.Email);

            // Önce tüm gönderilen mesajları getir
            var sendMessageList = _context.Messages.Where(x => x.SenderEmail == emailValue);
            // Eğer arama terimi varsa filtrele
            if (!string.IsNullOrEmpty(search))
            {
                string term = search.ToLower();
                sendMessageList = sendMessageList.Where(x =>
                    x.Subject.ToLower().Contains(term) ||
                    x.ReceiverEmail.ToLower().Contains(term) ||
                    x.MessageDetail.ToLower().Contains(term));
            }

            ViewBag.SearchTerm = search;
            return View(await sendMessageList.ToListAsync());
        }
  

        [HttpGet]
        public IActionResult CreateMessage()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(Message message)
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            string SenderEmail = values.Email;
            message.SenderEmail = SenderEmail;
            // Gelen kutusu (alıcı olarak bu kullanıcı var)
            ViewBag.InboxCount = _context.Messages.Count(m => m.ReceiverEmail == values.Email);

            // Giden kutusu (gönderen olarak bu kullanıcı var)
            ViewBag.SendboxCount = _context.Messages.Count(m => m.SenderEmail == values.Email);

            message.IsRead = false;
            message.SendDate = DateTime.Now;
            _context.Messages.Add(message);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi";
            return RedirectToAction("SendBox");
        }

        public async Task<IActionResult> MessageDetail(int id)
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            // Gelen kutusu (alıcı olarak bu kullanıcı var)
            ViewBag.InboxCount = _context.Messages.Count(m => m.ReceiverEmail == values.Email);

            // Giden kutusu (gönderen olarak bu kullanıcı var)
            ViewBag.SendboxCount = _context.Messages.Count(m => m.SenderEmail == values.Email);
            var message = _context.Messages.FirstOrDefault(x => x.MessageId == id);
            return View(message);
        }
    }
}
