using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BossActionType
{
    Idle = 0,
    Moving = 1,
    Attacking = 2,
    Stuning = 3,
    Enraging = 4,
    Dying = 5,
    CounterAttack = 6,
    StartBattle = 7,
}
