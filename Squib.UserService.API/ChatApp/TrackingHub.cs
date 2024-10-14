using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Squib.UserService.API.ChatApp
{
    public class TrackingHub : Hub
    {
        public async Task SendLocation(string deviceId, double latitude, double longitude)
        {
            await Clients.All.SendAsync("ReceiveLocation", deviceId, latitude, longitude);
        }

        // For group location
        public async Task SendGroupLocation(string groupName, string deviceId, double latitude, double longitude)
        {
            await Clients.Group(groupName).SendAsync("ReceiveGroupLocation", deviceId, latitude, longitude);
        }

        // Add client to a group
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("GroupNotification", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        // Remove client from a group
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("GroupNotification", $"{Context.ConnectionId} has left the group {groupName}.");
        }
    }
}
