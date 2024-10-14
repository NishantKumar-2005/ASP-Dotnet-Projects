using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Squib.UserService.API.ChatApp
{
    public class TrackingHub : Hub
    {
       
        public async Task SendLocation(string deviceId, double latitude, double longitude)
        {
            await Clients.All.SendAsync("ReceiveLocation", deviceId, latitude, longitude);
        }

        
        public async Task SendGroupLocation(string groupName, string deviceId, double latitude, double longitude)
        {
            await Clients.Group(groupName).SendAsync("ReceiveGroupLocation", deviceId, latitude, longitude);
        }

        // Method for clients to join a group
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveGroupNotification", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        // Method for clients to leave a group
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveGroupNotification", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        // Optionally override OnDisconnectedAsync to handle cleanup
        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            // Optionally notify other group members
            await base.OnDisconnectedAsync(exception);
        }
    }
}
