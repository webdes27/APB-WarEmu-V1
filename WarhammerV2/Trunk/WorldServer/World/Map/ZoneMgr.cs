﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using FrameWork.Logger;

using Common;

namespace WorldServer
{
    public class ZoneMgr
    {
        public UInt16 ZoneId;
        public Zone_Info Info;
        public RegionMgr Region;
        public bool Running;

        public ZoneMgr(RegionMgr Region,Zone_Info Info)
        {
            this.Region = Region;
            this.ZoneId = Info.ZoneId;
            this.Info = Info;
            this.Running = true;
        }
        public void Stop()
        {
            Log.Success("ZoneMgr", "[" + ZoneId + "] Stop");
        }


        #region Objects

        static public UInt16 MAX_OBJECTS = 65000;

        public List<Object> _Objects = new List<Object>();
        public List<Player> _Players = new List<Player>();

        public Object GetObject(int Id)
        {
            lock (_Objects)
                return _Objects.Find(obj => obj != null && obj.Oid == Id);
        }
        public int GetPlayers()
        {
            return _Players.Count();
        }
        public void AddObject(Object Obj)
        {
            if (Obj.Zone == this)
                return;

            if (Obj.Zone != null && Obj.Zone != this)
                Obj.Zone.RemoveObject(Obj);

            lock (_Objects)
            {
                Obj.Zone = this;

                _Objects.Add(Obj);

                if (Obj.IsPlayer())
                    _Players.Add(Obj.GetPlayer());
            }
        }
        public void RemoveObject(Object Obj)
        {
            Log.Success("RemoveObjectZone", Obj.Name);
            lock (_Objects)
            {
                _Objects.Remove(Obj);
                if (Obj.IsPlayer())
                    _Players.Remove(Obj.GetPlayer());
            }
            Obj._ZoneMgr = null;
        }
        public bool Run()
        {
            if (!Running)
                return false;

            lock (_Objects)
            {
                for (int i = 0; i < _Objects.Count; ++i)
                {
                        if (_Objects[i] != null && _Objects[i].Zone == this)
                            if (!_Objects[i].IsLoad())
                                _Objects[i].Load();
                            else
                                _Objects[i].Update();
                }
            }

            return true;
        }

        #endregion


        #region Range

        public List<Player> GetRangedPlayer(Object Obj, int Range)
        {
            Log.Info("ZoneMgr", "GetRangedPlayer zoneid = " + ZoneId + ",ObjSize=" + _Objects.Count);
            lock (_Objects)
            {
                return _Players.FindAll(Ranged => Ranged != null &&
                    RegionMgr.IsRange(Ranged.XOffset, Obj.XOffset, Range) && RegionMgr.IsRange(Ranged.YOffset, Obj.YOffset, Range));
                    
            }
        }
        public List<Object> GetRangedObject(Object Obj, int Range)
        {
            Log.Info("ZoneMgr", "GetRangedObject zoneid = " + ZoneId +",ObjSize="+_Objects.Count);
            lock (_Objects)
            {
                return _Objects.FindAll(Ranged => Ranged != null &&
                    RegionMgr.IsRange(Ranged.XOffset, Obj.XOffset, Range) && RegionMgr.IsRange(Ranged.YOffset, Obj.YOffset, Range));

            }
        }

        public UInt16 CalculPin(uint WorldPos,bool x)
        {
            UInt16 Pin = 0;

            int BaseOffset = x ? Info.OffX << 12 : Info.OffY << 12;
            if (BaseOffset <= WorldPos)
                Pin = (UInt16)(WorldPos - BaseOffset);

            return Pin;
        }
        public UInt16 CalculPin(UInt16 Pos, UInt16 Offset, bool x)
        {
            int BaseOffset = x ? Info.OffX : Info.OffY;

            if (Offset >= BaseOffset + 8) // Si on est sur la partie supérieur /2+32768
                Pos += 32768;

            return Pos;
        }
        public UInt16 CalculCombat(UInt16 Pos, UInt16 OffSet, bool x)
        {
            Pos /= 2;
            UInt16 Base = (UInt16)(x == true ? Info.OffX : Info.OffY);

            if (Base > OffSet) return Pos;

            // 0>32768 Tous les 4 Offset
            OffSet -= Base;
            // 212-=200 = 12
            while (OffSet >= 4)
            {
                // 8 = 16384
                // 4 = 32768
                // 0 = 49152
                OffSet-=4;
                Pos += 16384;
            }

            return Pos;
        }

        #endregion
    }
}
