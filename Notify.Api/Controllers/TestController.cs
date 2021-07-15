using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notify.Core.Services.Interfaces;
using System.Linq;

namespace Notify.Api.Controllers
{
    [AllowAnonymous]

    [Route("api/[controller]/[action]")]
    public class TestController : Controller
    {
        private readonly INotificationService _notifyTest;
        public TestController( INotificationService notifyTest)
        {
            _notifyTest = notifyTest;
        }


        [HttpGet]
        public  IActionResult GetNotification()
        {
            return Json(_notifyTest.GetAll().ToList());
        }
    }
}