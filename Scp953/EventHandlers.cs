namespace Scp953
{
    using Components;
    using Exiled.API.Features;
    using System;
    using System.Linq;

    public class EventHandlers
    {
        public EventHandlers(Config config) => _config = config;
        private readonly Config _config;

        public void OnRoundStart()
        {
            Random random = new Random();
            if (random.Next(101) > _config.SpawnChance)
                return;

            Player.Get(Team.SCP).FirstOrDefault()?.GameObject.AddComponent<Scp953Component>();
        }
    }
}