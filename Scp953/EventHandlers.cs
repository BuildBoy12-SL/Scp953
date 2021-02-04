namespace Scp953
{
    using Components;
    using Exiled.API.Features;
    using MEC;
    using System;
    using System.Linq;

    public class EventHandlers
    {
        public EventHandlers(Config config) => _config = config;
        private readonly Config _config;

        public void OnRoundStart()
        {
            Timing.CallDelayed(1.5f, () =>
            {
                var scps = Player.Get(Team.SCP).ToList();
                if (scps.Count <= 1)
                    return;

                Random random = new Random();
                if (random.Next(101) > _config.SpawnChance)
                    return;

                Player player = scps.FirstOrDefault();
                if (player == null)
                    return;

                if (Scp953.RemoveClass != null)
                    Scp953.RemoveClass.Invoke(null, new[] {player});

                player.GameObject.AddComponent<Scp953Component>();
            });
        }
    }
}