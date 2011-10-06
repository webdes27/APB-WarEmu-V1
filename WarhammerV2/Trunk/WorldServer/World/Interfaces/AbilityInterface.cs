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

using Common;
using FrameWork;

namespace WorldServer
{
    public class AbilityInterface : BaseInterface
    {
        public List<Ability_Info> Abilities = new List<Ability_Info>();

        public AbilityInterface(Object Owner)
            : base(Owner)
        {

        }

        public override bool Load()
        {
            UpdateAbilities();
            return base.Load();
        }

        public void UpdateAbilities()
        {
            if (HasPlayer())
                Abilities = AbilityMgr.GetCareerAbility(GetPlayer()._Info.CareerLine, GetPlayer().Level);
        }

        public void SendAbilities()
        {
            if (!HasPlayer())
                return;

            Log.Info("AbilityInterface", "Sending " + Abilities.Count + " Abilities");

            PacketOut Out = new PacketOut((byte)Opcodes.F_CHARACTER_INFO);
            Out.WriteByte(1); // Action
            Out.WriteByte((byte)Abilities.Count);
            Out.WriteUInt16(0x300);

            foreach (Ability_Info Info in Abilities)
            {
                Out.WriteUInt16(Info.Entry);
                Out.WriteByte(Info.Level);
            }

            GetPlayer().SendPacket(Out);

            PacketOut AutoAttack = new PacketOut((byte)Opcodes.F_CHARACTER_INFO);
            AutoAttack.WriteByte(1); // Action
            AutoAttack.WriteByte(1); // Count
            AutoAttack.WriteUInt16(0x300);
            AutoAttack.WriteUInt16(245);
            AutoAttack.WriteByte(1);
            GetPlayer().SendPacket(AutoAttack);

        }


        public bool IsCasting()
        {
            return false;
        }
    }
}