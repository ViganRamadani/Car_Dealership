using Car_Dealership.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Dealership.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
           await Clients.All.SendAsync("RecieveMessage",name, message);
        }
    }
}
