﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENet;
using static ENet.Native;
using IntWarsSharp.Logic.Packets;
using IntWarsSharp.Logic.Items;

namespace IntWarsSharp.Core.Logic.PacketHandlers.Packets
{
    class HandleSellItem : IPacketHandler
    {
        public unsafe bool HandlePacket(ENetPeer* peer, byte[] data, Game game)
        {
            var sell = new SellItem(data);
            var client = game.getPeerInfo(peer);

            var i = game.getPeerInfo(peer).getChampion().getInventory().getItemSlot(sell.slotId);
            if (i == null)
                return false;

            float sellPrice = i.getTemplate().getTotalPrice() * i.getTemplate().getSellBackModifier();
            client.getChampion().getStats().setGold(client.getChampion().getStats().getGold() + sellPrice);

            if (i.getTemplate().getMaxStack() > 1)
            {
                i.decrementStacks();
                PacketNotifier.notifyRemoveItem(client.getChampion(), sell.slotId, i.getStacks());

                if (i.getStacks() == 0)
                    client.getChampion().getInventory().removeItem(sell.slotId);
            }
            else
            {
                PacketNotifier.notifyRemoveItem(client.getChampion(), sell.slotId, 0);
                client.getChampion().getInventory().removeItem(sell.slotId);
            }

            return true;
        }
    }
}
