namespace Scp953
{
    using Exiled.API.Features;
    using System.Reflection;

    public class Scp953 : Plugin<Config>
    {
        private EventHandlers _eventHandlers;
        internal static Scp953 Singleton;
        internal static MethodInfo RemoveClass;

        public override void OnEnabled()
        {
            Singleton = this;
            _eventHandlers = new EventHandlers(Config);
            Exiled.Events.Handlers.Server.RoundStarted += _eventHandlers.OnRoundStart;
            base.OnEnabled();

            foreach (var plugin in Exiled.Loader.Loader.Plugins)
            {
                if (plugin.Name == "Subclass")
                {
                    RemoveClass = plugin.Assembly.GetType("Subclass.API").GetMethod("RemoveClass");
                }
            }
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