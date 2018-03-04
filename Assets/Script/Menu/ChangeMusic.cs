using UnityEngine;
using System.Collections;

public class ChangeMusic : MonoBehaviour
{

    public AudioClip levelMusic;
    public float FadedTime;


    private AudioSource source;
    public AudioSource source2;


    // Use this for initialization
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }


    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            source.clip = levelMusic;
            source.volume = 0.3f;
            source.Play();
            source2.volume = 0.2f;
            source2.Play();
        }
    }
}