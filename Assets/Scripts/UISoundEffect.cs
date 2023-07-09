using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UISoundEffect", menuName = "ScriptableObjects/UISoundEffect", order = 1)]
public class UISoundEffect : ScriptableObject
{
    public AudioClip Clip;
    [Range(0f, 1f)]
    public float Volume = 1f;

    public void Play()
    {
        if (Clip != null)
        {
            AudioManager.AudioSource.PlayOneShot(Clip, Volume);
            // AudioSource.pl(Clip, Camera.main.transform.position, Volume);
        }
    }
}
