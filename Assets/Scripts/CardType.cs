using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MPack;


[CreateAssetMenu(fileName = "CardType", menuName = "ScriptableObjects/CardType", order = 1)]
public class CardType : ScriptableObject
{
    public string CardName;
    public Image CardImage;

    [Header("Temp")]
    public ColorReference BackgroundColor;
    public ColorReference SecondaryColor;
}
