﻿using BugTracker.Signalr.hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;


[assembly: OwinStartupAttribute(typeof(BugTracker.Startup))]
namespace BugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
          
            ConfigureAuth(app);

            app.MapSignalR();

           
        }
    }
}




