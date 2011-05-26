﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FrameWork;

[DataTable(DatabaseName = "Characters", TableName = "Characters", PreCache = true)]
[Serializable]
public class Character : DataObject
{
    [PrimaryKey(AutoIncrement = true, IncrementValue = 3)]
    public int Id;

    [DataElement]
    public long AccountId;

    [DataElement]
    public byte RealmId;

    [DataElement(Varchar = 19)]
    public string Name;

    [DataElement()]
    public long Level;

    [DataElement()]
    public long Race;

    [DataElement()]
    public long Class;

    [DataElement()]
    public long Sex;

    [DataElement]
    public long HeadModelID;

    [DataElement()]
    public long HairModelID;

    [DataElement()]
    public string Data;
}