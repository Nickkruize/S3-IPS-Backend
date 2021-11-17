using Microsoft.AspNetCore.SignalR;
using S3_webshop.Hubs.Clients;
using S3_webshop.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
    }
}
