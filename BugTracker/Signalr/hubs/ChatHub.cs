using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using BugTracker.Models;
using Microsoft.AspNet.SignalR.Transports;

namespace BugTracker.Signalr.hubs
{
    public class ChatHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        public void Send(string message)
        {
            string name = Context.User.Identity.Name;
            Clients.All.broadcastMessage(name, message);
        }

        public void SendUser(string userId, string message)
        {
            string name = Context.User.Identity.Name;

            foreach (var connectionId in _connections.GetConnections(userId))
            {
                Clients.Client(connectionId).addChatMessage(name + ": " + message);
            }
        }

        public void GetConnectedUsers()
        {
            var connectedUsers = _connections.GetConnectedUsers();
            Clients.All.LoadConnectedUsers(connectedUsers);
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }

}
