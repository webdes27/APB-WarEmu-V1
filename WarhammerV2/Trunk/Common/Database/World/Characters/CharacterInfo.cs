﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FrameWork.Database;

namespace Common
{
    // Valeur Fixe d'un character
    [DataTable(PreCache = false, TableName = "CharacterInfo", DatabaseName = "World")]
    [Serializable]
    public class CharacterInfo : DataObject
    {
        public byte _CareerLine;
        public byte _Career;
        public string _CareerName;
        public byte _Realm;
        public UInt16 _Region;
        public UInt16 _ZoneId;
        public int _WorldX;
        public int _WorldY;
        public int _WorldZ;
        public int _WorldO;
        public ushort _RallyPt;
        public uint _Skills;

        [DataElement(Unique = true)]
        public byte CareerLine
        {
            get { return _CareerLine; }
            set { _CareerLine = value; Dirty = true; }
        }

        [DataElement(AllowDbNull=false)]
        public byte Career
        {
            get { return _Career; }
            set { _Career = value; Dirty = true; }
        }

        [DataElement(AllowDbNull = false,Varchar=255)]
        public string CareerName
        {
            get { return _CareerName; }
            set { _CareerName = value; Dirty = true; }
        }

        [DataElement(AllowDbNull = false)]
        public byte Realm
        {
            get { return _Realm; }
            set { _Realm = value; Dirty = true; }
        }

        [DataElement(AllowDbNull = false)]
        public UInt16 Region
        {
            get { return _Region; }
            set { _Region = value; Dirty = true; }
        }

        [DataElement(AllowDbNull = false)]
        public UInt16 ZoneId
        {
            get { return _ZoneId; }
            set { _ZoneId = value; Dirty = true; }
        }

        [DataElement(AllowDbNull = false)]
        public int WorldX
        {
            get { return _WorldX; }
            set { _WorldX = value; Dirty = true; }
        }

        [DataElement(AllowDbNull = false)]
        public int WorldY
        {
            get { return _WorldY; }
            set { _WorldY = value; Dirty = true; }
        }

        [DataElement(AllowDbNull = false)]
        public int WorldZ
        {
            get { return _WorldZ; }
            set { _WorldZ = value; Dirty = true; }
        }

        [DataElement(AllowDbNull = false)]
        public int WorldO
        {
            get { return _WorldO; }
            set { _WorldO = value; Dirty = true; }
        }

        [DataElement(AllowDbNull = false)]
        public ushort RallyPt
        {
            get { return _RallyPt; }
            set { _RallyPt = value; Dirty = true; }
        }

        [DataElement(AllowDbNull = false)]
        public uint Skills
        {
            get { return _Skills; }
            set { _Skills = value; Dirty = true; }
        }
    }
}