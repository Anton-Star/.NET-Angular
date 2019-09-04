using Microsoft.AspNet.SignalR;

namespace FactWeb.Mvc.Hubs
{
    public class CacheHub : Hub
    {

        public void Invalidated(string dbName)
        {
            this.Clients.All.Invalidated(dbName);
        }
    }
}