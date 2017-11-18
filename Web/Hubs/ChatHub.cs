using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Hubs
{
    [HubName("Chat")]
    public class ChatHub : Hub
    {
        [HubMethodName("NotifyClients")]
        public static void NotifyCurrentEmployeeInformationToAllClients()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            // the update client method will update the connected client about any recent changes in the server data
            context.Clients.All.updatedClients();
        }
    }
}