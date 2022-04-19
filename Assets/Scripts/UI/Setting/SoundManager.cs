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
}


public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField]
    private Slider _bgmSlider;
    [SerializeField]
    private Slider _effectSoundSlider;

    int minValue = -40, maxValue = 0;

    private void OnEnable()
    {
       _audioMixer.GetFloat("Effect", out float effectValue);
        _audioMixer.GetFloat("BGM", out float bgmValue);
        _bgmSlider.value = bgmValue;
        _effectSoundSlider.value = effectValue; 
    }   

    public void OnSetAudio(int soundType)
    {
        float sound = _bgmSlider.value;
        switch (soundType)
        {
            case (int)SoundType.Bgm:
                _audioMixer.SetFloat("BGM", Mathf.Log10(sound) * 20);
                break;
            case (int)SoundType.Effect:
                _audioMixer.SetFloat("Effect", Mathf.Log10(sound) * 20);
                break; 
        }
        


        //PlayerPrefs.SetFloat("Bgm",);
        //PlayerPrefs.SetFloat("Effect",);
    }
}
