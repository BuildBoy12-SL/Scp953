namespace Scp953
{
    using Exiled.API.Features;

    public class Scp953 : Plugin<Config>
    {
        private EventHandlers _eventHandlers;
        internal static Scp953 Singleton;

        public override void OnEnabled()
        {
            Singleton = this;
            _eventHandlers = new EventHandlers(Config);
            Exiled.Events.Handlers.Server.RoundStarted += _eventHandlers.OnRoundStart;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= _eventHandlers.OnRoundStart;
            _eventHandlers = null;
            Singleton = null;
            base.OnDisabled();
        }

        public override string Author { get; } = "Build";
    }
}