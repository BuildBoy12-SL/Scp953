namespace Scp953
{
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using System.Collections.Generic;

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int Health { get; set; } = 900;
        public RoleType Role { get; set; } = RoleType.Tutorial;
        public string Tag { get; set; } = "<color=red>SCP-953</color>";
        public string SpawnDoor { get; set; } = "SERVERS_BOTTOM";
        public int SpawnChance { get; set; } = 15;

        public Broadcast SpawnMessage { get; set; } = new Broadcast(
            "You have spawned as <b>Scp-953</b>!\nWhen you kill someone, you will automatically disguise as their class.\nWhen you get shot, you will lose your disguise.");

        public List<ItemType> Inventory { get; set; } = new List<ItemType>
        {
            ItemType.GunProject90
        };
    }
}