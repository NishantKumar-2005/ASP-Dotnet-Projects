using System;
using Microsoft.AspNetCore.SignalR;

namespace Squib.UserService.API.ChatApp;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    //send message to group

    public async Task SendMessageToGroup(string groupName, string user, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
    }

    //add to group

    public async Task AddToGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined the group {groupName}.");
    }

    //remove from group

    public async Task RemoveFromGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the group {groupName}.");
    }
    public override async Task OnDisconnectedAsync(Exception exception)
{
    // Log the reason for the disconnection
    Console.WriteLine($"Client disconnected. Reason: {exception?.Message ?? "No exception"}");

    if (exception != null)
    {
        // Optionally rethrow the exception or handle it.
        throw exception;
    }

    await base.OnDisconnectedAsync(exception);
}
public async Task SendTypingNotification(string groupName, string user)
    {
        await Clients.Group(groupName).SendAsync("UserTyping", user);
    }



}
