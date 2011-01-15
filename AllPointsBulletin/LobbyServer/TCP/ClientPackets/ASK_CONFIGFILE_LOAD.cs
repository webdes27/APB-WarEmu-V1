﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FrameWork.Logger;
using FrameWork.NetWork;
using FrameWork.zlib;

using Common;

namespace LobbyServer
{
    [PacketHandlerAttribute(PacketHandlerType.TCP, (int)Opcodes.ASK_CONFIGFILE_LOAD, "onAskConfigfileLoad")]
    public class ASK_CONFIGFILE_LOAD : IPacketHandler
    {
        public int HandlePacket(BaseClient client, PacketIn packet)
        {
            LobbyClient cclient = (LobbyClient)client;

            byte FileId = packet.GetUint8();

            PacketOut Out = new PacketOut((UInt32)Opcodes.ANS_CONFIGFILE_LOAD);
            Out.WriteInt32Reverse(0);
            Out.WriteByte(FileId);

            byte[] Result = ZlibMgr.Compress(
                                            Program.FileMgr.GetFileByte(cclient.Account.Id,FileId,true,"","")
                                            );
            cclient.SendTCP(Out);

            return 0;
        }
    }
}