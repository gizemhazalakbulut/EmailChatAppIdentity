using EmailChatAppIdentity.Context;
using EmailChatAppIdentity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmailChatAppIdentity.Controllers
{
    public class ProfileController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _context;

        public ProfileController(UserManager<AppUser> userManager, EmailContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> ProfileDetail()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.name = values.Name;
            ViewBag.surname = values.Surname;
            ViewBag.userName = values.UserName;
            ViewBag.phoneNumber = values.PhoneNumber;
            ViewBag.email = values.Email;
            ViewBag.city = values.City;
            ViewBag.profileImageUrl = values.ProfileImageUrl;
            var messageCount = _context.Messages.Count(x => x.ReceiverEmail == values.Email);
            ViewBag.messageCount = messageCount;
            var sendMessageCount = _context.Messages.Count(x => x.SenderEmail == values.Email);
            ViewBag.sendMessageCount = sendMessageCount;
            var totalMessageCount = messageCount + sendMessageCount;
            ViewBag.totalMessageCount = totalMessageCount;
            // Gelen kutusu (alıcı olarak bu kullanıcı var)
            ViewBag.InboxCount = _context.Messages.Count(m => m.ReceiverEmail == values.Email);

            // Giden kutusu (gönderen olarak bu kullanıcı var)
            ViewBag.SendboxCount = _context.Messages.Count(m => m.SenderEmail == values.Email);
            return View();
        }
    }
}
