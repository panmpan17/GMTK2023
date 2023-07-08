using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpriteList", menuName = "ScriptableObjects/SpriteList", order = 1)]
public class SpriteList : ScriptableObject
{
    public Sprite[] List;

    public Sprite GetRandom()
    {
        return List[Random.Range(0, List.Length)];
    }
}
