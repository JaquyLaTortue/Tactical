﻿using UnityEngine;

public class MonsterMain : Entity
{
    public MonsterCapacity MonsterCapacity { get; private set; }

    public MonsterHealth MonsterHealth { get; private set; }

    [field: SerializeField]
    public MonsterBase MonsterBase { get; private set; }

    private void Awake()
    {
        HpMax = MonsterBase.HpMax;
        HpCurrent = MonsterBase.HpMax;
        PaMax = MonsterBase.PaMax;
        PaCurrent = MonsterBase.PaMax;
        Atk = MonsterBase.Atk;
        Def = MonsterBase.Def;
    }
}
