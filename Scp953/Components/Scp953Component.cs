using MEC;

namespace Scp953.Components
{
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Interactables.Interobjects.DoorUtils;
    using System;
    using UnityEngine;

    public class Scp953Component : MonoBehaviour
    {
        private Config _config;
        private Player _player;

        public void Awake()
        {
            _config = Scp953.Singleton.Config;
            _player = Player.Get(gameObject);
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.Escaping += OnEscaping;
            Exiled.Events.Handlers.Player.Handcuffing += OnHandcuffing;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Player.Shooting += OnShooting;
            Exiled.Events.Handlers.Player.Shot += OnShot;
            Exiled.Events.Handlers.Player.UsingMedicalItem += OnUsingMedicalItem;

            DoorVariant spawnDoor = null;
            foreach (var door in Map.Doors)
            {
                var extension = door.GetComponent<DoorNametagExtension>();
                if (extension != null && string.Equals(extension.GetName, _config.SpawnDoor,
                    StringComparison.CurrentCultureIgnoreCase))
                {
                    spawnDoor = door;
                    break;
                }

                if (!string.Equals(door.name, _config.SpawnDoor,
                    StringComparison.CurrentCultureIgnoreCase)) continue;

                spawnDoor = door;
                break;
            }

            if (spawnDoor == null)
            {
                Log.Error($"Could not find the specified spawn location for Scp953. ({_config.SpawnDoor})");
                return;
            }

            UnDisguise();
            Timing.CallDelayed(3f, () => _player.Health = _player.MaxHealth = _config.Health);
            _player.Position = spawnDoor.transform.position + new Vector3(1, 2, 1);
            _player.EnableEffect<CustomPlayerEffects.Scp207>();
            _player.ResetInventory(_config.Inventory);
            _player.Broadcast(_config.SpawnMessage);
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (ev.Killer == _player)
                Disguise(ev.Target.Role);

            if (ev.Target == _player)
                Destroy(this);
        }

        private void OnEscaping(EscapingEventArgs ev)
        {
            if (ev.Player == _player)
                ev.IsAllowed = false;
        }

        private void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (ev.Cuffer == _player && _player.Role != _config.Role)
                ev.IsAllowed = false;
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Target == _player && (ev.DamageType == DamageTypes.Scp207 || ev.Attacker.IsScp ||
                                         ev.Attacker.CustomInfo == "<color=#FF0000>SCP-035</color>"))
            {
                ev.IsAllowed = false;
            }

            if (ev.Attacker == _player && (ev.Target.IsScp ||
                                           ev.Attacker.CustomInfo == "<color=#FF0000>SCP-035</color>"))
            {
                ev.IsAllowed = false;
            }
        }

        private void OnLeft(LeftEventArgs ev)
        {
            if (ev.Player == _player)
                Destroy(this);
        }

        private void OnShooting(ShootingEventArgs ev)
        {
            if (Server.FriendlyFire || ev.Target == null)
                return;
            
            Player target = Player.Get(ev.Target);
            if (target == null || target != _player) return;
            ev.Shooter.IsFriendlyFireEnabled = true;
            Timing.CallDelayed(0.1f, () => ev.Shooter.IsFriendlyFireEnabled = false);
        }
        
        private void OnShot(ShotEventArgs ev)
        {
            Player target = Player.Get(ev.Target);
            if (target != null && target == _player && target.Role != _player.Role)
                UnDisguise();
        }

        private void OnUsingMedicalItem(UsingMedicalItemEventArgs ev)
        {
            if (ev.Item.IsScp())
                ev.IsAllowed = false;
        }

        private void OnDestroy()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.Escaping -= OnEscaping;
            Exiled.Events.Handlers.Player.Handcuffing -= OnHandcuffing;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Exiled.Events.Handlers.Player.Shot -= OnShot;
            Exiled.Events.Handlers.Player.UsingMedicalItem -= OnUsingMedicalItem;

            _player.CustomInfo = string.Empty;
            _player.DisableEffect<CustomPlayerEffects.Scp207>();
        }

        public void Disguise(RoleType roleType)
        {
            float health = _player.Health;
            _player.SetRole(roleType, true);
            _player.Health = health;
            _player.MaxHealth = _config.Health;
            _player.CustomInfo = string.Empty;
        }

        public void UnDisguise()
        {
            float health = _player.Health;
            _player.SetRole(_config.Role, true);
            _player.Health = health;
            _player.MaxHealth = _config.Health;
            _player.CustomInfo = _config.Tag;
        }
    }
}