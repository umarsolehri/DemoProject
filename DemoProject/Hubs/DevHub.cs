using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Configuration;
using Microsoft.AspNet.SignalR.Hubs;

namespace DemoProject.Hubs
{
    [HubName("DevHub")]
    public class DevHub : Hub
    {
        private static string conString = ConfigurationManager.ConnectionStrings["DemoProjectContext"].ToString();
        public void Hello()
        {
            Clients.All.hello();
        }

        public static void DevNoti()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<DevHub>();
            context.Clients.All.updateDev();
        }

    }
}