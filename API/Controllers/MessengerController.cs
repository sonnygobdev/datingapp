using System.Threading.Tasks;
using API.Helpers;
using API.HubConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    public class MessengerController:BaseApiController
    {
        private IHubContext<MessageHub> _hub;

        public MessengerController(IHubContext<MessageHub> hub)
        {
            _hub = hub;
        }

        public IActionResult Get(){
            var timerManager = new TimerManager(() => {
                   _hub.Clients.All.SendAsync("updatestatus",MessageManager.GetMessages());
            });

            return Ok(new {Message = "Request Completed"});
        }

        [HttpPost("postmessage")]
        public async Task<IActionResult> PostMessage(MessageData messageData){
            
             await _hub.Clients.All.SendAsync("updatestatus",messageData);

             return Ok(new {Message = "Request Completed"});
        }
    }
}