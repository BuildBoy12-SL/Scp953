namespace Scp953.Commands
{
    using CommandSystem;
    using Components;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn953 : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("953.spawn"))
            {
                response = "Insufficient permission. Required: 953.spawn";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Syntax: spawn953 <Player>";
                return false;
            }

            Player player = Player.Get(arguments.At(0));
            if (player == null)
            {
                response = $"Unable to find a user from '{arguments.At(0)}'.";
                return false;
            }

            player.GameObject.AddComponent<Scp953Component>();
            response = $"Set {player.Nickname} to a Scp935.";
            return true;
        }

        public string Command => "spawn953";
        public string[] Aliases => Array.Empty<string>();
        public string Description => "Spawns a user as a Scp-953";
    }
}