using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioSource AudioSource { get; private set; }

    [SerializeField]
    private AudioSource audioSource;

    void Awake()
    {
        if (AudioSource != null)
        {
            Destroy(gameObject);
            return;
        }

        AudioSource = audioSource;

        DontDestroyOnLoad(gameObject);
    }
}
