﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FrameWork;

namespace Common
{
    [ISerializableAttribute((long)Opcodes.ProtocolHandshakeClientKey)]
    public class HandshakeClientKey : ISerializablePacket
    {
        [ArrayBitAttribute(0)]
        public byte[] ClientKey;

        public override void OnRead(RiftClient From)
        {
            Log.Dump("ClientKey", ClientKey, 0, ClientKey.Length);

            From.InitCrypto(ClientKey);

            HandshakeCompression Cp = new HandshakeCompression();
            Cp.Enabled = true;
            From.SendSerialized(Cp);
            From.EnableSendCompress();


            HandshakeServerKey ServerKey = new HandshakeServerKey();
            ServerKey.Nid = 420;
            ServerKey.ServerKey = From.LocalPublicKey;
            From.SendSerialized(ServerKey);
        }
    }
}
