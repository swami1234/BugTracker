using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace BugTracker.Signalr.hubs
{
    public class ChatHub : Hub
    {
        

        public void send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message, DateTime.Now);
        }

       
    }
}