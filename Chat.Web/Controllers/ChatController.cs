using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Chat.DAL.Repositories.Interfaces;
using PusherServer;

namespace Chat.Web.Controllers
{
    [Authorize]
    public class ChatController : BaseController
    {
        public ChatController(IUnitOfWork uow): base(uow)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chats(int Id)
        {
            var user = GetUser;
            var data = unitOfWork.ChatRepository.GetAll(user.Id, Id, 1, 50);
            ViewBag.Reciever = Id;
            ViewBag.Sender = user.Name;
            ViewBag.SenderAvatar = user.Avatar;
            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> Pushermessage(string message, int id)
        {
            var user = GetUser;
            unitOfWork.ChatRepository.Create(new DAL.Models.Chat
            {
                Message = message,
                Reciever = new DAL.Models.User { Id = id},
                Sender = new DAL.Models.User { Id = user.Id}
            });

            var options = new PusherOptions();
            options.Cluster = "eu";
            var pusher = new Pusher("444963", "e9864ac961f7c5480aa0", "8f4e29465eb90513439a", options);
            ITriggerResult result = await pusher.TriggerAsync("asp_channel", "asp_event", new { message = message, name = "Anonymous" });

            return new HttpStatusCodeResult((int)HttpStatusCode.OK);
        }
    }
}