using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.NCN
{

    public class NotificationHub : Microsoft.AspNet.SignalR.Hub
    {
        public void SendNotification(string message)
        {
            this.Clients.All.Notify(message);
        }
    }

}