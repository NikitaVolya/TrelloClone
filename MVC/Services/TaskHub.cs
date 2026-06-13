using Microsoft.AspNetCore.SignalR;

namespace MVC.Services
{
    public class TaskHub : Hub
    {
        public async Task SendComment(int taslId, string comment)
        {
            await Clients.All.SendAsync("ReveiveComment", taslId, comment);
        }
    }
}
