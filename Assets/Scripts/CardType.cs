using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "CardType", menuName = "ScriptableObjects/CardType", order = 1)]
public class CardType : ScriptableObject
{
    public string CardName;
    public Image CardImage;
}
