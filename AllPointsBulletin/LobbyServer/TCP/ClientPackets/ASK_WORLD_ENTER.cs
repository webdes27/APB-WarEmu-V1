﻿/*
 * Copyright (C) 2011 APS
 *	http://AllPrivateServer.com
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FrameWork;

using Common;

namespace LobbyServer
{
    public class ASK_WORLD_ENTER : IPacketHandler
    {
        [PacketHandlerAttribute(PacketHandlerType.TCP, (int)Opcodes.ASK_WORLD_ENTER, "onAskWorldEnter")]
        static public void HandlePacket(BaseClient client, PacketIn packet)
        {
            LobbyClient cclient = (LobbyClient)client;

            byte sloid = packet.GetUint8();
            Program.CharMgr.SetEnter(cclient.Account.Id, sloid);

            WorldInfo Info = Program.CharMgr.GetWorldInfo(cclient.Account.WorldId);

            PacketOut Out = new PacketOut((UInt32)Opcodes.ANS_WORLD_ENTER);

            if (Info == null)
                Out.WriteUInt32R(1);
            else
            {
                Out.WriteUInt32R(0);

                Out.WriteInt32R(Info.Ip); // WorldServerIp
                Out.WriteUInt16R((UInt16)Info.Port); // Port
                Out.WriteInt64R(TCPManager.GetTimeStamp());
            }

            cclient.SendTCP(Out);
        }
    }
}
