using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource seSpeaker = default;
    // [SerializeField] AudioSource bgmSpeaker = default;
    // [SerializeField] AudioClip[] seClips = default;
    // [SerializeField] AudioClip[] bgmClips = default;


    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public enum BGM
    {
        Title,
        Field,
        Battle,
    }


    public enum SE
    {
        Attack,
        Message,
        Button,
    }

    public void PlaySE(SE se)
    {
        return;
        //int id = (int)se;
        //seSpeaker.PlayOneShot(seClips[id]);
    }

    public bool IsPlayingMessage()
    {
        return seSpeaker.isPlaying;
    }

    public void PlayBGM(BGM bgm)
    {
        return;
        //int id = (int)bgm;
        //if (bgm == BGM.Battle)
        //{
        //    bgmSpeaker.volume = 0.2f;
        //}
        //else
        //{
        //    bgmSpeaker.volume = 1f;
        //}
        //bgmSpeaker.clip = bgmClips[id];
        //bgmSpeaker.Play();
    }
}
