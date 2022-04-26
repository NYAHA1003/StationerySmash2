using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Util;

namespace Util
{
    enum SoundType
    {
        Bgm,
        Effect
    }
    enum EffSoundType
    {
        Exp,
        Attack,
        Button,
        Button1,
        Spawn,
        Money,
        Die,
        paperButton,
        LevelUp
    }
    enum BGMSoundType
    { 
        Main,
        Intro,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        GongJackSo
    }
}


public class SoundManager : MonoBehaviour
{

    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField]
    private List<AudioSource> effSources;
    [SerializeField]
    private AudioSource bgmAudioSource;
    [SerializeField]
    private List<AudioSource> bgmSources;
    [SerializeField]
    private Slider _bgmSlider;
    [SerializeField]
    private Slider _effectSoundSlider;

    int minValue = -40, maxValue = 0;

    float[] sound = new float[2];

    private void OnEnable()
    {
        _audioMixer.GetFloat("BGM", out float bgmValue);
        _audioMixer.GetFloat("Effect", out float effectValue);
        _bgmSlider.value = bgmValue;
        _effectSoundSlider.value = effectValue; 
    }   

    public void OnSetAudio(int soundType)
    {
        sound[0] = _bgmSlider.value;
        sound[1] = _effectSoundSlider.value;
        switch (soundType)
        {
            case (int)SoundType.Bgm:
                _audioMixer.SetFloat("BGM", Mathf.Log10(sound[soundType]) * 20);
                break;
            case (int)SoundType.Effect:
                _audioMixer.SetFloat("Effect", Mathf.Log10(sound[soundType]) * 20);
                break; 
        }
       
        //PlayerPrefs.SetFloat("Bgm",);
        //PlayerPrefs.SetFloat("Effect",);
    }
    public void PlayEffValum(int sound)
    {
        effSources[sound].Play();
    }
}
