﻿using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace LeagueSandbox.GameServer.GameObjects.Other
{
    public class Placeable : ObjAiBase, IPlaceable
    {
        public string Name { get; }
        public IObjAiBase Owner { get; } // We'll probably want to change this in the future

        public Placeable(
            Game game,
            IObjAiBase owner,
            float x,
            float y,
            string model,
            string name,
            uint netId = 0
        ) : base(game, model, new Stats.Stats(), 40, x, y, 0, netId)
        {
            SetTeam(owner.Team);

            Owner = owner;

            SetVisibleByTeam(Team, true);

            MoveOrder = MoveOrder.MOVE_ORDER_MOVE;

            Name = name;
        }

        public override void OnAdded()
        {
            base.OnAdded();
            _game.PacketNotifier.NotifySpawn(this);
        }
    }
}
