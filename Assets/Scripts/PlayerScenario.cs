using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerScenario", menuName = "ScriptableObjects/PlayerScenario", order = 1)]
public class PlayerScenario : ScriptableObject
{
    public StringList PlayerName;
    public Profession Profession;
}


public enum Profession
{
    Tank,
    Healer,
    DamageArcher,
    DamageSwordsman,
}