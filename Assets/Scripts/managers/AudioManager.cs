using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioPack audioPack;
    
    private int priority = 0;

    // Start is called before the first frame update
    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        AudioListener.volume = Globals.BASE_VOLUME;
    }
       
    public void PlaySound_Click()
    {
        playStandartSound(0, audioPack.SimpleClick);        
    }

    public void PlaySound_BackRotate()
    {
        playStandartSound(0, audioPack.ReverseClick);        
    }

    public void PlaySound_Success()
    {
        playStandartSound(1, audioPack.Happy01);        
    }

    public void PlaySound_CardShuffle()
    {
        playStandartSound(0, audioPack.CardsShuffle);
    }

    public void PlaySound_WinGame()
    {
        playStandartSound(2, audioPack.WinMelody);
    }

    public void PlaySound_LoseGame()
    {
        playStandartSound(2, audioPack.LoseMelody);
    }

    public void StopAny()
    {
        _audio.Stop();
    }

    private void playStandartSound(int _priority, AudioClip clip)
    {
        if ((priority > _priority && _audio.isPlaying) || (_audio.clip == clip && _audio.isPlaying)) return;

        priority = _priority;
        _audio.Stop();
        _audio.clip = clip;
        _audio.Play();
    }

    public void Mute()
    {
        AudioListener.volume = 0;
    }

    public void UnMute()
    {
        AudioListener.volume = Globals.BASE_VOLUME;
    }
}
