﻿//
// @brief: 声音管理类
// @version: 1.0.0
// @author lhy
// @date: 2019/11/15
// 
// 
//

using UnityEngine;

public class AudioManager :BaseManager {
    public AudioManager(GameFacade facade) : base(facade) { }
    private const string Sound_Prefix = "Sounds/";
    public const string Sound_Alert = "Alert";
    public const string Sound_ArrowShoot = "ArrowShoot";
    public const string Sound_Bg_Fast = "Bg(fast)";
    public const string Sound_Bg_Moderate = "Bg(moderate)";
    public const string Sound_ButtonClick = "ButtonClick";
    public const string Sound_Miss = "Miss";
    public const string Sound_ShootPerson = "ShootPerson";
    public const string Sound_Timer = "Timer";

    private AudioSource bgAudioSource;
    private AudioSource normalAudioSource;
    public override void OnInit()
    {
        GameObject audioSourceGO = new GameObject("AudioSurce(GameObject)");
        bgAudioSource = audioSourceGO.AddComponent<AudioSource>();
        normalAudioSource = audioSourceGO.AddComponent<AudioSource>();

        //PlaySound(bgAudioSource, LoadSound(Sound_Bg_Moderate),0.05f, true);
    }
    public void PlayBgSound(string soundName) {
        PlaySound(bgAudioSource, LoadSound(soundName), 0.05f, true);
    }

    public void PlayNormalSound(string soundName)
    {
        PlaySound(normalAudioSource, LoadSound(soundName), 0.1f);
    }

    private void PlaySound(AudioSource audioSource,AudioClip clip,float volume,bool loop = false)
    {
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }
    private AudioClip LoadSound(string soundsName)
    {
        return Resources.Load<AudioClip>(Sound_Prefix + soundsName);
    }

}
